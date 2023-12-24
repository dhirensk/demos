Imports System.Media.SystemSounds
Imports System.Runtime.InteropServices
Module SoundAndAlerts
    Private Const APPCOMMAND_VOLUME_MUTE As Integer = &H80000
    Private Const APPCOMMAND_VOLUME_UP As Integer = &HA0000
    Private Const APPCOMMAND_VOLUME_DOWN As Integer = &H90000
    Private Const WM_APPCOMMAND As Integer = &H319
    Private dummyform As New System.Windows.Forms.Form()

    <DllImport("user32.dll")>
    Private Function SendMessageW(ByVal hWnd As IntPtr,
               ByVal Msg As Integer, ByVal wParam As IntPtr,
               ByVal lParam As IntPtr) As IntPtr
    End Function

    Public Sub SetVolume(ByVal volume As String)
        Dim vol = CInt(volume) / 2
        For i As Integer = 1 To 100
            SendMessageW(dummyform.Handle, WM_APPCOMMAND, dummyform.Handle, New IntPtr(APPCOMMAND_VOLUME_DOWN))
        Next

        For i As Integer = 1 To vol
            SendMessageW(dummyform.Handle, WM_APPCOMMAND, dummyform.Handle, New IntPtr(APPCOMMAND_VOLUME_UP))
        Next
    End Sub
    Public Sub PlayAlert(ByVal num_loops As String, ByVal customsound As String)
        Dim loops = CInt(num_loops)
        Dim resource = Nothing
        If String.IsNullOrEmpty(Trim(customsound)) Then
            For i As Integer = 1 To loops
                My.Computer.Audio.Play(My.Resources.ringout, AudioPlayMode.WaitToComplete)
            Next
        Else
            For i As Integer = 1 To loops
                My.Computer.Audio.Play(customsound, AudioPlayMode.WaitToComplete)
            Next
        End If


    End Sub




End Module
