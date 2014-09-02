<%@ Control Language="C#" CodeBehind="ForeignKey.ascx.cs" Inherits="VEDK_Dashboard.DynamicData.ForeignKeyField" %>

<asp:HyperLink ID="HyperLink1" runat="server"
    Text="<%# GetDisplayString() %>"
    NavigateUrl="<%# GetNavigateUrl() %>"  />

