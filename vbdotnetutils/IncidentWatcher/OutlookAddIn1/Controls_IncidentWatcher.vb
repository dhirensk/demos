Imports System.Diagnostics
Imports System.IO
Imports System.ComponentModel


Public Class Controls_IncidentWatcher
    Private message_c_AccountSid As String
    Private message_c_AuthToken As String
    Private message_c_FromPhone As String
    Private message_c_TwiMLBinVariable As String
    Private message_c_TwiMLBinURL As String
    Private message_c_Loops As String
    Private message_c_Volume As String
    Private message_c_CustomSound As String
    Private message_c_Subject_Regex_P1 As String
    Private message_c_Subject_Regex_P2 As String
    Private message_c_Subject_Regex_P3 As String
    Private message_c_Subject_Regex_P4 As String
    Private message_c_Body_Regex_P1 As String
    Private message_c_Body_Regex_P2 As String
    Private message_c_Body_Regex_P3 As String
    Private message_c_Body_Regex_P4 As String


    Private Sub c_playsample_Click(sender As Object, e As EventArgs) Handles c_playsample.Click
        SoundAndAlerts.SetVolume(c_Volume.SelectedItem.ToString())

        SoundAndAlerts.PlayAlert(c_Loops.SelectedItem.ToString(), c_CustomSound.Text)
    End Sub

    Private Sub c_showpopup_Click(sender As Object, e As EventArgs) Handles c_showpopup.Click
        ' MsgBox("You have unread Incident Emails", vbExclamation, "Alert")
        Globals.ThisAddIn.MsgBoxThreadedAlert("You have unread incident emails")
    End Sub

    Private Sub c_SelectWav_Click(sender As Object, e As EventArgs) Handles c_SelectWav.Click
        If OpenFileDialog1.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            c_CustomSound.Text = OpenFileDialog1.FileName
            If c_CustomSound.Text <> My.Settings.s_CustomSound Then
                message_c_CustomSound = "Custom Sound field Added"

            End If
        End If
    End Sub
    Public Property f_message_c_AccountSid As String
        Get
            Return message_c_AccountSid
        End Get

        Set(value As String)
            message_c_AccountSid = value
        End Set
    End Property

    Public Property f_message_c_AuthToken As String
        Get
            Return message_c_AuthToken
        End Get

        Set(value As String)
            message_c_AuthToken = value
        End Set
    End Property

    Public Property f_message_c_FromPhone As String
        Get
            Return message_c_FromPhone
        End Get

        Set(value As String)
            message_c_FromPhone = value
        End Set
    End Property


    Public Property f_message_c_TwiMLBinVariable As String
        Get
            Return message_c_TwiMLBinVariable
        End Get

        Set(value As String)
            message_c_TwiMLBinVariable = value
        End Set
    End Property

    Public Property f_message_c_TwiMLBinURL As String
        Get
            Return message_c_TwiMLBinURL
        End Get

        Set(value As String)
            message_c_TwiMLBinURL = value
        End Set
    End Property


    Public Property f_message_c_Loops As String
        Get
            Return message_c_Loops
        End Get

        Set(value As String)
            message_c_Loops = value
        End Set
    End Property
    Public Property f_message_c_Volume As String
        Get
            Return message_c_Volume
        End Get

        Set(value As String)
            message_c_Volume = value
        End Set
    End Property

    Public Property f_message_c_CustomSound As String
        Get
            Return message_c_CustomSound
        End Get

        Set(value As String)
            message_c_CustomSound = value
        End Set
    End Property

    Public Property f_message_c_Subject_Regex_P1 As String
        Get
            Return message_c_Subject_Regex_P1
        End Get

        Set(value As String)
            message_c_Subject_Regex_P1 = value
        End Set
    End Property

    Public Property f_message_c_Subject_Regex_P2 As String
        Get
            Return message_c_Subject_Regex_P2
        End Get

        Set(value As String)
            message_c_Subject_Regex_P2 = value
        End Set
    End Property

    Public Property f_message_c_Subject_Regex_P3 As String
        Get
            Return message_c_Subject_Regex_P3
        End Get

        Set(value As String)
            message_c_Subject_Regex_P3 = value
        End Set
    End Property

    Public Property f_message_c_Subject_Regex_P4 As String
        Get
            Return message_c_Subject_Regex_P4
        End Get

        Set(value As String)
            message_c_Subject_Regex_P4 = value
        End Set
    End Property

    Public Property f_message_c_Body_Regex_P1 As String
        Get
            Return message_c_Body_Regex_P1
        End Get

        Set(value As String)
            message_c_Body_Regex_P1 = value
        End Set
    End Property

    Public Property f_message_c_Body_Regex_P2 As String
        Get
            Return message_c_Body_Regex_P2
        End Get

        Set(value As String)
            message_c_Body_Regex_P2 = value
        End Set
    End Property

    Public Property f_message_c_Body_Regex_P3 As String
        Get
            Return message_c_Body_Regex_P3
        End Get

        Set(value As String)
            message_c_Body_Regex_P3 = value
        End Set
    End Property

    Public Property f_message_c_Body_Regex_P4 As String
        Get
            Return message_c_Body_Regex_P4
        End Get

        Set(value As String)
            message_c_Body_Regex_P4 = value
        End Set
    End Property




    Public Sub c_SaveSettings_Click(sender As Object, e As EventArgs) Handles c_SaveSettings.Click
        My.Settings.s_AccountSid = c_AccountSid.Text
        My.Settings.s_AuthToken = c_AuthToken.Text
        My.Settings.s_PhoneFrom = c_FromPhone.Text

        My.Settings.s_siteURL = c_SiteURL.Text
        My.Settings.s_SPList = c_SPList.Text
        My.Settings.s_EmailID = c_EmailID.Text
        My.Settings.s_TwiMLBinVariable = c_TwiMLBinVariable.Text
        My.Settings.s_TwiMLBinURL = c_TwiMLBinURL.Text
        My.Settings.s_Loops = c_Loops.SelectedItem.ToString
        My.Settings.s_Volume = c_Volume.SelectedItem.ToString
        My.Settings.s_CustomSound = c_CustomSound.Text
        My.Settings.s_Subject_Regex_P1 = c_Subject_Regex_P1.Text
        My.Settings.s_Subject_Regex_P2 = c_Subject_Regex_P2.Text
        My.Settings.s_Subject_Regex_P3 = c_Subject_Regex_P3.Text
        My.Settings.s_Subject_Regex_P4 = c_Subject_Regex_P4.Text
        My.Settings.s_Body_Regex_P1 = c_Body_Regex_P1.Text
        My.Settings.s_Body_Regex_P2 = c_Body_Regex_P2.Text
        My.Settings.s_Body_Regex_P3 = c_Body_Regex_P3.Text
        My.Settings.s_Body_Regex_P4 = c_Body_Regex_P4.Text


        My.Settings.Save()

        message_c_AccountSid = ""
        message_c_AuthToken = ""
        message_c_FromPhone = ""
        message_c_TwiMLBinVariable = ""
        message_c_TwiMLBinURL = ""
        message_c_Loops = ""
        message_c_Volume = ""
        message_c_CustomSound = ""
        message_c_Subject_Regex_P1 = ""
        message_c_Subject_Regex_P2 = ""
        message_c_Subject_Regex_P3 = ""
        message_c_Subject_Regex_P4 = ""
        message_c_Body_Regex_P1 = ""
        message_c_Body_Regex_P2 = ""
        message_c_Body_Regex_P3 = ""
        message_c_Body_Regex_P4 = ""

        Globals.ThisAddIn.LoadDefaults()
        MsgBox("Settings Saved Successfully", vbOKOnly)
        'TODO interrupt running thread and relaunch

    End Sub

    Private Sub c_CleanSettings_Click(sender As Object, e As EventArgs) Handles c_CleanSettings.Click
        My.Settings.dt_Project = Nothing
        My.Settings.dt_EscalationLevel = Nothing
        My.Settings.dt_PersonName = Nothing
        My.Settings.dt_PersonEmail = Nothing
        My.Settings.dt_EscalationPerson = Nothing
        My.Settings.dt_EscalationPersonEmail = Nothing
        My.Settings.dt_EscalationPersonPhone = Nothing
        My.Settings.dt_IncidentLevel = Nothing
        My.Settings.dt_EscalationStartTime = Nothing
        My.Settings.dt_EscalationEndTime = Nothing
        My.Settings.dt_ReminderBeep = Nothing
        My.Settings.dt_ReminderCall = Nothing

        My.Settings.dt_Sr = Nothing
        My.Settings.dt_ColumnName = Nothing
        My.Settings.dt_RequiredColumn = Nothing
        My.Settings.dt_ColumnType = Nothing
        My.Settings.dt_ColumnTypeExpected = Nothing
        My.Settings.dt_ColumnStatus = Nothing

        DataTableLoader.LoadDataGridView()
        DataTableLoader.LoadDataGridView2()
        message_c_AccountSid = ""
        message_c_AuthToken = ""
        message_c_FromPhone = ""
        message_c_TwiMLBinVariable = ""
        message_c_TwiMLBinURL = ""
        message_c_Loops = ""
        message_c_Volume = ""
        message_c_CustomSound = ""
        message_c_Subject_Regex_P1 = ""
        message_c_Subject_Regex_P2 = ""
        message_c_Subject_Regex_P3 = ""
        message_c_Subject_Regex_P4 = ""
        message_c_Body_Regex_P1 = ""
        message_c_Body_Regex_P2 = ""
        message_c_Body_Regex_P3 = ""
        message_c_Body_Regex_P4 = ""
        My.Settings.Save()
        Globals.ThisAddIn.LoadDefaults()
        'TODO interrupt running thread and relaunch if required
    End Sub

    Private Sub c_SPO_Login_Click(sender As Object, e As EventArgs) Handles c_SPO_Login.Click

        Dim list_status = SP_Authentication.Main()
        If list_status = "Valid" Then
            DataTableLoader.LoadDataGridView()
        End If


    End Sub



    Private Sub c_AccountSid_TextChanged(sender As Object, e As EventArgs) Handles c_AccountSid.TextChanged
        If c_AccountSid.Text <> My.Settings.s_AccountSid And c_AccountSid.IsHandleCreated Then
            message_c_AccountSid = "AccountSid field changed"

        End If
    End Sub

    Private Sub c_AuthToken_TextChanged(sender As Object, e As EventArgs) Handles c_AuthToken.TextChanged
        If c_AuthToken.Text <> My.Settings.s_AuthToken And c_AuthToken.IsHandleCreated Then
            message_c_AuthToken = "AuthToken field changed"

        End If
    End Sub

    Private Sub c_FromPhone_TextChanged(sender As Object, e As EventArgs) Handles c_FromPhone.TextChanged
        If c_FromPhone.Text <> My.Settings.s_PhoneFrom And c_FromPhone.IsHandleCreated Then
            message_c_FromPhone = "Phone number (from) field changed"

        End If
    End Sub

    Private Sub c_Subject_Regex_P1_TextChanged(sender As Object, e As EventArgs) Handles c_Subject_Regex_P1.TextChanged
        If c_Subject_Regex_P1.Text <> My.Settings.s_Subject_Regex_P1 And c_Subject_Regex_P1.IsHandleCreated Then
            message_c_Subject_Regex_P1 = "P1 Subject Regex field changed"

        End If
    End Sub

    Private Sub c_Subject_Regex_P2_TextChanged(sender As Object, e As EventArgs) Handles c_Subject_Regex_P2.TextChanged
        If c_Subject_Regex_P2.Text <> My.Settings.s_Subject_Regex_P2 And c_Subject_Regex_P2.IsHandleCreated Then
            message_c_Subject_Regex_P2 = "P2 Subject Regex field changed"
        End If
    End Sub

    Private Sub c_Subject_Regex_P3_TextChanged(sender As Object, e As EventArgs) Handles c_Subject_Regex_P3.TextChanged
        If c_Subject_Regex_P3.Text <> My.Settings.s_Subject_Regex_P3 And c_Subject_Regex_P3.IsHandleCreated Then
            message_c_Subject_Regex_P3 = "P3 Subject Regex field changed"
        End If
    End Sub

    Private Sub c_Subject_Regex_P4_TextChanged(sender As Object, e As EventArgs) Handles c_Subject_Regex_P4.TextChanged
        If c_Subject_Regex_P4.Text <> My.Settings.s_Subject_Regex_P4 And c_Subject_Regex_P4.IsHandleCreated Then
            message_c_Subject_Regex_P4 = "P4 Subject Regex field changed"
        End If
    End Sub

    Private Sub c_Body_Regex_P1_TextChanged(sender As Object, e As EventArgs) Handles c_Body_Regex_P1.TextChanged
        If c_Body_Regex_P1.Text <> My.Settings.s_Body_Regex_P1 And c_Body_Regex_P1.IsHandleCreated Then
            message_c_Body_Regex_P1 = "P1 Body Regex field changed"

        End If
    End Sub

    Private Sub c_Body_Regex_P2_TextChanged(sender As Object, e As EventArgs) Handles c_Body_Regex_P2.TextChanged
        If c_Body_Regex_P2.Text <> My.Settings.s_Body_Regex_P2 And c_Body_Regex_P2.IsHandleCreated Then
            message_c_Body_Regex_P2 = "P2 Body Regex field changed"
        End If
    End Sub

    Private Sub c_Body_Regex_P3_TextChanged(sender As Object, e As EventArgs) Handles c_Body_Regex_P3.TextChanged
        If c_Body_Regex_P3.Text <> My.Settings.s_Body_Regex_P3 And c_Body_Regex_P3.IsHandleCreated Then
            message_c_Body_Regex_P3 = "P3 Body Regex field changed"
        End If
    End Sub

    Private Sub c_Body_Regex_P4_TextChanged(sender As Object, e As EventArgs) Handles c_Body_Regex_P4.TextChanged
        If c_Body_Regex_P4.Text <> My.Settings.s_Body_Regex_P4 And c_Body_Regex_P4.IsHandleCreated Then
            message_c_Body_Regex_P4 = "P4 Body Regex field changed"
        End If
    End Sub




    Private Sub c_Loops_SelectedIndexChanged(sender As Object, e As EventArgs) Handles c_Loops.SelectedIndexChanged
        If c_Loops.SelectedItem.ToString <> My.Settings.s_Loops And c_Loops.IsHandleCreated Then
            message_c_Loops = "Number of Loops changed"
        End If
    End Sub

    Private Sub c_Volume_SelectedIndexChanged(sender As Object, e As EventArgs) Handles c_Volume.SelectedIndexChanged
        If c_Volume.SelectedItem.ToString <> My.Settings.s_Volume And c_Volume.IsHandleCreated Then
            message_c_Volume = "Alert Volume changed"
        End If
    End Sub

    Private Sub c_help_Click(sender As Object, e As EventArgs) Handles c_help.Click
        'https://stackoverflow.com/questions/9886957/find-install-directory-and-working-directory-of-vsto-outlook-addin-or-any-offic
        'Get the assembly information
        Dim assemblyInfo As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()

        'Location Is where the assembly Is run from 
        ' Dim assemblyLocation = assemblyInfo.Location

        'CodeBase Is the location of the ClickOnce deployment files
        Dim uriCodeBase As Uri = New Uri(assemblyInfo.CodeBase)
        Dim InstallLocation = Path.GetDirectoryName(uriCodeBase.LocalPath.ToString())
        Try
            Process.Start(InstallLocation & "\Resources\Help.docx")
        Catch ex1 As Win32Exception
            Globals.ThisAddIn.MsgBoxThreadedAlert("Resource at path " & InstallLocation & "\Resources\Help.docx " & "not found")
        Catch ex2 As Exception
            Globals.ThisAddIn.MsgBoxThreadedAlert(ex2.Message)
        End Try

    End Sub


    Private Sub c_TwiMLBinVariable_TextChanged(sender As Object, e As EventArgs) Handles c_TwiMLBinVariable.TextChanged
        If c_TwiMLBinVariable.Text <> My.Settings.s_TwiMLBinVariable And c_TwiMLBinVariable.IsHandleCreated Then
            message_c_TwiMLBinVariable = "TwiML Bin Variable Name Changed"
        End If
    End Sub

    Private Sub c_TwiMLBinURL_TextChanged(sender As Object, e As EventArgs) Handles c_TwiMLBinURL.TextChanged
        If c_TwiMLBinURL.Text <> My.Settings.s_TwiMLBinURL And c_TwiMLBinURL.IsHandleCreated Then
            message_c_TwiMLBinURL = "TwiML Bin URL Changed"
        End If
    End Sub


End Class
