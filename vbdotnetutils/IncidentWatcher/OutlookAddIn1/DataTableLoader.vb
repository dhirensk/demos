Imports System.Data
Imports System.IO
Imports System.Reflection
Imports System.Windows.Forms
Imports Microsoft.Office.Tools


Module DataTableLoader

    Public Sub LoadDataGridView()
        Dim dt As DataTable = ConvertStringCollectionsToDataTable()
        Globals.ThisAddIn.getControls_IncidentWatcher.DataGridView1.DataSource = dt

    End Sub


    Private Function ConvertStringCollectionsToDataTable() As DataTable
        Dim dt As New DataTable()
        dt.Columns.Add("Project", GetType(String))
        dt.Columns.Add("EscalationLevel", GetType(String))
        dt.Columns.Add("PersonName", GetType(String))
        dt.Columns.Add("PersonEmail", GetType(String))
        dt.Columns.Add("EscalationPerson", GetType(String))
        dt.Columns.Add("EscalationPersonEmail", GetType(String))
        dt.Columns.Add("EscalationPersonPhone", GetType(String))
        dt.Columns.Add("IncidentLevel", GetType(String))
        dt.Columns.Add("EscalationStartTime", GetType(String))
        dt.Columns.Add("EscalationEndTime", GetType(String))
        dt.Columns.Add("ReminderBeep", GetType(String))
        dt.Columns.Add("ReminderCall", GetType(String))
        dt.Rows.Clear()

        If My.Settings.dt_Project IsNot Nothing Then
            Dim num_rows = My.Settings.dt_Project.Count
            'initialize an object array of size 12
            Dim row(11) As Object
            For i = 0 To num_rows - 1

                row(0) = My.Settings.dt_Project.Item(i)
                row(1) = My.Settings.dt_EscalationLevel.Item(i)
                row(2) = My.Settings.dt_PersonName.Item(i)
                row(3) = My.Settings.dt_PersonEmail.Item(i)
                row(4) = My.Settings.dt_EscalationPerson.Item(i)
                row(5) = My.Settings.dt_EscalationPersonEmail.Item(i)
                row(6) = My.Settings.dt_EscalationPersonPhone.Item(i)
                row(7) = My.Settings.dt_IncidentLevel.Item(i)
                row(8) = My.Settings.dt_EscalationStartTime.Item(i)
                row(9) = My.Settings.dt_EscalationEndTime.Item(i)
                row(10) = My.Settings.dt_ReminderBeep.Item(i)
                row(11) = My.Settings.dt_ReminderCall.Item(i)

                dt.Rows.Add(row)
            Next
        End If

        Return dt
    End Function

    Public Sub LoadDataGridView2()
        Dim dt As DataTable = New DataTable

        dt.Columns.Add("Sr", GetType(Integer))
        dt.Columns.Add("ColumnName", GetType(String))
        dt.Columns.Add("RequiredColumn", GetType(String))
        dt.Columns.Add("ColumnType", GetType(String))
        dt.Columns.Add("ColumnTypeExpected", GetType(String))
        dt.Columns.Add("ColumnStatus", GetType(String))

        If My.Settings.dt_Sr IsNot Nothing Then
            Dim num_rows = My.Settings.dt_Sr.Count
            'initialize an object array of size 12
            Dim row(5) As Object
            For i = 0 To num_rows - 1

                row(0) = CInt(My.Settings.dt_Sr.Item(i))
                row(1) = My.Settings.dt_ColumnName.Item(i)
                row(2) = My.Settings.dt_RequiredColumn.Item(i)
                row(3) = My.Settings.dt_ColumnType.Item(i)
                row(4) = My.Settings.dt_ColumnTypeExpected.Item(i)
                row(5) = My.Settings.dt_ColumnStatus.Item(i)

                dt.Rows.Add(row)
            Next
        Else
            ' Add default columns if none exist in mysettings
            Dim row1 = dt.NewRow
            row1("Sr") = 1
            row1("ColumnName") = "Project"
            row1("RequiredColumn") = "Yes"
            row1("ColumnType") = "Unknown"
            row1("ColumnTypeExpected") = "Text"
            row1("ColumnStatus") = "Missing"
            dt.Rows.Add(row1)

            Dim row2 = dt.NewRow
            row2("Sr") = 2
            row2("ColumnName") = "Escalation Level"
            row2("RequiredColumn") = "Yes"
            row2("ColumnType") = "Unknown"
            row2("ColumnTypeExpected") = "Number"
            row2("ColumnStatus") = "Missing"
            dt.Rows.Add(row2)

            Dim row3 = dt.NewRow
            row3("Sr") = 3
            row3("ColumnName") = "Person Name"
            row3("RequiredColumn") = "Yes"
            row3("ColumnType") = "Unknown"
            row3("ColumnTypeExpected") = "User"
            row3("ColumnStatus") = "Missing"
            dt.Rows.Add(row3)

            Dim row4 = dt.NewRow
            row4("Sr") = 4
            row4("ColumnName") = "Person Email"
            row4("RequiredColumn") = "Yes"
            row4("ColumnType") = "Unknown"
            row4("ColumnTypeExpected") = "User"
            row4("ColumnStatus") = "Missing"
            dt.Rows.Add(row4)

            Dim row5 = dt.NewRow
            row5("Sr") = 5
            row5("ColumnName") = "Escalation Person"
            row5("RequiredColumn") = "Yes"
            row5("ColumnType") = "Unknown"
            row5("ColumnTypeExpected") = "User"
            row5("ColumnStatus") = "Missing"
            dt.Rows.Add(row5)

            Dim row6 = dt.NewRow
            row6("Sr") = 6
            row6("ColumnName") = "Escalation Person Email"
            row6("RequiredColumn") = "Yes"
            row6("ColumnType") = "Unknown"
            row6("ColumnTypeExpected") = "User"
            row6("ColumnStatus") = "Missing"
            dt.Rows.Add(row6)

            Dim row7 = dt.NewRow
            row7("Sr") = 7
            row7("ColumnName") = "Escalation Person Phone"
            row7("RequiredColumn") = "Yes"
            row7("ColumnType") = "Unknown"
            row7("ColumnTypeExpected") = "User"
            row7("ColumnStatus") = "Missing"
            dt.Rows.Add(row7)

            Dim row8 = dt.NewRow
            row8("Sr") = 8
            row8("ColumnName") = "Incident Level"
            row8("RequiredColumn") = "Yes"
            row8("ColumnType") = "Unknown"
            row8("ColumnTypeExpected") = "User"
            row8("ColumnStatus") = "Missing"
            dt.Rows.Add(row8)

            Dim row9 = dt.NewRow
            row9("Sr") = 9
            row9("ColumnName") = "Escalation Start Time"
            row9("RequiredColumn") = "Yes"
            row9("ColumnType") = "Unknown"
            row9("ColumnTypeExpected") = "Number"
            row9("ColumnStatus") = "Missing"
            dt.Rows.Add(row9)

            Dim row10 = dt.NewRow
            row10("Sr") = 10
            row10("ColumnName") = "Escalation End Time"
            row10("RequiredColumn") = "Yes"
            row10("ColumnType") = "Unknown"
            row10("ColumnTypeExpected") = "Number"
            row10("ColumnStatus") = "Missing"
            dt.Rows.Add(row10)

            Dim row11 = dt.NewRow
            row11("Sr") = 11
            row11("ColumnName") = "Reminder Beep At"
            row11("RequiredColumn") = "Yes"
            row11("ColumnType") = "Unknown"
            row11("ColumnTypeExpected") = "Number"
            row11("ColumnStatus") = "Missing"
            dt.Rows.Add(row11)

            Dim row12 = dt.NewRow
            row12("Sr") = 12
            row12("ColumnName") = "Reminder Call At"
            row12("RequiredColumn") = "Yes"
            row12("ColumnType") = "Unknown"
            row12("ColumnTypeExpected") = "Number"
            row12("ColumnStatus") = "Missing"
            dt.Rows.Add(row12)
        End If
        Globals.ThisAddIn.getControls_IncidentWatcher.DataGridView2.DataSource = dt

    End Sub
End Module
