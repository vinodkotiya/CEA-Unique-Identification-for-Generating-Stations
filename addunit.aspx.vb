Imports dbOperation

Imports ceaCommon

Imports System.Data
Partial Class addunit
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

            divLogin.InnerHtml = "<h1 class='text-white text-uppercase mb-10'>GENERATING UNIT(S) REGISTRATION</h1> " &
                                    "<p class='text-white mb-30'>" & Session("developer") & "</p> <p class='text-white mb-30'>Please furnish the details.</p>" &
                                    " <a href='#yearselect' class='primary-btn d-inline-flex align-items-center'><span class='mr-10'>You are entering Unit Basic Details</span><span class='lnr lnr-arrow-right'></span></a>"
            divHed.InnerHtml = "<b>Adding a new Unit:</b>"
            divInfo.InnerHtml = getMessage("Enter the Unit details. Check existing Units from list.", "info")
            'executeDB("insert into login (eid, log, logintime) values (" & Session("peid") & " , 'PDashboard Authorized Access: " & Now.ToString() & " - " & Request.UserHostAddress & "', current_timestamp)", "logdb")
            Dim project_code = Request.Params("project_code")
            lbProjectCode.Text = project_code
            lbProjectName.Text = getDBsingle("select projectname from Projects where project_code =" & Request.Params("project_code") & " limit 1")
            lbProjectType.Text = getDBsingle("select projtype from Projects where project_code =" & Request.Params("project_code") & " limit 1")
            ddlFuel.DataSource = getDBTable("select fuel from Fuels where cat ='" & lbProjectType.Text & "'")
            ddlFuel.DataBind()
            ddlFuel_SelectedIndexChanged(vbNull, AdCreatedEventArgs.Empty)

        End If
        '' do postback stuff here
        '    Response.Write("<div id='bottomline'>At Page Load" & e1.Message & "</div>")
        'End Try
    End Sub

    Private Sub btnSignUp_Click(sender As Object, e As EventArgs) Handles btnSignUp.Click

        If ddlFuel.SelectedIndex <= -1 Then
            divInfo.InnerHtml = getMessage("Select Fuel", "error") 'success error info warning
            Exit Sub
        End If

        If ddlTech.SelectedIndex <= -1 Then
            divInfo.InnerHtml = getMessage("Select Technology", "error") 'success error info warning
            Exit Sub
        End If
        If ddlFuel.SelectedValue = "Thermal" And String.IsNullOrEmpty(txtFuelLinkage.Text) Then
            divInfo.InnerHtml = getMessage("Enter Fuel Linkage", "error") 'success error info warning
            Exit Sub
        End If
        If Not isNumeric(txtUnitNo.Text) Then
            divInfo.InnerHtml = getMessage("Unit to be numeric", "error") 'success error info warning
            Exit Sub
        End If
        '' check if unit already exist with same numbr
        Dim q1 = "select unit from Units where project_code = '" & lbProjectCode.Text & "' and unit='" & txtUnitNo.Text & "' limit 1"
        Dim rt = getDBsingle(q1)
        If rt = txtUnitNo.Text Then
            divInfo.InnerHtml = getMessage("Unit Number already exist. Duplicate Units not allowed." & rt & q1, "error") 'success error info warning
            Exit Sub
        Else
            divInfo.InnerHtml = getMessage("Unit Number not exist. " & rt, "info") 'success error info warning
            '    Exit Sub
        End If
        '' Everything is fine now save the data
        divInfo.InnerHtml = getMessage("Everything is fine now save the data", "info") 'success error info warning
        lbSure.Visible = True
        btnSignUp.Visible = False
        btnSubmit.Visible = True
        btnCancel.Visible = True
        'Else
        '    divInfo.InnerHtml = getMessage("Failed to insert", "error") 'success error info warning
        'End If

    End Sub



    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Dim projectname = ""
        Dim q = "insert into Units (project_code,project, unit, capacity,fuel, fuel_linkage,tech, approvestatus, developer, last_updated,lastupdateby) " &
        " values('" & lbProjectCode.Text & "','" & lbProjectName.Text & "','" & txtUnitNo.Text & "','" & txtCapacity.Text & "','" & ddlFuel.SelectedValue & "','" & txtFuelLinkage.Text & "','" & ddlTech.SelectedValue & "','Incomplete Data','" & Session("developer") & "',current_timestamp,'" & Request.UserHostAddress & "-" & Session("ceauser") & "');"

        Dim ret = executeDB(q)
        If ret = "ok" Then
            divInfo.InnerHtml = getMessage("Data insewrted succesfully " & txtUnitNo.Text, "info") 'success error info warning
            Response.Redirect("gen.aspx?form=updateUnit&primarykey=project_code%3d" & lbProjectCode.Text & " and unit=" & txtUnitNo.Text)
        End If


    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Response.Redirect("genhome.aspx")
    End Sub

    Private Sub ddlFuel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlFuel.SelectedIndexChanged
        ddlTech.DataSource = getDBTable("select tech from tech where fuel like '%" & ddlFuel.SelectedValue & "%'")
        ddlTech.DataBind()
        If ddlFuel.SelectedValue = "Thermal" Then txtFuelLinkage.Text = Visible
    End Sub
End Class
