Imports dbOperation

Imports ceaCommon

Imports System.Data
Partial Class help
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

        If Not Page.IsPostBack Then
            divLogin.InnerHtml = "<h1 class='text-white text-uppercase mb-10'>HELP</h1> " &
                                    "<p class='text-white mb-30'>Please go through the FAQ(Frequently Asked Questions) and Flow chart. For step by step process refer to Help Manual.</p>" &
                                    " <a href='/CEA/upload/CEAHelp.pdf' target='_blank'  class='primary-btn d-inline-flex align-items-center'><span class='mr-10'>Download & Read</span><span class='lnr lnr-arrow-right'></span></a>"
            divHed.InnerHtml = "<b>Flow Chart:</b> . <br/>" & "<img src='/CEA/upload/flow1.png' /> <br/><img src='/CEA/upload/flow2.png' /> <br/><img src='/CEA/upload/flow3.png' /> "


            'executeDB("insert into login (eid, log, logintime) values (" & Session("peid") & " , 'PDashboard Authorized Access: " & Now.ToString() & " - " & Request.UserHostAddress & "', current_timestamp)", "logdb")



        End If
        '' do postback stuff here
        '    Response.Write("<div id='bottomline'>At Page Load" & e1.Message & "</div>")
        'End Try
    End Sub

End Class
