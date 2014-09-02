<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="millog.aspx.cs"
    Inherits="VEDK_Dashboard.content.millog.millog" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script type="text/javascript" src="<%=WebHelper.Utils.AbsUri %>/Scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript" src="<%=WebHelper.Utils.AbsUri %>/Scripts/BigInt.js"></script>
    <script type="text/javascript" src="<%=WebHelper.Utils.AbsUri %>/Scripts/millog.js"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div id="picker_div" style="display: none">
        Filter By GSRN
        <select id="picker" onchange="ddlChange(this)">
        </select>
    </div>
    <br />
    <div id="dashboard_div">
    </div>
    <div id="chart_div">
    </div>
    <div id="control_div">
    </div>
    <div id="column_div">
    </div>
</asp:Content>
