Imports dbOperation

Imports ceaCommon

Imports System.Data
Partial Class pf
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
        divPage.InnerHtml = "Load Time: " & pageLoadTime ' getCC(pageLoadTime)
    End Sub

    Private Sub perf_Load(sender As Object, e As EventArgs) Handles Me.Load
        Session("peid") = "8718"
        If Session("peid") Is Nothing Or String.IsNullOrEmpty(Session("peid")) Then
            '   Response.Redirect("admin.aspx")
            'app id = 2018051000023
            ' Response.Redirect("https://mapp.ntpc.co.in/sso/oAuthntpcs/applogin/2018051000023")
            divLogin.InnerHtml = "<h1 class='text-white text-uppercase mb-10'>Performance Feedback System</h1> " &
                                    "<p class='text-white mb-30'>Feedback is an essential communication tool in business performance management. This application make the process simple on mobile, Pad, PC.</p>" &
                                    " <a href='https://mapp.ntpc.co.in/sso/oAuthntpcs/applogin/2018051000023' class='primary-btn d-inline-flex align-items-center'><span class='mr-10'>Click to Login</span><span class='lnr lnr-arrow-right'></span></a>"

            Exit Sub
        End If
        divLogin.InnerHtml = "<h1 class='text-white text-uppercase mb-10'>Performance Feedback System</h1> " &
                                    "<p class='text-white mb-30'>Feedback is an essential communication tool in business performance management. This application make the process simple on mobile, Pad, PC.</p>" &
                                    " <a href='#yearselect' class='primary-btn d-inline-flex align-items-center'><span class='mr-10'>Get Started</span><span class='lnr lnr-arrow-right'></span></a>"

        '  Try

        If Not Page.IsPostBack Then
            'Dim mode = Request.Params("mode")
            'Dim shid = Request.Params("shid")
            'Dim hr = Request.Params("hr")
            'If Request.Params("mode") Is Nothing Then mode = "0"
            'If Request.Params("shid") Is Nothing Then shid = "%"
            'If Request.Params("hr") Is Nothing Then hr = "%"
            'Dim tab = dualTab(Session("weid"))
            'If tab Then Session("eid") = Session("weid")
            '  divMenu.InnerHtml = makeePACEmenuWS(mode, tab)
            'llload form years 2016 onwards automatically 
            'If Session("formyear") Is Nothing Then Session("formyear") = "2017"
            'Dim myDT = getDBTable("Select distinct formyear As yr , formyear || '-' || substr(formyear + 1,3,2) as yrdt from kpa_feedback where formyear > 2016 order by formyear desc")
            '' If myDT.Rows.Count = 1 Then rblFormYear.Visible = False
            'rblFormYear.DataSource = myDT
            'rblFormYear.DataBind()
            '' rblFormYear.SelectedValue = Session("formyear").ToString
            'Dim image = "<img src='images/pics/" & Session("peid").ToString.PadLeft(6, "0") & ".jpg' style='border:1px; align:middle; width:120px;height:150px;' onerror=" & Chr(34) & "this.src='images/user.jpg';" & Chr(34) & "/> "
            'divName.InnerHtml = "<div class='thumb' style='height:120px; '>" & image &
            '                    "<h4 class='text-white'>" & getnamefromEid(Session("peid")) & "</h4>" &
            '                    "<p class='text-white'>" & getHindiName(Session("peid")) & "</p>	</div>"

            ''  ShowPanel(mode, Request.Params("mylevel"), shid, hr)
            'executeDB("update hits set view = view+1 where page = 'EPACEP'")

            'GridView1.DataSource = getDBTable("select * from Field_master")
            'GridView1.DataBind()


            'executeDB("insert into login (eid, log, logintime) values (" & Session("peid") & " , 'PDashboard Authorized Access: " & Now.ToString() & " - " & Request.UserHostAddress & "', current_timestamp)", "logdb")



        End If
        '' do postback stuff here
        pnlForm.Controls.Add(CreateDynamicControls("testform"))
        'Catch e1 As Exception
        '    Response.Write("<div id='bottomline'>At Page Load" & e1.Message & "</div>")
        'End Try
    End Sub


    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        divMsg.InnerHtml = "you clicked: "
        Dim PlaceHolder1 As PlaceHolder = CType(pnlForm.FindControl("PlaceHolder1"), PlaceHolder)
        '        divMsg.InnerHtml = save("testform", PlaceHolder1)
    End Sub
End Class
