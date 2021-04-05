
Imports System.Linq
Imports System.ServiceModel.Web
Imports Microsoft.VisualBasic
Imports System.Web.HttpContext
Imports System.Web.Caching
Imports System.Data
Imports System.Data.SqlClient
Imports MySql.Data
Imports MySql.Data.MySqlClient


' TODO: replace [[class name]] with your data class name
'Inherits DataService(Of [[class name]])

' This method is called only once to initialize service-wide policies.
'Public Shared Sub InitializeService(ByVal config As IDataServiceConfiguration)
' TODO: set rules to indicate which entity sets and service operations are visible, updatable, etc.
' Examples:
' config.SetEntitySetAccessRule("MyEntityset", EntitySetRights.AllRead)
' config.SetServiceOperationAccessRule("MyServiceOperation", ServiceOperationRights.All)
'End Sub
Public Class dbOperation
    Public Shared Function ConvertDataTableToHTML(dt As DataTable) As String
        Dim html As String = "" '"<table>"
        ''add header row
        'html += "<tr>"
        'For i As Integer = 0 To dt.Columns.Count - 1
        '    html += "<td>" + dt.Columns(i).ColumnName + "</td>"
        'Next
        'html += "</tr>"
        'add rows
        For i As Integer = 0 To dt.Rows.Count - 1
            html += "<tr>"
            For j As Integer = 0 To dt.Columns.Count - 1
                'html += "<td  class='tg-small'>" + dt.Rows(i)(j).ToString() + "</td>"
                html += "<td  style='border-width: 1px; padding: 8px;  border-style: solid;  border-color: #666666;  background-color: #ffffff;'>" + dt.Rows(i)(j).ToString() + "</td>"
            Next
            html += "</tr>"
        Next
        html += "" '</table>
        Return html
    End Function
    Public Shared Function checkDatabase(ByVal myQuery As String) As Boolean
        ''this function will check any record exist or not in table according to myQuery
        Using connection As New MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("vinConn1").ConnectionString)
            ' connection.Close()
            connection.Open()

            Dim sqlComm As MySqlCommand
            Dim sqlReader As MySqlDataReader
            Dim dt As New DataTable()
            Dim dataTableRowCount As Integer
            Try
                sqlComm = New MySqlCommand(myQuery, connection)
                sqlReader = sqlComm.ExecuteReader()
                dt.Load(sqlReader)
                dataTableRowCount = dt.Rows.Count
                sqlReader.Close()
                sqlComm.Dispose()
                If dataTableRowCount = 1 Then

                    connection.Close()
                    Return True
                Else
                    connection.Close()
                    Return False
                End If

            Catch e As Exception
                'lblDebug.text = e.Message
                connection.Close()
                Return False
            End Try
            connection.Close()
        End Using
    End Function
    Public Shared Function authenticateUser(ByVal myQuery As String) As String()
        ''this function will check any record exist or not in table according to myQuery
        Using connection As New MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("vinConn1").ConnectionString)
            ' connection.Close()
            connection.Open()
            Dim result(4) As String
            Dim sqlComm As MySqlCommand
            Dim sqlReader As MySqlDataReader
            Dim dt As New DataTable()
            Dim dataTableRowCount As Integer
            Try
                sqlComm = New MySqlCommand(myQuery, connection)
                sqlReader = sqlComm.ExecuteReader()
                dt.Load(sqlReader)
                dataTableRowCount = dt.Rows.Count
                sqlReader.Close()
                sqlComm.Dispose()
                If dataTableRowCount = 1 Then
                    ' username,first_name,last_name
                    result(0) = "ok"
                    result(1) = dt.Rows(0).Item(0).ToString()
                    result(2) = dt.Rows(0).Item(1).ToString()
                    result(3) = dt.Rows(0).Item(2).ToString()
                    connection.Close()
                    Return result
                Else
                    connection.Close()
                    result(0) = "too many Row"
                    Return result
                End If

            Catch e As Exception
                'lblDebug.text = e.Message
                connection.Close()
                result(0) = "Error: " & e.Message
                Return result
            End Try
            connection.Close()
        End Using
    End Function
    Public Shared Function getDBSingleHashSepearated(ByVal myQuery As String) As String
        ''this function will return single value from table by concatenating rows with hash
        Using connection As New MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("vinConn1").ConnectionString)
            ' connection.Close()
            connection.Open()
            Dim result As String = ""
            Dim sqlComm As MySqlCommand
            Dim sqlReader As MySqlDataReader
            Dim dt As New DataTable()
            Dim j As Integer = 0

            Try
                sqlComm = New MySqlCommand(myQuery, connection)
                sqlReader = sqlComm.ExecuteReader()
                dt.Load(sqlReader)
                Dim i = dt.Rows.Count

                sqlReader.Close()
                sqlComm.Dispose()
                If i = 0 Then
                    connection.Close()
                    Return "NA"

                End If

                While j < i

                    result = result & dt.Rows(j).Item(0).ToString() & "#"

                    j = j + 1
                End While

                connection.Close()
                Return result


            Catch e As Exception
                'lblDebug.text = e.Message
                connection.Close()
                result = e.Message
                Return result
            End Try
            'connection.Close()
        End Using
    End Function
    Public Shared Function getDBSingleWOHash(ByVal myQuery As String) As String
        ''this function will return single value from table by concatenating rows with hash
        Using connection As New MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("vinConn1").ConnectionString)
            ' connection.Close()
            connection.Open()
            Dim result As String = ""
            Dim sqlComm As MySqlCommand
            Dim sqlReader As MySqlDataReader
            Dim dt As New DataTable()
            Dim j As Integer = 0

            Try
                sqlComm = New MySqlCommand(myQuery, connection)
                sqlReader = sqlComm.ExecuteReader()
                dt.Load(sqlReader)
                Dim i = dt.Rows.Count

                sqlReader.Close()
                sqlComm.Dispose()
                If i = 0 Then
                    connection.Close()
                    Return "<font color=green>Up To Date</font>"

                End If

                While j < i

                    result = result & dt.Rows(j).Item(0).ToString()

                    j = j + 1
                End While

                connection.Close()
                Return "<fnt color=red>" & result & "</font>"


            Catch e As Exception
                'lblDebug.text = e.Message
                connection.Close()
                result = e.Message
                Return result
            End Try
            'connection.Close()
        End Using
    End Function
    Public Shared Function getDBSingleArray(ByVal myQuery As String) As String()
        Using connection As New MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("vinConn1").ConnectionString)
            ' connection.Close()
            connection.Open()
            Dim result As String
            Dim sqlComm As MySqlCommand
            Dim sqlReader As MySqlDataReader
            Dim dt As New DataTable()
            Dim dataTableRowCount As Integer
            Try
                sqlComm = New MySqlCommand(myQuery, connection)
                sqlReader = sqlComm.ExecuteReader()
                dt.Load(sqlReader)
                dataTableRowCount = dt.Rows.Count
                sqlReader.Close()
                sqlComm.Dispose()

                Dim j As Integer = 0

                Dim i = dt.Rows.Count


                Dim onedarray(i) As String
                If i = 0 Then
                    connection.Close()
                    onedarray(0) = "NA"
                    Return onedarray

                End If

                While j < i
                    ' If Not String.IsNullOrEmpty(dt.Rows(j).Item(0).ToString()) Then
                    onedarray(j) = dt.Rows(j).Item(0).ToString()
                    j = j + 1
                    ' End If

                End While

                connection.Close()
                Return onedarray



            Catch e As Exception
                'lblDebug.text = e.Message
                connection.Close()
                Dim i(1)
                i(0) = "error" & e.Message
                Return i
            End Try
            connection.Close()
        End Using


        'connection.Close()

    End Function
    Public Shared Function getDBsingle(ByVal myQuery As String) As String
        ''this function will return single value from table according to myQuery
        Using connection As New MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("vinConn1").ConnectionString)
            ' connection.Close()
            connection.Open()
            Dim result As String
            Dim sqlComm As MySqlCommand
            Dim sqlReader As MySqlDataReader
            Dim dt As New DataTable()
            Dim dataTableRowCount As Integer
            Try
                sqlComm = New MySqlCommand(myQuery, connection)
                sqlReader = sqlComm.ExecuteReader()
                dt.Load(sqlReader)
                dataTableRowCount = dt.Rows.Count
                sqlReader.Close()
                sqlComm.Dispose()
                If dataTableRowCount = 1 Then
                    result = dt.Rows(0).Item(0).ToString()
                    connection.Close()
                    Return result
                Else
                    connection.Close()
                    Return "Too many Records Found"
                End If

            Catch e As Exception
                'lblDebug.text = e.Message
                connection.Close()
                Return e.Message
            End Try
            connection.Close()
        End Using
    End Function
    Public Shared Function getempname(ByVal eid As String) As String
        ''this function will return single value from table according to myQuery
        Using connection As New MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("vinConn1").ConnectionString)
            ' connection.Close()
            connection.Open()
            Dim myquery = "select firstname from employee where TRIM(LEADING '0' FROM empno) = " & eid
            Dim result As String
            Dim sqlComm As MySqlCommand
            Dim sqlReader As MySqlDataReader
            Dim dt As New DataTable()
            Dim dataTableRowCount As Integer
            Try
                sqlComm = New MySqlCommand(myquery, connection)
                sqlReader = sqlComm.ExecuteReader()
                dt.Load(sqlReader)
                dataTableRowCount = dt.Rows.Count
                sqlReader.Close()
                sqlComm.Dispose()
                If dataTableRowCount = 1 Then
                    result = dt.Rows(0).Item(0).ToString()
                    connection.Close()
                    Return result
                Else
                    connection.Close()
                    Return "Too many Records Found"
                End If

            Catch e As Exception
                'lblDebug.text = e.Message
                connection.Close()
                Return e.Message
            End Try
            connection.Close()
        End Using
    End Function
    Public Shared Function get2DArray(ByVal myQuery As String) As String(,)
        ''this function will return 2DArray from table according to myQuery
        Using connection As New MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("vinConn1").ConnectionString)
            ' connection.Close()
            connection.Open()
            Dim result As String
            Dim sqlComm As MySqlCommand
            Dim sqlReader As MySqlDataReader
            Dim dt As New DataTable()

            Try
                sqlComm = New MySqlCommand(myQuery, connection)
                sqlReader = sqlComm.ExecuteReader()
                dt.Load(sqlReader)
                sqlReader.Close()
                sqlComm.Dispose()
                Dim i As Integer = dt.Rows.Count
                Dim j As Integer = 0
                Dim Twodarray(i, i) As String
                While j < i


                    Twodarray(j, 0) = dt.Rows(j).Item(0).ToString
                    Twodarray(j, 1) = dt.Rows(j).Item(1).ToString
                    j = j + 1
                End While
                connection.Close()
                Return Twodarray

            Catch e As Exception
                'lblDebug.text = e.Message
                connection.Close()
                ' Return New String(e.Message, "")
            End Try
            connection.Close()
        End Using
    End Function
    Public Shared Function getDBTable(ByVal myQuery As String) As DataTable
        ''this function will return DataTable from table according to myQuery
        Using connection As New MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("vinConn1").ConnectionString)
            ' connection.Close()
            connection.Open()

            Dim sqlComm As MySqlCommand
            Dim sqlReader As MySqlDataReader
            Dim dt As New DataTable()
            Dim dataTableRowCount As Integer
            Try
                sqlComm = New MySqlCommand(myQuery, connection)
                sqlComm.CommandTimeout = 300
                sqlReader = sqlComm.ExecuteReader()
                dt.Load(sqlReader)
                dataTableRowCount = dt.Rows.Count
                sqlReader.Close()
                sqlComm.Dispose()
                If Not dt Is Nothing Then

                    connection.Close()
                    Return dt
                Else
                    connection.Close()
                    Return dt.NewRow("Too many/NO Records Found")
                End If

            Catch e As Exception
                'lblDebug.text = e.Message
                connection.Close()
                Dim dtErr As New DataTable
                dtErr.Columns.Add("v", GetType(String))
                dtErr.Rows.Add(e.Message)
                Return dtErr
            End Try
            connection.Close()
        End Using
    End Function
    Public Shared Function recentUpdate(ByVal myQuery As String) As String()
        ''this function will return single value from table according to myQuery
        Using connection As New MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("vinConn1").ConnectionString)
            ' connection.Close()
            connection.Open()
            Dim result() As String
            Dim sqlComm As MySqlCommand
            Dim sqlReader As MySqlDataReader
            Dim dt As New DataTable()
            Dim j As Integer = 0
            Try
                sqlComm = New MySqlCommand(myQuery, connection)
                sqlReader = sqlComm.ExecuteReader()
                dt.Load(sqlReader)
                Dim i = dt.Rows.Count

                sqlReader.Close()
                sqlComm.Dispose()
                If i = 0 Then
                    connection.Close()
                    'Return "Too many Records Found"

                End If
                ReDim result(i + 1)
                result(0) = i.ToString  'store array length at 0
                Dim thedate As String
                '   Dim usProvider As IFormatProvider = New System.Globalization.CultureInfo("en-US")  ' uk style date dd/MM/yyyy

                While j < i

                    ''CONVERT MM/DD/YYY TO DD/MM/YYYY WITH TIME
                    thedate = dt.Rows(j).Item(2).ToString()
                    If Not String.IsNullOrEmpty(thedate) Then
                        ' Return DateTime.Parse(thedate.Trim, ukProvider, System.Globalization.DateTimeStyles.NoCurrentDateDefault)
                        '    thedate = (String.Format("{0:d.M.yyyy HH:mm:ss}", DateTime.Parse(thedate.Trim, usProvider, System.Globalization.DateTimeStyles.NoCurrentDateDefault)))
                    Else
                        thedate = " No Date"
                    End If
                    result(j + 1) = dt.Rows(j).Item(0).ToString() & " " & dt.Rows(j).Item(1).ToString() & " last updated on: " & thedate
                    j = j + 1
                End While

                connection.Close()
                Return result


            Catch e As Exception
                'lblDebug.text = e.Message
                connection.Close()
                result(0) = e.Message
                Return result
            End Try
            'connection.Close()
        End Using
    End Function
    Public Shared Function insertTraining(ByVal pid As String, ByVal eid As String) As String

        'Create Connection String
        'Dim DBConn As New SqlConnection("Data Source=.\SQLEXPRESS;AttachDbFilename=|DataDirectory|\intradb.mdf;Integrated Security=True;User Instance=True")
        Dim mysql As String
        Using connection As New MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("vinConn1").ConnectionString)
            ' connection.Close()


            Dim sqlComm As MySqlCommand
            Dim sqlReader As MySqlDataReader

            Try
                'connection.Close()
                connection.Open()
                sqlComm = New MySqlCommand(mysql, connection)
                sqlReader = sqlComm.ExecuteReader()
                'Add Insert Statement
                sqlComm.Dispose()
                'Add Database Parameters
                'DBCmd.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = txtName.Text
                'DBCmd.Parameters.Add("@ContactNumber", SqlDbType.NChar).Value = txtCNumber.Text
                'DBCmd.Parameters.Add("@Address", SqlDbType.NVarChar).Value = txtAddress.Text
                'DBCmd.Parameters.Add("@Country", SqlDbType.NVarChar).Value = ddlCountry.SelectedItem.Text

                'lbldebug.text = "inserted"
                connection.Close()
                insertTraining = "ok"
                'Set the value of DataAdapter
                'DBAdap = New SqlDataAdapter("SELECT * FROM table1", DBConn)
                'Fill the DataSet
                'DBAdap.Fill(DS)
                'Bind with GridView control and Display the Record
                'gvShowRecord.DataSource = DS
                'gvShowRecord.DataBind()

            Catch exp As Exception
                'lbldebug.Text = exp.Message
                connection.Close()
                insertTraining = exp.Message

            End Try
            'Close Database connection
            'and Dispose Database objects
        End Using

    End Function
    Public Shared Function createlog(ByVal eid As String, ByVal ename As String, ByVal uri As String) As Boolean
        Dim epoch As Long
        epoch = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000
        Dim log = "insert into log ( msg,user, ip, priority, class, timestamp) values ('DBX' , '" & ename & "', '" & HttpContext.Current.Request.UserHostAddress & "' , '" & eid & "' , '" & uri & "', " & epoch & ")"
        executeDB(log)
        Return True
    End Function
    Public Shared Function executeDB(ByVal mysql As String) As String

        'Create Connection String
        'Dim DBConn As New SqlConnection("Data Source=.\SQLEXPRESS;AttachDbFilename=|DataDirectory|\intradb.mdf;Integrated Security=True;User Instance=True")
        Using connection As New MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("vinConn1").ConnectionString)
            ' connection.Close()


            Dim sqlComm As MySqlCommand
            Dim sqlReader As MySqlDataReader

            Try
                'connection.Close()
                connection.Open()
                sqlComm = New MySqlCommand(mysql, connection)
                sqlReader = sqlComm.ExecuteReader()
                'Add Insert Statement
                sqlComm.Dispose()
                'Add Database Parameters
                'DBCmd.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = txtName.Text
                'DBCmd.Parameters.Add("@ContactNumber", SqlDbType.NChar).Value = txtCNumber.Text
                'DBCmd.Parameters.Add("@Address", SqlDbType.NVarChar).Value = txtAddress.Text
                'DBCmd.Parameters.Add("@Country", SqlDbType.NVarChar).Value = ddlCountry.SelectedItem.Text

                'lbldebug.text = "inserted"
                connection.Close()
                executeDB = "ok"
                'Set the value of DataAdapter
                'DBAdap = New SqlDataAdapter("SELECT * FROM table1", DBConn)
                'Fill the DataSet
                'DBAdap.Fill(DS)
                'Bind with GridView control and Display the Record
                'gvShowRecord.DataSource = DS
                'gvShowRecord.DataBind()

            Catch exp As Exception
                'lbldebug.Text = exp.Message
                connection.Close()
                executeDB = exp.Message

            End Try
            'Close Database connection
            'and Dispose Database objects
        End Using

    End Function






    Public Shared Function GetCompareStatement(ByVal firstValue As String, ByVal secondValue As String, ByVal operation As String) As String
        Select Case operation
            Case "Contains"
                Return "LIKE '%" + firstValue + "%' "
            Case "Begins with"
                Return "LIKE '" + firstValue + "%' "
            Case "Ends with"
                Return "LIKE '%" + firstValue + "' "
            Case "Equals"
                Return "= '" + firstValue + "' "
            Case "Date Equals"
                Return "= CONVERT(DATETIME, '" + firstValue + "', 101) "
            Case "After"
                Return "> CONVERT(DATETIME, '" + firstValue + "', 101) "
            Case "Before"
                Return "< CONVERT(DATETIME, '" + firstValue + "', 101) "
            Case "Between"
                Return "BETWEEN CONVERT(DATETIME, '" + firstValue + "', 101) AND CONVERT(DATETIME, '" + secondValue + "', 101) "
            Case Else
                Return ""
        End Select
    End Function

    Public Shared Sub rebindGridView(ByVal query As String, ByVal gridViewControl As GridView)
        'Binds Paging/Sorting GridView with data from the specified query
        ' Bind GridView to current query & always store ur query into session("currentquery") before calling
        ' reason is whenever page indexed changed or sort.. then it will show data from currentquery

        Using connection As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("vinConn1").ConnectionString)
            Dim sqlComm As SqlCommand
            Dim sqlReader As SqlDataReader
            Dim dt As New DataTable()
            Dim dataTableRowCount As Integer
            Try
                'connection.Close()
                connection.Open()
                sqlComm = New SqlCommand(query, connection)
                sqlReader = sqlComm.ExecuteReader()
                dt.Load(sqlReader)
                sqlComm.Dispose()
                dataTableRowCount = dt.Rows.Count

                If dataTableRowCount > 0 Then
                    '   Dim vincache As Cache
                    '  vincache.Insert("cache" + gridName, dt, vbNull, Now.AddMinutes(10), TimeSpan.Zero)
                    gridViewControl.DataSource = dt
                    gridViewControl.DataBind()
                End If
                sqlReader.Close()
            Catch e As Exception
                'lblDebug.text = e.Message

                connection.Close()
            End Try


            connection.Close()
        End Using
    End Sub

    Public Shared Sub rebindListview(ByVal query As String, ByVal ListviewControl As ListView)
        'Binds Paging/Sorting GridView with data from the specified query
        ' Bind GridView to current query & always store ur query into session("currentquery") before calling
        ' reason is whenever page indexed changed or sort.. then it will show data from currentquery

        Using connection As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("vinConn1").ConnectionString)
            Dim sqlComm As SqlCommand
            Dim sqlReader As SqlDataReader
            Dim dt As New DataTable()
            Dim dataTableRowCount As Integer
            Try
                'connection.Close()
                connection.Open()
                sqlComm = New SqlCommand(query, connection)
                sqlReader = sqlComm.ExecuteReader()
                dt.Load(sqlReader)
                sqlComm.Dispose()
                dataTableRowCount = dt.Rows.Count

                If dataTableRowCount > 0 Then
                    ListviewControl.DataSource = dt
                    ListviewControl.DataBind()
                End If
                sqlReader.Close()
            Catch e As Exception
                'lblDebug.text = e.Message

                connection.Close()
            End Try


            connection.Close()
        End Using
    End Sub

    Public Shared Function SortDataTable(ByVal dataTable As DataTable, ByVal isPageIndexChanging As Boolean) As DataView
        If Not dataTable Is Nothing Then
            Dim dataView As New DataView(dataTable)
            If GridViewSortExpression <> String.Empty Then
                If isPageIndexChanging Then
                    dataView.Sort = String.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection)
                Else
                    dataView.Sort = String.Format("{0} {1}", GridViewSortExpression, GetSortDirection())
                End If
            End If
            Return dataView
        Else
            Return New DataView()
        End If
    End Function

    Public Shared Property GridViewSortExpression() As String
        Get
            Return IIf(Current.Session("SortExpression") = Nothing, String.Empty, Current.Session("SortExpression"))
        End Get

        Set(ByVal value As String)
            Current.Session("SortExpression") = value
        End Set
    End Property

    Private Shared Function GetSortDirection() As String
        Select Case GridViewSortDirection
            Case "ASC"
                GridViewSortDirection = "DESC"
            Case "DESC"
                GridViewSortDirection = "ASC"
        End Select
        Return GridViewSortDirection
    End Function

    Public Shared Property GridViewSortDirection() As String
        Get
            Return IIf(Current.Session("SortDirection") = Nothing, "ASC", Current.Session("SortDirection"))
        End Get
        Set(ByVal value As String)
            Current.Session("SortDirection") = value
        End Set
    End Property

    Public Shared Function clearsession() As Boolean
        'return true for logout
        Current.Session("eid") = ""
        Current.Session("name") = ""
        Current.Session("designation") = ""
        Current.Session("dept") = ""
        Current.Session("requestedpage") = ""
        'Current.Session("privilage") = ""
        Current.Session.Abandon()
        'Response.Redirect("default.aspx")
        Return True
    End Function
    Public Shared Function insertRecord(ByVal mysql As String) As Boolean

        'Create Connection String
        'Dim DBConn As New SqlConnection("Data Source=.\SQLEXPRESS;AttachDbFilename=|DataDirectory|\intradb.mdf;Integrated Security=True;User Instance=True")
        Using connection As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("vinConn1").ConnectionString)
            Dim sqlComm As SqlCommand
            Dim sqlReader As SqlDataReader


            Try
                'connection.Close()
                connection.Open()
                sqlComm = New SqlCommand(mysql, connection)
                sqlReader = sqlComm.ExecuteReader()
                'Add Insert Statement
                sqlComm.Dispose()
                'Add Database Parameters
                'DBCmd.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = txtName.Text
                'DBCmd.Parameters.Add("@ContactNumber", SqlDbType.NChar).Value = txtCNumber.Text
                'DBCmd.Parameters.Add("@Address", SqlDbType.NVarChar).Value = txtAddress.Text
                'DBCmd.Parameters.Add("@Country", SqlDbType.NVarChar).Value = ddlCountry.SelectedItem.Text

                'lbldebug.text = "inserted"
                connection.Close()
                insertRecord = True
                'Set the value of DataAdapter
                'DBAdap = New SqlDataAdapter("SELECT * FROM table1", DBConn)
                'Fill the DataSet
                'DBAdap.Fill(DS)
                'Bind with GridView control and Display the Record
                'gvShowRecord.DataSource = DS
                'gvShowRecord.DataBind()

            Catch exp As Exception
                'lbldebug.Text = exp.Message
                connection.Close()
                insertRecord = False

            End Try
            'Close Database connection
            'and Dispose Database objects
        End Using

    End Function
    Public Shared Function executeQuerydb2(ByVal mysql As String) As Boolean

        'Create Connection String
        'Dim DBConn As New SqlConnection("Data Source=.\SQLEXPRESS;AttachDbFilename=|DataDirectory|\intradb.mdf;Integrated Security=True;User Instance=True")
        Using connection As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("vinConn12").ConnectionString)
            Dim sqlComm As SqlCommand
            Dim sqlReader As SqlDataReader


            Try
                'connection.Close()
                connection.Open()
                sqlComm = New SqlCommand(mysql, connection)
                sqlReader = sqlComm.ExecuteReader()
                'Add Insert Statement
                sqlComm.Dispose()

                connection.Close()
                executeQuerydb2 = True


            Catch exp As Exception
                'lbldebug.Text = exp.Message
                connection.Close()
                executeQuerydb2 = False

            End Try

        End Using

    End Function
    Public Shared Function executeQuerydb3(ByVal mysql As String) As Boolean

        'Create Connection String
        'Dim DBConn As New SqlConnection("Data Source=.\SQLEXPRESS;AttachDbFilename=|DataDirectory|\intradb.mdf;Integrated Security=True;User Instance=True")
        Using connection As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("vinConn13").ConnectionString)
            Dim sqlComm As SqlCommand
            Dim sqlReader As SqlDataReader


            Try
                'connection.Close()
                connection.Open()
                sqlComm = New SqlCommand(mysql, connection)
                sqlReader = sqlComm.ExecuteReader()
                'Add Insert Statement
                sqlComm.Dispose()

                connection.Close()
                executeQuerydb3 = True


            Catch exp As Exception
                'lbldebug.Text = exp.Message
                connection.Close()
                executeQuerydb3 = False

            End Try

        End Using

    End Function
    Public Shared Function doCalculation(ByVal cpf_id As String, ByVal Actual As String, ByVal pkgid As String) As String
        '' pkgid neeeded to calculate civil wtg
        Dim q
        Try
            Dim err = ""

            ''' get package type
            ''' 
            q = "select pkgtype from Packages where id =" & pkgid & " limit 1"
            Dim pkgtype = getDBsingle(q)
            Dim measure = getDBsingle("select uom from CPF_master where id =" & cpf_id)
            If pkgtype = "Civil" Then : q = "select max_score * area_weight_civil as score from CPF_master where id =" & cpf_id
            Else q = "select max_score * area_weight as score from CPF_master where id =" & cpf_id
            End If
            Dim score = 0.0
            Dim vBool = Double.TryParse(getDBsingle(q), score)
            If Not (vBool) Then Return "Error: Can't convert maxscore"

            Dim act = 0.0
            vBool = Double.TryParse(Actual, act)
            If Not (vBool) Then Return "Error: Can't convert actual"

            If measure.StartsWith("perc") Then
                Dim final = (score * act) / 100
                ' lblFinal.Text = final.tostring()
                ' Return err & measure & Math.Round(final, 2)
                Return Math.Round(final, 2)
            ElseIf measure.StartsWith("numpercentage") Then
                Dim final = 0.0
                Dim percentage = 0.0
                Dim number = 0.0
                ''retrive calculation condition
                Dim val() = measure.ToString.Split("-")
                Dim range As ArrayList = New ArrayList()
                For Each v In val
                    If Not String.IsNullOrEmpty(v) And Not v.Contains("numpercentage") Then
                        '''' now v contains 100@5 , 40@10 0@99
                        err = err & v
                        Dim s As String() = v.Split("@")
                        Dim perc As String = s(0)  '100
                        Dim num As String = s(1) '5
                        vBool = Double.TryParse(perc, percentage)
                        If Not (vBool) Then Return "Error: Can't convert percentage"
                        vBool = Double.TryParse(num, number)
                        If Not (vBool) Then Return "Error: Can't convert number"

                        If act <= number Then
                            final = (score * perc) / 100
                            err = err & final
                            Exit For
                        Else
                            err = err & "Error: number higher act" & act & " num=" & number
                        End If
                    End If
                Next


                'Return err & Math.Round(final, 2)
                Return Math.Round(final, 2)
            ElseIf measure.StartsWith("number") Then


                Dim final = (score * act)
                ' lblFinal.Text = final.tostring()
                Return Math.Round(final, 2)
            End If

        Catch ex As Exception
            Return "Error:" & ex.Message
        End Try

    End Function

End Class
