<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="trade.aspx.cs" Inherits="VEDK_Dashboard.trades.trade" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <asp:UpdatePanel ID="upAgreements" runat="server">
    <ContentTemplate>
        <asp:Timer ID="tUpdateLayout" runat="server" OnTick="tUpdateLayout_Tick" Interval="10000">
        </asp:Timer>
   <h2>Handler de sidste 20 dage<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
           ConnectionString="<%$ ConnectionStrings:130087_EssConnectionString %>" 
           
           
           
           
           SelectCommand="SELECT [Dato prissikring dato], COUNT(Park_GSRN) AS Expr1 FROM [Vindenergi Danmark amba$DVE Agreement Table] WHERE ([Dato prissikring dato] &gt; GETDATE() - 20) GROUP BY [Dato prissikring dato] ORDER BY [Dato prissikring dato]">
       </asp:SqlDataSource>
        </h2>





        <asp:Chart ID="Chart3" runat="server" DataSourceID="SqlDataSource2" 
            Height="451px" Width="552px" >
            <Series>
                <asp:Series Name="Series1" XValueMember="Dato prissikring dato" 
                    IsXValueIndexed="true" Label="#VAL"
                LabelBorderColor="LightSteelBlue" LabelToolTip="#VALX;#VAL" 
                    YValueMembers="Expr1">
                </asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1">
                    <AxisY Title="Antal aftaler">
                    </AxisY>
                    <AxisX Title="Dato">
                    </AxisX>
                </asp:ChartArea>
            </ChartAreas>
            <Titles>
                <asp:Title Name="fdsfdsfsdfsdfds">
                </asp:Title>
            </Titles>
        </asp:Chart>
        <asp:SqlDataSource ID="CountNumberOfTrades" runat="server" 
            ConnectionString="<%$ ConnectionStrings:130087_EssConnectionString %>" 
            
            
            
            SelectCommand="SELECT [Vindenergi Danmark amba$DVE Agreement Table].[Dato prissikring dato], SUM([Vindenergi Danmark amba$Møllekartotek_DVE].[KW-Effekt]) AS Effekt FROM [Vindenergi Danmark amba$Møllekartotek_DVE] INNER JOIN [Vindenergi Danmark amba$DVE Agreement Table] ON [Vindenergi Danmark amba$Møllekartotek_DVE].Park_GSRN = [Vindenergi Danmark amba$DVE Agreement Table].Park_GSRN WHERE ([Vindenergi Danmark amba$DVE Agreement Table].[Dato prissikring dato] &gt; GETDATE() - 20) GROUP BY [Vindenergi Danmark amba$DVE Agreement Table].[Dato prissikring dato] ORDER BY [Vindenergi Danmark amba$DVE Agreement Table].[Dato prissikring dato]">
        </asp:SqlDataSource>




        <asp:Chart ID="Chart4" runat="server" DataSourceID="CountNumberOfTrades" 
            Height="449px" Width="537px">
            <Series>
                <asp:Series IsXValueIndexed="true" Label="#VAL" 
                    LabelBorderColor="LightSteelBlue" LabelToolTip="#VALX;#VAL" Name="Series1" 
                    XValueMember="Dato prissikring dato" YValueMembers="Effekt">
                </asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1">
                    <AxisY Title="Effekt kWh">
                    </AxisY>
                    <AxisX Title="Dato">
                    </AxisX>
                </asp:ChartArea>
            </ChartAreas>
            <Titles>
                <asp:Title Name="fdsfdsfsdfsdfds">
                </asp:Title>
            </Titles>
        </asp:Chart>




        <br />
        <h2>De seneste 20 aftaler</h2>
        <br />
        <asp:GridView ID="GridView1" runat="server" AllowSorting="True" 
            AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" 
            BorderStyle="None" BorderWidth="1px" CellPadding="3" 
            DataSourceID="ListLatestTrade" style="text-align: center">
            <Columns>
                <asp:BoundField DataField="Dato prissikring dato" HeaderText="Dato prissikring dato" 
                    SortExpression="Dato prissikring dato" />
                <asp:BoundField DataField="Park_GSRN" HeaderText="Park_GSRN" 
                    SortExpression="Park_GSRN" />
                <asp:BoundField DataField="Pris" HeaderText="Pris" 
                    SortExpression="Pris" DataFormatString="{0:C}" ReadOnly="True" />
                <asp:BoundField DataField="Produktnavn" HeaderText="Produktnavn" 
                    SortExpression="Produktnavn" />
                <asp:BoundField DataField="Description" HeaderText="Description" 
                    SortExpression="Description" />
                <asp:BoundField DataField="Agreement Type" HeaderText="Agreement Type" 
                    SortExpression="Agreement Type" />
                <asp:BoundField DataField="VD-ID" HeaderText="VD-ID" 
                    SortExpression="VD-ID" />
                <asp:BoundField DataField="ProduktType" HeaderText="ProduktType" 
                    SortExpression="ProduktType" />
                <asp:BoundField DataField="Shareholder ID" HeaderText="Shareholder ID" 
                    SortExpression="Shareholder ID" />
                <asp:BoundField DataField="Supplier ID" HeaderText="Supplier ID" 
                    SortExpression="Supplier ID" />
            </Columns>
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <RowStyle ForeColor="#000066" />
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#007DBB" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#00547E" />
        </asp:GridView>
        <asp:SqlDataSource ID="ListLatestTrade" runat="server" 
            ConnectionString="<%$ ConnectionStrings:130087_EssConnectionString %>" 
            
            
            
            SelectCommand="SELECT TOP (20)  [Dato prissikring dato], Park_GSRN, Pris * 100 AS Pris, Produktnavn, Description, [Agreement Type], [VD-ID], [Agreement Type] AS ProduktType, [Shareholder ID], [Supplier ID] FROM [Vindenergi Danmark amba$DVE Agreement Table] ORDER BY [Dato prissikring dato] DESC">
        </asp:SqlDataSource>
        <br />





       

        <br />
        <br />

    </ContentTemplate>
    </asp:UpdatePanel>







</asp:Content>
