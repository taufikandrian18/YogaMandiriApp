<%@ Page Title="" Language="C#" MasterPageFile="~/Visitor.Master" AutoEventWireup="true" CodeBehind="AddSupplierPage.aspx.cs" Inherits="TestCoba.AddSupplierPage" %>
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
                    <div>Tambah / Edit Supplier
                        <div class="page-title-subheading">Halaman Ini Untuk Menambahkan Atau Merubah Data Supplier.
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
        <asp:Label ID="lblStatusPage" runat="server" Text="Add Supplier"></asp:Label></h5>
            <div class="form-row">
                <div class="col-md-6">
                    <div class="position-relative form-group">
                        <asp:Label ID="lblSupName" runat="server" Text="Supplier Name"></asp:Label>
                        <asp:TextBox ID="txtSupName" runat="server" placeholder="Supplier Name" CssClass="form-control text-uppercase" required="required"></asp:TextBox>
                        <asp:Label ID="vldSupName" runat="server" CssClass="alert-danger" Text=""></asp:Label>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="position-relative form-group">
                        <asp:Label ID="lblAddr" runat="server" Text="Address"></asp:Label>
                        <asp:TextBox ID="txtAddr" runat="server" placeholder="Apartment, studio, or floor" TextMode="MultiLine" CssClass="form-control" required="required"></asp:TextBox>
                        <asp:Label ID="vldAddr" runat="server" CssClass="alert-danger" Text=""></asp:Label>
                    </div>
                </div>
            </div>
            <div class="position-relative form-group">
                <asp:Label ID="lblPhone" runat="server" Text="Phone"></asp:Label>
                <asp:TextBox ID="txtPhone" runat="server" placeholder="Phone" TextMode="Phone" CssClass="form-control" required="required"></asp:TextBox>
                <asp:Label ID="vldPhone" runat="server" CssClass="alert-danger" Text=""></asp:Label>
            </div>
            <div class="position-relative form-group">
                <asp:Label ID="lblEmail" runat="server" Text="Email"></asp:Label>
                <asp:TextBox ID="txtEmail" runat="server" placeholder="Email" TextMode="Email" CssClass="form-control" required="required"></asp:TextBox>
                <asp:Label ID="vldEmail" runat="server" CssClass="alert-danger" Text=""></asp:Label>
            </div>
            <%--<asp:Button ID="backButton" runat="server" CssClass="mt-2 btn btn-danger" Text="Back" OnClientClick="JavaScript:window.history.back(1);return false;" />--%>
            <asp:Button ID="bckBtn" runat="server" CssClass="mt-2 btn btn-danger" UseSubmitBehavior="false" Text="Back" OnClick="bckBtn_Click" />
            <asp:LinkButton ID="btnSupOK" runat="server" CssClass="mt-2 btn btn-primary" OnClick="btnSupOK_Click">Submit</asp:LinkButton>
    </div>
</asp:Content>
