<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="VEDK_Dashboard._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

  
 <h2>Welcome to Vindenergi Danmarks internal administration dashboard</h2>
<div id="containervindchart">
        <iframe id="myiframevindchart" src="http://www.emd.dk/el/vindenergi/vindenergi.html" scrolling="no">
        </iframe>
</div>
  

</asp:Content>
