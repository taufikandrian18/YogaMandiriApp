<%@ Page Title="" Language="C#" MasterPageFile="~/Visitor.Master" AutoEventWireup="true" CodeBehind="NeracaPage.aspx.cs" Inherits="TestCoba.NeracaPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        th{
            background-color: #ffffff;
        }
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
                <div>Neraca Laba Rugi
                    <div class="page-title-subheading">Halaman Untuk Melihat Neraca Laba Rugi.
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
    <div class="page-title-actions">
        <a id="nrcLbRgHr" runat="server" href="NeracaTransaksi.aspx" class="btn btn-primary" style="margin-left:30px">Per Transaksi</a>
    </div>  
    <div class="card-body">  
        <div class="table table-responsive table-light table-bordered table-hover">
            <table class="mb-0 table">
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" CssClass="font-weight-bold" Text="Pencarian Berdasarkan : "></asp:Label>
                        <asp:DropDownList ID="ddlCheckBulanTahun" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCheckBulanTahun_SelectedIndexChanged">
                            <asp:ListItem Text="" Value=""></asp:ListItem>
                            <asp:ListItem Text="-------------------" Value=""></asp:ListItem>
                            <asp:ListItem Text="Tanggal" Value="Tanggal"></asp:ListItem>
                            <asp:ListItem Text="Bulan" Value="Bulan"></asp:ListItem>
                            <asp:ListItem Text="Tahun" Value="Tahun"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label2" runat="server" CssClass="font-weight-bold" Text="Periode : "></asp:Label>
                        <asp:DropDownList ID="ddlPilihPeriode" runat="server" Width="200px" CssClass="form-control"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblBlnVis" runat="server" CssClass="font-weight-bold" Text="Bulan : " Visible="false"></asp:Label>
                        <asp:DropDownList ID="ddlBlnVis" runat="server" Width="200px" CssClass="form-control" Visible="false"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblThnVis" runat="server" CssClass="font-weight-bold" Text="Tahun : " Visible="false"></asp:Label>
                        <asp:DropDownList ID="ddlThnVis" runat="server" Width="200px" CssClass="form-control" Visible="false"></asp:DropDownList>
                    </td>
                    <td colspan="2" style="text-align:right">
                        <br />
                        <asp:Button ID="btnGenerateReport" runat="server" Text="Generate" CssClass="btn btn-secondary" OnClick="btnGenerateReport_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="card-body">
        <div class="table table-responsive table-light table-hover">
            <table class="mb-0 table">
                <thead>
                    <tr>
                        <th colspan="3">&nbsp;</th>
                        <th colspan="2">
                            <asp:Label ID="Label5" runat="server" CssClass="font-weight-bold" Text="Laporan Laba Rugi"></asp:Label>
                        </th>
                        <th style="text-align:right">
                            <asp:Button id="btnCetakGnrt" CssClass="btn btn-warning" runat="server" Enabled="false" Text="Cetak" OnClick="btnCetakGnrt_Click"/>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>
                            <asp:Label ID="Label8" runat="server" CssClass="font-weight-bold" Text="1"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label10" runat="server" CssClass="font-weight-bold" Text="Laba Kotor"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblQtySel" runat="server" CssClass="font-weight-bold" Text="QTY : "></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblQtyVal" runat="server" Text="0" CssClass="font-weight-bold" ForeColor="Red"></asp:Label>
                        </td>
                        <td colspan="2" style="text-align:right;padding-right:30px">
                            <asp:Label ID="txtLabaKotor" runat="server" CssClass="font-weight-bold" Text="0" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <asp:Repeater ID="rptListBiaya" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td style="font-weight:bold">
                                    <%# Container.ItemIndex + 2 %>
                                </td>
                                <td style="font-weight:bold">
                                    <%#Eval("JenisBiayaMst" , "{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}")%>
                                </td>
                                <td colspan="2">&nbsp;</td>
                                <td colspan="2" style="font-weight:bold;text-align:right;padding-right:30px">
                                    <%#Eval("JumlahBiaya" , "{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr>
                        <td colspan="4">&nbsp;</td>
                        <td colspan="2" style="text-align:right">
                            <asp:Label ID="Label12" runat="server" CssClass="font-weight-bold" Text="__________________________(+)"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <asp:Label ID="Label13" runat="server" CssClass="font-weight-bold" Text="Jumlah"></asp:Label>
                        </td>
                        <td colspan="2">&nbsp;</td>
                        <td colspan="2" style="text-align:right;padding-right:30px"><asp:Label ID="GTBiaya" runat="server" CssClass="font-weight-bold" Text="0" ForeColor="Red"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label15" runat="server" CssClass="font-weight-bold" Text="8"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label14" runat="server" CssClass="font-weight-bold" Text="Laba"></asp:Label>
                        </td>
                        <td colspan="2">&nbsp;</td>
                        <td colspan="2" style="text-align:right;padding-right:30px"><asp:Label ID="GTLaba" runat="server" CssClass="font-weight-bold" Text="0"></asp:Label></td>
                    </tr>
                    <asp:Repeater ID="rptListPajak" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td style="font-weight:bold">
                                    <%# Container.ItemIndex + Convert.ToInt32(Label15.Text.Trim()) + 1 %>
                                </td>
                                <td style="font-weight:bold">
                                    <%#Eval("JenisBiayaMst")%>
                                </td>
                                <td colspan="2">&nbsp;</td>
                                <td colspan="2" style="font-weight:bold;text-align:right;padding-right:30px">
                                    <%#Eval("JumlahBiaya","{0:Rp###,###,##0.00;(Rp###,###,##0.00);'Rp0'}")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr>
                        <td colspan="4">&nbsp;</td>
                        <td colspan="2" style="text-align:right">
                            <asp:Label ID="Label16" runat="server" CssClass="font-weight-bold" Text="__________________________(-)"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label17" runat="server" CssClass="font-weight-bold" Text="10"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label18" runat="server" CssClass="font-weight-bold" Text="Laba Setelah Pajak"></asp:Label>
                        </td>
                        <td colspan="2">&nbsp;</td>
                        <td colspan="2" style="text-align:right;padding-right:30px"><asp:Label ID="GTSetelahPajak" runat="server" CssClass="font-weight-bold" Text="0" ForeColor="Red"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <asp:Label ID="Label19" runat="server" CssClass="font-weight-bold" Text="Dibulatkan Penuh"></asp:Label>
                        </td>
                        <td colspan="2">&nbsp;</td>
                        <td colspan="2" style="text-align:right;padding-right:30px"><asp:Label ID="GTRounding" runat="server" CssClass="font-weight-bold" Text="0" ForeColor="Red"></asp:Label></td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</asp:Content>
