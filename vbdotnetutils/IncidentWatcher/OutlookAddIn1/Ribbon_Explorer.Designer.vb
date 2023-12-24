Partial Class Ribbon_Explorer
    Inherits Microsoft.Office.Tools.Ribbon.RibbonBase

    <System.Diagnostics.DebuggerNonUserCode()>
    Public Sub New(ByVal container As System.ComponentModel.IContainer)
        MyClass.New()

        'Required for Windows.Forms Class Composition Designer support
        If (container IsNot Nothing) Then
            container.Add(Me)
        End If

    End Sub

    <System.Diagnostics.DebuggerNonUserCode()>
    Public Sub New()
        MyBase.New(Globals.Factory.GetRibbonFactory())

        'This call is required by the Component Designer.
        InitializeComponent()

    End Sub

    'Component overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Tab1 = Me.Factory.CreateRibbonTab
        Me.Group_IncidentWatcher = Me.Factory.CreateRibbonGroup
        Me.Button_WatcherSettings = Me.Factory.CreateRibbonButton
        Me.ToggleSwitch = Me.Factory.CreateRibbonToggleButton
        Me.Tab1.SuspendLayout()
        Me.Group_IncidentWatcher.SuspendLayout()
        Me.SuspendLayout()
        '
        'Tab1
        '
        Me.Tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office
        Me.Tab1.ControlId.OfficeId = "TabMail"
        Me.Tab1.Groups.Add(Me.Group_IncidentWatcher)
        Me.Tab1.Label = "TabMail"
        Me.Tab1.Name = "Tab1"
        '
        'Group_IncidentWatcher
        '
        Me.Group_IncidentWatcher.Items.Add(Me.Button_WatcherSettings)
        Me.Group_IncidentWatcher.Items.Add(Me.ToggleSwitch)
        Me.Group_IncidentWatcher.Label = "Incident Watcher"
        Me.Group_IncidentWatcher.Name = "Group_IncidentWatcher"
        '
        'Button_WatcherSettings
        '
        Me.Button_WatcherSettings.Image = Global.OutlookAddIn1.My.Resources.Resources.icons8_automatic_24
        Me.Button_WatcherSettings.Label = "Settings"
        Me.Button_WatcherSettings.Name = "Button_WatcherSettings"
        Me.Button_WatcherSettings.ShowImage = True
        '
        'ToggleSwitch
        '
        Me.ToggleSwitch.Image = Global.OutlookAddIn1.My.Resources.Resources.icons8_toggle_off_16
        Me.ToggleSwitch.Label = "Disabled"
        Me.ToggleSwitch.Name = "ToggleSwitch"
        Me.ToggleSwitch.ShowImage = True
        '
        'Ribbon_Explorer
        '
        Me.Name = "Ribbon_Explorer"
        Me.RibbonType = "Microsoft.Outlook.Explorer"
        Me.Tabs.Add(Me.Tab1)
        Me.Tab1.ResumeLayout(False)
        Me.Tab1.PerformLayout()
        Me.Group_IncidentWatcher.ResumeLayout(False)
        Me.Group_IncidentWatcher.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Tab1 As Microsoft.Office.Tools.Ribbon.RibbonTab
    Friend WithEvents Group_IncidentWatcher As Microsoft.Office.Tools.Ribbon.RibbonGroup
    Friend WithEvents Button_WatcherSettings As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents ToggleSwitch As Microsoft.Office.Tools.Ribbon.RibbonToggleButton
End Class

Partial Class ThisRibbonCollection

    <System.Diagnostics.DebuggerNonUserCode()> _
    Friend ReadOnly Property Ribbon1() As Ribbon_Explorer
        Get
            Return Me.GetRibbon(Of Ribbon_Explorer)()
        End Get
    End Property
End Class
