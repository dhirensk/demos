Imports System.Threading
Imports Microsoft.Office.Tools.Ribbon

Public Class Ribbon_Explorer


    Private Sub ToggleButton1_Click(sender As Object, e As RibbonControlEventArgs) Handles ToggleSwitch.Click
        If My.Settings.ToggleValue Then
            My.Settings.ToggleValue = False
            My.Settings.Save()
            ToggleSwitch.Label = "Disabled"
            ToggleSwitch.Image = My.Resources.icons8_toggle_off_16
            Globals.ThisAddIn.getAlertDict.Clear()
            Globals.ThisAddIn.getCallDict.Clear()

        Else
            Dim message = SettingsValidation.ValidateControls
            If String.IsNullOrWhiteSpace(message) Then
                My.Settings.ToggleValue = True
                ToggleSwitch.Label = "Enabled"
                ToggleSwitch.Image = My.Resources.icons8_toggle_on_16
                MsgBox("Incident Watcher Application enabled successfully", vbOKOnly)
                My.Settings.s_LastGoodState = DateTime.Now
                My.Settings.Save()

            Else
                MsgBox(message, vbExclamation)
            End If

        End If
    End Sub

    Private Sub Ribbon_Explorer_Load(sender As Object, e As RibbonUIEventArgs) Handles MyBase.Load

        ToggleSwitch.Checked = My.Settings.ToggleValue
        If ToggleSwitch.Checked Then
            ToggleSwitch.Label = "Enabled"
            ToggleSwitch.Image = My.Resources.icons8_toggle_on_16
            My.Settings.Save()
        Else
            ToggleSwitch.Label = "Disabled"
            ToggleSwitch.Image = My.Resources.icons8_toggle_off_16
            Globals.ThisAddIn.getAlertDict.Clear()
            Globals.ThisAddIn.getCallDict.Clear()
        End If
    End Sub

    Private Sub Ribbon_Explorer_LoadImage(sender As Object, e As RibbonLoadImageEventArgs) Handles MyBase.LoadImage

    End Sub

    Private Sub Button_WatcherSettings_Click(sender As Object, e As RibbonControlEventArgs) Handles Button_WatcherSettings.Click

        Globals.ThisAddIn.getIncidentWatcher.Visible = True


    End Sub
End Class
