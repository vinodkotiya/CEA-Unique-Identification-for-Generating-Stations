Imports dbOperation

Imports ceaCommon

Imports System.Data
Partial Class addproj
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
                                    " <a href='#yearselect' class='primary-btn d-inline-flex align-items-center'><span class='mr-10'>You are entering Project Basic Details</span><span class='lnr lnr-arrow-right'></span></a>"
            divHed.InnerHtml = "<b>Adding a new Project:</b>"
            divInfo.InnerHtml = getMessage("Enter the project details. Check existing projects from list.", "info")
            'executeDB("insert into login (eid, log, logintime) values (" & Session("peid") & " , 'PDashboard Authorized Access: " & Now.ToString() & " - " & Request.UserHostAddress & "', current_timestamp)", "logdb")
            Dim mydt = getDBTable("select projectname from Projects where developer='" & Session("developer") & "' and not developer is null and not developer = ''")
            ddlExistingProject.DataSource = mydt
            ddlExistingProject.DataBind()

            If mydt.Rows.Count = 0 Then
                ddlExistingProject.Items.Add(New ListItem("No existing projects", "NA"))
                ddlExistingProject.SelectedValue = "NA"
            Else
                ddlExistingProject.Items.Add(New ListItem("See already added Projects", "NA"))
                ddlExistingProject.SelectedValue = "NA"
            End If
            ddlDistrict.DataSource = getDBTable("select district, concat(district , ' (' , state , ')' ) as name  from states where 1 order by state, district")
            ddlDistrict.DataBind()
            ddlDistrict.Items.Add(New ListItem("Select District from the list", "NA"))
            ddlDistrict.SelectedValue = "NA"


        End If
        '' do postback stuff here
        '    Response.Write("<div id='bottomline'>At Page Load" & e1.Message & "</div>")
        'End Try
    End Sub

    Private Sub btnSignUp_Click(sender As Object, e As EventArgs) Handles btnSignUp.Click

        If rblType.SelectedIndex <= -1 Then
            divInfo.InnerHtml = getMessage("Select utility type", "error") 'success error info warning
            Exit Sub
        End If

        If rblProjType.SelectedIndex <= -1 Then
            divInfo.InnerHtml = getMessage("Select Project Type", "error") 'success error info warning
            Exit Sub
        End If

        If rblStatus.SelectedIndex <= -1 Then
            divInfo.InnerHtml = getMessage("Select Project Status", "error") 'success error info warning
            Exit Sub
        End If

        'If checkForSQLInjection(txtSignupPassword.Text) Then
        '    divInfo.InnerHtml = getMessage("Some invalid characters found in Password. Use pure alphanumeric ID", "error") 'success error info warning
        '    Exit Sub
        'End If
        If ddlExistingProject.SelectedValue = "NA" And cbkDeveloper.Checked = False Then
            divInfo.InnerHtml = getMessage("Select Project name from List", "error") 'success error info warning
            Exit Sub
        End If
        If cbkDeveloper.Checked = True Then
            If ContainsSpecialChars(txtProjectName.Text) Then
                divInfo.InnerHtml = getMessage("Project name contains special character", "error") 'success error info warning
                Exit Sub
            End If
            If txtProjectName.Text.Length < 8 Then
                divInfo.InnerHtml = getMessage("Project name must have atleast 8 character", "error") 'success error info warning
                Exit Sub
            End If
            If isNumeric(txtProjectName.Text) Then
                divInfo.InnerHtml = getMessage("Project Name Can't be numeric", "error") 'success error info warning
                Exit Sub
            End If
            '' check if project already exist with same name
            Dim q1 = "select projectname from Projects where projectname = '" & txtProjectName.Text & "' and developer='" & Session("developer") & "' limit 1"
            Dim rt = getDBsingle(q1)
            If rt = txtProjectName.Text Then
                divInfo.InnerHtml = getMessage("Project Name already exist. Duplicate projects not allowed." & rt, "error") 'success error info warning
                Exit Sub
            Else
                divInfo.InnerHtml = getMessage("Project Name not exist. " & rt, "info") 'success error info warning
                '    Exit Sub
            End If
        End If

        '' Everything is fine now save the data
        divInfo.InnerHtml = getMessage("Everything is fine now save the data", "success") 'success error info warning

        lbSure.Visible = True
        btnSignUp.Visible = False
        btnSubmit.Visible = True
        btnCancel.Visible = True
        'Else
        '    divInfo.InnerHtml = getMessage("Failed to insert", "error") 'success error info warning
        'End If

    End Sub

    Private Sub rblStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblStatus.SelectedIndexChanged
        If rblStatus.SelectedValue = "Conceptualisation" And rblProjType.SelectedValue = "Hydro" Then
            rblHydro.Visible = True
        Else
            rblHydro.Visible = False
        End If
        '    divInfo.InnerHtml = getMessage("clicked " & rblProjType.SelectedValue, "info") 'success error info warning
    End Sub

    Private Sub ddlDistrict_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDistrict.SelectedIndexChanged
        lbState.Text = getDBsingle("select state from states where district = '" & ddlDistrict.SelectedValue & "' limit 1")
        lbRegion.Text = getDBsingle("select region from states where district = '" & ddlDistrict.SelectedValue & "' limit 1")
    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Dim projectname = ""
        If cbkDeveloper.Checked = True Then projectname = txtProjectName.Text.Replace("'", "")
        Dim substatus = rblHydro.SelectedValue
        If Not (rblStatus.SelectedValue = "Conceptualisation" And rblProjType.SelectedValue = "Hydro") Then substatus = "NA"
        Dim q = "insert into Projects (type, projtype,status,substatus,ProjectName, district, state, region, developer, ceauser,last_updated,lastupdateby) " &
        " values('" & rblType.SelectedValue & "','" & rblProjType.SelectedValue & "','" & rblStatus.SelectedValue & "','" & substatus & "','" & projectname & "','" & ddlDistrict.SelectedValue & "','" & lbState.Text & "','" & lbRegion.Text & "','" & Session("developer") & "','" & Session("ceauser") & "',current_timestamp,'" & Request.UserHostAddress & "'); SELECT LAST_INSERT_ID();"

        Dim projectid = getDBsingle(q)
        ' If ret = "ok" Then
        divInfo.InnerHtml = getMessage("Data insewrted succesfully " & projectid, "info") 'success error info warning

        '''
        Response.Redirect("gen.aspx?form=addProj&primarykey=project_code=" & projectid)

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Response.Redirect("genhome.aspx")
    End Sub
End Class
