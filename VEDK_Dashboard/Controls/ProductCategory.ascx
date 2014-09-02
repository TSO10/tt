<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductCategory.ascx.cs"
    Inherits="VEDK_Dashboard.Controls.ProductCategory" %>
<%@ Register TagPrefix="cus" TagName="SuppliersChart" Src="~/Controls/SuppliersChart.ascx" %>
<table width="100%">
    <tr>
        <td valign="top" class="category-td">
            <asp:LinkButton runat="server" OnClientClick="helper.ShowLoadImage(this)" ID="lnkCategoryName"
                OnClick="lnkCategoryName_Click" ClientIDMode="AutoID" />
            <br />
            <img class="img-load" runat="server" src="~/images/ajax_loader_big.gif" />
            <span runat="server" id="spInfoNoProducts" enableviewstate="true" visible="false">No
                products</span>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Panel runat="server" ID="pnlProducts" Visible="false">
                <table class="pricesGrid" cellspacing="0" rules="all" border="1" style="width: 100%;
                    border-collapse: collapse;">
                    <tbody>
                        <tr>
                            <th scope="col">
                                VEDK ProductID
                            </th>
                            <th scope="col">
                                Name
                            </th>
                            <th scope="col">
                                Description
                            </th>
                            <th scope="col">
                                Start Date
                            </th>
                            <th scope="col">
                                Stop Date
                            </th>
                            <th scope="col">
                                Region
                            </th>
                            <asp:Literal runat="server" ID="lSuppliersHeaders" />
                        </tr>
                        <asp:Repeater runat="server" ID="rProducts" OnItemDataBound="rProducts_ItemDataBound"
                            OnItemCommand="rProducts_ItemCommand">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:LinkButton OnClientClick="ShowHideImage(this)" ClientIDMode="AutoID" runat="server" ID="lnkShowChart"
                                            Text="+" CommandName="ShowChart" CommandArgument='<%# Bind("VEDK_ProductID") %>' />
                                        <asp:Label runat="server" Text='<%# Bind("VEDK_ProductID") %>' ID="lblproductId" />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Text='<%# Bind("productName") %>' ID="lblproductName" />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Text='<%# Bind("productDescription") %>' ID="lblproductDescription" />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"productStartDate","{0:d}")%>'
                                            ID="lblproductStartDate" />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"productEndDate","{0:d}")%>'
                                            ID="lblproductEndDate" />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Text='<%# Bind("VEDK_Region.RegionName") %>' ID="lblRegionName" />
                                    </td>
                                    <asp:Literal runat="server" ID="ltSuppliersInfo" />
                                </tr>
                                <tr>
                                    <td colspan="100">
                                        <div class="display-none">
                                            <span runat="server" id="spChart" enableviewstate="false"></span>
                                            <cus:SuppliersChart runat="server" ID="sCart" Visible="false" />
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </asp:Panel>
        </td>
    </tr>
</table>
