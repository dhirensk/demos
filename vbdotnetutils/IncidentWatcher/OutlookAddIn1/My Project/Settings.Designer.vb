﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On



<Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
 Global.System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.9.0.0"),  _
 Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
Partial Friend NotInheritable Class MySettings
    Inherits Global.System.Configuration.ApplicationSettingsBase
    
    Private Shared defaultInstance As MySettings = CType(Global.System.Configuration.ApplicationSettingsBase.Synchronized(New MySettings()),MySettings)
    
#Region "My.Settings Auto-Save Functionality"
#If _MyType = "WindowsForms" Then
    Private Shared addedHandler As Boolean

    Private Shared addedHandlerLockObject As New Object

    <Global.System.Diagnostics.DebuggerNonUserCodeAttribute(), Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)> _
    Private Shared Sub AutoSaveSettings(sender As Global.System.Object, e As Global.System.EventArgs)
        If My.Application.SaveMySettingsOnExit Then
            My.Settings.Save()
        End If
    End Sub
#End If
#End Region
    
    Public Shared ReadOnly Property [Default]() As MySettings
        Get
            
#If _MyType = "WindowsForms" Then
               If Not addedHandler Then
                    SyncLock addedHandlerLockObject
                        If Not addedHandler Then
                            AddHandler My.Application.Shutdown, AddressOf AutoSaveSettings
                            addedHandler = True
                        End If
                    End SyncLock
                End If
#End If
            Return defaultInstance
        End Get
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("False")>  _
    Public Property ToggleValue() As Boolean
        Get
            Return CType(Me("ToggleValue"),Boolean)
        End Get
        Set
            Me("ToggleValue") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("")>  _
    Public Property s_Loops() As String
        Get
            Return CType(Me("s_Loops"),String)
        End Get
        Set
            Me("s_Loops") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("")>  _
    Public Property s_Volume() As String
        Get
            Return CType(Me("s_Volume"),String)
        End Get
        Set
            Me("s_Volume") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("")>  _
    Public Property s_Subject_Regex_P1() As String
        Get
            Return CType(Me("s_Subject_Regex_P1"),String)
        End Get
        Set
            Me("s_Subject_Regex_P1") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("")>  _
    Public Property s_Subject_Regex_P2() As String
        Get
            Return CType(Me("s_Subject_Regex_P2"),String)
        End Get
        Set
            Me("s_Subject_Regex_P2") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("")>  _
    Public Property s_Subject_Regex_P3() As String
        Get
            Return CType(Me("s_Subject_Regex_P3"),String)
        End Get
        Set
            Me("s_Subject_Regex_P3") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("")>  _
    Public Property s_AccountSid() As String
        Get
            Return CType(Me("s_AccountSid"),String)
        End Get
        Set
            Me("s_AccountSid") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("")>  _
    Public Property s_AuthToken() As String
        Get
            Return CType(Me("s_AuthToken"),String)
        End Get
        Set
            Me("s_AuthToken") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("")>  _
    Public Property s_PhoneFrom() As String
        Get
            Return CType(Me("s_PhoneFrom"),String)
        End Get
        Set
            Me("s_PhoneFrom") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("")>  _
    Public Property s_CustomSound() As String
        Get
            Return CType(Me("s_CustomSound"),String)
        End Get
        Set
            Me("s_CustomSound") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Public Property dt_Project() As Global.System.Collections.Specialized.StringCollection
        Get
            Return CType(Me("dt_Project"),Global.System.Collections.Specialized.StringCollection)
        End Get
        Set
            Me("dt_Project") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Public Property dt_EscalationLevel() As Global.System.Collections.Specialized.StringCollection
        Get
            Return CType(Me("dt_EscalationLevel"),Global.System.Collections.Specialized.StringCollection)
        End Get
        Set
            Me("dt_EscalationLevel") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Public Property dt_PersonName() As Global.System.Collections.Specialized.StringCollection
        Get
            Return CType(Me("dt_PersonName"),Global.System.Collections.Specialized.StringCollection)
        End Get
        Set
            Me("dt_PersonName") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Public Property dt_PersonEmail() As Global.System.Collections.Specialized.StringCollection
        Get
            Return CType(Me("dt_PersonEmail"),Global.System.Collections.Specialized.StringCollection)
        End Get
        Set
            Me("dt_PersonEmail") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Public Property dt_EscalationPerson() As Global.System.Collections.Specialized.StringCollection
        Get
            Return CType(Me("dt_EscalationPerson"),Global.System.Collections.Specialized.StringCollection)
        End Get
        Set
            Me("dt_EscalationPerson") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Public Property dt_EscalationPersonEmail() As Global.System.Collections.Specialized.StringCollection
        Get
            Return CType(Me("dt_EscalationPersonEmail"),Global.System.Collections.Specialized.StringCollection)
        End Get
        Set
            Me("dt_EscalationPersonEmail") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Public Property dt_EscalationPersonPhone() As Global.System.Collections.Specialized.StringCollection
        Get
            Return CType(Me("dt_EscalationPersonPhone"),Global.System.Collections.Specialized.StringCollection)
        End Get
        Set
            Me("dt_EscalationPersonPhone") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Public Property dt_IncidentLevel() As Global.System.Collections.Specialized.StringCollection
        Get
            Return CType(Me("dt_IncidentLevel"),Global.System.Collections.Specialized.StringCollection)
        End Get
        Set
            Me("dt_IncidentLevel") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Public Property dt_EscalationStartTime() As Global.System.Collections.Specialized.StringCollection
        Get
            Return CType(Me("dt_EscalationStartTime"),Global.System.Collections.Specialized.StringCollection)
        End Get
        Set
            Me("dt_EscalationStartTime") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Public Property dt_EscalationEndTime() As Global.System.Collections.Specialized.StringCollection
        Get
            Return CType(Me("dt_EscalationEndTime"),Global.System.Collections.Specialized.StringCollection)
        End Get
        Set
            Me("dt_EscalationEndTime") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Public Property dt_ReminderBeep() As Global.System.Collections.Specialized.StringCollection
        Get
            Return CType(Me("dt_ReminderBeep"),Global.System.Collections.Specialized.StringCollection)
        End Get
        Set
            Me("dt_ReminderBeep") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Public Property dt_ReminderCall() As Global.System.Collections.Specialized.StringCollection
        Get
            Return CType(Me("dt_ReminderCall"),Global.System.Collections.Specialized.StringCollection)
        End Get
        Set
            Me("dt_ReminderCall") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("")>  _
    Public Property s_Subject_Regex_P4() As String
        Get
            Return CType(Me("s_Subject_Regex_P4"),String)
        End Get
        Set
            Me("s_Subject_Regex_P4") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("")>  _
    Public Property s_Body_Regex_P1() As String
        Get
            Return CType(Me("s_Body_Regex_P1"),String)
        End Get
        Set
            Me("s_Body_Regex_P1") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("")>  _
    Public Property s_Body_Regex_P2() As String
        Get
            Return CType(Me("s_Body_Regex_P2"),String)
        End Get
        Set
            Me("s_Body_Regex_P2") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("")>  _
    Public Property s_Body_Regex_P3() As String
        Get
            Return CType(Me("s_Body_Regex_P3"),String)
        End Get
        Set
            Me("s_Body_Regex_P3") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("")>  _
    Public Property s_Body_Regex_P4() As String
        Get
            Return CType(Me("s_Body_Regex_P4"),String)
        End Get
        Set
            Me("s_Body_Regex_P4") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Public Property s_EmailsUnderEscalation() As Global.System.Collections.Specialized.StringCollection
        Get
            Return CType(Me("s_EmailsUnderEscalation"),Global.System.Collections.Specialized.StringCollection)
        End Get
        Set
            Me("s_EmailsUnderEscalation") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("Disabled")>  _
    Public Property s_PreviousState() As String
        Get
            Return CType(Me("s_PreviousState"),String)
        End Get
        Set
            Me("s_PreviousState") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Public Property s_LastGoodState() As Date
        Get
            Return CType(Me("s_LastGoodState"),Date)
        End Get
        Set
            Me("s_LastGoodState") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("https://incidentwatcher.sharepoint.com/sites/MSteams")>  _
    Public Property s_siteURL() As String
        Get
            Return CType(Me("s_siteURL"),String)
        End Get
        Set
            Me("s_siteURL") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("IncidentWatcher")>  _
    Public Property s_SPList() As String
        Get
            Return CType(Me("s_SPList"),String)
        End Get
        Set
            Me("s_SPList") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Public Property dt_Sr() As Global.System.Collections.Specialized.StringCollection
        Get
            Return CType(Me("dt_Sr"),Global.System.Collections.Specialized.StringCollection)
        End Get
        Set
            Me("dt_Sr") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Public Property dt_ColumnName() As Global.System.Collections.Specialized.StringCollection
        Get
            Return CType(Me("dt_ColumnName"),Global.System.Collections.Specialized.StringCollection)
        End Get
        Set
            Me("dt_ColumnName") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Public Property dt_RequiredColumn() As Global.System.Collections.Specialized.StringCollection
        Get
            Return CType(Me("dt_RequiredColumn"),Global.System.Collections.Specialized.StringCollection)
        End Get
        Set
            Me("dt_RequiredColumn") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Public Property dt_ColumnType() As Global.System.Collections.Specialized.StringCollection
        Get
            Return CType(Me("dt_ColumnType"),Global.System.Collections.Specialized.StringCollection)
        End Get
        Set
            Me("dt_ColumnType") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Public Property dt_ColumnTypeExpected() As Global.System.Collections.Specialized.StringCollection
        Get
            Return CType(Me("dt_ColumnTypeExpected"),Global.System.Collections.Specialized.StringCollection)
        End Get
        Set
            Me("dt_ColumnTypeExpected") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Public Property dt_ColumnStatus() As Global.System.Collections.Specialized.StringCollection
        Get
            Return CType(Me("dt_ColumnStatus"),Global.System.Collections.Specialized.StringCollection)
        End Get
        Set
            Me("dt_ColumnStatus") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("dhirensk@incidentwatcher.onmicrosoft.com")>  _
    Public Property s_EmailID() As String
        Get
            Return CType(Me("s_EmailID"),String)
        End Get
        Set
            Me("s_EmailID") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("ReadEmailBody")>  _
    Public Property s_TwiMLBinVariable() As String
        Get
            Return CType(Me("s_TwiMLBinVariable"),String)
        End Get
        Set
            Me("s_TwiMLBinVariable") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("https://handler.twilio.com/twiml/EHc34f2378abce70c5edd2750ee4d11249")>  _
    Public Property s_TwiMLBinURL() As String
        Get
            Return CType(Me("s_TwiMLBinURL"),String)
        End Get
        Set
            Me("s_TwiMLBinURL") = value
        End Set
    End Property
End Class

Namespace My
    
    <Global.Microsoft.VisualBasic.HideModuleNameAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()>  _
    Friend Module MySettingsProperty
        
        <Global.System.ComponentModel.Design.HelpKeywordAttribute("My.Settings")>  _
        Friend ReadOnly Property Settings() As Global.OutlookAddIn1.MySettings
            Get
                Return Global.OutlookAddIn1.MySettings.Default
            End Get
        End Property
    End Module
End Namespace
