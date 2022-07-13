<%@ Page Title="" Language="C#" MasterPageFile="~/Visitor.Master" AutoEventWireup="true" CodeBehind="BayarCicilan.aspx.cs" Inherits="TestCoba.BayarCicilan" %>
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
                    <div>Bayar Cicilan
                        <div class="page-title-subheading">Halaman Ini Dipergunakan Untuk Menambahkan atau Membayar Data Cicilan.
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
        <asp:Label ID="lblStatusPage" runat="server" Text="Bayar Cicilan"></asp:Label></h5>
            <div class="form-row">
                <div class="col-md-6">
                    <div class="position-relative form-group">
                        <asp:Label ID="lblInvoice" runat="server" Text="No Invoice"></asp:Label>
                        <asp:TextBox ID="txtInvoice" runat="server" CssClass="form-control text-uppercase" Enabled="false"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="position-relative form-group">
                        <asp:Label ID="lblJmlTerbayar" runat="server" Text="Jumlah Terbayar"></asp:Label>
                        <asp:TextBox ID="txtTerbayar" runat="server" CssClass="form-control text-uppercase" Enabled="false"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="position-relative form-group">
                        <asp:Label ID="lblBayar" runat="server" Text="Jumlah Bayar"></asp:Label>
                        <asp:TextBox ID="txtJumlahBayar" runat="server" placeholder="Rp0" CssClass="form-control" required="required" onkeypress="return allowOnlyNumber(event);"></asp:TextBox>
                        <asp:Label ID="vldCapacity" runat="server" CssClass="alert-danger" Text=""></asp:Label>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="position-relative form-group">
                        <asp:Label ID="lblTotalTransaksi" runat="server" Text="Total Transaksi"></asp:Label>
                        <asp:TextBox ID="txtTransaksi" runat="server" CssClass="form-control text-uppercase" Enabled="false"></asp:TextBox>
                    </div>
                </div>
            </div>
            <%--<asp:Button ID="backButton" runat="server" CssClass="mt-2 btn btn-danger" Text="Back" OnClientClick="JavaScript:window.history.back(1);return false;" />--%>
            <asp:Button ID="bckBtn" runat="server" CssClass="mt-2 btn btn-danger" Text="Back" OnClick="bckBtn_Click" />
            <asp:LinkButton ID="btnIvnOK" runat="server" CssClass="mt-2 btn btn-primary" OnClick="btnIvnOK_Click">Submit</asp:LinkButton>
    </div>
</asp:Content>
