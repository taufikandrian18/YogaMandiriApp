<%@ Page Title="" Language="C#" MasterPageFile="~/Visitor.Master" AutoEventWireup="true" CodeBehind="BiayaPage.aspx.cs" Inherits="TestCoba.BiayaPage" %>
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
                <div>List Biaya
                    <div class="page-title-subheading">Halaman Untuk Memuat Seluruh List Biaya Dengan Periode Tertentu.
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
    <table>
        <tr>
            <td id="btnValVsbl" runat="server"><a href="AddBiayaPage.aspx" class="btn btn-primary">Tambah List Biaya</a></td>
            <td>&nbsp;</td>
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
                <asp:Label ID="Label4" runat="server" CssClass="font-weight-bold" Text="Jenis Biaya"></asp:Label>
                <asp:DropDownList ID="ddlJenisBiaya" runat="server" CssClass="form-control">
                    <asp:ListItem Text="" Value=""></asp:ListItem>
                    <asp:ListItem Text="-------------------" Value=""></asp:ListItem>
                    <asp:ListItem Text="Biaya Peralatan" Value="Biaya Peralatan"></asp:ListItem>
                    <asp:ListItem Text="Biaya Produksi" Value="Biaya Produksi"></asp:ListItem>
                    <asp:ListItem Text="Biaya Operasional" Value="Biaya Operasional"></asp:ListItem>
                    <asp:ListItem Text="Biaya Pribadi Ibu" Value="Biaya Pribadi Ibu"></asp:ListItem>
                    <asp:ListItem Text="Biaya Pribadi Bapak" Value="Biaya Pribadi Bapak"></asp:ListItem>
                    <asp:ListItem Text="Biaya Bunga Bank" Value="Biaya Bunga Bank"></asp:ListItem>
                    <asp:ListItem Text="Biaya Pribadi Yoga" Value="Biaya Pribadi Yoga"></asp:ListItem>
                    <asp:ListItem Text="Biaya Pribadi Jihan" Value="Biaya Pribadi Jihan"></asp:ListItem>
                    <asp:ListItem Text="Biaya Pribadi Marcel" Value="Biaya Pribadi Marcel"></asp:ListItem>
                    <asp:ListItem Text="Pajak" Value="Pajak"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:Label ID="Label5" runat="server" CssClass="font-weight-bold" Text="Sort By"></asp:Label>
                <asp:DropDownList ID="ddlSortBy" runat="server" CssClass="form-control">
                    <asp:ListItem Text="" Value=""></asp:ListItem>
                    <asp:ListItem Text="-------------------" Value=""></asp:ListItem>
                    <asp:ListItem Text="Jenis Biaya" Value="JenisBiayaMst"></asp:ListItem>
                    <asp:ListItem Text="Tanggal Biaya" Value="TanggalBiayaMst"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <br />
                <asp:Button ID="BtnSearch" runat="server" Text="Search" CssClass="btn btn-dark" OnClick="BtnSearch_Click" />
            </td>
        </tr>
    </table>
    <hr />
    <div class="card-body">
        <div class="table table-responsive table-light table-bordered table-hover">
            <table class="mb-0 table">
                <thead>
                    <tr style="background-color:white">
                        <th>Jenis Biaya</th>
                        <th>Tanggal Biaya</th>
                        <th>Total Biaya</th>
                        <th id="thVal" runat="server" colspan="2" style="text-align:center">Action</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptListPembiayaan" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%#Eval("JenisBiayaMst")%>
                                </td>
                                <td>
                                    <%#Eval("TanggalBiayaMst")%>
                                </td>
                                <td>
                                    <%#Eval("TotalBiayaMst", "{0:c}")%>
                                </td>
                                <td id="tdVal1" runat="server" style="text-align:right">
                                    <a href="AddBiayaPage.aspx?id=<%#Eval("IdBiayaMst")%>" /><span class="btn btn-alternate">Update</span>
                                </td>
                                <td id="tdVal2" runat="server">
                                    <a href="ViewDetailBiaya.aspx?id=<%#Eval("IdBiayaMst")%>" /><span class="btn btn-info">View</span>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
    </div>
</asp:Content>
