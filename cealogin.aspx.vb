Imports dbOperation

Imports ceaCommon

Imports System.Data
Partial Class cealogin
    Inherits System.Web.UI.Page
    Private starttime As Integer
    Private endTime As Integer
    Private pageLoadTime As Integer
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        starttime = Environment.TickCount
    End Sub
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        endTime = Environment.TickCount
        pageLoadTime = endTime - starttime
        divPage.InnerHtml = getCC(pageLoadTime)
    End Sub

    Private Sub perf_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Request.Params("logout") Is Nothing Then
            Session("eid") = ""
            Session.Clear()
            Session.Abandon()
            divInfo.InnerHtml = getMessage("Logout Successfull. Click here to <a href=cealogin.aspx>Login Again </a>", "success") 'success error info warning
            Exit Sub
        End If


        If Not Page.IsPostBack Then
            divLogin.InnerHtml = "<h1 class='text-white text-uppercase mb-10'>CEA Login: </h1> " &
                                    "<p class='text-white mb-30'>All Generating Unit(s) Developer can do the Login from here.</p> <p class='text-white mb-30'>For login related help. Contact the CEA Team: Phone: xxxxxxxx Email : xxxxxxx</p>" &
                                    " <a href='#yearselect' class='primary-btn d-inline-flex align-items-center'><span class='mr-10'>Login</span><span class='lnr lnr-arrow-right'></span></a>"
            divHed.InnerHtml = "<b>  Login/Signup:</b>"
            pnlLogin.Visible = True
            If Request.Params("mode") = "created" Then divHed.InnerHtml = "<b>  User Created:</b>"
            divInfo.InnerHtml = getMessage("Create New ID or Use following for testing. User vinod2 password 123 ", "info")
            'executeDB("insert into login (eid, log, logintime) values (" & Session("peid") & " , 'PDashboard Authorized Access: " & Now.ToString() & " - " & Request.UserHostAddress & "', current_timestamp)", "logdb")

        End If
        '' do postback stuff here
        '    Response.Write("<div id='bottomline'>At Page Load" & e1.Message & "</div>")
        'End Try
    End Sub

    Private Sub btnSignIn_Click(sender As Object, e As EventArgs) Handles btnSignIn.Click
        If String.IsNullOrEmpty(txtLogin.Text) Then
            divInfo.InnerHtml = getMessage("Enter Something", "error") 'success error info warning
            Exit Sub
        End If
        If txtLogin.Text.Contains(" ") Then
            divInfo.InnerHtml = getMessage("Space not allowed in User ID", "error") 'success error info warning
            Exit Sub
        End If
        If isNumeric(txtLogin.Text) Then
            divInfo.InnerHtml = getMessage("Numeric only not allowed in User ID", "error") 'success error info warning
            Exit Sub
        End If
        If checkForSQLInjection(txtLogin.Text) Then
            divInfo.InnerHtml = getMessage("Some invalid characters found. Use pure alphanumeric ID", "error") 'success error info warning
            Exit Sub
        End If
        pnlSignUp.Visible = False
        pnlLogin.Visible = False
        pnlAuthenticate.Visible = False
        pnlChange.Visible = False
        pnlForgot.Visible = False
        '''
         'Now check if user id id for signup or login
        Dim q = "select ceauser from users_cea where ceauser = '" & txtLogin.Text.Replace("'", "") & "' limit 1"
        Dim ret = getDBsingle(q)
        If ret = txtLogin.Text Then
            ''' user exist
            ''' 
            divHed.InnerHtml = "<b>  Login:</b>"
            pnlAuthenticate.Visible = True
            txtAuthUser.Text = txtLogin.Text
            divInfo.InnerHtml = getMessage("Your Last Login " & getDBsingle("select last_updated from users_cea where ceauser = '" & txtLogin.Text.Replace("'", "") & "' limit 1"), "success") 'success error info warning
        Else
            '' user not exist
            divHed.InnerHtml = "<b> Signup:</b>"
            txtSignupUser.Text = txtLogin.Text
            pnlSignUp.Visible = True

            ddlDeveloper.DataSource = getDBTable("select developer from Developers where approved=1")
            ddlDeveloper.DataBind()
            ddlDeveloper.Items.Add(New ListItem("Select your Company", "NA"))
            ddlDeveloper.SelectedValue = "NA"
        End If
    End Sub

    Private Sub btnSignUp_Click(sender As Object, e As EventArgs) Handles btnSignUp.Click

        If String.IsNullOrEmpty(txtSignupPassword.Text) Then
            divInfo.InnerHtml = getMessage("Enter Password", "error") 'success error info warning
            Exit Sub
        End If
        If Not ContainsSpecialChars(txtSignupPassword.Text) Then
            divInfo.InnerHtml = getMessage("Password must contain special character", "error") 'success error info warning
            Exit Sub
        End If
        If txtSignupPassword.Text.Length < 8 Then
            divInfo.InnerHtml = getMessage("Password must have atleast 8 character", "error") 'success error info warning
            Exit Sub
        End If
        If String.IsNullOrEmpty(txtSignupName.Text) Then
            divInfo.InnerHtml = getMessage("Enter Name", "error") 'success error info warning
            Exit Sub
        End If
        If isNumeric(txtSignupName.Text) Then
            divInfo.InnerHtml = getMessage("Name Can't be numeric", "error") 'success error info warning
            Exit Sub
        End If
        If String.IsNullOrEmpty(txtSignupEmail.Text) Then
            divInfo.InnerHtml = getMessage("Enter Email", "error") 'success error info warning
            Exit Sub
        End If
        If Not isEmail(txtSignupEmail.Text) Then
            divInfo.InnerHtml = getMessage("Enter Valid Email", "error") 'success error info warning
            Exit Sub
        End If
        If String.IsNullOrEmpty(txtSignupPhone.Text) Then
            divInfo.InnerHtml = getMessage("Enter Phone", "error") 'success error info warning
            Exit Sub
        End If
        If Not isNumeric(txtSignupPhone.Text) Then
            divInfo.InnerHtml = getMessage("Phone must be numeric", "error") 'success error info warning
            Exit Sub
        End If
        If checkForSQLInjection(txtSignupPassword.Text) Then
            divInfo.InnerHtml = getMessage("Some invalid characters found in Password. Use pure alphanumeric ID", "error") 'success error info warning
            Exit Sub
        End If
        If ddlDeveloper.SelectedValue = "NA" And cbkDeveloper.Checked = False Then
            divInfo.InnerHtml = getMessage("Select Company name from List", "error") 'success error info warning
            Exit Sub
        End If
        If String.IsNullOrEmpty(txtSignupDeveloper.Text) And cbkDeveloper.Checked = True Then
            divInfo.InnerHtml = getMessage("Enter Company name", "error") 'success error info warning
            Exit Sub
        End If

        '' Everything is fine now save the data
        Dim company = ddlDeveloper.SelectedValue
        If cbkDeveloper.Checked = True Then company = txtSignupDeveloper.Text.Replace("'", "")
        Dim q = "insert into users_cea (ceauser, ceapwd, name,company,email,phone,last_updated,lastupdateby) " &
            " values('" & txtSignupUser.Text.Replace("'", "") & "',md5('" & txtSignupPassword.Text.Replace("'", "") & "'),'" & txtSignupName.Text.Replace("'", "") & "','" & company & "','" & txtSignupEmail.Text.Replace("'", "") & "','" & txtSignupPhone.Text.Replace("'", "") & "',current_timestamp,'" & Request.UserHostAddress & "')"

        Dim ret = executeDB(q)
        If ret = "ok" Then
            divInfo.InnerHtml = getMessage("Data insewrted succesfully", "info") 'success error info warning
            '' now insert a  company in developers with approved = 0
            ret = executeDB("insert into Developers (developer, approved) values('" & company & "',0)")
            If ret = "ok" Then
                divInfo.InnerHtml = getMessage("Company inserted succesfully", "info") 'success error info warning
            Else
                divInfo.InnerHtml = getMessage("Failed to insert New Company", "error") 'success error info warning
                Exit Sub
            End If
            '''
            'Redirect to Login Page
            Response.Redirect("cealogin.aspx?mode=created")

        Else
            divInfo.InnerHtml = getMessage("Failed to insert", "error") 'success error info warning
        End If

    End Sub

    Private Sub btnAuth_Click(sender As Object, e As EventArgs) Handles btnAuth.Click
        If String.IsNullOrEmpty(txtAuthPassword.Text) Then
            divInfo.InnerHtml = getMessage("Enter Password", "error") 'success error info warning
            Exit Sub
        End If
        If checkForSQLInjection(txtAuthPassword.Text) Then
            divInfo.InnerHtml = getMessage("Some invalid characters found in Password. Use pure alphanumeric ID", "error") 'success error info warning
            Exit Sub
        End If

        '' now do authentication
        Dim q = "select company, name, resp, status from users_cea where ceauser = '" & txtAuthUser.Text.Replace("'", "") & "' and ceapwd = md5('" & txtAuthPassword.Text & "') limit 1"
        Dim mydt = getDBTable(q)
        If mydt.rows.count = 1 Then
            '' succesfull login, update login time
            executeDB("update users_cea set last_updated = current_timestamp, lastupdateby = '" & Request.UserHostAddress & "' where ceauser = '" & txtAuthUser.Text.Replace("'", "") & "'")
            ''' check if cea user
            ''' 
            Session("ceauser") = txtAuthUser.Text.Replace("'", "")
            Session("name") = mydt.Rows(0)(1)
            If mydt.Rows(0)(0) = "CEA" Then
                Session("resp") = mydt.Rows(0)(2)
                Session("status") = mydt.Rows(0)(3)
                Response.Redirect("ceahome.aspx")
            Else
                Session("developer") = mydt.Rows(0)(0)
                Response.Redirect("genhome.aspx")
            End If

        Else
            divInfo.InnerHtml = getMessage("User/Password Incorrect", "error") 'success error info warning
        End If
    End Sub

    Private Sub btnChangePassword_Click(sender As Object, e As EventArgs) Handles btnChangePassword.Click
        If String.IsNullOrEmpty(txtChangePassword.Text) Then
            divInfo.InnerHtml = getMessage("Enter Password", "error") 'success error info warning
            Exit Sub
        End If
        If String.IsNullOrEmpty(txtCahngeOldPassword.Text) Then
            divInfo.InnerHtml = getMessage("Enter Old Password", "error") 'success error info warning
            Exit Sub
        End If

        '' now update the password
        Dim q = "update users_cea set ceapwd = '' ,last_updated = current_timestamp,lastupdateby = '" & Request.UserHostAddress & "' where ceauser = '" & txtChangeUser.Text.Replace("'", "") & "'"
        Dim ret = executeDB(q)
        If ret = "ok" Then
            divInfo.InnerHtml = getMessage("Password Changed succesfully", "info") 'success error info warning
        Else
            divInfo.InnerHtml = getMessage("Failed to change", "error") 'success error info warning
        End If
    End Sub

    Private Sub lbChange_Click(sender As Object, e As EventArgs) Handles lbChange.Click
        pnlSignUp.Visible = False
        pnlLogin.Visible = False
        pnlAuthenticate.Visible = False
        pnlChange.Visible = True
        pnlForgot.Visible = False
        txtChangeUser.Text = txtAuthUser.Text
    End Sub

    Private Sub lbForgot_Click(sender As Object, e As EventArgs) Handles lbForgot.Click
        pnlSignUp.Visible = False
        pnlLogin.Visible = False
        pnlAuthenticate.Visible = False
        pnlChange.Visible = False
        pnlForgot.Visible = True
        txtForgotUser.Text = txtAuthUser.Text

    End Sub

    Private Sub btnForgot_Click(sender As Object, e As EventArgs) Handles btnForgot.Click
        divInfo.InnerHtml = getMessage("Password shall be emailed and SMS: under development. Meanwhile take help of CEA Team", "error") 'success error info warning
    End Sub
End Class
