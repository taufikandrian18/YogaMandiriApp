<%@ Page Title="" Language="C#" MasterPageFile="~/Visitor.Master" AutoEventWireup="true" CodeBehind="ViewDetailBiaya.aspx.cs" Inherits="TestCoba.ViewDetailBiaya" %>
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
                <div>
                    <asp:Label ID="lblHeaderPage" runat="server" Text="View Biaya"></asp:Label>
                    <div class="page-title-subheading">Halaman Ini Untuk Melihat Detail Data Biaya.
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
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        </h5>
        <div class="form-row">
            <div class="col-md-4">
                <div class="position-relative form-group">
                    <asp:Label ID="Label2" runat="server" Font-Bold="true" Text="Jenis Biaya : "></asp:Label>
                    <asp:Label ID="txtJenisBiaya" runat="server" Text="Jenis Biaya"></asp:Label>
                </div>
            </div>
            <div class="col-md-4">
                <div class="position-relative form-group">
                    <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Tanggal Biaya : "></asp:Label>
                    <asp:Label ID="txtTanggalInvoice" runat="server" Text="Tanggal Biaya"></asp:Label>
                </div>
            </div>
        </div>
        <hr />
        <div id="table_box_bootstrap" class="table table-responsive table-light table-hover">
            <table class="mb-0 table">
                <asp:Repeater ID="rptPembiayaan" runat="server">
                    <HeaderTemplate>
                        <tr style="background-color:white">
                            <th colspan="2" style="width:400px">
                                <asp:Label ID="lblItemName" runat="server" Font-Bold="true" Text="Item Name"></asp:Label>
                            </th>
                            <th style="width:400px">
                                <asp:Label ID="lblTotalPrice" runat="server" Font-Bold="true" Text="Total Price"></asp:Label>
                            </th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td colspan="2" style="text-transform:uppercase">
                                <%#Eval("NamaBiayaDtl")%>
                            </td>
                            <td>
                                <%#Eval("HargaBiayaDtl", "{0:c}")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <tr>
                    <td>
                        <div class="position-relative form-group">
                            <asp:Button ID="backButton" runat="server" CssClass="mt-2 btn btn-danger" Text="Back" OnClientClick="JavaScript:window.history.go(-1);return false;" />
                        </div>
                    </td>
                    <td style="font-weight:bold;text-align:right">
                        <div class="position-relative form-group">
                            <asp:Label ID="lblGrandTotal" runat="server" Text="Grand Total : "></asp:Label>
                        </div>
                    </td>
                    <td style="font-weight:bold">
                        <div class="position-relative form-group">
                            <asp:Label ID="lblGTVal" runat="server" Font-Bold="true" Text="Label"></asp:Label>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
