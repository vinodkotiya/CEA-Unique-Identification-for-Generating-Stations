Imports dbOperation

Imports ceaCommon

Imports System.Data
Partial Class genhome
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
        If Session("developer") Is Nothing Or String.IsNullOrEmpty(Session("developer")) Then
            Response.Redirect("cealogin.aspx")
            Exit Sub
        End If

        '  Try

        If Not Page.IsPostBack Then
            divUSer.InnerHtml = "	<h6 class='title'>User: " & Session("ceauser") & "</h6>"

            divLogin.InnerHtml = "<h1 class='text-white text-uppercase mb-10'>Developer: " & Session("developer") & "</h1> " &
                                    "<p class='text-white mb-30'>For registering generating unit(s), you have to first furnish the project details. Under that project you can add one or more generating units by clicking on Add Unit Button.<br/>Please choose the Action accordingly.</p>" &
                                    " <a href='addproj.aspx' class='primary-btn d-inline-flex align-items-center'><span class='mr-10'>Add New Project</span><span class='lnr lnr-arrow-right'></span></a>"
            divHed.InnerHtml = "<b>Your Projects:</b> Add Generating Unit(s)"

            Dim q = "select project_code, projectname from Projects where not developer = '' and developer = '" & Session("developer") & "'"
            Dim mydt = getDBTable(q)
            If mydt.Rows.Count > 0 Then
                ddlProject.DataSource = mydt
                ddlProject.DataBind()

                btnAddUnit.Visible = True

                '' show unit details
                ddlProject_SelectedIndexChanged(vbNull, EventArgs.Empty)
            Else
                ddlProject.Items.Add(New ListItem("You have not added any Projects", "NA"))
                btnAddProj.Visible = True
            End If

            executeDB("insert into login (eid, log, logintime) values ('" & Request.UserHostAddress & "', 'genhome.aspx Access at: " & Now.ToString() & " by " & Session("ceauser") & "', current_timestamp)")


        End If
        '' do postback stuff here
        '    Response.Write("<div id='bottomline'>At Page Load" & e1.Message & "</div>")
        'End Try
    End Sub
    Public Function projAction(ByVal project_code As String, ByVal unit As String) As Boolean
        '' if status is approved then don't allow to change then dont allow t
        Dim status = getDBsingle("select approvestatus from Units where project_code = '" & project_code & "' and unit = '" & unit & "'")
        If status = "Approved" Then Return False  ''allow to enter data
        Return True
    End Function

    Private Sub btnAddUnit_Click(sender As Object, e As EventArgs) Handles btnAddUnit.Click
        'rblUnitType.Items.Clear()
        'rblUnitType.Items.Add(New ListItem("Unit under Planning Stage", "new"))
        'rblUnitType.Items.Add(New ListItem("Unit under Construction Stage", "ongoing"))
        'rblUnitType.Items.Add(New ListItem("Existing Generating Unit", "comissioned"))
        'rblUnitType.Visible = True
        Response.Redirect("addunit.aspx?project_code=" & ddlProject.SelectedValue)
    End Sub

    Private Sub ddlProject_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlProject.SelectedIndexChanged
        Dim mydt = getDBTable("select project_code, unit, capacity, approvestatus, regnumber from Units where project_code = " & ddlProject.SelectedValue)
        If mydt.Rows.Count > 0 Then
            gvMyProjs.Visible = True
            gvMyProjs.DataSource = mydt
            gvMyProjs.DataBind()

        Else
            gvMyProjs.Visible = False
        End If
        ''' check if project details are completed
        ''' 
        Dim ret = getDBsingle("select address from Projects where not address is null and project_code = " & ddlProject.SelectedValue & " limit 1")
        If String.IsNullOrEmpty(ret) Then
            divInfo.InnerHtml = getMessage("Project detail incomplete " & ret, "info") 'success error info warning
            btnCompleteProj.Visible = True
            btnAddUnit.Visible = False
            Exit Sub
        Else
            divInfo.InnerHtml = getMessage("Project detail complete. You can now add units ", "info") 'success error info warning
            btnCompleteProj.Visible = False
            btnAddUnit.Visible = True
            btnAddUnit.Text = "Add Generating Unit for " & ddlProject.SelectedItem.Text
        End If


    End Sub

    Private Sub gvMyProjs_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvMyProjs.RowCommand
        Dim value = e.CommandArgument.ToString()
        If e.CommandName = "View" Then
            Dim q = "select '' as test, Projectname as Project, u.stage, sector, u.Fuel, u.fuel_linkage as 'Fuel Linkage', Address, District, State, region, latitude, longitude, owner as 'Name of Concerned Persons(s) for Info', concat('Phone: ', phone, ' Fax:', fax, ' Email:', email) as Contact,  unit as 'Unit No', capacity as 'Capacity (in MW)', tech as 'Technology', grid as 'Grid Type', loa_dt as 'Letter of Award (LoA) Date', ant_actcod as 'Commissioning Date', captive as 'Name of the Industry/Installation, in case of Captive', approvestatus as 'Approve Status', regnumber as 'Registration Number'  from Units u, Projects p where p.project_code = u.project_code and u.project_code = " & ddlProject.SelectedValue & " and unit = " & value & " limit 1 "

            pnlForm.Controls.Add(CreateVerticalViewofSingleRow(q))
            divInfo.InnerHtml = getMessage("Showing Unit details of selected Project", "info")

        ElseIf e.CommandName = "Print" Then
            '   Session("regnumber") = value
            Response.Redirect("content.aspx?primarykey=project_code%3d" & ddlProject.SelectedValue & " and unit=" & value)
        Else
            '  divInfo.InnerHtml = getMessage("Let the CEA Team decide additional details", "info") 'success error info warning
            ''' A check is needed to not allow update for other user of same developer.
            '''  to do that
            '''  
            Dim ceauser = getDBsingle("select lastupdateby from Units where project_code = " & ddlProject.SelectedValue & " and unit = " & value & " limit 1")
            If Not ceauser.Contains(Session("ceauser")) Then
                divInfo.InnerHtml = getMessage("Your id " & Session("ceauser") & " can not update Unit detail entered by User" & ceauser, "error")
                Exit Sub
            End If
            Response.Redirect("gen.aspx?form=updateUnit&primarykey=project_code%3d" & ddlProject.SelectedValue & " and unit=" & value)
        End If
        ' divMsg.InnerHtml = q

    End Sub

    Private Sub btnAddProj_Click(sender As Object, e As EventArgs) Handles btnAddProj.Click
        Response.Redirect("addproj.aspx")
    End Sub

    Private Sub btnCompleteProj_Click(sender As Object, e As EventArgs) Handles btnCompleteProj.Click
        Response.Redirect("gen.aspx?form=addProj&primarykey=project_code=" & ddlProject.SelectedValue)
    End Sub
End Class
