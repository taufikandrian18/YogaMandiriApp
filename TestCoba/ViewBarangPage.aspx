<%@ Page Title="" Language="C#" MasterPageFile="~/Visitor.Master" AutoEventWireup="true" CodeBehind="ViewBarangPage.aspx.cs" Inherits="TestCoba.ViewBarangPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="app-page-title">
            <div class="page-title-wrapper">
                <div class="page-title-heading">
                    <div class="page-title-icon">
                        <i class="pe-7s-car icon-gradient bg-mean-fruit">
                        </i>
                    </div>
                    <div>Detail Barang
                        <div class="page-title-subheading">Halaman Untuk Memuat Detail Barang.
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
        <div class="card-body">
            <h5 class="card-title">
            <asp:Label ID="lblStatusPage" runat="server" Text="View Barang"></asp:Label></h5>
            <div class="form-row">
                <div class="col-md-2">
                    <div class="position-relative form-group">
                        <asp:Label ID="lblNamaBrg" runat="server" Text="Nama Barang : "></asp:Label><br />
                        <asp:Label ID="valNoInv" runat="server" Font-Bold="true" Text="Label"></asp:Label>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="position-relative form-group">
                        <asp:Label ID="lblSupName" runat="server" Text="Supplier Name"></asp:Label><br />
                        <asp:Label ID="lblValNoSup" runat="server" Font-Bold="true" Text="Label"></asp:Label>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="position-relative form-group">
                        <asp:Label ID="lblInvName" runat="server" Text="Inventory"></asp:Label><br />
                        <asp:Label ID="lblInvVal" runat="server" Font-Bold="true" Text="Label"></asp:Label>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="position-relative form-group">
                        <asp:Label ID="jmlQty" runat="server" Text="Quantity"></asp:Label><br />
                        <asp:Label ID="valQty" runat="server" Font-Bold="true" Text="Label"></asp:Label>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="position-relative form-group">
                        <asp:Label ID="jmlBox" runat="server" Text="Inventory"></asp:Label><br />
                        <asp:Label ID="valBox" runat="server" Font-Bold="true" Text="Label"></asp:Label>
                    </div>
                </div>
            </div>
            <div id="table_box_bootstrap" class="table table-responsive table-light table-bordered table-hover">
            <div class="position-relative form-group text-center">
                <asp:Label ID="lblNormal" runat="server" Font-Bold="true" Text="Label" Visible="false"></asp:Label>
            </div>
            <table class="mb-0 table">
                <thead>
                    <tr style="background-color:white">
                        <th>Tanggal Barang Beli</th>
                        <th>No Invoice Pembelian</th>
                        <th>Nama Barang</th>
                        <th>Qty</th>
                        <th>Jml Box</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RepeaterListItm" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%#Eval("TanggalPOPembelian")%>
                                </td>
                                <td>
                                    <%#Eval("NoInvoicePembelian")%>
                                </td>
                                <td>
                                    <%#Eval("NamaBarang")%>
                                </td>
                                <td>
                                    <%#Eval("QtyBarangPOPembelianDtl")%>
                                </td>
                                <td>
                                    <%#Eval("JumlahBoxPOPembelianDtl")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr>
                        <td colspan="5">
                            <div class="position-relative form-group">
                                <asp:Button ID="backButton" runat="server" CssClass="mt-2 btn btn-danger" Text="Back" OnClientClick="JavaScript:window.history.back(1);return false;" />
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div id="table_box_bootstrapJual" runat="server" visible="false" class="table table-responsive table-light table-bordered table-hover">
            <div class="position-relative form-group text-center">
                <asp:Label ID="lblPenjualan" runat="server" Font-Bold="true" Text="Label" Visible="false"></asp:Label>
            </div>
            <table class="mb-0 table">
                <thead>
                    <tr style="background-color:white">
                        <th>Tanggal Barang Jual</th>
                        <th>No Invoice Penjualan</th>
                        <th>Nama Barang</th>
                        <th>Qty</th>
                        <th>Jml Box</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="Repeater1" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%#Eval("TanggalPOPenjualan")%>
                                </td>
                                <td>
                                    <%#Eval("NoInvoicePenjualan")%>
                                </td>
                                <td>
                                    <%#Eval("NamaBarang")%>
                                </td>
                                <td>
                                    <%#Eval("QtyBarangPOPenjualanDtl")%>
                                </td>
                                <td>
                                    <%#Eval("JumlahBoxPOPenjualanDtl")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr>
                        <td colspan="5">
                            <div class="position-relative form-group">
                                <asp:Button ID="Button1" runat="server" CssClass="mt-2 btn btn-danger" Text="Back" OnClientClick="JavaScript:window.history.go(-1);return false;" />
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        </div>
</asp:Content>
