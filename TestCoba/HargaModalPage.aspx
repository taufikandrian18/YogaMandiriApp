<%@ Page Title="" Language="C#" MasterPageFile="~/Visitor.Master" AutoEventWireup="true" CodeBehind="HargaModalPage.aspx.cs" Inherits="TestCoba.HargaModalPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        tr:nth-child(odd) {
            background-color: #f2f2f2;
        }
    </style> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="app-page-title">
            <div class="page-title-wrapper">
                <div class="page-title-heading">
                    <div class="page-title-icon">
                        <i class="pe-7s-car icon-gradient bg-mean-fruit">
                        </i>
                    </div>
                    <div>List Rekap Modal
                        <div class="page-title-subheading">Halaman Untuk Memuat Seluruh List Harga Modal yang Telah Terekam di Sistem.
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <hr />
        <div class="card-body">
            <div class="form-row">
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <div class="col-md-2">
                    <div class="position-relative form-group">
                        <asp:Label ID="lblTanggalInvoice" runat="server" Text="Tanggal Penjualan"></asp:Label>
                        <asp:TextBox ID="txtCldBrgBeli" runat="server" placeholder="Tanggal Jual" Text="mm/dd/yyyy" onkeydown="return false;" CssClass="form-control fa-calendar" required="required"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtCldBrgBeli" runat="server" Format="MM/dd/yyyy" />
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="position-relative form-group">
                        <br />
                        <asp:Button ID="btnCari" runat="server" CssClass="btn btn-dark" Text="Search" OnClick="btnCari_Click" />
                    </div>
                </div>
            </div>
        </div>
        <div class="card-body">
            <div id="table_box_bootstrap" class="table table-responsive table-light table-bordered table-hover">
                <table class="mb-0 table">
                    <thead>
                        <tr style="background-color:white">
                            <th>Supplier Name</th>
                            <th>Tanggal Penjualan</th>
                            <th>Tanggal Beli Barang</th>
                            <th>Status Invoice Penjualan</th>
                            <th>Customer Name</th>
                            <th>Nama Barang</th>
                            <th>Qty Barang</th>
                            <th>Harga Jual</th>
                            <th>Harga Beli</th>
                            <th>Selisih penjualan</th>
                            <th>Total Laba Per Produk</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="RepeaterListItm" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <%#Eval("SupplierName")%>
                                    </td>
                                    <td>
                                        <%#Eval("TanggalPOPenjualan")%>
                                    </td>
                                    <td>
                                        <%#Eval("TanggalPOPembelian")%>
                                    </td>
                                    <td>
                                        <%#Eval("StatusInvoicePenjualan")%>
                                    </td>
                                    <td>
                                        <%#Eval("CustomerName")%>
                                    </td>
                                    <td>
                                        <%#Eval("NamaBarang")%>
                                    </td>
                                    <td>
                                        <%#Eval("QtyBarangPOPenjualanDtl")%> Kg
                                    </td>
                                    <td>
                                        <%#Eval("HargaPOPenjualanDtl", "{0:c}")%>
                                    </td>
                                    <td>
                                        <%#Eval("HargaPOPembelianDtl", "{0:c}")%>
                                    </td>
                                    <td>
                                        <%#Eval("selisih", "{0:c}")%>
                                    </td>
                                    <td>
                                        <%#Eval("TotalSelisih", "{0:c}")%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
        </div>
</asp:Content>
