Imports dbOperation

Imports ceaCommon

Imports System.Data
Partial Class Default1
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
        'Response.Redirect("https://mapp.ntpc.co.in/cea")
        'Session("peid") = "8718"
        'If Session("peid") Is Nothing Or String.IsNullOrEmpty(Session("peid")) Then
        '    '   Response.Redirect("admin.aspx")
        '    'app id = 2018051000023
        '    ' Response.Redirect("https://mapp.ntpc.co.in/sso/oAuthntpcs/applogin/2018051000023")
        '    divLogin.InnerHtml = "<h1 class='text-white text-uppercase mb-10'>Performance Feedback System</h1> " &
        '                            "<p class='text-white mb-30'>Feedback is an essential communication tool in business performance management. This application make the process simple on mobile, Pad, PC.</p>" &
        '                            " <a href='https://mapp.ntpc.co.in/sso/oAuthntpcs/applogin/2018051000023' class='primary-btn d-inline-flex align-items-center'><span class='mr-10'>Click to Login</span><span class='lnr lnr-arrow-right'></span></a>"

        '    Exit Sub
        'End If
        divLogin.InnerHtml = "<h1 class='text-white text-uppercase mb-10'>Registration Of Generating Units</h1> " &
                                    "<p class='text-white mb-30'>Section 74 of Electricity Act, 2003 & and Regulation 4 & 5 of CEA (Furnishing of statistics, returns and information) Regulations, 2007, mandates every licensee, generating company, or person(s) generating electricity for its or his own use to furnish the statistics, returns or other information relating to generation, transmission, distribution, trading to CEA, the complete data/information to be made available to CEA from Captive Power Plants, RE Generators & conventional generating units of IPP’s.</p>" &
                   "<p  class='text-white mb-30'> Hence, mandatory registration of all the electricity generating units, above a specified capacity, through this National Level Data Registry System, by assigning each of them a unique registration number, so that generating capacity of all the electricity generating units is available with CEA. </p>" &
                                    " <a href='#yearselect' class='primary-btn d-inline-flex align-items-center'><span class='mr-10'>Get Started</span><span class='lnr lnr-arrow-right'></span></a>"

        '  Try

        If Not Page.IsPostBack Then
            gvDivision.DataSource = getDBTable("select name as Division, concat(resp , ' (', status, ')') as Responsibility from users_cea where company = 'CEA'")
            gvDivision.DataBind()

            executeDB("insert into login (eid, log, logintime) values ('" & Request.UserHostAddress & "', 'Homepage Access at: " & Now.ToString() & "', current_timestamp)")



        End If
        '' do postback stuff here
        '   pnlForm.Controls.Add(CreateDynamicControls("testform"))
        'Catch e1 As Exception
        '    Response.Write("<div id='bottomline'>At Page Load" & e1.Message & "</div>")
        'End Try
    End Sub



End Class
