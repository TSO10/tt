<%@ Page Title="Parkers" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Parkers.aspx.cs" Inherits="VEDK_Dashboard.Parkers" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:DropDownList ID="ddlSuppliers" DataTextField="Name" DataValueField="Supplierguid"
        runat="server">
    </asp:DropDownList>

    <asp:TextBox ID="tbDate" runat="server" CssClass="DDTextBox" Columns="20"></asp:TextBox>
    <asp:ImageButton ID="calendarButton" runat="server" ImageUrl="~/images/calendar.png"
        CausesValidation="false" />
    <asp:CalendarExtender runat="server" TargetControlID="tbDate" PopupButtonID="calendarButton" />
    <asp:CheckBox id="chbAllowPagign" runat="server"
                    Checked="true"                  
                    Text="Allow paging"
                    TextAlign="Right"/>
    <asp:Button Text="Get content" ID="getContent" runat="server" OnClick="getContent_Click" />
    <asp:Button Text="Export to Excel" ID="btnExport" runat="server" OnClick="btnExport_Click" />
    <asp:Label id="lblRowsCount" runat="server" />    
    <asp:GridView ID="parkersList" runat="server" AutoGenerateColumns="true" AllowPaging="true"
        AllowSorting="True" OnSorting="parkersList_Sorting"  PageSize="20" OnPageIndexChanging="parkersList_PageIndexChanging"
        Width="100%">        
    </asp:GridView>
</asp:Content>
