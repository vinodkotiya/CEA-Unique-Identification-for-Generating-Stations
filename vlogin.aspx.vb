Imports dbOp
Imports Common
Partial Class vlogin
    Inherits System.Web.UI.Page

    Private Sub login_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            head.InnerHtml = "<spam>CEA: REGISTRATION OF GENERATING UNITS</spam> <br /> Access Denied - 403"

            If Request.Form.Count > 0 Then
                Dim appsecret = "KN1jL9ca$w6" 'sdcg9+Q2jyNP/Az3sqEmjg=="
                '  Dim appsecret = "sdcg9+Q2jyNP/Az3sqEmjg=="
                '  Dim eid = Decrypt(Request.Form("eid"), appsecret)
                Dim eid = Decrypt(Request.Form("EmpId"), appsecret)
                Dim email = Decrypt(Request.Form("EmailId"), appsecret)
                Dim empname = Decrypt(Request.Form("EmpName"), appsecret)
                Dim Dept = Decrypt(Request.Form("Dept"), appsecret)
                Dim Location = Decrypt(Request.Form("Location"), appsecret)
                Dim Mobile = Decrypt(Request.Form("Mobile"), appsecret)

                'lblEid.Text =
                'lblEmail.Text =
                'divMsg.InnerHtml = "<p>eid=" & eid & " Email: " & email & "</p>"
                'Exit Sub
                '''check if non ntpc user, create an eid for it
                'If eid.Contains("@") Then
                '    eid = getOrGenerateEid(eid)
                'End If
                '' Check if eid is authorised for this application
                'If isAuthorised(eid) = False Then
                '    divMsg.InnerHtml = "<p>Your Emp ID No: " & eid & " for Email: " & email & " Emp Name " & empname & " Dept " & Dept & " Location " & Location & " Mobile " & Mobile & " is not authorised to use this application. Contact the administrator to get authorisation. Communicate your Emp ID: " & eid & " on phone to administrator.</p>"
                '    executeDB("insert into login (eid, log, logintime) values (" & eid & " , 'Email " & email & " login.aspx App access not allowed : at " & Now.ToString() & " - " & Request.UserHostAddress & "', current_timestamp)", "logdb")
                '    Exit Sub
                'End If
                'Session("eid") = eid
                Session("email") = email
                ' executeDB("insert into login (eid, log, logintime) values (0 , '" & Session("email") & "  Email Session got from SSO so sending to client.aspx Page Access : at " & Now.ToString() & " - " & Request.UserHostAddress & "', current_timestamp)", "logdb")
                If Session("redirectafterlogin") Is Nothing Then
                    Dim testid = 1
                    If Not Session("testid") Then testid = 1
                    Response.Redirect("client.aspx?testid=" & testid & "&mobile=" & Mobile & "&location=" & Location)
                Else
                    Response.Redirect(Session("redirectafterlogin"))
                End If
            End If
        End If
        Session("eid") = "ntpc"
        If Request.Params("mode") = "gen" Then
            Response.Redirect("genhome.aspx")
        End If
        If Session("eid") Is Nothing Then
            If Not Request.Params("redirectafterlogin") Is Nothing Then
                Session("redirectafterlogin") = Request.Params("redirectafterlogin")
            End If
            ' Response.Redirect("https://cc.ntpc.co.in/sso/Default.aspx?appid=12343212")
            Response.Redirect("https://mapp.ntpc.co.in/sso/oAuthntpcs/applogin/2018011600022")
        End If
        If Not Request.Params("logout") Is Nothing Then
            Session("eid") = ""
            Session.Clear()
            Session.Abandon()
            divMsg.InnerHtml = "<p>Logout Successfull. <a href=Default.aspx > Click here to return</a></p>"
        End If
    End Sub
End Class
