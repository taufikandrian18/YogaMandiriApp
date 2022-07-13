<%@ Page Title="" Language="C#" MasterPageFile="~/Visitor.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="TestCoba.index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style></style>
    <script></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="app-page-title">
            <div class="page-title-wrapper">
                <div class="page-title-heading">
                    <div class="page-title-icon">
                        <i class="pe-7s-car icon-gradient bg-mean-fruit">
                        </i>
                    </div>
                    <div>Analytics Dashboard
                        <div class="page-title-subheading">Halaman Ini Memuat Seluruh Ringkasan Informasi Yang Terekam Didalam Sistem.
                        </div>
                    </div>
                </div>
                <div class="page-title-actions">
                    <!--<button type="button" data-toggle="tooltip" title="Example Tooltip" data-placement="bottom" class="btn-shadow mr-3 btn btn-dark">
                        <i class="fa fa-star"></i>
                    </button>-->
                </div>    
            </div>
        </div>
        <div id="statHeader" runat="server" class="main-card mb-3 card">
            <div class="no-gutters row">
                <div id="statSubHeader1" runat="server" class="col-md-4">
                    <div class="pt-0 pb-0 card-body">
                        <ul class="list-group list-group-flush">
                            <li class="list-group-item">
                                <div class="widget-content p-0">
                                    <div class="widget-content-outer">
                                        <div class="widget-content-wrapper">
                                            <div class="widget-content-left">
                                                <div class="widget-heading">Total Pembelian</div>
                                                <div class="widget-subheading">Pengeluaran Setahun</div>
                                            </div>
                                            <div class="widget-content-right">
                                                <div class="widget-numbers text-success">
                                                    <asp:Label ID="lblTotPmb" runat="server" Text=""></asp:Label></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </li>
                            <li class="list-group-item">
                                <div class="widget-content p-0">
                                    <div class="widget-content-outer">
                                        <div class="widget-content-wrapper">
                                            <div class="widget-content-left">
                                                <div class="widget-heading">Total Customer</div>
                                                <div class="widget-subheading">Jumlah Customer Selama Setahun</div>
                                            </div>
                                            <div class="widget-content-right">
                                                <div class="widget-numbers text-primary">
                                                    <asp:Label ID="lblTotCust" runat="server" Text=""></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
                <div id="statSubHeader2" runat="server" class="col-md-4">
                    <div class="pt-0 pb-0 card-body">
                        <ul class="list-group list-group-flush">
                            <li class="list-group-item">
                                <div class="widget-content p-0">
                                    <div class="widget-content-outer">
                                        <div class="widget-content-wrapper">
                                            <div class="widget-content-left">
                                                <div class="widget-heading">Total Supplier</div>
                                                <div class="widget-subheading">Jumlah Supplier Dalam Setahun</div>
                                            </div>
                                            <div class="widget-content-right">
                                                <div class="widget-numbers text-primary">
                                                    <asp:Label ID="lblTotSup" runat="server" Text=""></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </li>
                            <li class="list-group-item">
                                <div class="widget-content p-0">
                                    <div class="widget-content-outer">
                                        <div class="widget-content-wrapper">
                                            <div class="widget-content-left">
                                                <div class="widget-heading">Total Penjualan</div>
                                                <div class="widget-subheading">Jml/Kg Setahun</div>
                                            </div>
                                            <div class="widget-content-right">
                                                <div class="widget-numbers text-warning">
                                                    <asp:Label ID="lblTotPenjKg" runat="server" Text=""></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
                <div id="statSubHeader3" runat="server" class="col-md-4">
                    <div class="pt-0 pb-0 card-body">
                        <ul class="list-group list-group-flush">
                            <li class="list-group-item">
                                <div class="widget-content p-0">
                                    <div class="widget-content-outer">
                                        <div class="widget-content-wrapper">
                                            <div class="widget-content-left">
                                                <div class="widget-heading">Total Pembelian</div>
                                                <div class="widget-subheading">Jml/Kg Setahun</div>
                                            </div>
                                            <div class="widget-content-right">
                                                <div class="widget-numbers text-warning">
                                                    <asp:Label ID="lblTotPembKg" runat="server" Text=""></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </li>
                            <li class="list-group-item">
                                <div class="widget-content p-0">
                                    <div class="widget-content-outer">
                                        <div class="widget-content-wrapper">
                                            <div class="widget-content-left">
                                                <div class="widget-heading">Total Penjualan</div>
                                                <div class="widget-subheading">Pendapatan Setahun</div>
                                            </div>
                                            <div class="widget-content-right">
                                                <div class="widget-numbers text-success">
                                                    <asp:Label ID="lblTotPenj" runat="server" Text=""></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <div id="chartHeader" runat="server" class="main-card mb-3 card">
            <div class="no-gutters row">
                <table id="chartSubHeader1" runat="server" border="0" style="margin-left:35px">
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" CssClass="font-weight-bold" Text="Tipe Transaksi"></asp:Label>
                            <asp:DropDownList ID="ddlCountries" CssClass="form-control" runat="server">
                                <asp:ListItem Text="Penjualan" Value="POPenjualanDtl" />
                                <asp:ListItem Text="Pembelian" Value="POPembelianDtl" />
                            </asp:DropDownList>
         
                            
                        </td>
                        <td style="padding-left:20px">
                            <asp:Label ID="Label2" runat="server" CssClass="font-weight-bold" Text="Tipe Chart"></asp:Label>
                            <asp:DropDownList ID="ddlChartType" CssClass="form-control" runat="server">
                                <asp:ListItem Text="Pie" Value="1" />
                                <asp:ListItem Text="Doughnut" Value="2" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="dvChart">
                            </div>
                        </td>
                        <td style="padding-left:20px">
                            <div id="dvLegend">
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
     <script type="text/javascript">
         $(function () {
             LoadChart();
             $("[id*=ddlCountries]").bind("change", function () {
                 LoadChart();
             });
             $("[id*=ddlChartType]").bind("change", function () {
                 LoadChart();
             });
         });
         function LoadChart() {
             var chartType = parseInt($("[id*=ddlChartType]").val());
             $.ajax({
                 type: "POST",
                 url: "index.aspx/GetChart",
                 data: "{country: '" + $("[id*=ddlCountries]").val() + "'}",
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (r) {
                     $("#dvChart").html("");
                     $("#dvLegend").html("");
                     var data = eval(r.d);
                     var el = document.createElement('canvas');
                     $("#dvChart")[0].appendChild(el);

                     //Fix for IE 8
                     if ($.browser.msie && $.browser.version == "8.0") {
                         G_vmlCanvasManager.initElement(el);
                     }
                     var ctx = el.getContext('2d');
                     var userStrengthsChart;
                     switch (chartType) {
                         case 1:
                             userStrengthsChart = new Chart(ctx).Pie(data);
                             break;
                         case 2:
                             userStrengthsChart = new Chart(ctx).Doughnut(data);
                             break;
                     }
                     for (var i = 0; i < data.length; i++) {
                         var div = $("<div />");
                         div.css("margin-bottom", "10px");
                         div.html("<span style = 'display:inline-block;height:10px;width:10px;background-color:" + data[i].color + "'></span> " + data[i].text);
                         $("#dvLegend").append(div);
                     }
                 },
                 failure: function (response) {
                     alert('There was an error.');
                 }
             });
         }
</script>
</asp:Content>
