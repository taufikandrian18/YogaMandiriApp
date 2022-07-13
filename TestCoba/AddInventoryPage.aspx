<%@ Page Title="" Language="C#" MasterPageFile="~/Visitor.Master" AutoEventWireup="true" CodeBehind="AddInventoryPage.aspx.cs" Inherits="TestCoba.AddInventoryPage" %>
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
                    <div>Tambah / Edit Gudang
                        <div class="page-title-subheading">Halaman Ini Dipergunakan Untuk Menambahkan atau Merubah Data Gudang dan Kapasitasnya.
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
        <asp:Label ID="lblStatusPage" runat="server" Text="Add Inventory"></asp:Label></h5>
            <div class="form-row">
                <div class="col-md-6">
                    <div class="position-relative form-group">
                        <asp:Label ID="lblStorage" runat="server" Text="Nama Gudang"></asp:Label>
                        <asp:TextBox ID="txtStorage" runat="server" placeholder="Nama Gudang" CssClass="form-control text-uppercase" required="required"></asp:TextBox>
                        <asp:Label ID="vldStorage" runat="server" CssClass="alert-danger" Text=""></asp:Label>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="position-relative form-group">
                        <asp:Label ID="lblCaapacity" runat="server" Text="Kapasitas Gudang"></asp:Label>
                        <asp:TextBox ID="txtCapacity" runat="server" placeholder="Kapasitas Gudang" CssClass="form-control" required="required" onkeypress="return allowOnlyNumber(event);"></asp:TextBox>
                        <asp:Label ID="vldCapacity" runat="server" CssClass="alert-danger" Text=""></asp:Label>
                    </div>
                </div>
            </div>
            <%--<asp:Button ID="backButton" runat="server" CssClass="mt-2 btn btn-danger" Text="Back" OnClientClick="JavaScript:window.history.back(1);return false;" />--%>
            <asp:Button ID="bckBtn" runat="server" CssClass="mt-2 btn btn-danger" Text="Back" UseSubmitBehavior="false" OnClick="bckBtn_Click" />
            <asp:LinkButton ID="btnIvnOK" runat="server" CssClass="mt-2 btn btn-primary" OnClick="btnIvnOK_Click">Submit</asp:LinkButton>
    </div>
    <script type="text/javascript">
        function allowOnlyNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>
</asp:Content>
