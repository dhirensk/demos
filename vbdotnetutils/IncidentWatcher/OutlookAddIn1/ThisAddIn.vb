Imports System.Runtime.InteropServices
Imports Microsoft.SharePoint.Client
Imports System.Net
Imports System.Xml
Imports System.Threading
Imports System.Text.RegularExpressions
Imports System.Data
Imports System.Windows.Forms
Imports System.Collections.Specialized
Imports System.Threading.Tasks

Public Class ThisAddIn

    'Alert user of pending email notifications before the plugin is activated, to avoid sending alerts
    ' Make sure escalation level 1 has completed before firing escalation level 2 actions

    Dim outlookNameSpace As Outlook.NameSpace
    Dim inbox As Outlook.MAPIFolder
    Dim WithEvents items As Outlook.Items
    Private WithEvents controls_incidentwatcher As Controls_IncidentWatcher
    Private WithEvents taskpane_incidentwatcher As Microsoft.Office.Tools.CustomTaskPane
    Private thread As Thread
    Private alertdict As Dictionary(Of String, DateTime)
    Private calldict As Dictionary(Of String, StringCollection)





    Private Sub ThisAddIn_Startup() Handles Me.Startup

        'start the plugin in disabled state
        My.Settings.ToggleValue = False
        My.Settings.Save()

        'My.Settings.Reset()
        controls_incidentwatcher = New Controls_IncidentWatcher
        taskpane_incidentwatcher = Me.CustomTaskPanes.Add(controls_incidentwatcher, "Incident Watcher")
        taskpane_incidentwatcher.Width = 1200
        taskpane_incidentwatcher.Visible = False
        DataTableLoader.LoadDataGridView2()
        DataTableLoader.LoadDataGridView()

        LoadDefaults()
        'CSVLoader.Find_CSV(controls_incidentwatcher)
        ' SP_Authentication.Main()
        'SP_Authentication2.Main2()
        'DataTableLoader.DataTableLoader(controls_incidentwatcher)
        'TwilioCalls.OutGoingCall()
        ' MsgBox("ok", vbOKOnly)



        thead = New Thread(AddressOf ThreadTask)
        thead.IsBackground = True
        thead.Start()




        Dim AlertTask As New Task(AddressOf AlertAction)
        ' Run it synchronously
        AlertTask.RunSynchronously()


    End Sub

    Async Sub AlertAction()
        Do
            If alertdict IsNot Nothing Then
                ' MsgBoxThreadedOkOnly(alertdict.Count)
                If alertdict.Count > 0 Then

                    'create a list to perform enumeration, as enumeration and remove cannot be performed at same time on dict object 
                    For Each alert In alertdict.Keys.ToList
                        If alert.Contains("_beep") Then
                            Dim calculated_beeptime As Date
                            alertdict.TryGetValue(alert, calculated_beeptime)

                            If System.DateTime.Now > calculated_beeptime Then
                                MsgBoxThreadedAlert("You have unread incident emails")
                                controls_incidentwatcher.c_playsample.PerformClick()
                                alertdict.Remove(alert)
                            End If
                        End If

                        If alert.Contains("_call") Then
                            Dim calculated_calltime As Date
                            alertdict.TryGetValue(alert, calculated_calltime)

                            If System.DateTime.Now > calculated_calltime Then
                                Dim callitem As New StringCollection
                                calldict.TryGetValue(alert, callitem)
                                TwilioCalls.OutGoingCall(callitem)
                                alertdict.Remove(alert)
                                calldict.Remove(alert)
                            End If
                        End If


                    Next

                End If

            End If
            Await Task.Delay(10000)
        Loop
    End Sub



    Sub MsgBoxThreadedAlert(msg As String)
        Dim thread As New Threading.Thread(
            Sub()
                MsgBox(msg, vbExclamation, "Alert")
            End Sub)
        thread.Start()
    End Sub

    Sub MsgBoxThreadedError(msg As String)
        Dim thread As New Threading.Thread(
            Sub()
                MsgBox(msg, vbCritical, "Error")
            End Sub)
        thread.Start()
    End Sub

    Sub MsgBoxThreadedOkOnly(msg As String)
        Dim thread As New Threading.Thread(
            Sub()
                MsgBox(msg, vbOKOnly, "Note")
            End Sub)
        thread.Start()
    End Sub

    ' checks if any usersettings have been changed in UI but not saved by user
    ' if changes are detected, popsup a message to save. if cancelled reverts to old settings
    Private Sub taskpane_incidentwatcher_closed(sender As Object, e As EventArgs) Handles taskpane_incidentwatcher.VisibleChanged
        If taskpane_incidentwatcher.Visible = False Then

            Dim messagebuilder = New StringBuilder
            If Not String.IsNullOrWhiteSpace(controls_incidentwatcher.f_message_c_AccountSid) Then
                messagebuilder.Append(controls_incidentwatcher.f_message_c_AccountSid)
                messagebuilder.AppendLine()
            End If
            If Not String.IsNullOrWhiteSpace(controls_incidentwatcher.f_message_c_AuthToken) Then
                messagebuilder.Append(controls_incidentwatcher.f_message_c_AuthToken)
                messagebuilder.AppendLine()
            End If
            If Not String.IsNullOrWhiteSpace(controls_incidentwatcher.f_message_c_FromPhone) Then
                messagebuilder.Append(controls_incidentwatcher.f_message_c_FromPhone)
                messagebuilder.AppendLine()
            End If
            If Not String.IsNullOrWhiteSpace(controls_incidentwatcher.f_message_c_TwiMLBinVariable) Then
                messagebuilder.Append(controls_incidentwatcher.f_message_c_TwiMLBinVariable)
                messagebuilder.AppendLine()
            End If

            If Not String.IsNullOrWhiteSpace(controls_incidentwatcher.f_message_c_TwiMLBinURL) Then
                messagebuilder.Append(controls_incidentwatcher.f_message_c_TwiMLBinURL)
                messagebuilder.AppendLine()
            End If

            If Not String.IsNullOrWhiteSpace(controls_incidentwatcher.f_message_c_Loops) Then
                messagebuilder.Append(controls_incidentwatcher.f_message_c_Loops)
                messagebuilder.AppendLine()
            End If
            If Not String.IsNullOrWhiteSpace(controls_incidentwatcher.f_message_c_Volume) Then
                messagebuilder.Append(controls_incidentwatcher.f_message_c_Volume)
                messagebuilder.AppendLine()
            End If
            If Not String.IsNullOrWhiteSpace(controls_incidentwatcher.f_message_c_CustomSound) Then
                messagebuilder.Append(controls_incidentwatcher.f_message_c_CustomSound)
                messagebuilder.AppendLine()
            End If
            If Not String.IsNullOrWhiteSpace(controls_incidentwatcher.f_message_c_Subject_Regex_P1) Then
                messagebuilder.Append(controls_incidentwatcher.f_message_c_Subject_Regex_P1)
                messagebuilder.AppendLine()
            End If
            If Not String.IsNullOrWhiteSpace(controls_incidentwatcher.f_message_c_Subject_Regex_P2) Then
                messagebuilder.Append(controls_incidentwatcher.f_message_c_Subject_Regex_P2)
                messagebuilder.AppendLine()
            End If
            If Not String.IsNullOrWhiteSpace(controls_incidentwatcher.f_message_c_Subject_Regex_P3) Then
                messagebuilder.Append(controls_incidentwatcher.f_message_c_Subject_Regex_P4)
                messagebuilder.AppendLine()
            End If
            If Not String.IsNullOrWhiteSpace(controls_incidentwatcher.f_message_c_Subject_Regex_P4) Then
                messagebuilder.Append(controls_incidentwatcher.f_message_c_Subject_Regex_P4)
                messagebuilder.AppendLine()
            End If

            If Not String.IsNullOrWhiteSpace(controls_incidentwatcher.f_message_c_Body_Regex_P1) Then
                messagebuilder.Append(controls_incidentwatcher.f_message_c_Body_Regex_P1)
                messagebuilder.AppendLine()
            End If

            If Not String.IsNullOrWhiteSpace(controls_incidentwatcher.f_message_c_Body_Regex_P2) Then
                messagebuilder.Append(controls_incidentwatcher.f_message_c_Body_Regex_P2)
                messagebuilder.AppendLine()
            End If

            If Not String.IsNullOrWhiteSpace(controls_incidentwatcher.f_message_c_Body_Regex_P3) Then
                messagebuilder.Append(controls_incidentwatcher.f_message_c_Body_Regex_P3)
                messagebuilder.AppendLine()
            End If

            If Not String.IsNullOrWhiteSpace(controls_incidentwatcher.f_message_c_Body_Regex_P4) Then
                messagebuilder.Append(controls_incidentwatcher.f_message_c_Body_Regex_P4)
                messagebuilder.AppendLine()
            End If


            If Not String.IsNullOrWhiteSpace(messagebuilder.ToString) Then


                Dim result As Integer = MsgBox(messagebuilder.ToString + "Save?", vbYesNo, "Save Changes?")
                If result = MsgBoxResult.No Then
                    controls_incidentwatcher.f_message_c_AccountSid = ""
                    controls_incidentwatcher.f_message_c_AuthToken = ""
                    controls_incidentwatcher.f_message_c_FromPhone = ""
                    controls_incidentwatcher.f_message_c_TwiMLBinVariable = ""
                    controls_incidentwatcher.f_message_c_TwiMLBinURL = ""
                    controls_incidentwatcher.f_message_c_Loops = ""
                    controls_incidentwatcher.f_message_c_Volume = ""
                    controls_incidentwatcher.f_message_c_CustomSound = ""
                    controls_incidentwatcher.f_message_c_Subject_Regex_P1 = ""
                    controls_incidentwatcher.f_message_c_Subject_Regex_P2 = ""
                    controls_incidentwatcher.f_message_c_Subject_Regex_P3 = ""
                    controls_incidentwatcher.f_message_c_Subject_Regex_P4 = ""
                    controls_incidentwatcher.f_message_c_Body_Regex_P1 = ""
                    controls_incidentwatcher.f_message_c_Body_Regex_P2 = ""
                    controls_incidentwatcher.f_message_c_Body_Regex_P3 = ""
                    controls_incidentwatcher.f_message_c_Body_Regex_P4 = ""
                    LoadDefaults()
                ElseIf result = MsgBoxResult.Yes Then

                    'controls_incidentwatcher.c_SaveSettings.PerformClick()
                    'cannot perform click when CustomTaskPane is not visible
                    controls_incidentwatcher.c_SaveSettings_Click(Nothing, Nothing)

                End If
            End If
        End If

    End Sub

    Public ReadOnly Property getIncidentWatcher() As Microsoft.Office.Tools.CustomTaskPane
        Get
            Return taskpane_incidentwatcher
        End Get
    End Property

    Public ReadOnly Property getControls_IncidentWatcher() As Controls_IncidentWatcher
        Get
            Return controls_incidentwatcher
        End Get
    End Property

    Public ReadOnly Property getAlertDict() As Dictionary(Of String, DateTime)
        Get
            Return alertdict
        End Get
    End Property

    Public ReadOnly Property getCallDict() As Dictionary(Of String, StringCollection)
        Get
            Return calldict
        End Get
    End Property


    Private Sub Items_ItemAdd(ByVal item As Object) Handles items.ItemAdd
        Dim filter As String = "test test test"
        If TypeOf (item) Is Outlook.MailItem Then
            Dim mail As Outlook.MailItem = item
            If mail.MessageClass = "IPM.Note" And mail.Subject.ToUpper.Contains(filter.ToUpper) Then
                'mail.Move(outlookNameSpace.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderJunk))
                '  mail.LastModificationTime

            End If
        End If

    End Sub

    Public Sub LoadDefaults()
        '  Dim updatingUI As Boolean = True
        '  Dim pattern As String = "M-d-yyyy HH:mm:ss"

        controls_incidentwatcher.c_SiteURL.Text = My.Settings.s_siteURL
        controls_incidentwatcher.c_SPList.Text = My.Settings.s_SPList
        controls_incidentwatcher.c_AccountSid.Text = My.Settings.s_AccountSid
        controls_incidentwatcher.c_AuthToken.Text = My.Settings.s_AuthToken
        controls_incidentwatcher.c_FromPhone.Text = My.Settings.s_PhoneFrom
        controls_incidentwatcher.c_TwiMLBinVariable.Text = My.Settings.s_TwiMLBinVariable
        controls_incidentwatcher.c_TwiMLBinURL.Text = My.Settings.s_TwiMLBinURL
        controls_incidentwatcher.c_Subject_Regex_P1.Text = My.Settings.s_Subject_Regex_P1
        controls_incidentwatcher.c_Subject_Regex_P2.Text = My.Settings.s_Subject_Regex_P2
        controls_incidentwatcher.c_Subject_Regex_P3.Text = My.Settings.s_Subject_Regex_P3
        controls_incidentwatcher.c_Subject_Regex_P4.Text = My.Settings.s_Subject_Regex_P4
        controls_incidentwatcher.c_Body_Regex_P1.Text = My.Settings.s_Body_Regex_P1
        controls_incidentwatcher.c_Body_Regex_P2.Text = My.Settings.s_Body_Regex_P2
        controls_incidentwatcher.c_Body_Regex_P3.Text = My.Settings.s_Body_Regex_P3
        controls_incidentwatcher.c_Body_Regex_P4.Text = My.Settings.s_Body_Regex_P4
        controls_incidentwatcher.c_CustomSound.Text = My.Settings.s_CustomSound
        controls_incidentwatcher.c_Loops.SelectedItem = My.Settings.s_Loops
        controls_incidentwatcher.c_Volume.SelectedItem = My.Settings.s_Volume
        controls_incidentwatcher.c_SiteURL.Text = My.Settings.s_siteURL
        controls_incidentwatcher.c_SPList.Text = My.Settings.s_SPList
        controls_incidentwatcher.c_EmailID.Text = My.Settings.s_EmailID

    End Sub

    Private Sub ThisAddIn_Shutdown() Handles Me.Shutdown

    End Sub

    Private Sub ThreadTask()
        alertdict = New Dictionary(Of String, DateTime)
        calldict = New Dictionary(Of String, StringCollection)
        Do
            'MsgBox("inside do", vbOKOnly)
            Dim P1_Subject_Regex As Regex = New Regex(My.Settings.s_Subject_Regex_P1)
            Dim filter_body As String = "P1"
            Dim pattern As String = "M-d-yyyy HH:mm:ss"

            Dim Subject_Regex_P1 = New Regex(My.Settings.s_Subject_Regex_P1)
            Dim Subject_Regex_P2 = New Regex(My.Settings.s_Subject_Regex_P2)
            Dim Subject_Regex_P3 = New Regex(My.Settings.s_Subject_Regex_P3)
            Dim Subject_Regex_P4 = New Regex(My.Settings.s_Subject_Regex_P4)

            Dim Body_Regex_P1 = New Regex(My.Settings.s_Body_Regex_P1)
            Dim Body_Regex_P2 = New Regex(My.Settings.s_Body_Regex_P2)
            Dim Body_Regex_P3 = New Regex(My.Settings.s_Body_Regex_P3)
            Dim Body_Regex_P4 = New Regex(My.Settings.s_Body_Regex_P4)

            Dim datagridview = controls_incidentwatcher.DataGridView1

            'MsgBox(rows & cols, vbOKOnly)
            Dim Now As Integer = CInt(System.DateTime.Now.ToString("HHmm"))




            If My.Settings.ToggleValue Then
                ' MsgBox("inside timecheck", vbOKOnly)
                Dim outlookNameSpace = Me.Application.GetNamespace("MAPI")
                Dim inbox = outlookNameSpace.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox)
                Dim items = inbox.Items.Restrict("[unread]=true")

                Dim escalation_messages As New StringBuilder
                'for loop should append to the escalation count and message if escalations detected
                For i = 1 To items.Count
                    Dim item = items.Item(i)
                    If TypeOf (item) Is Outlook.MailItem Then
                        Dim mail As Outlook.MailItem = item
                        Dim incidentcategory As String
                        If mail.MessageClass = "IPM.Note" Then
                            'mail.Move(outlookNameSpace.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderJunk))
                            '  mail.LastModificationTime
                            Dim mail_subject = mail.Subject.ToString
                            Dim mail_body = mail.Body
                            'check for email matching P1/P2/P3/P4

                            Dim Subject_match_P1 As Match = Subject_Regex_P1.Match(mail_subject)
                            Dim Subject_match_P2 As Match = Subject_Regex_P2.Match(mail_subject)
                            Dim Subject_match_P3 As Match = Subject_Regex_P3.Match(mail_subject)
                            Dim Subject_match_P4 As Match = Subject_Regex_P4.Match(mail_subject)

                            Dim Body_match_P1 As Match = Body_Regex_P1.Match(mail_body)
                            Dim Body_match_P2 As Match = Body_Regex_P2.Match(mail_body)
                            Dim Body_match_P3 As Match = Body_Regex_P3.Match(mail_body)
                            Dim Body_match_P4 As Match = Body_Regex_P4.Match(mail_body)

                            ' gives 1 if regex match
                            'gives 1 if subject present
                            Dim weight_P1 As String =
                            Convert.ToInt32(Subject_match_P1.Success) &
                            Convert.ToInt32(Not String.IsNullOrWhiteSpace(My.Settings.s_Subject_Regex_P1)) &
                            Convert.ToInt32(Body_match_P1.Success) &
                            Convert.ToInt32(Not String.IsNullOrWhiteSpace(My.Settings.s_Body_Regex_P1))

                            Dim weight_P2 As String =
                            Convert.ToInt32(Subject_match_P2.Success) &
                            Convert.ToInt32(Not String.IsNullOrWhiteSpace(My.Settings.s_Subject_Regex_P2)) &
                            Convert.ToInt32(Body_match_P2.Success) &
                            Convert.ToInt32(Not String.IsNullOrWhiteSpace(My.Settings.s_Body_Regex_P2))

                            Dim weight_P3 As String =
                            Convert.ToInt32(Subject_match_P3.Success) &
                            Convert.ToInt32(Not String.IsNullOrWhiteSpace(My.Settings.s_Subject_Regex_P3)) &
                            Convert.ToInt32(Body_match_P3.Success) &
                            Convert.ToInt32(Not String.IsNullOrWhiteSpace(My.Settings.s_Body_Regex_P3))

                            Dim weight_P4 As String =
                            Convert.ToInt32(Subject_match_P4.Success) &
                            Convert.ToInt32(Not String.IsNullOrWhiteSpace(My.Settings.s_Subject_Regex_P4)) &
                            Convert.ToInt32(Body_match_P4.Success) &
                            Convert.ToInt32(Not String.IsNullOrWhiteSpace(My.Settings.s_Body_Regex_P4))


                            Dim incident_dict As New Dictionary(Of String, Integer)
                            incident_dict.Add("1", weight_P1)
                            incident_dict.Add("2", weight_P2)
                            incident_dict.Add("3", weight_P3)
                            incident_dict.Add("4", weight_P4)
                            'return key of max weight
                            incidentcategory = incident_dict.FirstOrDefault(Function(x) x.Value = incident_dict.Values.Max).Key
                            Dim incidentweight As Integer
                            incident_dict.TryGetValue(incidentcategory, incidentweight)

                            'atleast one of the regex or both should be present and regex should always match
                            If incidentweight = 1111 OrElse 1110 OrElse 1011 Then
                                'Process the email further else ignore

                                Dim lastmodtime = mail.LastModificationTime

                                Dim incidentcategorycolumn = 7 '0 based index
                                Dim escalationlevelcolumn = 1
                                Dim email_deliverytime = mail.ReceivedTime
                                ' to avoid capturing unread activity if very old incident is manually unread
                                Dim email_modified_minutes = (lastmodtime - email_deliverytime).TotalMinutes
                                For Each row As DataGridViewRow In datagridview.Rows
                                    ' last good state is set to system time after plugin is started

                                    ' MsgBox("last good state" & My.Settings.s_LastGoodState, vbOKOnly)
                                    'MsgBox("email delivery time" & email_deliverytime, vbOKOnly)
                                    'MsgBox("email modified minutes" & email_modified_minutes, vbOKOnly)

                                    ' check if
                                    ' If email_deliverytime >=  email_modified_minutes < 2 Then


                                    If email_modified_minutes < 2 Then

                                        Dim cell_incidentlevel = row.Cells("IncidentLevel").Value
                                        If Not IsDBNull(cell_incidentlevel) Then
                                            If cell_incidentlevel = incidentcategory Then
                                                ' incidentrow escalation rule found. should be only 1 row in datagrid per escalation level
                                                '  MsgBox("incident matches the rule")
                                                ' Process Beep Event
                                                Dim cell_project = row.Cells("Project").Value
                                                Dim cell_person = row.Cells("PersonName").Value
                                                Dim cell_personemail = row.Cells("PersonEmail").Value
                                                Dim cell_escalationperson = row.Cells("EscalationPerson").Value
                                                Dim cell_escalationpersonphone = row.Cells("EscalationPersonPhone").Value
                                                Dim cell_escalationpersonemail = row.Cells("EscalationPersonEmail").Value
                                                Dim cell_escalationlevel = row.Cells("EscalationLevel").Value
                                                Dim cell_startminutes = row.Cells("EscalationStartTime").Value
                                                Dim cell_endminutes = row.Cells("EscalationEndTime").Value
                                                Dim cell_beepminutes = row.Cells("ReminderBeep").Value
                                                Dim cell_callatminutes = row.Cells("ReminderCall").Value

                                                'check if beep event, starttime & endtime present in datagrid. if not ignore the event processing
                                                If Not (IsDBNull(cell_startminutes) Or IsDBNull(cell_endminutes) Or IsDBNull(cell_beepminutes)) Then

                                                    Dim beepminutes = CDbl(cell_beepminutes)
                                                    Dim startminutes = CDbl(cell_startminutes)
                                                    Dim endminutes = CDbl(cell_endminutes)
                                                    '  MsgBox("row " & row.Index & "beepminutes " & beepminutes, vbOKOnly)
                                                    Dim currenttime = DateTime.Now
                                                    Dim calculated_starttime = email_deliverytime.AddMinutes(startminutes)
                                                    Dim calculated_endtime = email_deliverytime.AddMinutes(endminutes)
                                                    Dim calculated_beeptime = calculated_starttime.AddMinutes(beepminutes)

                                                    If beepminutes > 0 Then
                                                        Do
                                                            If calculated_beeptime >= currenttime Then
                                                                Exit Do
                                                            End If
                                                            calculated_beeptime = calculated_beeptime.AddMinutes(beepminutes)

                                                        Loop
                                                    End If
                                                    'MsgBox("currenttime " & currenttime.ToString("HHmm"), vbOKOnly)
                                                    'MsgBox(" calculated_starttime " & calculated_starttime, vbOKOnly)
                                                    'MsgBox(calculated_endtime & " calculated_endtime", vbOKOnly)
                                                    'MsgBox("calculated_beeptime" & calculated_beeptime.ToString("HHmm"), vbOKOnly)

                                                    'Add Beep event to dictionary
                                                    If currenttime >= calculated_starttime And currenttime < calculated_endtime And beepminutes > 0 Then
                                                        If Not alertdict.ContainsKey(mail.EntryID & "_beep") Then

                                                            alertdict.Add(mail.EntryID & "_beep", calculated_beeptime)
                                                            ' MsgBoxThreadedOkOnly("calculated_beeptime " & calculated_beeptime)
                                                        End If
                                                    End If
                                                End If


                                                'Process Call Event
                                                'check if call event, startime & endtime present in datagrid. if not ignore the process

                                                If Not (IsDBNull(cell_startminutes) Or IsDBNull(cell_endminutes) Or IsDBNull(cell_callatminutes)) Then
                                                    Dim callatminute = CDbl(cell_callatminutes)
                                                    Dim startminutes = CDbl(cell_startminutes)
                                                    Dim endminutes = CDbl(cell_endminutes)
                                                    Dim currenttime = DateTime.Now
                                                    Dim calculated_starttime = email_deliverytime.AddMinutes(startminutes)
                                                    Dim calculated_endtime = email_deliverytime.AddMinutes(endminutes)
                                                    Dim calculated_calltime = email_deliverytime.AddMinutes(callatminute)

                                                    If currenttime < calculated_endtime And
                                                    calculated_calltime > currenttime And callatminute > 0 Then
                                                        Dim key = mail.EntryID & row.Index & "_call"
                                                        If Not alertdict.ContainsKey(key) Then
                                                            alertdict.Add(key, calculated_calltime)
                                                            '  MsgBox("calculated_calltime start " & calculated_calltime &
                                                            '      "calculated start time " & calculated_starttime &
                                                            '      "calculated end time" & calculated_endtime, vbOKOnly)
                                                        End If
                                                        If Not calldict.ContainsKey(key) Then
                                                            Dim callitem As New StringCollection
                                                            '0 incident level
                                                            callitem.Add(cell_incidentlevel)
                                                            '1 project
                                                            If Not IsDBNull(cell_project) Then
                                                                callitem.Add(cell_project)
                                                            Else
                                                                callitem.Add("Project name missing")
                                                            End If
                                                            '2 escalation level
                                                            If Not IsDBNull(cell_escalationlevel) Then
                                                                callitem.Add(cell_escalationlevel)
                                                            Else
                                                                callitem.Add("Escalation Level missing")
                                                            End If
                                                            '3  person name
                                                            If Not IsDBNull(cell_person) Then
                                                                callitem.Add(cell_person)
                                                            Else
                                                                callitem.Add("Person Name missing")
                                                            End If
                                                            '4 person email
                                                            If Not IsDBNull(cell_personemail) Then
                                                                callitem.Add(cell_personemail)
                                                            Else
                                                                callitem.Add("Person Email missing")
                                                            End If
                                                            '5 escalation person
                                                            If Not IsDBNull(cell_escalationperson) Then
                                                                callitem.Add(cell_escalationperson)
                                                            Else
                                                                callitem.Add("Escalation Person name missing")
                                                            End If
                                                            '6 escalation person email
                                                            If Not IsDBNull(cell_escalationpersonemail) Then
                                                                callitem.Add(cell_escalationpersonemail)
                                                            Else
                                                                callitem.Add("Escalation person email missing")
                                                            End If
                                                            '7 escalation person phone
                                                            If Not IsDBNull(cell_escalationpersonphone) Then
                                                                callitem.Add(cell_escalationpersonphone)
                                                            Else
                                                                callitem.Add("Escalation Person Phone missing")
                                                            End If
                                                            '8 email subject
                                                            callitem.Add(mail_subject)
                                                            '9 email body
                                                            callitem.Add(mail_body)
                                                            '10 email received date
                                                            callitem.Add(mail.ReceivedTime.ToString)


                                                            calldict.Add(key, callitem)
                                                            '  MsgBoxThreadedOkOnly("calculated_calltime & Delivery Time " & calculated_calltime & " " & email_deliverytime)
                                                        End If
                                                    End If
                                                End If

                                            End If
                                        End If
                                    End If

                                Next


                                'Else MsgBox("nicetry", vbOKOnly)

                            End If
                        End If

                    End If
                Next

            End If
            Thread.Sleep(15000)
        Loop
    End Sub

End Class
