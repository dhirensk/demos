
Imports System.Net
Imports System.Xml
Imports Microsoft.SharePoint.Client
Imports Microsoft.SharePoint
Imports System.Linq
Imports System.Data
Imports System.Windows.Forms
Imports System.Drawing

Module SP_Authentication
    'https://code.msdn.microsoft.com/office/SharePoint-Online-0bdeb2ca
    ''' <summary>
    ''' 
    ''' </summary>
    <STAThread()>
    Function Main() As String
        'This example extends the SPO_AuthenticateUsingCSOM sample
        'After authenticating, this example connects to the Lists web
        'service and gets the items in the Documents library, then
        'lists them in the console window

        'Adjust this string to point to your site on Office 365
        'Dim siteURL As String = "https://{yourcompanydomain}.sharepoint.com"
        ' Dim siteURL As String = "https://{yourcompanydomain}.sharepoint.com/sites/{team}/"
        Dim siteURL As String = My.Settings.s_siteURL
        Dim SPList As String = My.Settings.s_SPList
        Dim currentemail As String = My.Settings.s_EmailID
        'Console.WriteLine("Opening Site: " + siteURL)
        Dim return_value = "Invalid"
        If String.IsNullOrWhiteSpace(siteURL) Then
            MsgBox("Site URL not set")
            Return return_value
        End If

        If String.IsNullOrWhiteSpace(SPList) Then
            MsgBox("List Name not entered")
            Return return_value
        End If


        'Call the ClaimClientContext class to do claims mode authentication
        Using ClientContext As ClientContext = ClaimsClientContext.GetAuthenticatedContext(siteURL)
            If Not ClientContext Is Nothing Then
                'We have the client context object so claims-based authentication is complete
                ClientContext.Load(ClientContext.Web)
                ClientContext.ExecuteQuery()
                'Find out about the SP.Web object
                'Dim currentemail =  Globals.ThisAddIn.Application.Session.CurrentUser.Address
                'Display the name of the SharePoint site
                'Console.WriteLine(ClientContext.Web.Title)

                'TODO get current user email id
                'currentemail = "dhirensk@incidentwatcher.onmicrosoft.com"



                Dim Web As Web = ClientContext.Web
                'TODO get list by title
                'Dim list As List = Web.Lists.GetById(New Guid("6A0E6B06-24D3-4095-953B-19AF0662B978"))
                Dim list As List = Web.Lists.GetByTitle(SPList)
                'use U2U CAML Query Builder to verify that PersonEmail exists as a FieldRef Name

                'get completelist
                Dim query As New CamlQuery()
                query.ViewXml = "<View><Query></Query></View>"
                Dim listitems As ListItemCollection = list.GetItems(query)
                ClientContext.Load(listitems)
                ClientContext.ExecuteQuery()

                'Console.WriteLine("hello" + Str(listitems.LongCount))

                'validate list if not valid return and dont continue
                'return "invalid"
                Dim count = listitems.Count
                Dim person_email_internalname As String = ""
                If count >= 1 Then
                    'reset the columns to default
                    My.Settings.dt_Sr = Nothing
                    My.Settings.dt_ColumnName = Nothing
                    My.Settings.dt_RequiredColumn = Nothing
                    My.Settings.dt_ColumnType = Nothing
                    My.Settings.dt_ColumnTypeExpected = Nothing
                    My.Settings.dt_ColumnStatus = Nothing
                    My.Settings.Save()
                    'load default data after clearing the settings
                    DataTableLoader.LoadDataGridView2()

                    Dim fields = list.Fields
                    ClientContext.Load(fields)
                    ClientContext.ExecuteQuery()

                    Dim dt = TryCast(Globals.ThisAddIn.getControls_IncidentWatcher.DataGridView2.DataSource, DataTable)


                    For Each field In fields
                        ClientContext.Load(field)
                        ClientContext.ExecuteQuery()
                        Dim Sr As Integer = 13
                        Dim tmp As Integer = 13
                        Dim column_name As String = field.Title

                        Dim Required_Column As String = "Unknown"
                        Dim column_type As String = field.TypeAsString
                        Dim column_type_expected As String = "Unknown"
                        Dim column_status As String = "Unknown"


                        Select Case field.Title
                            Case "Project"
                                Sr = 1
                                Required_Column = "Yes"
                                column_type_expected = "Text"
                            Case "Escalation Level"
                                Sr = 2
                                Required_Column = "Yes"
                                column_type_expected = "Number"
                            Case "Person Name"
                                Sr = 3
                                Required_Column = "Yes"
                                column_type_expected = "User"
                            Case "Person Email"
                                Sr = 4
                                Required_Column = "Yes"
                                column_type_expected = "User"
                                person_email_internalname = field.InternalName
                            Case "Escalation Person"
                                Sr = 5
                                Required_Column = "Yes"
                                column_type_expected = "User"
                            Case "Escalation Person Email"
                                Sr = 6
                                Required_Column = "Yes"
                                column_type_expected = "User"
                            Case "Escalation Person Phone"
                                Sr = 7
                                Required_Column = "Yes"
                                column_type_expected = "Text"
                            Case "Incident Level"
                                Sr = 8
                                Required_Column = "Yes"
                                column_type_expected = "Number"
                            Case "Escalation Start Time"
                                Sr = 9
                                Required_Column = "Yes"
                                column_type_expected = "Number"
                            Case "Escalation End Time"
                                Sr = 10
                                Required_Column = "Yes"
                                column_type_expected = "Number"
                            Case "Reminder Beep At"
                                Sr = 11
                                Required_Column = "Yes"
                                column_type_expected = "Number"
                            Case "Reminder Call At"
                                Sr = 12
                                Required_Column = "Yes"
                                column_type_expected = "Number"
                            Case Else
                                'https://sharepoint.stackexchange.com/questions/11090/is-there-a-way-to-see-if-an-spfield-was-created-by-a-user/11092
                                'exclude system fields
                                If Not field.FromBaseType Then
                                    Required_Column = "No"
                                    Sr = tmp
                                    tmp = tmp + 1
                                End If
                        End Select
                        If Required_Column <> "Yes" Then
                            column_status = "Not Required"
                        ElseIf column_type <> column_type_expected Then
                            column_status = "DataType Mismatch"
                        ElseIf column_type = column_type_expected Then
                            column_status = "Valid"
                        Else
                            column_status = "Unknown"
                        End If

                        'filter system columns by filtering out unknown
                        If Required_Column = "Yes" Or Required_Column = "No" Then
                            Dim row = dt.NewRow
                            row("Sr") = Sr
                            row("ColumnName") = column_name
                            row("RequiredColumn") = Required_Column
                            row("ColumnType") = column_type
                            row("ColumnTypeExpected") = column_type_expected
                            row("ColumnStatus") = column_status
                            If Sr <= 12 Then
                                dt.Rows.RemoveAt(Sr - 1)
                            End If
                            dt.Rows.InsertAt(row, Sr - 1)
                        End If


                    Next
                    return_value = "Valid"
                    Globals.ThisAddIn.getControls_IncidentWatcher.DataGridView2.DataSource = dt
                    Dim sc_Sr As New System.Collections.Specialized.StringCollection
                    Dim sc_ColumnName As New System.Collections.Specialized.StringCollection
                    Dim sc_RequiredColumn As New System.Collections.Specialized.StringCollection
                    Dim sc_ColumnType As New System.Collections.Specialized.StringCollection
                    Dim sc_ColumnTypeExpected As New System.Collections.Specialized.StringCollection
                    Dim sc_ColumnStatus As New System.Collections.Specialized.StringCollection

                    'set return_value by checking ColumnStatus column of DataGridView2 for Unknown/Incorrect DataType
                    Dim datagridview = Globals.ThisAddIn.getControls_IncidentWatcher.DataGridView2

                    For Each row As DataGridViewRow In datagridview.Rows
                        sc_Sr.Add(row.Cells(0).Value)
                        sc_ColumnName.Add(row.Cells(1).Value)
                        sc_RequiredColumn.Add(row.Cells(2).Value)
                        sc_ColumnType.Add(row.Cells(3).Value)
                        sc_ColumnTypeExpected.Add(row.Cells(4).Value)
                        sc_ColumnStatus.Add(row.Cells(5).Value)
                        Dim check_status As String = row.Cells(5).Value
                        Dim required_column As String = row.Cells(2).Value
                        If required_column = "Yes" Then
                            If check_status <> "Valid" Then
                                row.DefaultCellStyle.BackColor = Color.LightGray
                                return_value = "Invalid"
                            End If
                        Else
                            row.DefaultCellStyle.BackColor = Color.LightGray
                        End If
                    Next
                    My.Settings.dt_Sr = sc_Sr
                    My.Settings.dt_ColumnName = sc_ColumnName
                    My.Settings.dt_RequiredColumn = sc_RequiredColumn
                    My.Settings.dt_ColumnType = sc_ColumnType
                    My.Settings.dt_ColumnTypeExpected = sc_ColumnTypeExpected
                    My.Settings.dt_ColumnStatus = sc_ColumnStatus
                    My.Settings.Save()
                End If

                If return_value = "Invalid" Then
                    MsgBox("Atleast one Required Column is Missing or Invalid", vbExclamation)
                End If
                If return_value = "Valid" Then
                    'filtered list based on user emailid
                    Dim queryfiltered As New CamlQuery()
                    queryfiltered.ViewXml = "<View><Query><Where><Eq><FieldRef Name=" + person_email_internalname + "/>" + "<Value Type='Text'>" + currentemail + "</Value></Eq></Where></Query></View>"
                    Dim listitemsfiltered As ListItemCollection = list.GetItems(queryfiltered)
                    ClientContext.Load(listitemsfiltered)

                    ClientContext.ExecuteQuery()
                    Dim listitem As ListItem

                    Dim sc_Project As New System.Collections.Specialized.StringCollection
                    Dim sc_EscalationLevel As New System.Collections.Specialized.StringCollection
                    Dim sc_PersonName As New System.Collections.Specialized.StringCollection
                    Dim sc_PersonEmail As New System.Collections.Specialized.StringCollection
                    Dim sc_EscalationPerson As New System.Collections.Specialized.StringCollection
                    Dim sc_EscalationPersonEmail As New System.Collections.Specialized.StringCollection
                    Dim sc_EscalationPersonPhone As New System.Collections.Specialized.StringCollection
                    Dim sc_IncidentLevel As New System.Collections.Specialized.StringCollection
                    Dim sc_EscalationStartTime As New System.Collections.Specialized.StringCollection
                    Dim sc_EscalationEndTime As New System.Collections.Specialized.StringCollection
                    Dim sc_ReminderBeep As New System.Collections.Specialized.StringCollection
                    Dim sc_ReminderCall As New System.Collections.Specialized.StringCollection


                    Dim field_Project As Field = list.Fields.GetByTitle("Project")
                    ClientContext.Load(field_Project)
                    ClientContext.ExecuteQuery()
                    Dim internalname_field_Project As String = field_Project.InternalName

                    Dim field_EscalationLevel As Field = list.Fields.GetByTitle("Escalation Level")
                    ClientContext.Load(field_EscalationLevel)
                    ClientContext.ExecuteQuery()
                    Dim internalname_field_EscalationLevel As String = field_EscalationLevel.InternalName

                    Dim field_PersonName As Field = list.Fields.GetByTitle("Person Name")
                    ClientContext.Load(field_PersonName)
                    ClientContext.ExecuteQuery()
                    Dim internalname_field_PersonName As String = field_PersonName.InternalName

                    Dim field_PersonEmail As Field = list.Fields.GetByTitle("Person Email")
                    ClientContext.Load(field_PersonEmail)
                    ClientContext.ExecuteQuery()
                    Dim internalname_field_PersonEmail As String = field_PersonEmail.InternalName

                    Dim field_EscalationPerson As Field = list.Fields.GetByTitle("Escalation Person")
                    ClientContext.Load(field_EscalationPerson)
                    ClientContext.ExecuteQuery()
                    Dim internalname_field_EscalationPerson As String = field_EscalationPerson.InternalName

                    Dim field_EscalationPersonEmail As Field = list.Fields.GetByTitle("Escalation Person Email")
                    ClientContext.Load(field_EscalationPersonEmail)
                    ClientContext.ExecuteQuery()
                    Dim internalname_field_EscalationPersonEmail As String = field_EscalationPersonEmail.InternalName

                    Dim field_EscalationPersonPhone As Field = list.Fields.GetByTitle("Escalation Person Phone")
                    ClientContext.Load(field_EscalationPersonPhone)
                    ClientContext.ExecuteQuery()
                    Dim internalname_field_EscalationPersonPhone As String = field_EscalationPersonPhone.InternalName

                    Dim field_IncidentLevel As Field = list.Fields.GetByTitle("Incident Level")
                    ClientContext.Load(field_IncidentLevel)
                    ClientContext.ExecuteQuery()
                    Dim internalname_field_IncidentLevel As String = field_IncidentLevel.InternalName

                    Dim field_EscalationStartTime As Field = list.Fields.GetByTitle("Escalation Start Time")
                    ClientContext.Load(field_EscalationStartTime)
                    ClientContext.ExecuteQuery()
                    Dim internalname_field_EscalationStartTime As String = field_EscalationStartTime.InternalName

                    Dim field_EscalationEndTime As Field = list.Fields.GetByTitle("Escalation End Time")
                    ClientContext.Load(field_EscalationEndTime)
                    ClientContext.ExecuteQuery()
                    Dim internalname_field_EscalationEndTime As String = field_EscalationEndTime.InternalName

                    Dim field_ReminderBeepAt As Field = list.Fields.GetByTitle("Reminder Beep At")
                    ClientContext.Load(field_ReminderBeepAt)
                    ClientContext.ExecuteQuery()
                    Dim internalname_field_ReminderBeepAt As String = field_ReminderBeepAt.InternalName

                    Dim field_ReminderCallAt As Field = list.Fields.GetByTitle("Reminder Call At")
                    ClientContext.Load(field_ReminderCallAt)
                    ClientContext.ExecuteQuery()
                    Dim internalname_field_ReminderCallAt As String = field_ReminderCallAt.InternalName

                    Dim countfiltered As Integer = listitemsfiltered.Count
                    If countfiltered < 1 Then
                        MsgBox("No Escalation Rules found for " & currentemail & " . Verify SharePoint List", vbExclamation)
                    End If

                    For Each listitem In listitemsfiltered

                        'lookedup internalname using field displayname

                        sc_Project.Add(listitem(internalname_field_Project))
                        'sc_EscalationLevel.Add(listitem("EscalationLevel"))
                        sc_EscalationLevel.Add(listitem(internalname_field_EscalationLevel))
                        'sc_PersonName.Add(CType(listitem("Person"), FieldUserValue).LookupValue)
                        sc_PersonName.Add(CType(listitem(internalname_field_PersonName), FieldUserValue).LookupValue)
                        'sc_PersonEmail.Add(CType(listitem("PersonEmail"), FieldUserValue).LookupValue)
                        sc_PersonEmail.Add(CType(listitem(internalname_field_PersonEmail), FieldUserValue).LookupValue)
                        'sc_EscalationPerson.Add(CType(listitem("EscalationPerson"), FieldUserValue).LookupValue)
                        sc_EscalationPerson.Add(CType(listitem(internalname_field_EscalationPerson), FieldUserValue).LookupValue)
                        'sc_EscalationPersonEmail.Add(CType(listitem("EscalationPersonEmail"), FieldUserValue).LookupValue)
                        sc_EscalationPersonEmail.Add(CType(listitem(internalname_field_EscalationPersonEmail), FieldUserValue).LookupValue)
                        'sc_EscalationPersonPhone.Add(listitem("Person_x0020_Phone"))
                        sc_EscalationPersonPhone.Add(listitem(internalname_field_EscalationPersonPhone))
                        'sc_IncidentLevel.Add(listitem("IncidentLevel"))
                        sc_IncidentLevel.Add(listitem(internalname_field_IncidentLevel))
                        'sc_EscalationStartTime.Add(listitem("Escalation_x0020_Start_x0020_Tim"))
                        sc_EscalationStartTime.Add(listitem(internalname_field_EscalationStartTime))
                        'sc_EscalationEndTime.Add(listitem("StartTime"))
                        sc_EscalationEndTime.Add(listitem(internalname_field_EscalationEndTime))
                        'sc_ReminderBeep.Add(listitem("Reminder_x0020_Beep_x002d_PopUp_"))
                        sc_ReminderBeep.Add(listitem(internalname_field_ReminderBeepAt))
                        'sc_ReminderCall.Add(listitem("Reminder_x0020_Call_x0020_At"))
                        sc_ReminderCall.Add(listitem(internalname_field_ReminderCallAt))

                    Next

                    My.Settings.dt_Project = sc_Project
                    My.Settings.dt_EscalationLevel = sc_EscalationLevel
                    My.Settings.dt_PersonName = sc_PersonName
                    My.Settings.dt_PersonEmail = sc_PersonEmail
                    My.Settings.dt_EscalationPerson = sc_EscalationPerson
                    My.Settings.dt_EscalationPersonEmail = sc_EscalationPersonEmail
                    My.Settings.dt_EscalationPersonPhone = sc_EscalationPersonPhone
                    My.Settings.dt_IncidentLevel = sc_IncidentLevel
                    My.Settings.dt_EscalationStartTime = sc_EscalationStartTime
                    My.Settings.dt_EscalationEndTime = sc_EscalationEndTime
                    My.Settings.dt_ReminderBeep = sc_ReminderBeep
                    My.Settings.dt_ReminderCall = sc_ReminderCall

                    My.Settings.Save()

                End If

            End If
        End Using
        Return return_value

    End Function


End Module

