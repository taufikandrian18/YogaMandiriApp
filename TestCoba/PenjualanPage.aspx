<%@ Page Title="" Language="C#" MasterPageFile="~/Visitor.Master" AutoEventWireup="true" CodeBehind="PenjualanPage.aspx.cs" Inherits="TestCoba.PenjualanPage" %>
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
                    <div>List Penjualan
                        <div class="page-title-subheading">Halaman Untuk Memuat Seluruh List Penjualan Dengan Periode Tertentu.
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
            <div class="position-relative form-group">
                <asp:Label ID="Label6" runat="server" CssClass="font-weight-bold" Text="Search Box"></asp:Label>
                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" Enabled="false" onkeyup='Filter(this);'></asp:TextBox>
            </div>
            <table>
            <tr>
                <td id="btnValVsbl" runat="server"><a href="AddPenjualanPage.aspx" class="btn btn-primary">Tambah Invoice Penjualan</a></td>
                <td>&nbsp;</td>
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
                    <asp:Label ID="Label4" runat="server" CssClass="font-weight-bold" Text="Status Invoice"></asp:Label>
                    <asp:DropDownList ID="ddlStatusInvoice" runat="server" CssClass="form-control">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="-------------------" Value=""></asp:ListItem>
                        <asp:ListItem Text="NORMAL" Value="NORMAL"></asp:ListItem>
                        <asp:ListItem Text="RETUR" Value="RETUR"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="Label5" runat="server" CssClass="font-weight-bold" Text="Sort By"></asp:Label>
                    <asp:DropDownList ID="ddlSortBy" runat="server" CssClass="form-control">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="-------------------" Value=""></asp:ListItem>
                        <asp:ListItem Text="Invoice" Value="a.NoInvoicePenjualan"></asp:ListItem>
                        <asp:ListItem Text="Jenis Tagihan" Value="a.JenisTagihan"></asp:ListItem>
                        <asp:ListItem Text="Pembelian" Value="a.TotalTagihanPenjualan"></asp:ListItem>
                        <asp:ListItem Text="Tanggal Beli" Value="a.TanggalPOPenjualan"></asp:ListItem>
                        <asp:ListItem Text="Jatuh Tempo" Value="a.TanggalPOJatuhTmpPenjualan"></asp:ListItem>
                        <asp:ListItem Text="Supplier" Value="b.SupplierName"></asp:ListItem>
                        <asp:ListItem Text="Inventory" Value="c.IvnStorage"></asp:ListItem>
                        <asp:ListItem Text="Customer" Value="d.CustomerName"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <br />
                    <asp:Button ID="BtnSearch" runat="server" Text="Search" CssClass="btn btn-secondary" OnClick="BtnSearch_Click" />
                </td>
            </tr>
        </table>
        <hr />
        <div class="card-body">
        <div class="table table-responsive table-light table-bordered table-hover">
        <table class="mb-0 table">
            <thead>
                <tr style="background-color:white">
                    <th style="visibility:hidden">id</th>
                    <th>No Invoice</th>
                    <th>Nama Customer</th>
                    <th>Tanggal Penjualan</th>
                    <th>Tanggal Jatuh Tempo</th>
                    <th>Jenis Tagihan</th>
                    <th>Total Penjualan</th>
                    <th>Status Invoice</th>
                    <th id="thVal" runat="server" colspan="3">Action</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="RepeaterListPembelian" runat="server" OnItemDataBound="RepeaterListPembelian_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td id="tdIdVal" runat="server" style="visibility:hidden">
                                <%#Eval("IdPOPenjualanMst")%>
                            </td>
                            <td>
                                <%#Eval("NoInvoicePenjualan")%>
                            </td>
                            <td>
                                <%#Eval("CustomerName")%>
                            </td>
                            <td>
                                <%#Eval("TanggalPOPenjualan")%>
                            </td>
                            <td>
                                <%#Eval("TanggalPOJatuhTmpPenjualan")%>
                            </td>
                            <td id="pnjTagihan" runat="server">
                                <%#Eval("JenisTagihan")%>
                            </td>
                            <td>
                                <%#Eval("TotalTagihanPenjualan", "{0:c}")%>
                            </td>
                            <td id="pnjStatus" runat="server">
                                <%#Eval("StatusInvoicePenjualan")%>
                            </td>
                            <td id="tdEdit" runat="server">
                                <a href="AddPenjualanPage.aspx?id=<%#Eval("IdPOPenjualanMst")%>&status=EDIT" /><span class="btn btn-danger">Edit</span>
                            </td>
                            <td id="tdVal1" runat="server">
                                <a href="ViewPenjualanPage.aspx?id=<%#Eval("IdPOPenjualanMst")%>" /><span class="btn btn-alternate">View</span>
                            </td>
                            <td id="tdVal2" runat="server">
                                <asp:Button id="btnCetakGnrt" CssClass="btn btn-info" runat="server" Text="Cetak" CommandArgument='<%#Eval("IdPOPenjualanMst") %>' OnClick="btnCetakGnrt_Click"/>
                            </td>
                            <td id="tdVal3" runat="server">
                                <a href="AddPenjualanPage.aspx?id=<%#Eval("IdPOPenjualanMst")%>" /><span class="btn btn-warning">Retur</span>
                            </td>
                            <td id="tdValCicilan" runat="server">
                                <a href="BayarCicilan.aspx?id=<%#Eval("IdPOPenjualanMst")%>" /><span class="btn btn-secondary">Bayar</span>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
            <tfoot>
                <tr>
                    <td colspan="3">Total Penjualan Per Hari : </td>
                    <td>
                        <asp:Label ID="GTBox" runat="server" CssClass="font-weight-bold" Text="0,00 Box"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:Label ID="GTQty" runat="server" CssClass="font-weight-bold" Text="0,00 Qty"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:Label ID="GTPenjualan" runat="server" CssClass="font-weight-bold" Text="Rp0"></asp:Label>
                    </td>
                    <td colspan="4">
                        <asp:Button ID="btnRkpBulan" runat="server" Text="Rekap Bulanan" CssClass="btn btn-success" Enabled="false" OnClick="btnRkpBulan_Click" />
                    </td>
                </tr>
            </tfoot>
        </table>
        </div>
    </div>
    <script type="text/javascript">
        function Filter(Obj) {

            var grid = document.getElementsByTagName('tbody');
            var terms = Obj.value.toUpperCase();
            var cellNr = 0; //your grid cellindex like name
            var ele;
            for (var r = 0; r <= grid[2].rows.length; r++) {
                ele = grid[2].rows[r].cells[cellNr].innerHTML.replace(/<[^>]+>/g, "");
                if (ele.toUpperCase().indexOf(terms) >= 0)
                    grid[2].rows[r].style.display = '';
                else grid[2].rows[r].style.display = 'none';
            }
        }
    </script>
</asp:Content>
