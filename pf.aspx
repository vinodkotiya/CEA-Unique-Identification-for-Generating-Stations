<%@ Page Language="VB" AutoEventWireup="false" CodeFile="pf.aspx.vb" Inherits="pf" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PERFORMANCE FEEDBACK</title>
    <!-- Mobile Specific Meta -->
	<meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
	<!-- Favicon-->
		<link href='./images/favicon.ico' rel='icon' type='image/x-icon'/>
	<!-- Author Meta -->
	<meta name="author" content="CodePixar" />
	<!-- Meta Description -->
	<meta name="description" content="" />
	<!-- Meta Keyword -->
	<meta name="keywords" content="" />
	<!-- meta character set -->
	<meta charset="UTF-8" />
	
	<link href="bobtheme/css/hindi.css" rel="stylesheet" />
		<!--
		CSS
		============================================= -->
		<link rel="stylesheet" href="bobtheme/css/linearicons.css" />
		<link rel="stylesheet" href="bobtheme/css/owl.carousel.css" />
		<link rel="stylesheet" href="bobtheme/css/font-awesome.min.css" />
		<link rel="stylesheet" href="bobtheme/css/nice-select.css" />
		<link rel="stylesheet" href="bobtheme/css/magnific-popup.css" />
		<link rel="stylesheet" href="bobtheme/css/bootstrap.css" />
		<link rel="stylesheet" href="bobtheme/css/main.css" />
     <link rel="stylesheet" type="text/css" href="bobtheme/css/footable.min.css" />
<link href="bobtheme/css/bootstrap-slider.css" rel="stylesheet" />
  <%--  <link href="bobtheme/css/highlightjs-github-theme.css" rel="stylesheet" />--%>
    <style>
     
    </style>
</head>
<body>
    <form id="form1" runat="server">
       <%--    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
         <asp:UpdatePanel ID="upAppraisal" runat="server">
            <ContentTemplate>--%>
        <asp:scriptmanager id="ScriptManager1" runat="server"></asp:scriptmanager>
    <div class="main-wrapper-first">
			<div class="hero-area relative">
				<header>
					<div class="container">
						<div class="header-wrap">
							<div class="header-top d-flex justify-content-between align-items-center">
								<div class="logo">
									<a href="#"><img src="images/ntpcthumb.png" style="height:30%; width:30%;" alt=""></a>
								</div>
								<div class="main-menubar d-flex align-items-center">
									<nav class="hide">
										<a href="upload/helppf.pdf" target="_blank">Help</a>
										<a href="saplogin.aspx?logout=1">LogOut</a>
										
									</nav>
									<div class="menu-bar"><span class="lnr lnr-menu"></span></div>
								</div>
							</div>
						</div>
					</div>
				</header>
				<div class="banner-area">
					<div class="container">
						<div class="row height align-items-center">
							<div class="col-lg-7">
								<div class="banner-content"  id="divLogin" runat="server">
									
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
		<div class="main-wrapper">
			<div class="working-process-area">
				<div class="container">
					<div class="row justify-content-center">
						<div class="col-lg-6">
							<div class="section-title text-center">
								<h2>Feedback Process</h2>
								<p>It will be very easy to enter the feedback data in these simple steps.</p>
							</div>
						</div>
					</div>
					<div class="total-work-process d-flex flex-wrap justify-content-around align-items-center">
						<div class="single-work-process">
							<div class="work-icon-box">
								<span class="fa fa-edit"></span>
							</div>
							<h4 class="caption">1. By Employee</h4>
						</div>
						<div class="work-arrow">
							<img src="bobtheme/img/elements/work-arrow.png" alt="">
						</div>
						<div class="single-work-process">
							<div class="work-icon-box">
								<span class="fa fa-handshake-o"></span>
							</div>
							<h4 class="caption">2. By Business Unit Head</h4>
						</div>
						<div class="work-arrow">
							<img src="bobtheme/img/elements/work-arrow.png" alt="">
						</div>
						<div class="single-work-process">
							<div class="work-icon-box">
								<span class="fa fa-users"></span>
							</div>
							<h4 class="caption">3. By RED</h4>
						</div>
						<div class="work-arrow">
							<img src="bobtheme/img/elements/work-arrow.png" alt="">
						</div>
						<div class="single-work-process">
							<div class="work-icon-box">
								<span class="lnr lnr-layers"></span>
							</div>
							<h4 class="caption">4. Submission to HR</h4>
						</div>
					</div>
				</div>
			</div>
		</div>
		<div class="main-wrapper">
			<!-- Start Feature Area -->
            <a name="yearselect"></a>
			<section class="featured-area">
				<div class="container">
					<div class="row justify-content-center">
						<div class="col-lg-6">
							<div class="section-title text-center">
								<h2 class="text-white">DASHBOARD</h2>
								<p class="text-white">Welcome! Choose the year for which you are going to do Performance Feedback </p>
                                 
							</div>
						</div>
                        	<div class="col-md-4">
							<div class="single-feature" id ="divName" runat="server">
								<%--<div class="thumb" style="height:120px; background: url(images/pics/009383.jpg);"></div>
								<div class="desc text-center mt-30">
									<h4 class="text-white">Creative Design</h4>
									<p class="text-white">Few would argue that, despite the advancements of feminism over the past three decades, women still face a double</p>
								
								</div>--%>
							</div>
						</div>
					</div>
				
				</div>
			</section>
			<!-- End Feature Area -->
			<!-- Start Remarkable Wroks Area -->
			<section class="remarkable-area">
				<div class="container">
				<%--	<div class="row justify-content-center">
						<div class="col-lg-6">
							<div class="section-title text-center">
								<h2>Dashboard</h2>
								<p>Select the Appropriate Action from the List</p>
							</div>
						</div>
					</div>--%>
					<div class="single-remark">
						<div class="row no-gutters">
							<%--<div class="col-lg-7 col-md-6">
								<div class="remark-thumb" style="background: url(bobtheme/img/r1.jpg);"></div>
							</div>
							<div class="col-lg-5 col-md-6">
								<div class="remark-desc">
									<h4>Vector Illustration</h4>
									<p>LCD screens are uniquely modern in style, and the liquid crystals that make them work have allowed humanity to create slimmer, more portable technology.</p>
									<a href="#" class="primary-btn"><span>View Project</span></a>
								</div>
							</div>--%>
                            <asp:Panel ID="pnlFeedback" runat="server" Visible="true" >
                                 <div class="story-box">
                                     <div id="form-main-container">
  <div id="tabs">
    <ul>
      <li class="active-tab"><a href="#"><span>One</span></a></li>
    <%--  <li><a href="#"><span>Two</span></a></li>
      <li><a href=""><span>Three</span></a></li>
      <li><a href="#"><span>Four</span></a></li>--%>
    </ul>
  </div>
                                          <div id="form-area">
                                               <div id="form-title">
                                                         Form Title
                                                    </div>
                                              <div id="form-body">
                                         <asp:Panel ID="pnlForm" runat="server" Visible="true" >
								
								<p>1.	Please enumerate the major tasks assigned and completed; any exceptional contribution from the date of assuming the change of GM. e.g. successful completion of an </p>   
                                                <asp:GridView ID="GridView1" runat="server"></asp:GridView>
                                             </asp:Panel>
 <asp:Button ID="btnSubmit" runat="server" Text="Save Data" style="background-image: -moz-linear-gradient(0deg, #3e69fe 0%, #4cd4e3 100%);   background-image: -webkit-linear-gradient(0deg, #3e69fe 0%, #4cd4e3 100%);    background-image: -ms-linear-gradient(0deg, #3e69fe 0%, #4cd4e3 100%);" CssClass="primary-btn submit-btn d-inline-flex align-items-center mr-10"  />  &nbsp;&nbsp;&nbsp;&nbsp;  &nbsp;&nbsp;&nbsp;&nbsp;  &nbsp;&nbsp;&nbsp;&nbsp; 
                                      <asp:Button ID="btnSave" runat="server" Text="Cancel"  CssClass="primary-btn submit-btn d-inline-flex align-items-center mr-10"   />
                                            </div>      <%-- form body div--%>
                                              </div>      <%-- form area div--%>
                               </div>      <%-- feedback div--%>
							</div>
                                 </asp:Panel>
                            <div id="divMsg" runat="server" />
						</div>
					</div>
				
				</div>
			</section>
			<!-- End Remarkable Wroks Area -->
			<!-- Start Story Area -->
			<section class="story-area">
				<div class="container">
					<div class="row align-items-center">
						<div class="col-lg-1">
						</div>
						<div class="col-lg-7">
							<div class="story-box">
								<h6 class="text-uppercase">PERFORMANCE FEEDBACK SYSTEM</h6>
								<p>The system is designed in user freindly manner. Employees can enter the data in few clicks. Reports are available anytime anywhere.</p>
								<a href="#yearselect" class="primary-btn d-inline-flex align-items-center"><span class="mr-10">Get Started</span><span class="lnr lnr-arrow-right"></span></a>
							</div>
						</div>
					</div>
				</div>
			</section>
			<!-- End Story Area -->


			<!-- Start Subscription Area -->
			<section class="subscription-area">
				<div class="container">
					<div class="row align-items-center">
						<div class="col-lg-6">
							<div class="section-title">
								<h3>Having Trouble???</h3>
								<span>Go through the Help Manual to Understand the Process Step by Step.</span>
							</div>
						</div>
						<div class="col-lg-6">
							<div id="mc_embed_signup">
								<%--<form target="_blank" novalidate action="https://spondonit.us12.list-manage.com/subscribe/post?u=1462626880ade1ac87bd9c93a&id=92a4423d01" method="get" class="subscription relative">
									<input type="email" name="EMAIL" placeholder="Your email address" onfocus="this.placeholder = ''" onblur="this.placeholder = 'Your email address'" required>
									<div style="position: absolute; left: -5000px;">
										<input type="text" name="b_36c4fd991d266f23781ded980_aefe40901a" tabindex="-1" value="">
									</div>
									<button class="primary-btn hover d-inline-flex align-items-center"><span class="mr-10">Get Started</span><span class="lnr lnr-arrow-right"></span></button>
									<div class="info"></div>
								</form>--%>
                            <button class="primary-btn hover d-inline-flex align-items-center"><span class="mr-10"> <a href="upload/helppf.pdf" style="color:white" target="_blank" > Help Manual</a></span><span class="lnr lnr-arrow-right"></span></button>
							</div>
						</div>
					</div>
				</div>
			</section>
			<!-- End Subscription Area -->
			<!-- Start Contact Form -->
		<%--	<section class="contact-form-area">
				<div class="container">
					<div class="row justify-content-center">
						<div class="col-lg-6">
							<div class="section-title text-center">
								<h2 class="text-white">Keep in Touch</h2>
								<p class="text-white">Most people who work in an office environment, buy computer products, or have a computer at home have had the “fun” experience of dealing </p>
							</div>
						</div>
					</div>
					-<form id="myForm"  class="contact-form">
						<div class="row justify-content-center">
							<div class="col-lg-5">
								<input type="text" name="fname" placeholder="Enter your name" onfocus="this.placeholder = ''" onblur="this.placeholder = 'Enter your name'" class="common-input mt-20" required>
							</div>
							<div class="col-lg-5">
								<input type="email" name="email" placeholder="Enter email address" pattern="[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{1,63}$" onfocus="this.placeholder = ''" onblur="this.placeholder = 'Enter email address'" class="common-input mt-20" required>
							</div>
							<div class="col-lg-10">
								<textarea class="common-textarea mt-20" name="message" placeholder="Messege" onfocus="this.placeholder = ''" onblur="this.placeholder = 'Messege'" required></textarea>
							</div>
							<div class="col-lg-10 d-flex justify-content-end">
								<button class="primary-btn submit-btn d-inline-flex align-items-center mt-20"><span class="mr-10">Send Message</span><span class="lnr lnr-arrow-right"></span></button> <br>
								<div class="alert-msg"></div>
							</div>
						</div>
					</form>
				</div>
			</section>--%>
			<!-- End Contact Form -->
			<!-- Start Footer Widget Area -->
			<section class="footer-widget-area">
			<%--	<div class="container">
					<div class="row">
						<div class="col-md-4">
							<div class="single-widget">
								<div class="desc">
									<h6 class="title">Address</h6>
									<p>56/8, panthapath, west <br> dhanmondi, kalabagan</p>
								</div>
							</div>
						</div>
						<div class="col-md-4">
							<div class="single-widget">
								<div class="desc">
									<h6 class="title">Email Address</h6>
									<div class="contact">
										<a href="mailto:info@dataarc.com">info@dataarc.com</a> <br>
										<a href="mailto:support@dataarc.com">support@dataarc.com</a>
									</div>
								</div>
							</div>
						</div>
						<div class="col-md-4">
							<div class="single-widget">
								<div class="desc">
									<h6 class="title">Phone Number</h6>
									<div class="contact">
										<a href="tel:1545">012 4562 982 3612</a> <br>
										<a href="tel:54512">012 6321 956 4587</a>
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>--%>
				<footer>
					<div class="container">
						<div class="footer-content d-flex justify-content-between align-items-center flex-wrap">
							<div class="logo">
								<%--<a href="index.html"><img src="bobtheme/img/logo.png" alt=""></a>--%>
							</div>
							<div class="copy-right-text" id="divPage" runat ="server">Copyright &copy; 2018 |  All rights reserved to <a href="#">NTPC Limited.</a> Designed by <a href="https://cc.ntpc.co.in" target="_blank">CC-IT, SCOPE</a></div>
							<div class="footer-social">
								<a href="saplogin.aspx?logout=1"><i class="fa  fa-times-rectangle-o"></i></a>
								<a href="saplogin.aspx?logout=1">Logout</a>
								
							</div>
						</div>
					</div>
				</footer>
			</section>
			<!-- End Footer Widget Area -->

		</div>



        <%--</ContentTemplate>
              <Triggers>--%>
                   <%-- <asp:AsyncPostBackTrigger ControlID="ddlProject" EventName="SelectedIndexChanged" />
                                       <asp:AsyncPostBackTrigger ControlID="ddlUsers" EventName="SelectedIndexChanged" />--%>
                   <%--  <asp:PostBackTrigger ControlID="btnSubOrdReportDownload" />
                     <asp:AsyncPostBackTrigger ControlID="rblSupportType" EventName="SelectedIndexChanged" />
                   <asp:AsyncPostBackTrigger ControlID="rblCluster" EventName="SelectedIndexChanged" />
                   <asp:AsyncPostBackTrigger ControlID="rblRegion" EventName="SelectedIndexChanged" />--%>
                <%--   <asp:AsyncPostBackTrigger ControlID="rblFormYear" EventName="SelectedIndexChanged" />--%>
                                         <%--   </Triggers>
            </asp:UpdatePanel>--%>
		
    </form>
    <script  >

    </script>
    <script src="bobtheme/js/vendor/jquery-2.2.4.min.js"></script>
		<script src="bobtheme/js/popper.min.js" integrity="sha384-b/U6ypiBEHpOf/4+1nzFpr53nxSS+GLCkfwBdFNTxtclqqenISfwAzpKaMNFNmj4" crossorigin="anonymous"></script>
		<script src="bobtheme/js/vendor/bootstrap.min.js"></script>
		<script src="bobtheme/js/jquery.ajaxchimp.min.js"></script>
		<script src="bobtheme/js/owl.carousel.min.js"></script>
		<script src="bobtheme/js/jquery.nice-select.min.js"></script>
		<script src="bobtheme/js/jquery.magnific-popup.min.js"></script>
		<script src="bobtheme/js/main.js"></script>
   
</body>
</html>
