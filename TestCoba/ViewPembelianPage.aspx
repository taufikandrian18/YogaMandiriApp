<%@ Page Title="" Language="C#" MasterPageFile="~/Visitor.Master" AutoEventWireup="true" CodeBehind="ViewPembelianPage.aspx.cs" Inherits="TestCoba.ViewPembelianPage" %>
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
                    <div>Detail Pembelian
                        <div class="page-title-subheading">Halaman Untuk Memuat Detail Pembelian.
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
            <asp:Label ID="lblStatusPage" runat="server" Text="View Pembelian"></asp:Label></h5>
            <div class="form-row">
                <div class="col-md-2">
                    <div class="position-relative form-group">
                        <asp:Label ID="lblNoInvoice" runat="server" Text="No Invoice : "></asp:Label><br />
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
                        <asp:Label ID="lblTanggalInvoice" runat="server" Text="Tanggal Pembelian"></asp:Label><br />
                        <asp:Label ID="lblTglInvVal" runat="server" Font-Bold="true" Text="Label"></asp:Label>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="position-relative form-group">
                        <asp:Label ID="lblJenisTagihan" runat="server" Text="Jenis Tagihan"></asp:Label><br />
                        <asp:Label ID="lblJnisTagihanVal" runat="server" Font-Bold="true" Text="Label"></asp:Label>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="position-relative form-group">
                        <asp:Label ID="lblJtuhTempo" runat="server" Text="Tanggal Jatuh Tempo"></asp:Label><br />
                        <asp:Label ID="lblJthTempVal" runat="server" Font-Bold="true" Text="Label"></asp:Label>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="position-relative form-group">
                        <asp:Label ID="lblBrgNameBank" runat="server" Text="Detail Rekening Bank"></asp:Label><br />
                        <asp:Label ID="lblNmBankVal" runat="server" Font-Bold="true" Text="Label"></asp:Label>
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
                        <asp:Label ID="lblStatusMst" runat="server" Text="Status Invoice"></asp:Label><br />
                        <asp:Label ID="valStatusMst" runat="server" Font-Bold="true" Text="Label"></asp:Label>
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
                        <th>Nama Barang</th>
                        <th>Item Qty</th>
                        <th>Harga / Kg</th>
                        <th>Jumlah Box</th>
                        <th>Total Harga</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RepeaterListItm" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%#Eval("NamaBarang")%>
                                </td>
                                <td>
                                    <%#Eval("QtyBarangPOPembelianDtl")%>
                                </td>
                                <td>
                                    <%#Eval("HargaPOPembelianDtl")%>
                                </td>
                                <td>
                                    <%#Eval("JumlahBoxPOPembelianDtl")%>
                                </td>
                                <td>
                                    <%#Eval("TotalHargaPOPembelianDtl", "{0:c}")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr>
                        <td colspan="3">
                            <div class="position-relative form-group">
                                <asp:Button ID="backButton" runat="server" CssClass="mt-2 btn btn-danger" Text="Back" OnClientClick="JavaScript:window.history.back(1);return false;" />
                            </div>
                        </td>
                        <td>
                            <div class="position-relative form-group">
                                <asp:Label ID="lblGrandTotal" runat="server" Text="Grand Total : "></asp:Label>
                            </div>
                        </td>
                        <td>
                            <div class="position-relative form-group">
                                <asp:Label ID="lblGTVal" runat="server" Font-Bold="true" Text="Label"></asp:Label>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div id="table_box_bootstrapRetur" runat="server" visible="false" class="table table-responsive table-light table-bordered table-hover">
            <div class="position-relative form-group text-center">
                <asp:Label ID="lblRetur" runat="server" Font-Bold="true" Text="Label" Visible="false"></asp:Label>
            </div>
            <table class="mb-0 table">
                <thead>
                    <tr style="background-color:white">
                        <th>Nama Barang</th>
                        <th>Item Qty</th>
                        <th>Harga / Kg</th>
                        <th>Jumlah Box</th>
                        <th>Total Harga</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="Repeater1" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%#Eval("NamaBarang")%>
                                </td>
                                <td>
                                    <%#Eval("QtyBarangPOPembelianReturDtl")%>
                                </td>
                                <td>
                                    <%#Eval("HargaPOPembelianReturDtl")%>
                                </td>
                                <td>
                                    <%#Eval("JumlahBoxPOPembelianReturDtl")%>
                                </td>
                                <td>
                                    <%#Eval("TotalHargaPOPembelianReturDtl", "{0:c}")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr>
                        <td colspan="3">
                            <div class="position-relative form-group">
                                <asp:Button ID="Button1" runat="server" CssClass="mt-2 btn btn-danger" Text="Back" OnClientClick="JavaScript:window.history.go(-1);return false;" />
                            </div>
                        </td>
                        <td>
                            <div class="position-relative form-group">
                                <asp:Label ID="Label1" runat="server" Text="Grand Total : "></asp:Label>
                            </div>
                        </td>
                        <td>
                            <div class="position-relative form-group">
                                <asp:Label ID="Label2" runat="server" Font-Bold="true" Text="Label"></asp:Label>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        </div>
</asp:Content>
