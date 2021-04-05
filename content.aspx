<%@ Page Language="VB" AutoEventWireup="false" CodeFile="content.aspx.vb" Inherits="content" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Print Certificate</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href='favicon.ico' rel='icon' type='image/x-icon'/>
     <script src="js/jquery-1.8.3.min.js"></script>
		<!--[if lte IE 8]><script src="assets/js/html5shiv.js"></script><![endif]-->
		<link rel="stylesheet" href="css/main.css" />
		<!--[if lte IE 9]><link rel="stylesheet" href="assets/css/ie9.css" /><![endif]-->
		<!--[if lte IE 8]><link rel="stylesheet" href="assets/css/ie8.css" /><![endif]-->
		<noscript><link rel="stylesheet" href="assets/css/noscript.css" /></noscript>
    <script type = "text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=divCert.ClientID %>");
            var printWindow = window.open('', '', 'height=400,width=800');
           printWindow.document.write(panel.innerHTML);
          //  printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 500);
            return false;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
   <!-- Wrapper -->
			<div id="wrapper">

				<!-- Main -->
					<section id="main">
						<header>
							<span class="avatar">Congratulations !!</span>
							<div id="divCert" runat="server" />
          <asp:Button ID="btnEmail" runat="server" Text="Email Confirmation with Document" />
         <asp:Button ID="btnPrint" runat="server" Text="Print Certificate (Save Paper)" Visible="false" OnClientClick = "return PrintPanel();" />
         <asp:Button ID="btnDownload" Visible="false" runat="server" Text="Download Certificate" /> <br />
                            <asp:LinkButton ID="lbLang" runat="server" CausesValidation="False" PostBackUrl="content.aspx?lang=hindi" Text="हिंदी में प्रमाण पत्र लें" Visible="false" ></asp:LinkButton>
                                            <asp:LinkButton ID="lbLangHindi" runat="server" CausesValidation="False" PostBackUrl="content.aspx?lang=english" Visible="false" Text="Get Certificate in English"></asp:LinkButton>
                            <div id="divMsg" runat="server" />
						</header>
       </section>
        <footer>
                          
							<%--<ul class="icons">
                                <li>
                                  <a href="report.aspx" target="_blank" >Click to View Empoloyees who have read Safety Policy <br />(प्रतिज्ञा ले चुके कर्मचारी  ) </a> : <div class='count' runat="server" id="divCount">23</div> 
                                </li>
								<li><img src="images/ntpc.png"  width="200" height="120" style="margin-left: 0px;" /></li>
								
							</ul>--%>
						</footer>
                </section>
				<!-- Footer -->
					<footer id="footer">
						<ul class="copyright">
							<li>&copy; CC-IT</li><li>2017: <a href="http://www.ntpc.co.in">NTPC Limited</a></li>
						</ul>
					</footer>
    </div>
    </form>
     <!-- Scripts -->
			<!--[if lte IE 8]><script src="assets/js/respond.min.js"></script><![endif]-->
			<script>
				if ('addEventListener' in window) {
					window.addEventListener('load', function() { document.body.className = document.body.className.replace(/\bis-loading\b/, ''); });
					document.body.className += (navigator.userAgent.match(/(MSIE|rv:11\.0)/) ? ' is-ie' : '');
				}

				$('.count').each(function () {
				    $(this).prop('Counter', 0).animate({
				        Counter: $(this).text()
				    }, {
				        duration: 2000,
				        easing: 'swing',
				        step: function (now) {
				            $(this).text(Math.ceil(now));
				        }
				    });
				});

			</script>
</body>
</html>
