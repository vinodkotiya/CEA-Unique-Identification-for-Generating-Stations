Imports dbOperation

Imports ceaCommon

Imports System.Data
Partial Class ceahome
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
        'Session("developer") = "NTPC1"
        If Session("resp") Is Nothing Or String.IsNullOrEmpty(Session("resp")) Then
            Response.Redirect("cealogin.aspx")
            Exit Sub
        End If

        '  Try

        If Not Page.IsPostBack Then
            divUSer.InnerHtml = "	<h6 class='title'>User: " & Session("ceauser") & "</h6>"

            divLogin.InnerHtml = "<h1 class='text-white text-uppercase mb-10'>CEA: " & Session("name") & "</h1> " &
                                    "<p class='text-white mb-30'>Here CEA Divisions can view the details of Generating Units before approval. Correction can also be made.</p>" &
                                    " <a href='#yearselect' class='primary-btn d-inline-flex align-items-center'><span class='mr-10'>Start Approval</span><span class='lnr lnr-arrow-right'></span></a>"
            divHed.InnerHtml = "<b>Dashboard:</b> "


            '' show unit details

            ddlUnitStatus.Items.Add(New ListItem("Under Approval", "Under Approval"))
            ddlUnitStatus.Items.Add(New ListItem("Approved", "Approved"))
            ddlUnitStatus.Items.Add(New ListItem("Rejected", "Rejected"))
            ddlUnitStatus_SelectedIndexChanged(vbNull, EventArgs.Empty)

            executeDB("insert into login (eid, log, logintime) values ('" & Request.UserHostAddress & "', 'ceahome.aspx Access at: " & Now.ToString() & " by " & Session("ceauser") & "', current_timestamp)")

        End If
        '' do postback stuff here
        '    Response.Write("<div id='bottomline'>At Page Load" & e1.Message & "</div>")
        'End Try
    End Sub
    Public Function projAction(ByVal project_code As String, ByVal unit As String) As Boolean
        If project_code Then Return True  ''allow to enter data
        Return False
    End Function







    Private Sub gvMyProjs_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvMyProjs.RowCommand
        Dim value = e.CommandArgument.ToString()

        If e.CommandName = "View" Then
            Dim v() = value.Split("#")
            Dim q = "select '' as test, Projectname as Project, u.stage, sector, u.Fuel, u.fuel_linkage as 'Fuel Linkage', Address, District, State, region, latitude, longitude, owner as 'Name of Concerned Persons(s) for Info', concat('Phone: ', phone, ' Fax:', fax, ' Email:', email) as Contact,  unit as 'Unit No', capacity as 'Capacity (in MW)', tech as 'Technology', grid as 'Grid Type', loa_dt as 'Letter of Award (LoA) Date', ant_actcod as 'Commissioning Date', captive as 'Name of the Industry/Installation, in case of Captive', approvestatus as 'Approve Status', regnumber as 'Registration Number'  from Units u, Projects p where p.project_code = u.project_code and u.project_code = " & v(0) & " and unit = " & v(1) & " limit 1 "
            pnlForm.Controls.Add(CreateVerticalViewofSingleRow(q))
        ElseIf e.CommandName = "proj" Then
            Response.Redirect("gen.aspx?redirecturl=ceahome.aspx&form=updateProj&primarykey=project_code=" & value)
        ElseIf e.CommandName = "unit" Then
            Dim v() = value.Split("#")
            Response.Redirect("gen.aspx?redirecturl=ceahome.aspx&form=updateUnit&primarykey=project_code=" & v(0) & " and unit=" & v(1))
        ElseIf e.CommandName = "approve" Then
            Dim v() = value.Split("#")
            Response.Redirect("gen.aspx?redirecturl=ceahome.aspx&form=approveUnit&primarykey=project_code=" & v(0) & " and unit=" & v(1))
        End If
        ' divMsg.InnerHtml = q

    End Sub



    Private Sub ddlUnitStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlUnitStatus.SelectedIndexChanged
        Dim projQ = "select project_code from Projects where projtype in (" & Session("resp") & ") and status in (" & Session("status") & ")  "
        If Session("resp") = "Captive" Then
            '' change query
            projQ = "select project_code from Projects where type in (" & Session("resp") & ") and status in (" & Session("status") & ")  "
        ElseIf Session("resp").contains("Hydro Under") Then
            'Hydro Under Appraisal  Hydro Under Survey
            projQ = "select project_code from Projects where substatus in (" & Session("resp") & ") and status in (" & Session("status") & ")  "
        End If
        Dim mainQ = "select project_code, unit, concat(project_code, '#', unit) as projunit, project, capacity, approvestatus, regnumber from Units where approvestatus = '" & ddlUnitStatus.SelectedValue & "' and project_code in (" & projQ & ")"
        Dim mydt = getDBTable(mainQ)
        'divInfo.InnerHtml = getMessage("Data loaded succesfully " & mainQ, "info") 'success error info warning
        'Exit Sub
        If mydt.Rows.Count > 0 Then
            gvMyProjs.Visible = True
            gvMyProjs.DataSource = mydt
            gvMyProjs.DataBind()
            divInfo.InnerHtml = getMessage("You can correct the data and take decision for approval now", "info") 'success error info warning
        Else
            gvMyProjs.Visible = False
            divInfo.InnerHtml = getMessage("There are no projects submitted by Generating Unit(s) for approval in your division", "warning") 'success error info warning
        End If
    End Sub
End Class
