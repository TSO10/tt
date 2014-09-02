<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="energinet.aspx.cs" Inherits="VEDK_Dashboard.content.energinet.energinet" %>

<asp:Content ID="cHeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.0/themes/base/jquery-ui.css" />
    <script src="http://code.jquery.com/jquery-1.8.3.js"></script>
    <script src="http://code.jquery.com/ui/1.10.0/jquery-ui.js"></script>
    <script type="text/javascript" src="http://elycharts.com/sites/elycharts.com/repo/dist/elycharts.min.js"></script>
    <script type="text/javascript" src="http://elycharts.com/sites/elycharts.com/repo/lib/raphael.js"></script>
    <script type="text/javascript" src="<%=WebHelper.Utils.AbsUri %>/Scripts/energinet.js"></script>
</asp:Content>
<asp:Content ID="cMainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="elycharts-panel">
        <div id="chart" class="chart-panel">
        </div>
        <div id="datepicker" class="calendar">
        </div>
        <span id="loadInfo"></span>
    </div>
</asp:Content>
