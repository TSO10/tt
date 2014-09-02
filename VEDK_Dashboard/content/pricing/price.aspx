<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="price.aspx.cs"
    Inherits="VEDK_Dashboard.pricing.price" Title="Price" MaintainScrollPositionOnPostback="true" %>

<%@ Register TagPrefix="cus" TagName="ProductCategory" Src="~/Controls/ProductCategory.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        var time = 1000;
        var isopen = false;
        var scroll = 0;
        var activeTab = 0;
        function beginRequestHandler(sender, args) {
            $("#loadPanel").css("display", "block");
            scroll = $(window).scrollTop();
        }

        function InitFilterAnimation(sender, args) {
            InitChartContent();
            ShowChart();
            var panelId = '#<%=panelFilters.ClientID%>';
            if (isopen) {
                $(panelId).show();
            }
            else {
                $(panelId).hide();
            }
            $("#lnkFilter").click(function () {
                if ($(panelId).css("display") == "none") {
                    $(panelId).slideDown(time);
                    isopen = true;
                }
                else {
                    $(panelId).slideUp(time);
                    isopen = false;
                }
            });
            $(window).scrollTop(scroll);
        }
        function ShowChart() {
            var sp = $("span.display-slider").first();
            if ($(sp).length != 0) {
                $(sp).parent().slideDown(time);
            }
            else {
                $("div.display-none").show();
            }
        }

        $(document).ready(function () {
            InitFilterAnimation();
            if (Sys.WebForms.PageRequestManager)
                if (!Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack()) {
                    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginRequestHandler);
                    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitFilterAnimation);
                }
        });

        function ShowHideImage(lnk) {
            activeTab = 0;
            if ($(lnk).text() == "-") {
                $(lnk).parent().parent().parent().find("div img").slideUp(time);
            }
        }

        function InitChartContent() {
            InitChartSlider();
            InitChartTabs();
        }
        function InitChartTabs() {
            $("#tabs").tabs({ active: activeTab });
            $("#tabs").on("tabsactivate", function (event, ui) {
                activeTab = $("#tabs").tabs("option").active;
            });
        }
        function InitChartSlider() {
            if ($("#slider-range").length != 0) {
                if ($("#starTimeHiddenValue").val() != "") {
                    $("#slider-range").slider({
                        range: true,
                        min: 0,
                        max: parseInt($("#SliderTotal").val()),
                        values: [parseInt($("#SliderBeginHiddenValue").val()), parseInt($("#SliderEndHiddenValue").val())],
                        slide: function (event, ui) {
                            //$("#amount").val("$" + ui.values[0] + " - $" + ui.values[1]);
                        },
                        stop: function (event, ui) {
                            $("#SliderBeginHiddenValue").val(ui.values[0]);
                            $("#SliderEndHiddenValue").val(ui.values[1]);
                            $("#btnChangeChart").click();
                        }
                    });
                }
            }
        }

        //$(document).ready(function () {
        //    agreementUpdater.InitUpdater();
        //});
       
    </script>
    <asp:UpdatePanel runat="server" ID="upProducts">
        <ContentTemplate>
            <asp:Timer ID="tUpdateLayout" runat="server" OnTick="tUpdateLayout_Tick" Interval="10000" />
            <div class="main-load" id="loadPanel">
            </div>
            <h2>
                <a href="javascript:void(0)" id="lnkFilter">Filter</a></h2>
            <hr />
            <div id="panelFilters" runat="server" style="display: none;">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tbody>
                        <tr class="filters">
                            <td>
                                <p>
                                    <b>Product Categories:</b></p>
                                <asp:CheckBoxList runat="server" ID="chblCategories" RepeatDirection="Vertical" DataValueField="id"
                                    DataTextField="Name" AutoPostBack="true" OnTextChanged="chblCategories_TextChanged"
                                    RepeatLayout="Flow">
                                </asp:CheckBoxList>
                            </td>
                            <td>
                                <p>
                                    <b>Choosen Date:</b></p>
                                <p>
                                    <b>
                                        <asp:Literal runat="server" ID="litSelectedDate"></asp:Literal></b></p>
                                <asp:Calendar runat="server" ID="cSelectedDate" OnSelectionChanged="cSelectedDate_SelectionChanged">
                                </asp:Calendar>
                            </td>
                            <td>
                                <p>
                                    <b>Choosen region(s):</b></p>
                                <p>
                                    <b>
                                        <asp:Literal runat="server" ID="litSelectedRegion"></asp:Literal></b></p>
                                <asp:ImageButton runat="server" ID="imgNone" ImageUrl="~/images/blue.jpg" OnClick="imgNone_Click"
                                    Width="45px" />
                                <asp:ImageButton runat="server" ID="imgDK1" ImageUrl="~/images/blue-gray.jpg" OnClick="imgDK1_Click"
                                    Width="45px" />
                                <asp:ImageButton runat="server" ID="imgDK2" ImageUrl="~/images/gray-blue.jpg" OnClick="imgDK2_Click"
                                    Width="45px" />
                            </td>
                            <td class="supplierStatus">
                                <p>
                                    <b>Supplier Status:</b></p>
                                <asp:Repeater runat="server" ID="rSuppliers" OnItemDataBound="rSuppliersRecieved_ItemDataBound">
                                    <ItemTemplate>
                                        <p>
                                            <asp:Literal runat="server" ID="lblSupplier" /></p>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </td>
                            <td>
                                <asp:Image runat="server" ID="imgStoreStatus" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <hr />
            <br />
            <table border="1" cellspacing="0" width="100%">
                <asp:Repeater runat="server" ID="rCategories" OnItemDataBound="rCategories_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <cus:ProductCategory runat="server" ID="pcCategory" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <br />
    <br />

    <ul>
    <li>
        <p><img src="../../images/arrow_up.png" /> Prisen er højere end forrige pris.</p></li>
    <li><p><img src="../../images/arrow_down.png" /> Prisen er lavere end forrige pris.</p></li>
    <li><p><img src="../../images/exmark.png" /> Balanceansvarliges seneste pris er 1.5 øre større eller mindre end forrige.</p></li>
    <li><p><img src="../../images/stop.png" /> Forskellen mellem balanceansvarlige seneste priser er større end 2 øre.  </p></li>
    <li><p><img src="../../images/customerprice.png" /> Prisen er udsendt i den daglige pris-email/pris-SMS.</p></li>
    </ul>

    <div id="container">
        <iframe id="myiframe" src="http://www.vindenergi.dk/forside.aspx" scrolling="no">
        </iframe>
    </div>
</asp:Content>
