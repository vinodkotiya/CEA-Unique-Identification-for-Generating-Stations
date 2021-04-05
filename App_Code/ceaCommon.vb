Imports Microsoft.VisualBasic
Imports dbOperation
Imports System.Data


Public Class ceaCommon
    Public Shared Function getCC(ByVal pageload As String) As String
        Return "<p>HTML5 | Cache Enabled | AJAX | Page creation Time:" & pageload.ToString & " ms | Design & Developed By: <b style='color:white'>NTPC Limited (c) 2018</b> | All Time Visit: " & getDBsingle("select max(loginid) from login where 1 limit 1") & " </p>"
    End Function
    Public Shared Function getMessage(ByVal msg As String, ByVal type As String) As String
        If type = "info" Then
            Return "<div class='info message'> <h3>Info!</h3> <p>" & msg & "</p></div>"
        End If
        If type = "error" Then
            Return "<div class='error  message'> <h3>Error!</h3> <p>" & msg & "</p></div>"
        End If
        If type = "warning" Then
            Return "<div class='warning  message'> <h3>Warning!</h3> <p>" & msg & "</p></div>"
        End If
        If type = "success" Then
            Return "<div class='success  message'> <h3>Success!</h3> <p>" & msg & "</p></div>"
        End If
        Return msg
    End Function
    Public Shared Function checkForSQLInjection(ByVal userInput As String) As Boolean
        Dim isSQLInjection As Boolean = False
        Dim sqlCheckList As String() = {"--", ";--", ";", "/*", "*/", "@@", "char", "nchar", "varchar", "nvarchar", "alter", "begin", "cast", "create", "cursor", "declare", "delete", "drop", "end", "exec", "execute", "fetch", "insert", "kill", "select", "sys", "sysobjects", "syscolumns", "table", "update"}
        Dim CheckString As String = userInput.Replace("'", "''")

        For i As Integer = 0 To sqlCheckList.Length - 1

            If (CheckString.IndexOf(sqlCheckList(i), StringComparison.OrdinalIgnoreCase) >= 0) Then
                isSQLInjection = True
            End If
        Next

        Return isSQLInjection
    End Function
    Public Shared Function isEmail(s As String) As Boolean
        Return Regex.IsMatch(s, "^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$")
    End Function
    Public Shared Function ContainsSpecialChars(s As String) As Boolean
        Return s.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1
    End Function
    Public Shared Function isNumeric(ByVal val As String) As Boolean
        If Regex.IsMatch(val, "^[0-9 ]+$") Then
            Return True
        Else
            Return False
        End If

    End Function
    Public Shared Function isNumber(ByVal val As String) As Boolean
        Dim d As Double
        Try

            Double.TryParse(val, d)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Shared Function hideRegNo(ByVal regno As String, ByVal approvestatus As String) As String
        If approvestatus = "Under Approval" Or approvestatus = "Rejected" Then
            Return "-"  ''allow to enter data
        Else
            Return regno
        End If

    End Function

    Public Shared Function updateForm(ByVal formid As String, ByVal PlaceHolder1 As PlaceHolder, ByVal MatchPrimaryKey As String) As String
        ''' MatchPrimaryKey should be same in multiple tables for same form
        ''' 
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
        '' get tables in order to make update query
        Try
            Dim mydt = getDBTable("select field_id,field_name,field_type,default_values, required, table_name, table_fname from Field_master where formid = '" & formid & "' and not field_type = 'blank' order by table_name")
            Dim msg = ""
            Dim updateStmt As New System.Collections.Generic.List(Of String)() ' "update currTable set table_fname = '' ,table_fname = ''  where MatchPrimaryKey"
            Dim tables As New System.Collections.Generic.List(Of String)()
            Dim currTable = ""
            For Each r In mydt.Rows
                If currTable <> r("table_name") Then
                    currTable = r("table_name")
                    tables.Add(currTable)
                End If

            Next
            '' now append value fields
            For Each currTable In tables
                Dim values = ""
                Dim foundRows() As System.Data.DataRow
                foundRows = mydt.Select("table_name = '" & currTable & "'")
                For Each r In foundRows
                    Dim FieldName = Convert.ToString(r("field_name"))
                    Dim FieldID As String = Convert.ToString(r("field_id"))
                    Dim required As String = Convert.ToString(r("required"))
                    Dim FieldType = Convert.ToString(r("field_type"))
                    Dim table_fname As String = Convert.ToString(r("table_fname"))
                    If FieldType.ToLower().Trim() = "label" Then
                        Dim lb As Label = CType(PlaceHolder1.FindControl("lbl" & FieldID), Label)
                        If lb IsNot Nothing Then
                            values = values & " " & table_fname & " ='" & lb.Text.Replace("'", "") & "',"
                        End If
                    End If
                    If FieldType.ToLower().Trim() = "text" Or FieldType.ToLower().Trim() = "number" Or FieldType.ToLower().Trim() = "date" Or FieldType.ToLower().Trim() = "email" Then
                        Dim txtbox As TextBox = CType(PlaceHolder1.FindControl("txt" & FieldID), TextBox)
                        If txtbox IsNot Nothing Then
                            If checkForSQLInjection(txtbox.Text) Then Return "Invalid input found in " & FieldName
                            If required = "Y" And String.IsNullOrEmpty(txtbox.Text) Then Return "Required Field: " & FieldName
                            If FieldType.ToLower().Trim() = "text" Then If isNumeric(txtbox.Text) Then Return "Value must be text in " & FieldName
                            If FieldType.ToLower().Trim() = "number" Then If Not isNumber(txtbox.Text) Then Return "Value must be numeric in " & FieldName
                            If FieldType.ToLower().Trim() = "date" Then
                                Dim vin_dt = DateTime.ParseExact(txtbox.Text, "dd.MM.yyyy", Nothing)
                                values = values & " " & table_fname & " ='" & vin_dt.ToString("yyyy-MM-dd") & "',"
                            Else
                                values = values & " " & table_fname & " ='" & txtbox.Text.Replace("'", "") & "',"
                            End If
                        End If
                    End If


                    If FieldType.ToLower().Trim() = "dropdown" Then
                        Dim ddl As DropDownList = CType(PlaceHolder1.FindControl("ddl" & FieldID), DropDownList)
                        If ddl IsNot Nothing Then
                            If required = "Y" And ddl.SelectedIndex < 0 Then Return "Required Field: " & FieldName
                            values = values & " " & table_fname & " ='" & ddl.SelectedValue & "',"
                        End If
                    End If
                    If FieldType.ToLower().Trim() = "checkbox" Then
                        Dim cbl As CheckBoxList = CType(PlaceHolder1.FindControl("cbl" & FieldID), CheckBoxList)
                        If cbl IsNot Nothing Then
                            If required = "Y" And cbl.SelectedIndex < 0 Then Return "Required Field: " & FieldName
                            values = values & " " & table_fname & " ='" & cbl.SelectedValue & "',"
                        End If
                    End If
                    If FieldType.ToLower().Trim() = "radiobutton" Then
                        Dim rbl As RadioButtonList = CType(PlaceHolder1.FindControl("rbl" & FieldID), RadioButtonList)
                        If rbl IsNot Nothing Then
                            If required = "Y" And rbl.SelectedIndex < 0 Then Return "Required Field: " & FieldName
                            values = values & " " & table_fname & " ='" & rbl.SelectedValue & "',"
                        End If
                    End If

                Next
                ''trim lastcomma
                values = values.TrimEnd(",")
                '' append values in insert stmt
                updateStmt.Add(" update  " & currTable & " set " & values & " where " & MatchPrimaryKey)
            Next

            For Each q In updateStmt
                ' divMsg.InnerHtml = divMsg.InnerHtml & q
                msg = msg & "Executing query " '& q & " <br/>"
                Dim ret = executeDB(q)
                If ret = "ok" Then
                    msg = msg & "#Success# " & " <br/>"
                Else
                    msg = msg & "Error in query " & q & " <br/>"
                End If
            Next
            Return msg
        Catch ex As Exception
            Return "Error " & ex.Message
        End Try
    End Function
    Public Shared Function InsertForm(ByVal formid As String, ByVal PlaceHolder1 As PlaceHolder) As String
        '' get tables in order to make insert query

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
        Try
            Dim mydt = getDBTable("select field_id,field_name,field_type,default_values, required, table_name, table_fname from Field_master where formid = '" & formid & "' and not field_type = 'blank' order by table_name")
            Dim msg = ""
            Dim insertStmt As New System.Collections.Generic.List(Of String)() ' "insert into currTable values(table_fname,table_fname)"
            Dim tables As New System.Collections.Generic.List(Of String)()
            Dim currTable = ""
            For Each r In mydt.Rows
                If currTable <> r("table_name") Then
                    currTable = r("table_name")

                    Dim foundRows() As System.Data.DataRow
                    foundRows = mydt.Select("table_name = '" & currTable & "'")
                    Dim commaSeparatedFields As String = String.Join(",", foundRows.AsEnumerable().[Select](Function(x) x.Field(Of String)("table_fname").ToString()).ToArray())
                    insertStmt.Add("insert into " & currTable & " (" & commaSeparatedFields & ") ")
                    tables.Add(currTable)
                End If

            Next
            '' now append value fields
            Dim i = 0
            For Each currTable In tables
                Dim values = ""
                Dim foundRows() As System.Data.DataRow
                foundRows = mydt.Select("table_name = '" & currTable & "'")
                For Each r In foundRows
                    Dim FieldName = Convert.ToString(r("field_name"))
                    Dim FieldID As String = Convert.ToString(r("field_id"))
                    Dim required As String = Convert.ToString(r("required"))
                    Dim FieldType = Convert.ToString(r("field_type"))
                    If FieldType.ToLower().Trim() = "label" Then
                        Dim lb As Label = CType(PlaceHolder1.FindControl("lbl" & FieldID), Label)
                        If lb IsNot Nothing Then
                            values = values & " '" & lb.Text.Replace("'", "") & "',"
                        End If
                    End If
                    If FieldType.ToLower().Trim() = "text" Or FieldType.ToLower().Trim() = "number" Or FieldType.ToLower().Trim() = "date" Or FieldType.ToLower().Trim() = "email" Then
                        Dim txtbox As TextBox = CType(PlaceHolder1.FindControl("txt" & FieldID), TextBox)
                        If txtbox IsNot Nothing Then
                            If checkForSQLInjection(txtbox.Text) Then Return "Invalid input found in " & FieldName
                            If required = "Y" And String.IsNullOrEmpty(txtbox.Text) Then Return "Required Field: " & FieldName
                            If FieldType.ToLower().Trim() = "text" Then If isNumeric(txtbox.Text) Then Return "Value must be text in " & FieldName
                            If FieldType.ToLower().Trim() = "number" Then If Not isNumber(txtbox.Text) Then Return "Value must be numeric in " & FieldName
                            If FieldType.ToLower().Trim() = "date" Then
                                Dim vin_dt = DateTime.ParseExact(txtbox.Text, "dd.MM.yyyy", Nothing)
                                values = values & " '" & vin_dt.ToString("yyyy-MM-dd") & "',"
                            Else
                                values = values & " '" & txtbox.Text.Replace("'", "") & "',"
                            End If
                        End If
                    End If

                    If FieldType.ToLower().Trim() = "dropdown" Then
                        Dim ddl As DropDownList = CType(PlaceHolder1.FindControl("ddl" & FieldID), DropDownList)
                        If ddl IsNot Nothing Then
                            If required = "Y" And ddl.SelectedIndex < 0 Then Return "Required Field: " & FieldName
                            values = values & " '" & ddl.SelectedValue & "',"
                        End If
                    End If
                    If FieldType.ToLower().Trim() = "checkbox" Then
                        Dim cbl As CheckBoxList = CType(PlaceHolder1.FindControl("cbl" & FieldID), CheckBoxList)
                        If cbl IsNot Nothing Then
                            If required = "Y" And cbl.SelectedIndex < 0 Then Return "Required Field: " & FieldName
                            values = values & " '" & cbl.SelectedValue & "',"
                        End If
                    End If
                    If FieldType.ToLower().Trim() = "radiobutton" Then
                        Dim rbl As RadioButtonList = CType(PlaceHolder1.FindControl("rbl" & FieldID), RadioButtonList)
                        If rbl IsNot Nothing Then
                            If required = "Y" And rbl.SelectedIndex < 0 Then Return "Required Field: " & FieldName
                            values = values & " '" & rbl.SelectedValue & "',"
                        End If
                    End If

                Next
                ''trim lastcomma
                values = values.TrimEnd(",")
                '' append values in insert stmt
                insertStmt(i) = insertStmt(i) & " value (" & values & ") "
                i = i + 1
            Next

            For Each q In insertStmt
                ' divMsg.InnerHtml = divMsg.InnerHtml & q
                msg = msg & "Executing query " '& q & " <br/>"
                Dim ret = executeDB(q)
                If ret = "ok" Then
                    msg = msg & "#Success#  " & " <br/>"
                Else
                    msg = msg & "Error in query " & q & " <br/>"
                End If
            Next
            Return msg
        Catch ex As Exception
            Return "Error " & ex.Message
        End Try
    End Function
    Public Shared Function CreateDynamicControls(ByVal formid As String, Optional ByVal loadForUpdate As Boolean = False, Optional ByVal MatchPrimaryKey As String = "Needed if you want to get form for updation with prefilled values", Optional ByVal params As Dictionary(Of String, String) = Nothing) As PlaceHolder

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
        Try
            Dim dt = getDBTable("select field_id, field_name,field_type,default_values, required, table_name, table_fname from Field_master where  not field_type = 'blank' and formid = '" & formid & "' order by printorder")
            '  dt = CustomFields()
            Dim PlaceHolder1 As PlaceHolder = New PlaceHolder()
            PlaceHolder1.ID = "PlaceHolder1"
            If dt.Rows.Count > 0 Then

                For i As Int32 = 0 To dt.Rows.Count - 1

                    Dim tr As HtmlGenericControl = New HtmlGenericControl("tr")
                    Dim td As HtmlGenericControl = New HtmlGenericControl("td")
                    Dim td1 As HtmlGenericControl = New HtmlGenericControl("td")
                    Dim FieldName As String = Convert.ToString(dt.Rows(i)("field_name"))
                    Dim FieldID As String = Convert.ToString(dt.Rows(i)("field_id"))
                    Dim FieldType As String = Convert.ToString(dt.Rows(i)("field_type"))
                    Dim FieldValue As String = Convert.ToString(dt.Rows(i)("default_values"))
                    Dim required As String = Convert.ToString(dt.Rows(i)("required"))
                    Dim table As String = Convert.ToString(dt.Rows(i)("table_name"))
                    Dim field As String = Convert.ToString(dt.Rows(i)("table_fname"))
                    Dim star = "*"
                    If Not required.Contains("Y") Then star = ""
                    Dim lbcustomename As Label = New Label()
                    lbcustomename.Text = FieldName & star
                    lbcustomename.ID = "lb" & FieldID

                    lbcustomename.CssClass = "form-label"
                    td.Controls.Add(lbcustomename)
                    tr.Controls.Add(td)
                    If FieldType.ToLower().Trim() = "label" Then
                        Dim lb As Label = New Label()
                        lb.ID = "lbl" & FieldID
                        lb.Text = FieldValue
                        '''' Load Param value passed in parameter for both add/update forms
                        If FieldValue.StartsWith("PARAM") Then
                            Try
                                lb.Text = params.Item(FieldValue)
                            Catch ex3 As Exception
                            End Try
                            ''' for update retrive old values
                            'If loadForUpdate Then lb.Text = getDBsingle("select " & field & " from " & table & " where " & MatchPrimaryKey & " limit 1")
                        End If

                        lb.CssClass = "form-control"  'mt-20
                        td1.Controls.Add(lb)
                        PlaceHolder1.Controls.Add(New LiteralControl("<br />"))
                    ElseIf FieldType.ToLower().Trim() = "text" Or FieldType.ToLower().Trim() = "number" Or FieldType.ToLower().Trim() = "date" Or FieldType.ToLower().Trim() = "email" Then
                        Dim txtcustombox As TextBox = New TextBox()
                        txtcustombox.ID = "txt" & FieldID
                        If loadForUpdate Then
                            ' update form load with key values
                            If FieldType.ToLower().Trim() = "date" Then field = "DATE_FORMAT(" & field & ", '%d.%m.%Y')"
                            txtcustombox.Text = getDBsingle("select " & field & " from " & table & " where " & MatchPrimaryKey & " limit 1")
                        Else
                            ''' Fresh form load with default values
                            txtcustombox.Text = FieldValue
                        End If
                        '''' Load Param value passed in parameter for both add/update forms
                        If FieldValue.StartsWith("PARAM") Then
                            Try
                                txtcustombox.Text = params.Item(FieldValue)
                            Catch ex3 As Exception
                            End Try
                        End If
                        txtcustombox.CssClass = "form-control"  'mt-20
                        'If FieldType.ToLower().Trim() = "date" Then txtcustombox.CssClass = "form-control vindateClass" ''needed for jquery calander but now using ajax
                        td1.Controls.Add(txtcustombox)

                        If (FieldName = "Name of Concerned Persons(s) for Info") Then
                            Dim RegExName = New RegularExpressionValidator
                            RegExName.ID = "RegEx" & FieldID
                            RegExName.ControlToValidate = "txt" & FieldID
                            RegExName.ValidationExpression = "^[A-Za-z\s]{1,}[\.]{0,1}[A-Za-z\s]{0,}$"
                            RegExName.ErrorMessage = "Invalid name format"
                            ' RegEx1.Display = "Dynamic"
                            td1.Controls.Add(RegExName)
                        End If
                        If FieldType.ToLower().Trim() = "number" Then  '' add validators for number field
                            If (FieldName.ToLower().Trim() = "longitude") Then
                                Dim RegEx1 = New RegularExpressionValidator
                                RegEx1.ID = "RegEx" & FieldID
                                RegEx1.ControlToValidate = "txt" & FieldID
                                RegEx1.ValidationExpression = "^(\+|-)?(?:180(?:(?:\.0{1,6})?)|(?:[0-9]|[1-9][0-9]|1[0-7][0-9])(?:(?:\.[0-9]{1,6})?))$"
                                RegEx1.ErrorMessage = "Longitude Format Incorrect"
                                ' RegEx1.Display = "Dynamic"
                                td1.Controls.Add(RegEx1)
                            ElseIf (FieldName.ToLower().Trim() = "latitude") Then
                                Dim RegEx2 = New RegularExpressionValidator
                                RegEx2.ID = "RegEx" & FieldID
                                RegEx2.ControlToValidate = "txt" & FieldID
                                RegEx2.ValidationExpression = "^(\+|-)?(?:90(?:(?:\.0{1,6})?)|(?:[0-9]|[1-8][0-9])(?:(?:\.[0-9]{1,6})?))$"
                                RegEx2.ErrorMessage = "Latitude Format Incorrect"
                                '  RegEx2.Display = "Dynamic"
                                td1.Controls.Add(RegEx2)
                            Else
                                Dim comparevalid = New CompareValidator
                                comparevalid.ID = "comp" & FieldID
                                comparevalid.ControlToValidate = "txt" & FieldID
                                comparevalid.Type = ValidationDataType.[Double]
                                comparevalid.Operator = ValidationCompareOperator.DataTypeCheck
                                comparevalid.ErrorMessage = "Only Numeric Value Allowed"
                                td1.Controls.Add(comparevalid)
                            End If
                        End If
                        If FieldType.ToLower().Trim() = "email" Then  '' add validators for email field
                            Dim comparevalid = New RegularExpressionValidator
                            comparevalid.ID = "email" & FieldID
                            comparevalid.ControlToValidate = "txt" & FieldID
                            comparevalid.ValidationExpression = "\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                            comparevalid.ErrorMessage = "Only Emails Allowed"
                            td1.Controls.Add(comparevalid)
                        End If
                        If FieldType.ToLower().Trim() = "date" Then  '' add calander for date field
                            Dim calander = New AjaxControlToolkit.CalendarExtender
                            calander.ID = "cal" & FieldID
                            calander.TargetControlID = "txt" & FieldID
                            calander.Format = "dd.MM.yyyy"
                            td1.Controls.Add(calander)
                        End If
                        PlaceHolder1.Controls.Add(New LiteralControl("<br />"))

                    ElseIf FieldType.ToLower().Trim() = "checkbox" Then
                        Dim chkbox As CheckBoxList = New CheckBoxList()
                        chkbox.ID = "cbl" & FieldID

                        If FieldValue.StartsWith("TABLE") Then
                            Dim tabledetail() = FieldValue.Split(" ")
                            'table name
                            Dim tablename = tabledetail(1)
                            Dim textfield = tabledetail(3)
                            Dim valuefield = tabledetail(5)
                            chkbox.DataTextField = textfield
                            chkbox.DataValueField = valuefield
                            Dim q = "select " & textfield & ", " & valuefield & " from " & tablename
                            ' divMsg.InnerHtml = q
                            chkbox.DataSource = getDBTable(q)
                            chkbox.DataBind()

                        ElseIf FieldValue <> String.Empty Then
                            ''split the comma seperated value
                            Dim items() = FieldValue.Split(",")
                            For Each item In items
                                chkbox.Items.Add(New ListItem(item, item))
                            Next
                            'Else
                            '    divMsg.InnerHtml = "No values"

                        End If
                        If loadForUpdate Then
                            ' update form load with key values
                            Try
                                chkbox.SelectedValue = getDBsingle("select " & field & " from " & table & " where " & MatchPrimaryKey & " limit 1")
                            Catch ex2 As Exception
                            End Try
                        End If

                        td1.Controls.Add(chkbox)
                        PlaceHolder1.Controls.Add(New LiteralControl("<br />"))
                    ElseIf FieldType.ToLower().Trim() = "radiobutton" Then
                        Dim rbnlst As RadioButtonList = New RadioButtonList()
                        'AddHandler rbnlst.SelectedIndexChanged, AddressOf rbnlst_SelectedIndexChanged
                        rbnlst.ID = "rbl" & FieldID
                        If FieldValue.StartsWith("TABLE") Then
                            Dim tabledetail() = FieldValue.Split(" ")
                            'table name
                            Dim tablename = tabledetail(1)
                            Dim textfield = tabledetail(3)
                            Dim valuefield = tabledetail(5)
                            rbnlst.DataTextField = textfield
                            rbnlst.DataValueField = valuefield
                            Dim q = "select " & textfield & ", " & valuefield & " from " & tablename
                            ' divMsg.InnerHtml = q
                            rbnlst.DataSource = getDBTable(q)
                            rbnlst.DataBind()
                            ' rbnlst.AutoPostBack = True
                            'For Each rb In rbnlst.Items
                            '    Dim sval = rb.ToString.Trim()

                            'Next

                        ElseIf FieldValue <> String.Empty Then
                            ''split the comma seperated value
                            Dim items() = FieldValue.Split(",")
                            For Each item In items
                                rbnlst.Items.Add(New ListItem(item, item))
                            Next
                            'Else
                            '    divMsg.InnerHtml = "No values"

                        End If
                        If loadForUpdate Then
                            ' update form load with key values
                            Try
                                rbnlst.SelectedValue = getDBsingle("select " & field & " from " & table & " where " & MatchPrimaryKey & " limit 1")
                            Catch ex2 As Exception
                            End Try
                        End If
                        rbnlst.RepeatDirection = RepeatDirection.Horizontal
                        rbnlst.RepeatColumns = 5
                        td1.Controls.Add(rbnlst)
                        PlaceHolder1.Controls.Add(New LiteralControl("<br />"))

                    ElseIf FieldType.ToLower().Trim() = "dropdown" Then
                        Dim ddllst As DropDownList = New DropDownList()
                        ddllst.ID = "ddl" & FieldID
                        ''check if default value comes from table
                        If FieldValue.StartsWith("TABLE") Then
                            Dim tabledetail() = FieldValue.Split(" ")
                            'table name
                            Dim tablename = tabledetail(1)
                            Dim textfield = tabledetail(3)
                            Dim valuefield = tabledetail(5)
                            ddllst.DataTextField = textfield
                            ddllst.DataValueField = valuefield
                            ddllst.Style.Add("display", "block")
                            Dim q = "select " & textfield & ", " & valuefield & " from " & tablename
                            ' divMsg.InnerHtml = q
                            ddllst.DataSource = getDBTable(q)
                            ddllst.DataBind()

                        ElseIf FieldValue <> String.Empty Then
                            ''split the comma seperated value
                            Dim items() = FieldValue.Split(",")
                            For Each item In items
                                ddllst.Items.Add(New ListItem(item, item))
                            Next
                        End If
                        If loadForUpdate Then
                            ' update form load with key values
                            Try
                                ddllst.SelectedValue = getDBsingle("select " & field & " from " & table & " where " & MatchPrimaryKey & " limit 1")
                            Catch ex2 As Exception
                            End Try
                        End If
                        td1.Controls.Add(ddllst)
                        PlaceHolder1.Controls.Add(New LiteralControl("<br />"))
                    End If



                    tr.Controls.Add(td1)
                    PlaceHolder1.Controls.Add(tr)

                    'If i = dt.Rows.Count - 1 Then
                    '    tr = New HtmlGenericControl("tr")
                    '    td = New HtmlGenericControl("td")
                    '    Dim btnSubmit As Button = New Button()
                    '    btnSubmit.ID = "btnSubmit"
                    '    btnSubmit.CssClass = "primary-btn submit-btn d-inline-flex align-items-center mr-10"
                    '    '  btnSubmit.Style.Add("background-color", "-moz-linear-gradient(0deg, #3e69fe 0%, #4cd4e3 100%)")

                    '    '  btnSubmit.CommandArgument = i.ToString()
                    '    AddHandler btnSubmit.Click, AddressOf Me.btnsubmit_Click
                    '    'btnSubmit.OnClientClick = "return alert ('hi');"
                    '    'btnSubmit.Attributes.Add("runat", "server")
                    '    btnSubmit.Text = "Submit"
                    '    td.Controls.Add(btnSubmit)
                    '    td.Attributes.Add("Colspan", "2")
                    '    td.Attributes.Add("style", "text-align:center;")
                    '    tr.Controls.Add(td)
                    '    PlaceHolder1.Controls.Add(tr)
                    'End If
                Next
            End If
            Return PlaceHolder1
        Catch ex As Exception
            Dim PlaceHolder1 As PlaceHolder = New PlaceHolder()
            PlaceHolder1.ID = "PlaceHolder1"
            PlaceHolder1.Controls.Add(New LiteralControl(" <h3>Error </h3> " & ex.Message))
            Return PlaceHolder1
        End Try
    End Function
    Protected Sub rbnlst_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim rb As RadioButton = DirectCast(sender, RadioButton)
        'now rb is the one that was clicked
    End Sub
    Public Shared Function generateRegistrationNumber(ByVal project_code As String, ByVal unit As String, Optional ByVal company As String = "1") As String
        '10 digit numeric identification number in the format XXXXXXXXXX.  
        'The first digit will indicate whether it Is Generating Company, Transmission Company Or Distribution Company (Generating Company -1; Transmission Company -2; Distribution Company -3). 
        'In case of Generating Company the second digit will indicate the type of generating unit (Hydro-1/Thermal-2/Nuclear-3/RES-4). Digits from 3-10 (8 digits)
        'would be the unique registration number of the generating unit
        Dim r1 = company '1, 2, 3
        '' get category from Fuels
        Dim cat = getDBsingle("SELECT cat FROM `Fuels` WHERE fuel in (select fuel from Projects where project_code=" & project_code & ") limit 1")
        Dim r2 = "X"
        If cat = "Thermal" Then r2 = "2"
        If cat = "Hydro" Then r2 = "1"
        If cat = "Nuclear" Then r2 = "3"
        If cat = "RES" Then r2 = "4"
        Dim r3 = project_code.PadLeft("6", "0")
        Dim r4 = unit.PadLeft("2", "0")
        Return r1 & r2 & r3 & r4
        '' X represent fuel catagoey not found
    End Function

    Public Shared Function PivotTable(oldTable As DataTable,
                            Optional pivotColumnOrdinal As Integer = 0
                           ) As DataTable
        Dim newTable As New DataTable
        Dim dr As DataRow

        ' add pivot column name
        newTable.Columns.Add(oldTable.Columns(pivotColumnOrdinal).ColumnName)

        ' add pivot column values in each row as column headers to new Table
        For Each row In oldTable.Rows
            newTable.Columns.Add(row(pivotColumnOrdinal))
        Next

        ' loop through columns
        For col = 0 To oldTable.Columns.Count - 1
            'pivot column doen't get it's own row (it is already a header)
            If col = pivotColumnOrdinal Then Continue For

            ' each column becomes a new row
            dr = newTable.NewRow()

            ' add the Column Name in the first Column
            dr(0) = oldTable.Columns(col).ColumnName

            ' add data from every row to the pivoted row
            For row = 0 To oldTable.Rows.Count - 1
                dr(row + 1) = oldTable.Rows(row)(col)
            Next

            'add the DataRow to the new table
            newTable.Rows.Add(dr)
        Next

        Return newTable
    End Function
    Public Shared Function CreateVerticalViewofSingleRow(ByVal queryToShow1Row As String) As PlaceHolder


        Try
            Dim mydt = getDBTable(queryToShow1Row)
            '  dt = CustomFields()
            Dim dt = PivotTable(mydt, 0)
            Dim PlaceHolder1 As PlaceHolder = New PlaceHolder()
            PlaceHolder1.ID = "PlaceHolderView1"
            If dt.Rows.Count > 0 Then

                For i As Int32 = 0 To dt.Rows.Count - 1

                    Dim tr As HtmlGenericControl = New HtmlGenericControl("tr")
                    Dim td As HtmlGenericControl = New HtmlGenericControl("td")
                    Dim td1 As HtmlGenericControl = New HtmlGenericControl("td")
                    Dim FieldName As String = Convert.ToString(dt.Rows(i)(0))
                    Dim FieldID As String = i 'Convert.ToString(dt.Rows(i)("field_id"))
                    Dim FieldType As String = "label"
                    Dim FieldValue As String = Convert.ToString(dt.Rows(i)(1))

                    Dim lbcustomename As Label = New Label()
                    lbcustomename.Text = FieldName
                    lbcustomename.ID = "lb" & FieldID

                    lbcustomename.CssClass = "form-label"
                    td.Controls.Add(lbcustomename)
                    tr.Controls.Add(td)
                    If FieldType.ToLower().Trim() = "label" Then
                        Dim lb As Label = New Label()
                        lb.ID = "lbl" & FieldID
                        If String.IsNullOrEmpty(FieldValue) Then FieldValue = "-"
                        lb.Text = FieldValue
                        '''' Load Param value passed in parameter for both add/update forms


                        lb.CssClass = "form-control"  'mt-20
                        td1.Controls.Add(lb)
                        PlaceHolder1.Controls.Add(New LiteralControl("<br />"))

                    End If



                    tr.Controls.Add(td1)
                    PlaceHolder1.Controls.Add(tr)


                Next
            Else
                PlaceHolder1.Controls.Add(New LiteralControl(" <h3>Error </h3> " & " No data returned from query " & queryToShow1Row))
                Return PlaceHolder1
            End If
            Return PlaceHolder1
        Catch ex As Exception
            Dim PlaceHolder1 As PlaceHolder = New PlaceHolder()
            PlaceHolder1.ID = "PlaceHolderview1"
            PlaceHolder1.Controls.Add(New LiteralControl(" <h3>Error </h3> " & ex.Message & " " & queryToShow1Row))
            Return PlaceHolder1
        End Try
    End Function
    Public Shared Function Generate_OTP(ByVal uid As String) As String
        ' declare array string to generate random string with combination of small,capital letters and numbers
        'Dim charArr As Char() = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()
        Dim charArr As Char() = "0123456789".ToCharArray()
        Dim strrandom As String = String.Empty
        Dim objran As New Random()
        'Dim noofcharacters As Integer = Convert.ToInt32(txtCharacters.Text)
        Dim noofcharacters As Integer = 4
        For i As Integer = 0 To noofcharacters - 1
            'It will not allow Repetation of Characters
            Dim pos As Integer = objran.[Next](1, charArr.Length)
            If Not strrandom.Contains(charArr.GetValue(pos).ToString()) Then
                strrandom += charArr.GetValue(pos)
            Else
                i -= 1
            End If
        Next
        'lblResult.Text = strrandom
        Return strrandom
    End Function
End Class
