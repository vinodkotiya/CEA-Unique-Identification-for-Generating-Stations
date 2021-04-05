Imports dbOperation

Imports ceaCommon

Imports System.Data
Partial Class gen
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
        If Session("ceauser") Is Nothing Or String.IsNullOrEmpty(Session("ceauser")) Then
            Response.Redirect("cealogin.aspx")
            Exit Sub
        End If
        '  Try

        If Not Page.IsPostBack Then
            divUSer.InnerHtml = "	<h6 class='title'>User: " & Session("ceauser") & "</h6>"

            executeDB("insert into login (eid, log, logintime) values ('" & Request.UserHostAddress & "', 'gen.aspx Access at: " & Now.ToString() & " by " & Session("ceauser") & "', current_timestamp)")


        End If
        '' do postback stuff here

        '###############################################################################
        '"###################### Create Fresh Form ######################################
        '  call just function  
        ' use params dictionary for passing values as parameter to load on controls, Identified by PARAM1, PARAM2 mentioned in database
        '  pnlForm.Controls.Add(CreateDynamicControls("addproj", False, "", params))
        '  Supply formid in parameter and all controls will be returned in place holder
        '   Add this placeholder to any panel in your web page
        '###############################################################################
        '##################### Updation Form ###########################################
        ' second parameter loadforupdate should be true and MatchPrimaryKey should be project_code = '157' 
        ' pnlForm.Controls.Add(CreateDynamicControls("addproj", True, "project_code = 151"))
        '  It will retrieve old values using primary key and prefill the form for update.
        ' if you have multiple tables in Form then all table must have same key for retrival
        '  #############################################################################
        '  #############################################################################
        '  ##################   InsertFormData #########################################
        '  Dim ret = InsertForm("addproj", PlaceHolder1)
        '  #############################################################################
        '  ####################  Update Form Data ######################################
        '    Dim ret = updateForm("addproj", PlaceHolder1, "project_code = '151'")
        '   if you have multiple tables in Form  and wand to get updated multiple tables 
        '   then all table must have same key for updation
        ' ##############################################################################
        ' ... Add two pairs to it.
        Dim WelcomeText = ""
        Dim params As New Dictionary(Of String, String)  ''Load parameter value as default value
        ''''' For all labels you need to supply PARAM or Default value for update action else it will come blank
        params.Add("PARAM1", Session("developer"))
        params.Add("PARAM2", Session("ceauser") & " " & Request.UserHostAddress.Replace("'", ""))
        params.Add("PARAM3", Now.ToString("yyyy-MM-dd HH:mm:ss"))
        params.Add("PARAM4", Session("ceauser"))
        If Request.Params("form") = "addProj" Then
            'pnlForm.Controls.Add(CreateDynamicControls("addproj", False, "", params))
            ''gen.aspx?form=projAdd
            params("PARAM1") = getDBsingle("select developer from Projects where " & Request.Params("primarykey") & " limit 1")
            params.Add("PARAM5", getDBsingle("select projectname from Projects where " & Request.Params("primarykey") & " limit 1"))
            ' pnlForm.Controls.Add(CreateDynamicControls("addproj", True, "project_code = '151'"))
            pnlForm.Controls.Add(CreateDynamicControls("addproj", True, Request.Params("primarykey"), params))
            WelcomeText = "You are entering Project details for one/many Generating Units."
            divHed.InnerHtml = "Adding New Project for generating unit(s)"
        ElseIf Request.Params("form") = "updateProj" Then
            params("PARAM1") = getDBsingle("select developer from Projects where " & Request.Params("primarykey") & " limit 1")
            params("PARAM4") = getDBsingle("select ceauser from Projects where " & Request.Params("primarykey") & " limit 1")
            params.Add("PARAM5", getDBsingle("select projectname from Projects where " & Request.Params("primarykey") & " limit 1"))
            ' pnlForm.Controls.Add(CreateDynamicControls("addproj", True, "project_code = '151'"))
            pnlForm.Controls.Add(CreateDynamicControls("addproj", True, Request.Params("primarykey"), params))
            ''gen.aspx?form=updateProj&primarykey=project_code%3d161
            WelcomeText = "You are updating Project details for one/many Generating Units."
            divHed.InnerHtml = "Updating Project Detail for generating unit(s) by CEA"
        ElseIf Request.Params("form") = "addUnit" Then
            params.Add("PARAM7", Request.Params("project_code"))
            params.Add("PARAM5", getDBsingle("select projectname from Projects where project_code =" & Request.Params("project_code") & " limit 1"))
            params.Add("PARAM6", "1")
            Dim ret = getDBsingle("select max(unit) + 1 from Units where project_code =" & Request.Params("project_code") & " limit 1")
            If Not ret.Contains("Error") And Not String.IsNullOrEmpty(ret.ToString) Then params("PARAM6") = ret
            pnlForm.Controls.Add(CreateDynamicControls("addUnit", False, "", params))
            'gen.aspx?form=projAdd
            WelcomeText = "You are entering Gnerating Unit details."
            divHed.InnerHtml = "Adding New generating unit"
        ElseIf Request.Params("form") = "updateUnit" Then
            params("PARAM1") = getDBsingle("select developer from Units where " & Request.Params("primarykey") & " limit 1")
            params.Add("PARAM7", getDBsingle("select project_code from Units where  " & Request.Params("primarykey") & " limit 1"))
            params.Add("PARAM5", getDBsingle("select project from Units where  " & Request.Params("primarykey") & " limit 1"))
            params.Add("PARAM6", getDBsingle("select unit from Units where  " & Request.Params("primarykey") & " limit 1"))
            pnlForm.Controls.Add(CreateDynamicControls("addUnit", True, Request.Params("primarykey"), params))
            ''gen.aspx?form=updateUnit&primarykey=project_code%3d161 and unit=1
            WelcomeText = "You are Updating Gnerating Unit details."
            divHed.InnerHtml = "Updating generating unit"
        ElseIf Request.Params("form") = "approveUnit" Then
            Dim project_code = getDBsingle("select project_code from Units where  " & Request.Params("primarykey") & " limit 1")
            Dim unit = getDBsingle("select unit from Units where  " & Request.Params("primarykey") & " limit 1")
            params.Add("PARAM7", project_code)
            params.Add("PARAM5", getDBsingle("select project from Units where  " & Request.Params("primarykey") & " limit 1"))
            params.Add("PARAM6", generateRegistrationNumber(project_code, unit))
            pnlForm.Controls.Add(CreateDynamicControls("approveUnit", True, Request.Params("primarykey"), params))
            ''gen.aspx?form=updateUnit&primarykey=project_code%3d161 and unit=1
            WelcomeText = "You are Approving Gnerating Unit."
            divHed.InnerHtml = "Approval of generating unit"
        End If

        divLogin.InnerHtml = "<h1 class='text-white text-uppercase mb-10'>Generating Unit(s) Registration</h1> " &
                                    "<p class='text-white mb-30'><b>" & Session("developer") & " </b>.Please Furnish the Details</p>" &
                                    " <a href='#yearselect' class='primary-btn d-inline-flex align-items-center'><span class='mr-10'>" & WelcomeText & "</span></a>"

        'Catch e1 As Exception
        '    Response.Write("<div id='bottomline'>At Page Load" & e1.Message & "</div>")
        'End Try
    End Sub


    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click

        '###############################################################################
        '"###################### Create Fresh Form ######################################
        '  call just function  
        '  pnlForm.Controls.Add(CreateDynamicControls("addproj"))
        '  Supply formid in parameter and all controls will be returned in place holder
        '   Add this placeholder to any panel in your web page
        '###############################################################################
        '##################### Updation Form ###########################################
        ' second parameter loadforupdate should be true and MatchPrimaryKey should be project_code = '157' 
        ' pnlForm.Controls.Add(CreateDynamicControls("addproj", True, "project_code = 151"))
        '  It will retrieve old values using primary key and prefill the form for update.
        ' if you have multiple tables in Form then all table must have same key for retrival
        '  #############################################################################
        '  #############################################################################
        '  ##################   InsertFormData #########################################
        '  Dim ret = InsertForm("addproj", PlaceHolder1)
        '  #############################################################################
        '  ####################  Update Form Data ######################################
        '    Dim ret = updateForm("addproj", PlaceHolder1, "project_code = '151'")
        '   if you have multiple tables in Form  and wand to get updated multiple tables 
        '   then all table must have same key for updation
        ' ##############################################################################
        Dim PlaceHolder1 As PlaceHolder = CType(pnlForm.FindControl("PlaceHolder1"), PlaceHolder)
        Dim ret = "No Action"
        If Request.Params("form") = "addProj" Then
            '  ret = InsertForm("addproj", PlaceHolder1)
            ret = updateForm("addproj", PlaceHolder1, Request.Params("primarykey"))
        ElseIf Request.Params("form") = "updateProj" Then
            ret = updateForm("addproj", PlaceHolder1, Request.Params("primarykey"))
        ElseIf Request.Params("form") = "addUnit" Then
            ret = InsertForm("addUnit", PlaceHolder1)
        ElseIf Request.Params("form") = "updateUnit" Then
            ret = updateForm("addUnit", PlaceHolder1, Request.Params("primarykey"))
        ElseIf Request.Params("form") = "approveUnit" Then
            ret = updateForm("approveUnit", PlaceHolder1, Request.Params("primarykey"))
        End If
        Dim redirecturl = "genhome.aspx"
        If Not Request.Params("redirecturl") Is Nothing Then redirecturl = Request.Params("redirecturl")
        divInfo.InnerHtml = getMessage(ret, "info") 'success error info warning
        If ret.Contains("#Success#") Then
            '' Submit succesfull, 
            Response.Redirect(redirecturl & "?success")
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Dim redirecturl = "genhome.aspx"
        Response.Redirect(redirecturl & "?success")
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        btnSave.Visible = False
        btnSubmit.Visible = True
        btnCancel.Visible = True
        lbConfirm.Visible = True
    End Sub
End Class
