Imports System
Imports Twilio
Imports Twilio.Rest.Api.V2010.Account
Imports Twilio.Types
Imports Twilio.TwiML
Imports System.Web.HttpUtility
Imports System.Collections.Specialized
Imports System.Text.RegularExpressions
Imports System.Threading

Module TwilioCalls


    Public Sub OutGoingCall(callitem As StringCollection)

        Dim thread As New Threading.Thread(
            Sub()
                ' Globals.ThisAddIn.MsgBoxThreadedOkOnly("Reached Twilio Call Begin of Sub")
                '0 incident level 1 project '2 escalation level '3  person name '4 person email '5 escalation person
                '6 escalation person email '7 escalation person phone '8 email subject
                '9 email body '10 email received date
                Dim regex = New Regex("^\+[0-9][0-9]*$")
                Dim match As Match = regex.Match(Trim(callitem.Item(7)))
                If Not match.Success Then
                    Globals.ThisAddIn.MsgBoxThreadedError("Escalation Person Phone regex failed. Number should begin with + followed by country code.")
                    Exit Sub
                End If

                'e.g. Const accountSid = "ACfe5fgr67kDn63e803961d74b593b"
                'e.g. Const authToken = "35464f69b4sdggfdf2bf1e75754545"
                Dim accountSid As String = My.Settings.s_AccountSid
                Dim authToken As String = My.Settings.s_AuthToken


                TwilioClient.Init(accountSid, authToken)
                Dim to_number = New PhoneNumber(callitem.Item(7))
                Dim from_number = New PhoneNumber(My.Settings.s_PhoneFrom)
                Dim EmailMsg = ""
                If callitem.Item(2) <= 1 Then
                    EmailMsg = "This is a reminder call for " & "P" & callitem.Item(0) & " Incident in Project " & callitem.Item(1)
                Else
                    EmailMsg = "This is an Escalation level" & callitem.Item(2) & " call regarding " & "P" & callitem.Item(0) & " Incident in Project " & callitem.Item(1) & ". Resource Name is " & callitem.Item(3)
                End If

                Dim URL = My.Settings.s_TwiMLBinURL + "?ReadEmailBody=" + UrlEncode(EmailMsg)
                Dim TwilioCall = CallResource.Create(to_number, from_number, url:=New Uri(URL))
                '  Globals.ThisAddIn.MsgBoxThreadedOkOnly("Reached Twilio Call End of Sub")
                'Dim TwilioCall = CallResource.Create(to_number, from_number, url:=New Uri(URL), statusCallback:=New Uri(My.Settings.s_TwiMLBinURL), statusCallbackEvent:=New List(Of String) From {"answered"})

                Globals.ThisAddIn.MsgBoxThreadedOkOnly(TwilioCall.Status.ToString)

            End Sub
            )
        thread.Start()
    End Sub
End Module




