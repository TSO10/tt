<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SuppliersChart.ascx.cs"
    Inherits="VEDK_Dashboard.Controls.SuppliersChart" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<div class="pnl-left">
    <asp:Chart ID="tChart" runat="server" Width="800px">
        <Series>
            <asp:Series ChartArea="caChart" ChartType="Line" IsXValueIndexed="true" Name="sLine"
                XValueType="DateTime" LabelBorderDashStyle="NotSet" LabelBorderWidth="0">
            </asp:Series>
            <asp:Series ChartArea="caChart" ChartType="Point" IsXValueIndexed="true" Label="#VALX{dd-MM-yy HH-mm};#VAL"
                LabelBorderColor="LightSteelBlue" LabelToolTip="#VALX;#VAL" MarkerColor="Red"
                MarkerSize="7" MarkerStyle="Circle" Name="sPoint" XValueType="DateTime" Font="Microsoft Sans Serif, 9pt"
                LabelBorderWidth="1">
                <SmartLabelStyle MaxMovingDistance="90" MinMovingDistance="60" AllowOutsidePlotArea="Yes"
                    Enabled="true" />
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="caChart" BorderColor="Silver">
                <AxisY LineColor="Silver" Interval="0.25" IntervalType="Number">
                    <MajorGrid LineColor="Silver" />
                    <MajorTickMark LineColor="Silver" />
                </AxisY>
                <AxisX LineColor="Silver" TitleFont="Microsoft Sans Serif, 8pt, style=Bold">
                    <MajorGrid LineColor="Silver" />
                    <MinorGrid LineColor="Silver" />
                    <MajorTickMark LineColor="Silver" />
                    <MinorTickMark LineColor="Silver" />
                </AxisX>
                <AxisX2 LineColor="Silver">
                    <MajorTickMark LineColor="Silver" />
                </AxisX2>
                <AxisY2 LineColor="Silver">
                    <MajorTickMark LineColor="Silver" />
                </AxisY2>
            </asp:ChartArea>
        </ChartAreas>
    </asp:Chart>
    <div class="slider-range-content">
        <asp:HiddenField ID="SliderBeginHiddenValue" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="SliderEndHiddenValue" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="SliderTotal" ClientIDMode="Static" runat="server" />
        <div id="slider-range">
        </div>
        <asp:Button runat="server" ID="btnChangeChart" ClientIDMode="Static" Text="change"
            OnClick="btnChangeChart_Click" Style="display: none;" />
    </div>
</div>
<div class="pnl-rigth">
    <div id="tabs">
        <ul>
            <li><a href="#tabs-1">Statitics</a></li>
            <li><a href="#tabs-2">Opening</a></li>
        </ul>
        <div id="tabs-1">
            <h2>
                Filter dato</h2>
            <table>
                <tr>
                    <td>
                        Maxium:
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblSelectedDataMax" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Aveage:
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblSelectedDataAverage" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Minimum:
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblSelectedDataMin" />
                    </td>
                </tr>
            </table>
            <h2>
                Prisgraf</h2>
            <table>
                <tr>
                    <td>
                        <strong>Start Date:</strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblStartDate" Style="font-weight: 700" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>End Date: </strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblEndDate" Style="font-weight: 700" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Maxium:
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblChartSelectedDateMax" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Aveage:
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblChartSelectedDateAverage" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Minimum:
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblChartSelectedDateMin" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="tabs-2">
            <b><span>Chosen date:</span>
                <asp:Label ID="lblSelectedDate" runat="server" />
            </b>
            <div class="separator-height-20"></div>
            <asp:GridView runat="server" OnRowDataBound="grStatitics_RowDataBound" ID="grStatitics"
                ViewStateMode="Disabled" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="SupplierName" HeaderText="Supplier" />
                    <asp:TemplateField HeaderText="Price">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="ltPrice" />
                        </ItemTemplate>
                        <ItemStyle Width="100" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Difference">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="ltDifference" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>
