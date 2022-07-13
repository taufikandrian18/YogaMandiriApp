<%@ Page Title="" Language="C#" MasterPageFile="~/Visitor.Master" AutoEventWireup="true" CodeBehind="AddCustomerPage.aspx.cs" Inherits="TestCoba.AddCustomerPage" %>
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
                    <div>Tambah / Edit Customer
                        <div class="page-title-subheading">Halaman Ini Dipergunakan Untuk Menambahkan Atau Merubah Data Customer.
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
        <asp:Label ID="lblStatusPage" runat="server" Text="Add Customer"></asp:Label></h5>
            <div class="form-row">
                <div class="col-md-6">
                    <div class="position-relative form-group">
                        <asp:Label ID="lblCusName" runat="server" Text="Customer Name"></asp:Label>
                        <asp:TextBox ID="txtCusName" runat="server" placeholder="Customer Name" CssClass="form-control" required="required"></asp:TextBox>
                        <asp:Label ID="vldCusName" runat="server" CssClass="alert-danger" Text=""></asp:Label>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="position-relative form-group">
                        <asp:Label ID="lblCusAddr" runat="server" Text="Customer Address"></asp:Label>
                        <asp:TextBox ID="txtCusAddr" runat="server" placeholder="Apartment, studio, or floor" TextMode="MultiLine" CssClass="form-control" required="required"></asp:TextBox>
                        <asp:Label ID="vldCusAddr" runat="server" CssClass="alert-danger" Text=""></asp:Label>
                    </div>
                </div>
            </div>
            <div class="position-relative form-group">
                <asp:Label ID="lblCusPhone" runat="server" Text="Customer Phone"></asp:Label>
                <asp:TextBox ID="txtCusPhone" runat="server" placeholder="Phone" TextMode="Phone" CssClass="form-control" required="required"></asp:TextBox>
                <asp:Label ID="vldCusPhone" runat="server" CssClass="alert-danger" Text=""></asp:Label>
            </div>
            <div class="position-relative form-group">
                <asp:Label ID="lblCusEmail" runat="server" Text="Customer Email"></asp:Label>
                <asp:TextBox ID="txtCusEmail" runat="server" placeholder="Email" TextMode="Email" CssClass="form-control" required="required"></asp:TextBox>
                <asp:Label ID="vldCusEmail" runat="server" CssClass="alert-danger" Text=""></asp:Label>
            </div>
            <%--<asp:Button ID="backButton" runat="server" CssClass="mt-2 btn btn-danger" Text="Back" OnClientClick="JavaScript:window.history.back(1);return false;" />--%>
            <asp:Button ID="bckBtn" runat="server" CssClass="mt-2 btn btn-danger" Text="Back" UseSubmitBehavior="false" OnClick="bckBtn_Click" />
            <asp:LinkButton ID="btnCusOK" runat="server" CssClass="mt-2 btn btn-primary" OnClick="btnCusOK_Click">Submit</asp:LinkButton>
    </div>
</asp:Content>
