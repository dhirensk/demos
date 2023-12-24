Imports System.Text.RegularExpressions
Module SettingsValidation

    Public Function ValidateControls() As String
        Dim messages As New StringBuilder

        Dim v_Subject_Regex = Trim(My.Settings.s_Subject_Regex_P1 + My.Settings.s_Subject_Regex_P2 + My.Settings.s_Subject_Regex_P3 + My.Settings.s_Subject_Regex_P4)

        If String.IsNullOrWhiteSpace(v_Subject_Regex) Then
            messages.Append("Subject Search Regex Not set. Please rectify")
            messages.AppendLine()
            messages.AppendLine()
        End If

        If My.Settings.dt_Project Is Nothing Then
            messages.Append("No Data in Escalation Matrix. Please login to load Escalation Data")
            messages.AppendLine()
            messages.AppendLine()
        End If

        If String.IsNullOrWhiteSpace(My.Settings.s_AccountSid) Then
            messages.Append("AccountSid not set. Please rectify")
            messages.AppendLine()
            messages.AppendLine()
        End If

        If String.IsNullOrWhiteSpace(My.Settings.s_AuthToken) Then
            messages.Append("AuthToken not set. Please rectify")
            messages.AppendLine()
            messages.AppendLine()
        End If

        If String.IsNullOrWhiteSpace(My.Settings.s_PhoneFrom) Then

            messages.Append("Phone From not set. Please rectify")
            messages.AppendLine()
            messages.AppendLine()
        Else
            Dim regex = New Regex("^\+[0-9][0-9]*$")
            Dim match As Match = regex.Match(Trim(My.Settings.s_PhoneFrom))
            If Not match.Success Then
                messages.Append("Phone regex failed. Enter phone number starting with + followed by country code.")
                messages.Append("Example +919930493070")
                messages.AppendLine()
                messages.AppendLine()
            End If
        End If
        If String.IsNullOrWhiteSpace(My.Settings.s_TwiMLBinVariable) Then
            messages.Append("TwiML Bin Variable not set. Please rectify")
            messages.AppendLine()
            messages.AppendLine()
        End If

        If String.IsNullOrWhiteSpace(My.Settings.s_TwiMLBinURL) Then
            messages.Append("TwiML Bin URL not set. Please rectify")
            messages.AppendLine()
            messages.AppendLine()

        End If

        Dim finalmessage As String = messages.ToString
        Return finalmessage
    End Function


End Module
