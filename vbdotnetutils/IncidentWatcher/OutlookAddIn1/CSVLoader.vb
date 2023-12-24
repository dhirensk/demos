Imports System.Data
Imports System.IO
Imports System.Reflection
Imports System.Windows.Forms
Imports Microsoft.Office.Tools


Module CSVLoader
    Sub Find_CSV(ByRef controls_incidentwatcher As Controls_IncidentWatcher)
        ' https://support.microsoft.com/en-in/help/319291/how-to-embed-and-access-resources-by-using-visual-basic-net-or-visual

        Dim _textStreamReader As StreamReader = Nothing
        Dim _assembly As [Assembly]
        Try
            _assembly = [Assembly].GetExecutingAssembly()
            _textStreamReader = New StreamReader(_assembly.GetManifestResourceStream("OutlookAddIn1.excalationconfig.csv"))
            Try
                If _textStreamReader.Peek() <> -1 Then
                    '  MessageBox.Show(_textStreamReader.ReadLine())
                    Load_CSV(_textStreamReader, controls_incidentwatcher)
                End If
            Catch ex As Exception
                MessageBox.Show("Error reading stream!")
            End Try
        Catch ex As Exception
            MessageBox.Show("Resource wasn't found!", "Error")
        End Try



    End Sub

    Private Sub Load_CSV(ByRef _textStreamReader As StreamReader, ByRef controls_incidentwatcher As Controls_IncidentWatcher)
        Dim dt As DataTable = New DataTable

        dt.Columns.Add("EscalationLevel", GetType(String))
        dt.Columns.Add("PersonName", GetType(String))
        dt.Columns.Add("Email", GetType(String))
        dt.Columns.Add("Phone", GetType(String))
        dt.Columns.Add("StartTime", GetType(String))
        dt.Columns.Add("EndTime", GetType(String))
        dt.Columns.Add("ReminderBeep", GetType(String))
        dt.Columns.Add("ReminderCall", GetType(String))

        Dim index = 0
        Do While _textStreamReader.Peek() <> -1
            Dim TextLine As String = Nothing
            If index > 0 Then
                TextLine = _textStreamReader.ReadLine()
                Dim SplitLine = Split(TextLine, ",")
                dt.Rows.Add(SplitLine)
            Else
                TextLine = _textStreamReader.ReadLine()
            End If
            index = index + 1
        Loop

        controls_incidentwatcher.DataGridView1.AutoGenerateColumns = False
        'above property is not exposed in designer
        controls_incidentwatcher.DataGridView1.DataSource = dt

    End Sub




End Module
