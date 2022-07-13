<%@ Page Title="" Language="C#" MasterPageFile="~/Visitor.Master" AutoEventWireup="true" CodeBehind="NeracaTransaksi.aspx.cs" Inherits="TestCoba.NeracaTransaksi" %>
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
                    <div>Neraca Per Transaksi
                        <div class="page-title-subheading">Halaman Untuk Memuat Seluruh List Neraca per Transaksi Dengan Periode Tertentu.
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
            <div class="table table-responsive table-light table-bordered table-hover">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblTgl" runat="server" CssClass="font-weight-bold" Text="Tanggal"></asp:Label>
                            <asp:DropDownList ID="ddlTanggal" runat="server" CssClass="form-control">
                                <asp:ListItem Text="01" Value="01"></asp:ListItem>
                                <asp:ListItem Text="02" Value="02"></asp:ListItem>
                                <asp:ListItem Text="03" Value="03"></asp:ListItem>
                                <asp:ListItem Text="04" Value="04"></asp:ListItem>
                                <asp:ListItem Text="05" Value="05"></asp:ListItem>
                                <asp:ListItem Text="06" Value="06"></asp:ListItem>
                                <asp:ListItem Text="07" Value="07"></asp:ListItem>
                                <asp:ListItem Text="08" Value="08"></asp:ListItem>
                                <asp:ListItem Text="09" Value="09"></asp:ListItem>
                                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                <asp:ListItem Text="24" Value="24"></asp:ListItem>
                                <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                <asp:ListItem Text="26" Value="26"></asp:ListItem>
                                <asp:ListItem Text="27" Value="27"></asp:ListItem>
                                <asp:ListItem Text="28" Value="28"></asp:ListItem>
                                <asp:ListItem Text="29" Value="29"></asp:ListItem>
                                <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                <asp:ListItem Text="31" Value="31"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label1" runat="server" CssClass="font-weight-bold" Text="Bulan"></asp:Label>
                            <asp:DropDownList ID="ddlMonths" runat="server" CssClass="form-control">
                                <asp:ListItem Text="January" Value="01"></asp:ListItem>
                                <asp:ListItem Text="February" Value="02"></asp:ListItem>
                                <asp:ListItem Text="March" Value="03"></asp:ListItem>
                                <asp:ListItem Text="April" Value="04"></asp:ListItem>
                                <asp:ListItem Text="May" Value="05"></asp:ListItem>
                                <asp:ListItem Text="June" Value="06"></asp:ListItem>
                                <asp:ListItem Text="July" Value="07"></asp:ListItem>
                                <asp:ListItem Text="August" Value="08"></asp:ListItem>
                                <asp:ListItem Text="September" Value="09"></asp:ListItem>
                                <asp:ListItem Text="October" Value="10"></asp:ListItem>
                                <asp:ListItem Text="November" Value="11"></asp:ListItem>
                                <asp:ListItem Text="December" Value="12"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" CssClass="font-weight-bold" Text="Tahun"></asp:Label>
                            <asp:DropDownList ID="ddlYears" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <br />
                            <asp:Button ID="BtnSearch" runat="server" Text="Search" CssClass="btn btn-dark" OnClick="BtnSearch_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <hr />
        <div class="card-body">
        <div class="table table-responsive table-light table-bordered table-hover">
        <table class="mb-0 table">
            <thead>
                <tr style="background-color:white">
                    <th>No Faktur</th>
                    <th>TglFaktur</th>
                    <th>Supplier</th>
                    <th>Nama Barang</th>
                    <th>Customer</th>
                    <th>Jumlah(kg)</th>
                    <th>Box</th>
                    <th>HrgJual(kg)</th>
                    <th>HrgBeli(kg)</th>
                    <th>Selisih</th>
                    <th>TotSelisih</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="RepeaterListNrcTransaksi" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#Eval("NoInvoicePenjualan")%>
                            </td>
                            <td>
                                <%#Eval("TanggalPOPenjualan")%>
                            </td>
                            <td>
                                <%#Eval("SupplierName")%>
                            </td>
                            <td>
                                <%#Eval("NamaBarang")%>
                            </td>
                            <td>
                                <%#Eval("CustomerName")%>
                            </td>
                            <td>
                                <%#Eval("QtyBarangPOPenjualanDtl", "{0:n}")%>
                            </td>
                            <td>
                                <%#Eval("JumlahBoxPOPenjualanDtl")%>
                            </td>
                            <td>
                                <%#Eval("HargaPOPenjualanDtl", "{0:C}")%>
                            </td>
                            <td>
                                <%#Eval("hrg", "{0:C}")%>
                            </td>
                            <td>
                                <%#Eval("Selisih", "{0:n}")%>
                            </td>
                            <td>
                                <%#Eval("TotSelisih", "{0:n}")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
            <tfoot>
                <tr>
                    <td colspan="5">Total Margin Per Hari : </td>
                    <td colspan="5"><asp:Label ID="GTQty" runat="server" CssClass="font-weight-bold" Text="0,00 KG"></asp:Label></td>
                    <td>
                        <asp:Label ID="GTPembelian" runat="server" CssClass="font-weight-bold" Text="Rp0"></asp:Label>
                    </td>
                </tr>
            </tfoot>
        </table>
        </div>
    </div>
</asp:Content>
