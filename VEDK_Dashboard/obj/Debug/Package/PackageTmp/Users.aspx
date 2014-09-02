<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Users.aspx.cs"
    Inherits="VEDK_Dashboard.Users" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:MultiView ID="mvUsers" runat="server" ActiveViewIndex="0" OnActiveViewChanged="mvUsers_ActiveViewChanged">
        <asp:View ID="View1" runat="server">
            <table align="center">
                <tr>
                    <td>
                        <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvUsers_RowDataBound"
                            OnRowCommand="gvUsers_RowCommand" EmptyDataText="No users" Width="1100px" OnPageIndexChanging="gvUsers_PageIndexChanging"
                            PageSize="10" AllowPaging="True" CssClass="DDGridView" CellPadding="7">
                            <FooterStyle BackColor="#CCCCCC"/>
                            <PagerStyle CssClass="DDFooter" HorizontalAlign="Center"/>
                            <HeaderStyle CssClass="th"/>
                            <RowStyle BorderWidth="1px" CssClass="td"/>
                            <Columns>
                                <asp:TemplateField HeaderText="User Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserName" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Comments">
                                    <ItemTemplate>
                                        <asp:Label ID="lblComments" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created On">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCreatedOn" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Update On">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUpdateOn" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Is Admin">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnAdmin" runat="server" CommandName="AdminDisable" /></td>
                                    </ItemTemplate>
                                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <table align="center">
                                            <tr>
                                                <td width="17" align="center">
                                                    <asp:ImageButton ID="btnEdit" runat="server" CommandName="Editing" ImageUrl="~/images/Edit.gif"
                                                        AlternateText="Edit" />
                                                </td>
                                                <td width="17" align="center">
                                                    <asp:ImageButton ID="btnDelete" runat="server" CommandName="Deleting" ImageUrl="~/images/Delete.gif"
                                                        OnClientClick="return confirm('Sure delete this user?');" AlternateText="Delete" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <ItemStyle Width="50px" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Position="Bottom" PageButtonCount="20" Mode="NumericFirstLast" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <img id="Img2" runat="server" src="~/Administrators/DynamicData/Content/Images/plus.gif" alt="Insert new item" /><asp:LinkButton ID="LinkButton1" runat="server" ForeColor="#839CE7" OnClick="btnAddUser_Click">Add user</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="View2" runat="server">
            <table align="center">
                <tr id="trError" runat="server">
                    <td colspan="2" align="center">
                        <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        User Name:
                    </td>
                    <td>
                        <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ErrorMessage="*" ForeColor="Red"
                            ControlToValidate="txtUsername" ValidationGroup="User" Display="Dynamic" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        New Password:
                    </td>
                    <td>
                        <asp:TextBox ID="txtPwd" runat="server" TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Confirm New Password:
                    </td>
                    <td>
                        <asp:TextBox ID="txtConfPwd" runat="server" TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Comments:
                    </td>
                    <td>
                        <asp:TextBox ID="txtComments" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Is Admin:
                    </td>
                    <td>
                        <asp:CheckBox ID="chbAdmin" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Add" ValidationGroup="User" />
                        <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click"
                            ValidationGroup="User" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
