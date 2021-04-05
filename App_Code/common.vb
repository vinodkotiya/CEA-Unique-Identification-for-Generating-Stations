Imports Microsoft.VisualBasic
Imports dbOperation
Imports System.Data
Imports System.Security.Cryptography
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports iTextSharp.text.pdf
Imports iTextSharp.tool.xml
Imports System.Net.Mail

Public Class common
    Public Shared Function bindmenu(ByVal menu As Menu) As Boolean
        Dim xds As XmlDataSource = New XmlDataSource
        xds.DataFile = HttpContext.Current.Server.MapPath("menu.xml")
        xds.XPath = "Home/Role[@id='1']"

        menu.DataSource = xds
        menu.DataBind()
        menu.Items(0).Text = ""
        menu.Items(0).Value = ""
        menu.Items(0).Selectable = False

    End Function
    Public Shared Function LastUpdateTable(ByVal tablename As String, ByVal proj As String, ByVal unit As String, ByVal updatefield As String) As String
        '' the function returns last update time of tablename having proj
        'updatefield = lastupdated or  last_updated   , unit is optional
        Dim myquery As String = ""
        Dim thedate As String = "nil"
        Try
            If Not String.IsNullOrEmpty(unit) Then
                myquery = "SELECT max(" & updatefield & ") FROM " & tablename & " WHERE project= '" & proj & "' and unit = " & unit
            Else

                myquery = "SELECT max(" & updatefield & ") FROM " & tablename & " WHERE project= '" & proj & "'"
            End If
            ''CONVERT MM/DD/YYY TO DD/MM/YYYY WITH TIME
            Dim usProvider As IFormatProvider = New System.Globalization.CultureInfo("en-US")  ' uk style date dd/MM/yyyy
            thedate = getDBsingle(myquery)
            If Not String.IsNullOrEmpty(thedate) Then
                ' Return DateTime.Parse(thedate.Trim, ukProvider, System.Globalization.DateTimeStyles.NoCurrentDateDefault)
                Return thedate
                ' Return (String.Format("{0:d.M.yyyy HH:mm:ss}", DateTime.Parse(thedate.Trim, usProvider, System.Globalization.DateTimeStyles.NoCurrentDateDefault)))
                '                Return thedate
            Else
                Return " No Date"
            End If

        Catch e As Exception
            Return e.Message & thedate & myquery
        End Try

    End Function
    Public Shared Function RecentUpdates(ByVal tablename As String, ByVal proj As String, ByVal unit As String, ByVal updatefield As String) As String
        '' the function returns last update time of tablename having proj
        'updatefield = lastupdated or  last_updated   , unit is optional
        Dim myquery As String = ""
        Dim thedate As String = "nil"
        Try
            If Not String.IsNullOrEmpty(unit) Then
                myquery = "SELECT max(" & updatefield & ") FROM " & tablename & " WHERE project= '" & proj & "' and unit = " & unit
            Else

                myquery = "SELECT max(" & updatefield & ") FROM " & tablename & " WHERE project= '" & proj & "'"
            End If
            ''CONVERT MM/DD/YYY TO DD/MM/YYYY WITH TIME
            '  Dim usProvider As IFormatProvider = New System.Globalization.CultureInfo("en-US")  ' uk style date dd/MM/yyyy
            thedate = getDBsingle(myquery)
            If Not String.IsNullOrEmpty(thedate) Then
                Return thedate
                'Return DateTime.Parse(thedate.Trim, ukProvider, System.Globalization.DateTimeStyles.NoCurrentDateDefault)
                '    Return (String.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.Parse(thedate.Trim, usProvider, System.Globalization.DateTimeStyles.NoCurrentDateDefault)))
            Else
                Return " No Date"
            End If

        Catch e As Exception
            Return e.Message & thedate
        End Try

    End Function
    Public Shared Function makemenu(ByVal logout As Boolean) As String
        Dim temp = "<div id=menu> <ul class=menu> " &
         "<li class=last><a href=/dbx><span>Home</span></a></li> " &
                 "</li>" &
          "<li><a href=# class=parent><span>Reports</span></a>" &
                 "<div><ul>" &
                 "<li><a href=/ppm/cpf_dashboard.aspx target = _blank> <span>Contractor Performance</span></a></li>" &
                  "<li><a href=../dbx/pkg_apex.aspx target = _blank><span>Apex Package List</span></a></li>" &
                 "<li><a href=../dbx/Excep1.aspx?mode=DPR target = _blank><span>DPR & Fronts Status</span></a></li>" &
                 "<li><a href=/dbx/reports.aspx?rpt=mat_diversion_new.rpt target = _blank> <span>Material Diversion</span></a></li>" &
                         "<li><a href=../dbx/jsg.aspx target = _blank> <span>Master Network</span></a></li>" &
                         "<li><a href=# class=parent><span>Package List</span></a>" &
                             "<div><ul>" &
                                                   "<li><a href=../dbx/reports.aspx?rpt=packages.rpt target = _blank><span>Package</span></a></li>" &
                                 "<li><a href=../dbx/reports.aspx?rpt=packages_vendor.rpt target = _blank><span>Vendor</span></a></li>" &
                                "</ul></div>" &
                         "</li>" &
                                                                 "</ul></div>" &
                  "</li>" ' & _
        ''  "<li class=last><a href=training.aspx><span>Training</span></a></li> "
        ' "<li><a href=# class=parent><span>Data Updaaation</span></a>" &
        '     "<div><ul>" &
        '     "<li><a href=http://10.0.236.165/ppm/events.aspx?mode=PRT><span>PRT Sch entry</span></a></li>" &
        '                 "<li><a href=fronts.aspx><span>Civil Fronts</span></a></li>" &
        '                 "<li><a href=comm_ladder.aspx><span>Commissioning Ladder</span></a></li>" &
        '                 "<li><a href=contacts.aspx><span>Contacts</span></a></li>" &
        '                 "<li><a href=excep_visit.aspx><span>Exception-Visit</span></a></li>" &
        '                 
        '                 "<li><a href=mou.aspx><span>MOU</span></a></li>" &
        '                  "<li><a href=mou_ppm.aspx><span>MOU PP&M Dept</span></a></li>" &
        '                 "<li><a href=webmiles.aspx ><span>Critical Issues of WebMiles</span></a></li>" &
        '                 "<li><a href=milestone.aspx ><span>Milestones</span></a></li>" &
        '                 "<li><a href=milestones_delay.aspx ><span>Milestones Delay</span></a></li>" &
        '                  "<li><a href=pgtest.aspx ><span>PG Test</span></a></li>" &
        '                   "<li><a href=pkg_apex_update.aspx ><span>Apex Package List</span></a></li>" &
        '     "</ul></div>" &
        If HttpContext.Current.Session("eid") Then
            temp = temp & "<li class=last><a href=default.aspx?logout=1><span>Logout</span></a></li>"
            temp = temp & "<li class=last>" & getempname(HttpContext.Current.Session("eid")) & "</li>"
        Else
            temp = temp & "<li class=last><a href=javascript:vinModal('1');><span>Login</span></a></li>"
        End If

        temp = temp & "</ul> </div><br/>For help and support email at cpmc@ntpc.co.in / ajaychadha@ntpc.co.in"

        Return temp
    End Function

    'Public Class ExtComparer
    '    Implements IComparer(Of String)
    '    Public Function Compare(ByVal x As String, ByVal y As String) As Integer

    '        Return System.[String].Compare(Strings.Right(x, 3), Strings.Right(y, 3), System.StringComparison.Ordinal)
    '    End Function
    'End Class
    Public Shared Function getLatestURI(ByVal project As String, ByVal type As String) As String
        Dim myquery = "Select filename, right(filename,3) As ext from upload_cmg where project = '" & project & "' and type = '" & type & "'  and reviewdate =(select max(reviewdate) as reviewdate  from upload_cmg where project = '" & project & "' and type = '" & type & "' group by project ) order by ext "
        If Not type = "EXCEPTION" Then
            myquery = myquery & " limit 1 "
        End If
        Dim filename = getDBTable(myquery)
        Dim str = ""
        ' System.Array.Sort(filename)

        For Each f In filename.Rows
            If Not String.IsNullOrEmpty(f(0).ToString) Then
                Dim thefile As String = "..\ppm\upload\" & project & "\" & type & "\" & f(0).ToString
                Dim img As String = "../ppm/images/" & Right(f(0).ToString, 3) & ".gif"
                str = str & "&nbsp;&nbsp;<a href='" & thefile & "' ><img src=" & img & " border=0 /></a>"
            End If
        Next

        Return str
        'Try ' If System.IO.File.Exists(Server.MapPath("~") + thefile) Then
        '    Dim fs As System.IO.FileInfo = New System.IO.FileInfo(Server.MapPath("~") + thefile)
        '    e.Row.Cells(3).Text = "<img src=" & img & " width=13 height=13 border=0 onerror=" & Chr(34) & "this.src='images/file.gif';" & Chr(34) & "  />" & Math.Round((fs.Length / (1024 * 1024)), 2).ToString()   'Left(e.Row.Cells(2).Text, 2)
        'Catch e1 As Exception ' Else
        '    e.Row.Cells(3).Text = Math.Round((Rnd(6) * 600)).ToString  '"NA" & e1.Message
        'End Try 'End If

    End Function

    Public Shared Function isAdmin(ByVal eid As String) As Boolean
        Dim query = "SELECT g.name FROM users u INNER JOIN group_users gu" &
                    " on gu.userid=u.userid " &
                    " INNER JOIN groups g " &
                    " on g.gid = gu.gid " &
                    " where (username='" & eid & "' and g.name = 'PMC')"
        If checkDatabase(query) Then
            Return True
        Else

            Return False
        End If
    End Function
    'Protected Sub
    'Public Shared Function dt_to_table(dt As DataTable) As String
    '    'Building an HTML string.
    '    Dim html As New StringBuilder()

    '    'Table start.
    '    html.Append("<center><table border=1 class='table table-bordered table-inverse'>")

    '    'Building the Header row.
    '    html.Append("<tr>")
    '    For Each column As DataColumn In dt.Columns
    '        html.Append("<th class=success>")
    '        html.Append(column.ColumnName)
    '        html.Append("</th>")
    '    Next
    '    html.Append("</tr>")

    '    'Building the Data rows.
    '    For Each row As DataRow In dt.Rows
    '        html.Append("<tr class=info>")
    '        For Each column As DataColumn In dt.Columns
    '            html.Append("<td>")
    '            html.Append(row(column.ColumnName))
    '            html.Append("</td>")
    '        Next
    '        html.Append("</tr>")
    '    Next

    '    'Table end.
    '    html.Append("</table></center>")
    '    Return html.ToString

    'End Function
    Public Shared Function CleanHtml(ByVal html As String) As String
        '' Cleans all manner of evils from the rich text editors in IE, Firefox, Word, and Excel
        '' Only returns acceptable HTML, and converts line breaks to <br />
        '' Acceptable HTML includes HTML-encoded entities.
        html = html.Replace("&" & "nbsp;", " ").Trim() ' concat here due to SO formatting
        '' Does this have HTML tags?
        If html.IndexOf("<") >= 0 Then
            '' Make all tags lowercase
            html = Regex.Replace(html, "<[^>]+>", AddressOf LowerTag)
            '' Filter out anything except allowed tags
            '' Problem: this strips attributes, including href from a
            '' 
            Dim AcceptableTags As String = "i|b|u|sup|sub|ol|ul|li|br|h2|h3|h4|h5|span|div|p|a|img|blockquote"
            Dim WhiteListPattern As String = "</?(?(?=" & AcceptableTags & ")notag|[a-zA-Z0-9]+)(?:\s[a-zA-Z0-9\-]+=?(?:([""']?).*?\1?)?)*\s*/?>"
            html = Regex.Replace(html, WhiteListPattern, "", RegexOptions.Compiled)
            '' Make all BR/br tags look the same, and trim them of whitespace before/after
            html = Regex.Replace(html, "\s*<br[^>]*>\s*", "<br />", RegexOptions.Compiled)
        End If
        '' No CRs
        html = html.Replace(ControlChars.Cr, "")
        '' Convert remaining LFs to line breaks
        html = html.Replace(ControlChars.Lf, "<br />")
        '' Trim BRs at the end of any string, and spaces on either side
        Return Regex.Replace(html, "(<br />)+$", "", RegexOptions.Compiled).Trim()
    End Function

    Public Shared Function LowerTag(m As Match) As String
        Return m.ToString().ToLower()
    End Function
    Public Shared Sub MergeRowsWithSameContent(gvw As GridView, colnum As String())
        For rowIndex As Integer = gvw.Rows.Count - 2 To 0 Step -1
            Dim row As GridViewRow = gvw.Rows(rowIndex)
            Dim previousRow As GridViewRow = gvw.Rows(rowIndex + 1)

            'For i As Integer = 0 To row.Cells.Count - 1
            For Each i In colnum
                If row.Cells(i).Text = previousRow.Cells(i).Text Then
                    row.Cells(i).RowSpan = If(previousRow.Cells(i).RowSpan < 2, 2, previousRow.Cells(i).RowSpan + 1)
                    previousRow.Cells(i).Visible = False
                End If
            Next
        Next
    End Sub
    Public Shared Function Decrypt(cipherText As String, ByVal EncryptionKey As String) As String
        'If System.Web.HttpRuntime.Cache("marks" & cipherText) Is Nothing Then

        Dim cipherBytes As Byte() = Convert.FromBase64String(cipherText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D,
             &H65, &H64, &H76, &H65, &H64, &H65,
             &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write)
                    cs.Write(cipherBytes, 0, cipherBytes.Length)
                    cs.Close()
                End Using
                cipherText = Encoding.Unicode.GetString(ms.ToArray())
            End Using
        End Using
        '  System.Web.HttpRuntime.Cache.Insert("marks" & cipherText, cipherText, Nothing, DateTime.Now.AddHours(12.0), TimeSpan.Zero)

        'End If
        Return cipherText
    End Function
    Public Shared Sub gridpdf(gv As GridView, title As String)

        Using sw As New StringWriter()
            Using hw As New HtmlTextWriter(sw)
                Dim filename = gv.Caption & ".pdf"
                filename.Trim.Replace(" ", "")
                '    lblhead.Text = filename
                title = "<h3>" & title & "</h3>"
                hw.Write("<center><h2>NTPC Limited <br/>Project Planning & Monitoring</h2> ")
                hw.Write(title)
                ' hw.Write("<h3>Contractor Performance feedback<br/> and Evaluation</h3></center> ")
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
    Public Shared Function RemoveSpecialCharacters(ByVal str As String) As String
        Dim sb As StringBuilder = New StringBuilder()
        For Each c As Char In str
            If (c >= "0"c AndAlso c <= "9"c) OrElse (c >= "A"c AndAlso c <= "Z"c) OrElse (c >= "a"c AndAlso c <= "z"c) OrElse c = "."c OrElse c = "_"c OrElse c = "<"c OrElse c = ">"c OrElse c = " "c Then
                sb.Append(c)
            End If
        Next

        Return sb.ToString()
    End Function
    Public Shared Function GetDateInDDMMYY(ByVal dt As String) As String
        '1/30/2015 
        Try
            If Not dt.Contains("/") Then
                Return dt
            End If
            Dim str(3) As String
            str = dt.Split("/")
            Dim tempdt As String = String.Empty
            'For i As Integer = 2 To 0 Step -1
            tempdt = str(1).PadLeft(2, "0") + "." + str(0).PadLeft(2, "0") + "." + str(2)
            'Next
            tempdt = tempdt.Substring(0, 10)
            Return tempdt
        Catch ex As Exception
            Return dt
        End Try

    End Function
    Public Shared Function sendsms(mobile As String, msg As String) As String
        Dim rq = System.Net.WebRequest.Create(New Uri("http://10.0.236.42/playsms/web/ws.php?u=pmc&p=scope123&ta=pv&to=" & mobile & "&msg=" & msg))
        Try
            Dim rs = rq.GetResponse
            ''rs.Dump


        Catch Ex As System.Net.WebException
            Dim rs = Ex.Response
            Call (New StreamReader(rs.GetResponseStream)).ReadToEnd()
            Return "OTP can not be sent.."
        End Try
        Return "OTP sent.."
    End Function
    Public Shared Function sendemail(toemail As String, subject As String, text As String, attach1 As String) As String
        Using mm As New MailMessage("cpmc@ntpc.co.in", toemail)
            mm.Subject = subject
            mm.Body = text

            mm.IsBodyHtml = True
            If Not String.IsNullOrEmpty(attach1) Then mm.Attachments.Add(New Attachment(attach1))

            Dim smtp As New SmtpClient()
            If toemail.Contains("ntpc.co.in") Or toemail.Contains("NTPC.CO.IN") Then
                smtp.Host = "10.1.10.73" ''10.1.8.119
                smtp.EnableSsl = False
                Dim credentials As New System.Net.NetworkCredential()
                credentials.UserName = ""
                credentials.Password = ""
                smtp.UseDefaultCredentials = False
                smtp.Credentials = credentials
                ''   smtp.Port = 465

                smtp.Send(mm)
                Dim path As String = HttpContext.Current.Server.MapPath("~/reminderLog.htm")
                Using writer As New StreamWriter(path, True)
                    writer.WriteLine("</br><hr>Email sent successfully to: " & toemail & "Content :" & text & " -" & Date.Now)
                    writer.Close()
                End Using
                ''    Else

                '    smtp.Host = "smtp.gmail.com"
                'smtp.EnableSsl = True
                'Dim credentials As New System.Net.NetworkCredential()

                'credentials.UserName = "cpmc2009@gmail.com"
                'credentials.Password = "pmcscope123"
                'smtp.UseDefaultCredentials = True
                'smtp.Credentials = credentials
                'smtp.Port = 587
            End If


        End Using

        Return ""
    End Function
End Class
