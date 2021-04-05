Imports Microsoft.VisualBasic
Imports System.Data
Imports dbOperation
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports iTextSharp.text.pdf
Imports iTextSharp.tool.xml
Imports System.Net.Mail

Public Class CPF_common

    Public Shared Function cpf_detail(pkg_id As Integer, mthyear As String)
        Dim pkgtype = getDBsingle("select pkgtype from Packages where id =" & pkg_id)
        Dim qry_detail As String
        If pkgtype = "Civil" Then
            qry_detail = "Select c.id As 'S NO', c.area_score as Area,c.resp as Resposiblity,c.area_weight_civil as 'Area Weight Civil',c.parameter as Parameter,c.max_score as 'Max Score' ,c.uom_desc as 'Measure',cd.value as 'Actual', round(cd.score / c.area_weight_civil,2) as 'Score Obtained' from CPF_master c  left join CPF_detail  cd on c.id = cd.cpf_id and cd.pkg_id =" & pkg_id & " and cd.mthyear = '" & mthyear & "' order by c.id "
        Else
            qry_detail = "Select c.id As 'S NO', c.area_score as Area,c.resp as Resposiblity,c.area_weight as 'Area Weight',c.parameter as Parameter,c.max_score as 'Max Score' ,c.uom_desc as 'Measure',cd.value as 'Actual', round(cd.score / c.area_weight,2) as 'Score Obtained' from CPF_master c  left join CPF_detail  cd on c.id = cd.cpf_id and cd.pkg_id =" & pkg_id & " and cd.mthyear = '" & mthyear & "' order by c.id "
        End If

        'lbltext.Text = qry
        Dim dt As DataTable = getDBTable(qry_detail)
        dt.PrimaryKey = Nothing
        dt.Columns.Remove("S NO")
        Return dt
    End Function
    Public Shared Function cpf_project_summary(mthyear As String, param As String, paramtype As String) As DataTable

        Dim qrytemp As String = "1=1"

        Dim qrytemp1, colname As String
        If paramtype = "project" Then
            qrytemp = "pk.project ='" & param & "'"
            qrytemp1 = "pk.package"
            colname = "Package-Agency"
        ElseIf paramtype = "pkgcode" Then
            qrytemp = "pk.pkgcode ='" & param & "'"
            qrytemp1 = "pk.project"
            colname = "Project-Agency"
        ElseIf paramtype = "agency" Then
            qrytemp = "pk.agency ='" & param & "'"
            qrytemp1 = "concat(pk.project,'- ',pk.package)"
            colname = "Project-Package"
        End If

        Dim qry_summary As String
        Dim dt_proj_summary As DataTable
        qry_summary = "Select  distinct concat(" & qrytemp1 & ",'- ',pk.agency,IF(pk.pkgtype='Civil','*',''),'@',pk.id) as Agency,concat(c.area_score,'\n\n(',c.area_weight*100,')') as 'Area',  sum(cd.score)  as 'Weighted Score Obtained' from Packages pk left join CPF_master c on 1=1 left join CPF_detail cd on pk.id=cd.pkg_id and cd.mthyear = '" & mthyear & "'and c.id = cd.cpf_id where " & qrytemp & " group by c.area_score,pk.id,pk.agency order by pk.package,c.id  "
        '  qry_summary = "Select  distinct concat(pk.package,'- ',pk.agency) as Agency,c.area_score as 'Area',  round(sum(cd.score) ,2) as 'Weighted Score Obtained' from CPF_master c    left join CPF_detail  cd on c.id = cd.cpf_id right join Packages pk on   cd.mthyear = '" & mthyear & "' and pk.id=cd.pkg_id where  pk.project ='" & project & "' group by c.area_score,pk.package,pk.agency order by c.id,pk.package  "

        dt_proj_summary = getDBTable(qry_summary)


        dt_proj_summary = GetInversedDataTable(dt_proj_summary, "Area", "Agency", "Weighted Score Obtained", "", False)

        dt_proj_summary.Columns.Add("Sno", GetType(Integer)).SetOrdinal(0)
        dt_proj_summary.Columns.Add("Total", Type.GetType("System.Double"))
        dt_proj_summary.Columns.Add("Grade", Type.GetType("System.String"))
        dt_proj_summary.Columns.Add("Detail", Type.GetType("System.String"))

        Dim totalscore, count As Double
        count = 0
        For Each row As DataRow In dt_proj_summary.Rows
            totalscore = 0

            dt_proj_summary.Rows(count)("SNo") = count + 1
            count = count + 1
            For i As Integer = 2 To row.ItemArray.Count - 2

                If Not IsDBNull(row.Item(i)) Then
                    If row.Item(i) <= 0 Then
                        row.Item(i) = 0
                    End If
                    totalscore = row.Item(i) + totalscore

                End If
            Next i
            row.Item("Total") = totalscore
            row.Item("Detail") = "<a href=cpf_dashboard.aspx?pkgid=" & row.Item("Agency").ToString.Split("@")(1) & "&mthyear=" & mthyear &
            " target=_blank>Detail</a>"
            row.Item("Grade") = CPF_grade(row.Item("Total"), row.Item("Agency").ToString.Split("@")(1), mthyear)
            row.Item("Agency") = row.Item("Agency").ToString.Split("@")(0)

        Next
        dt_proj_summary.Columns("Agency").ColumnName = colname

        Return dt_proj_summary
    End Function
    Public Shared Function cpf_pkgcode(mthyear As String, pkgcode As String) As DataTable

        Dim qry_summary As String
        Dim dt_proj_summary As DataTable
        qry_summary = "select  distinct concat(pk.project,'<br />',pk.agency) as Agency,c.area_score as 'Area',  round(sum(cd.score) ,2) as 'Weighted Score Obtained' from Packages pk left join CPF_master c on 1=1 left join CPF_detail cd on pk.id=cd.pkg_id and cd.mthyear = '" & mthyear & "'and c.id = cd.cpf_id where  pk.pkgcode ='" & pkgcode & "' group by c.area_score,pk.package,pk.agency order by c.id, pk.package "
        '  qry_summary = "select  distinct concat(pk.package,'- ',pk.agency) as Agency,c.area_score as 'Area',  round(sum(cd.score) ,2) as 'Weighted Score Obtained' from CPF_master c    left join CPF_detail  cd on c.id = cd.cpf_id right join Packages pk on   cd.mthyear = '" & mthyear & "' and pk.id=cd.pkg_id where  pk.project ='" & project & "' group by c.area_score,pk.package,pk.agency order by c.id,pk.package  "

        dt_proj_summary = getDBTable(qry_summary)


        dt_proj_summary = GetInversedDataTable(dt_proj_summary, "Area", "Agency", "Weighted Score Obtained", "", False)
        '  dt_proj_summary.Columns.Remove("Column1")
        dt_proj_summary.Columns.Add("Sno", GetType(Integer)).SetOrdinal(0)
        dt_proj_summary.Columns.Add("Total", Type.GetType("System.Double"))
        '  dt_proj_summary.Columns.Add("Package", Type.GetType("System.String"))
        Dim totalscore, count As Double
        count = 0
        For Each row As DataRow In dt_proj_summary.Rows
            totalscore = 0

            dt_proj_summary.Rows(count)("SNo") = count + 1
            count = count + 1
            For i As Integer = 2 To row.ItemArray.Count - 2

                If Not IsDBNull(row.Item(i)) Then
                    If row.Item(i) <= 0 Then
                        row.Item(i) = 0
                    End If
                    totalscore = row.Item(i) + totalscore

                End If
            Next i
            row.Item("Total") = totalscore
        Next

        Return dt_proj_summary
    End Function

    Public Shared Function cpf_summary(pkg_id As Integer, mthyear As String, Optional ByVal total As Boolean = False)
        Dim pkgtype = getDBsingle("select pkgtype from Packages where id =" & pkg_id)
        Dim qry_summary As String
        If pkgtype = "Civil" Then
            qry_summary = "select  c.area_score as 'Area',sum( case when c.max_score >=0 then c.max_score else 0 end ) as 'Max Score <br /> (A)' ,round( sum( cd.score) / c.area_weight_civil,2) as 'Score Obtained <br /> (B)' ,c.area_weight_civil as 'Weightage <br /> (C)',floor(sum(( case when c.max_score >=0 then c.max_score else 0 end )*c.area_weight_civil) ) as 'Max weighted Score <br /> (AxC)', round(sum(cd.score) * c.area_weight_civil/c.area_weight_civil,2) as 'Weighted Score Obtained <br />(BxC)' from CPF_master c    join CPF_detail  cd on c.id = cd.cpf_id  and cd.pkg_id =" & pkg_id & " and cd.mthyear = '" & mthyear & "' group by c.area_score order by c.id  "

        Else
            qry_summary = "select  c.area_score as 'Area',sum( case when c.max_score >=0 then c.max_score else 0 end ) as 'Max Score <br /> (A)' ,round( sum( cd.score  ) / c.area_weight,2) as 'Score Obtained <br /> (B)' ,c.area_weight as 'Weightage <br /> (C)',floor(sum(( case when c.max_score >=0 then c.max_score else 0 end )*c.area_weight) ) as 'Max weighted Score <br /> (AxC)', round(sum(cd.score) * c.area_weight/c.area_weight,2) as 'Weighted Score Obtained <br />(BxC)' from CPF_master c    join CPF_detail  cd on c.id = cd.cpf_id  and cd.pkg_id =" & pkg_id & " and cd.mthyear = '" & mthyear & "' group by c.area_score order by c.id  "
        End If

        'Return (qry_summary)

        Dim dt_summary As DataTable = getDBTable(qry_summary)
        For Each row As DataRow In dt_summary.Rows
            If row.Item(2) <= 0 Then
                row.Item(2) = 0
            End If
            If row.Item(5) <= 0 Then
                row.Item(5) = 0
            End If

        Next row

        If total Then
            Dim total_weight As Decimal = dt_summary.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)(3))

            Dim total_max_weight_score As Decimal = dt_summary.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)(4))

            Dim total_score_obtained As Decimal = dt_summary.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)(5))


            dt_summary.Rows.Add("Total", 0, 0, total_weight, total_max_weight_score, total_score_obtained)
            ''    dt_summary.Rows.Add("Total1", "", "", "", "", "")



        End If

        Return (dt_summary)

        ' End If
    End Function

    Public Shared Function CPF_grade(total_score_obtained As Decimal, Optional pkgid As Integer = 0, Optional mthyear As String = "") As String
        Dim cpf_grade_desc As String

        If total_score_obtained > 0 And total_score_obtained < 50 Then
            cpf_grade_desc = "Unsatisfactory"

        ElseIf total_score_obtained >= 50 And total_score_obtained < 70 Then
            cpf_grade_desc = "Statisfactory"

        ElseIf total_score_obtained >= 70 And total_score_obtained < 80 Then
            cpf_grade_desc = "Good"

        ElseIf total_score_obtained >= 80 Then
            cpf_grade_desc = "Excellent"

        End If
        Dim rowcount As String = getDBsingle("select count(*) as rowcount from CPF_master cm join CPF_detail cd join Packages pk on cm.id = cd.cpf_id and cd.pkg_id = pk.id and cd.pkg_id =" & pkgid & " and  cd.mthyear='" & mthyear & "'")
        Dim pkgtype = getDBsingle("select pkgtype from Packages where id =" & pkgid)
        If pkgtype = "Civil" And rowcount > 0 And rowcount < 19 Then
            cpf_grade_desc = "Incomplete"
        ElseIf pkgtype <> "Civil" And rowcount > 0 And rowcount < 29 Then
            cpf_grade_desc = "Incomplete"
        End If
        Return (cpf_grade_desc)
    End Function
    Public Shared Function CPF_grade_color(total_score_obtained As Decimal, Optional pkgid As Integer = 0, Optional mthyear As String = "") As String
        Dim cpf_grade_desc As String

        If total_score_obtained > 0 And total_score_obtained <= 50 Then
            cpf_grade_desc = "LightPink"

        ElseIf total_score_obtained > 50 And total_score_obtained <= 70 Then
            cpf_grade_desc = "LightBlue"

        ElseIf total_score_obtained > 70 And total_score_obtained <= 80 Then
            cpf_grade_desc = "LightGreen"

        ElseIf total_score_obtained > 80 Then
            cpf_grade_desc = "LimeGreen"
        Else
            cpf_grade_desc = "White"


        End If
        Dim rowcount As String = getDBsingle("select count(*) as rowcount from CPF_master cm join CPF_detail cd join Packages pk on cm.id = cd.cpf_id and cd.pkg_id = pk.id and cd.pkg_id =" & pkgid & " and  cd.mthyear='" & mthyear & "'")
        Dim pkgtype = getDBsingle("select pkgtype from Packages where id =" & pkgid)
        If pkgtype = "Civil" And rowcount > 0 And rowcount < 19 Then
            cpf_grade_desc = "Red"
        ElseIf pkgtype <> "Civil" And rowcount > 0 And rowcount < 29 Then
            cpf_grade_desc = "Red"
        End If
        Return cpf_grade_desc

    End Function

    Public Shared Function dt_to_table(dt As DataTable, Optional cssclass As String = "", Optional caption As String = "") As String
        'Building an HTML string.
        Dim html As New StringBuilder()

        'Table start.
        If cssclass = "" Then
            html.Append("<table class='Grid1'>")
        Else
            html.Append("<table class='" & cssclass & "'>")
        End If
        If caption <> "" Then
            html.Append("<caption>" & caption & "</caption>")

        End If
        'Building the Header row.
        html.Append("<thead>")
        html.Append("<tr>")
        For Each column As DataColumn In dt.Columns
            html.Append("<th>")
            html.Append(column.ColumnName)
            html.Append("</th>")
        Next
        html.Append("</tr>")
        html.Append("</thead>")
        'Building the Data rows.
        For Each row As DataRow In dt.Rows
            html.Append("<tr>")
            For Each column As DataColumn In dt.Columns
                html.Append("<td>")
                html.Append(row(column.ColumnName))
                html.Append("</td>")
            Next
            html.Append("</tr>")
        Next

        'Table end.
        html.Append("</table>")
        Return html.ToString

    End Function
    Public Shared Sub MergeRowsWithSameContent(gvw As GridView)
        For rowIndex As Integer = gvw.Rows.Count - 2 To 0 Step -1
            Dim row As GridViewRow = gvw.Rows(rowIndex)
            Dim previousRow As GridViewRow = gvw.Rows(rowIndex + 1)

            'For i As Integer = 0 To row.Cells.Count - 1
            For i As Integer = 0 To 4
                If row.Cells(i).Text = previousRow.Cells(i).Text Then
                    row.Cells(i).RowSpan = If(previousRow.Cells(i).RowSpan < 2, 2, previousRow.Cells(i).RowSpan + 1)
                    previousRow.Cells(i).Visible = False
                End If
            Next
        Next
    End Sub

    Public Shared Function GetInversedDataTable(ByVal table As DataTable, ByVal columnX As String, ByVal columnY As String, ByVal columnZ As String, ByVal nullValue As String, ByVal sumValues As Boolean) As DataTable
        'Create a DataTable to Return
        Dim returnTable As New DataTable()

        If columnX = "" Then
            columnX = table.Columns(0).ColumnName
        End If

        'Add a Column at the beginning of the table
        returnTable.Columns.Add(columnY)


        'Read all DISTINCT values from columnX Column in the provided DataTale
        Dim columnXValues As New List(Of String)()

        For Each dr As DataRow In table.Rows

            Dim columnXTemp As String = dr(columnX).ToString()
            If Not columnXValues.Contains(columnXTemp) Then
                'Read each row value, if it's different from others provided, add to the list of values and creates a new Column with its value.
                columnXValues.Add(columnXTemp)
                returnTable.Columns.Add(columnXTemp)
            End If
        Next

        'Verify if Y and Z Axis columns re provided
        If columnY <> "" AndAlso columnZ <> "" Then
            'Read DISTINCT Values for Y Axis Column
            Dim columnYValues As New List(Of String)()

            For Each dr As DataRow In table.Rows
                If Not columnYValues.Contains(dr(columnY).ToString()) Then
                    columnYValues.Add(dr(columnY).ToString())
                End If
            Next

            'Loop all Column Y Distinct Value
            For Each columnYValue As String In columnYValues
                'Creates a new Row
                Dim drReturn As DataRow = returnTable.NewRow()
                drReturn(0) = columnYValue
                'foreach column Y value, The rows are selected distincted
                Dim rows As DataRow() = table.[Select]((columnY & "='") + columnYValue & "'")

                'Read each row to fill the DataTable
                For Each dr As DataRow In rows
                    Dim rowColumnTitle As String = dr(columnX).ToString()

                    'Read each column to fill the DataTable
                    For Each dc As DataColumn In returnTable.Columns
                        If dc.ColumnName = rowColumnTitle Then
                            'If Sum of Values is True it try to perform a Sum
                            'If sum is not possible due to value types, the value displayed is the last one read
                            If sumValues Then
                                Try
                                    drReturn(rowColumnTitle) = Convert.ToDecimal(drReturn(rowColumnTitle)) + Convert.ToDecimal(dr(columnZ))
                                Catch
                                    drReturn(rowColumnTitle) = dr(columnZ)
                                End Try
                            Else
                                drReturn(rowColumnTitle) = dr(columnZ)

                            End If
                        End If
                    Next
                Next

                returnTable.Rows.Add(drReturn)

            Next
        Else
            Throw New Exception("The columns to perform inversion are not provided")
        End If

        'if a nullValue is provided, fill the datable with it
        If nullValue <> "" Then
            For Each dr As DataRow In returnTable.Rows
                For Each dc As DataColumn In returnTable.Columns
                    If dr(dc.ColumnName).ToString() = "" Then
                        dr(dc.ColumnName) = nullValue
                    End If
                Next
            Next
        End If

        Return returnTable
    End Function
    Public Shared Function pkg_detail(pkgid As String) As DataTable
        Dim qry_package As String = "select  pk.agency as Vendor, pk.project as Project, pk.package as Package, date_format(pk.loa,'%d/%m/%y') as 'Award Date',date_format(pk.L2Compln,'%d/%m/%y') as 'Sch Completion Date', pk.value_inr as 'Award Value(In Crs)', period_diff(date_format(pk.L2Compln,'%y%m'),date_format(pk.loa,'%y%m')) as 'Sch Completion (In Mths)' from Packages pk where pk.id = '" & pkgid & "'"
        Return getDBTable(qry_package)
    End Function

    Public Shared Sub gridpdf(gv As GridView)

        Using sw As New StringWriter()
            Using hw As New HtmlTextWriter(sw)
                Dim filename = gv.Caption & ".pdf"
                filename.Trim.Replace(" ", "")
                '    lblhead.Text = filename
                hw.Write("<center><h2>NTPC Limited <br/>Project Planning & Monitoring</h2> ")
                hw.Write("<h3>Contractor Performance feedback<br/> and Evaluation</h3></center> ")
                ' lblhead.RenderControl(hw)
                '   hw.Write("")
                '   MergeRowsWithSameContent(gvdetail)
                '  gvdetail.c
                gv.RenderControl(hw)


                Dim sr As New StringReader(sw.ToString())
                Dim pdfDoc As New Document(PageSize.A4.Rotate, 10.0F, 10.0F, 20.0F, 20.0F)

                Dim writer As PdfWriter = PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream)
                pdfDoc.Open()
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr)
                pdfDoc.Close()

                HttpContext.Current.Response.ContentType = "application/pdf"

                '   Dim filename = gvsummary.Caption.ToString & ".pdf"
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=""" & filename & """")
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)
                HttpContext.Current.Response.Write(pdfDoc)
                HttpContext.Current.Response.End()
            End Using
        End Using


    End Sub
    Public Shared Sub ExportToExcel(gv As GridView, Optional fname As String = "file", Optional heading As String = "", Optional ntpcheader As Boolean = True)
        Dim filename = "file.xls"
        If IsNothing(fname) Then
            filename = gv.Caption & ".xls"

        End If

        filename.Trim.Replace(" ", "")
        HttpContext.Current.Response.Clear()
        HttpContext.Current.Response.Buffer = True
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=""" & filename & """")

        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel"
        HttpContext.Current.Response.Charset = "UTF-8"
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8
        Using sw As New StringWriter()
            Dim hw As New HtmlTextWriter(sw)
            If ntpcheader Then
                hw.Write("<center><h2>NTPC Limited <br/>Project Planning & Monitoring</h2> ")
            End If

            If IsNothing(heading) Then
                hw.Write("<h3>Contractor Performance feedback<br/> and Evaluation</h3></center> ")
            Else
                hw.Write(heading) '' & "</center>")
            End If

            'To Export all pages
            gv.AllowPaging = False


            ' gv_comm_ladder.HeaderRow.BackColor = Color.White
            For Each cell As TableCell In gv.HeaderRow.Cells
                cell.BackColor = gv.HeaderStyle.BackColor
            Next


            gv.RenderControl(hw)
            'style to format numbers to string
            '      Dim style As String = "<style> .textmode { } </style>"
            '     Response.Write(style)
            HttpContext.Current.Response.Output.Write(sw.ToString())
            HttpContext.Current.Response.Flush()
            HttpContext.Current.Response.[End]()
        End Using
    End Sub
    Public Shared Function sendemail(toemail As String, subject As String, text As String) As String
        Using mm As New MailMessage("cpmc@ntpc.co.in", toemail)
            mm.Subject = subject
            mm.Body = text

            mm.IsBodyHtml = True
            Dim smtp As New SmtpClient()
            If toemail.Contains("ntpc.co.in") Or toemail.Contains("NTPC.CO.IN") Then
                smtp.Host = "10.1.10.73" ''10.1.8.119
                ''    smtp.EnableSsl = False
                Dim credentials As New System.Net.NetworkCredential()
                credentials.UserName = ""
                credentials.Password = ""
                smtp.UseDefaultCredentials = False
                smtp.Credentials = credentials
                ''   smtp.Port = 25
                smtp.Send(mm)
                Dim path As String = HttpContext.Current.Server.MapPath("~/reminderLog.htm")
                Using writer As New StreamWriter(path, True)
                    writer.WriteLine("</br><hr>Email sent successfully to: " & toemail & "Content :" & text & " -" & Date.Now)
                    writer.Close()
                End Using
            Else

                smtp.Host = "smtp.gmail.com"
            smtp.EnableSsl = True
            Dim credentials As New System.Net.NetworkCredential()

            credentials.UserName = "cpmc2009@gmail.com"
            credentials.Password = "pmcscope123"
            smtp.UseDefaultCredentials = True
            smtp.Credentials = credentials
            smtp.Port = 587
            End If


        End Using


    End Function
    '' First parameter Is Row Collection  
    '' Second Is Start Column  
    '' Third Is total number of Columns to make group of Data.  
    Public Shared Sub ShowingGroupingDataInGridView(gridViewRows As GridViewRowCollection, startIndex As Integer, totalColumns As Integer)
        If totalColumns = 0 Then
            Return
        End If
        Dim i As Integer, count As Integer = 1
        Dim lst As New ArrayList()
        lst.Add(gridViewRows(0))
        Dim ctrl = gridViewRows(0).Cells(startIndex)
        For i = 1 To gridViewRows.Count - 1
            Dim nextTbCell As TableCell = gridViewRows(i).Cells(startIndex)
            If ctrl.Text = nextTbCell.Text Then
                count += 1
                nextTbCell.Visible = False
                lst.Add(gridViewRows(i))
            Else
                If count > 1 Then
                    ctrl.RowSpan = count
                    ShowingGroupingDataInGridView(New GridViewRowCollection(lst), startIndex + 1, totalColumns - 1)
                End If
                count = 1
                lst.Clear()
                ctrl = gridViewRows(i).Cells(startIndex)
                lst.Add(gridViewRows(i))
            End If
        Next
        If count > 1 Then
            ctrl.RowSpan = count
            ShowingGroupingDataInGridView(New GridViewRowCollection(lst), startIndex + 1, totalColumns - 1)
        End If
        count = 1
        lst.Clear()
    End Sub

End Class
