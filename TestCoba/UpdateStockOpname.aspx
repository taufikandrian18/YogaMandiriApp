﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Visitor.Master" AutoEventWireup="true" CodeBehind="UpdateStockOpname.aspx.cs" Inherits="TestCoba.UpdateStockOpname" %>
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
                <div>Adjust Stock Quantity
                    <div class="page-title-subheading">Halaman Ini Dipergunakan Untuk Memperbaharui Stock Quantity Barang.
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
        <h5 class="card-title"><asp:Label ID="lblStatusPage" runat="server" Text="Adjust Qty"></asp:Label></h5>
        <div class="form-row">
            <div class="col-md-4">
                <div class="position-relative form-group">
                    <asp:Label ID="lblItemCode" runat="server" Text="Item Code"></asp:Label>
                    <asp:TextBox ID="txtItemCode" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-4">
                <div class="position-relative form-group">
                    <asp:Label ID="lblBrgName" runat="server" Text="Nama Barang"></asp:Label>
                    <asp:TextBox ID="txtBrgName" runat="server" placeholder="Nama Barang" CssClass="form-control text-uppercase" required="required"></asp:TextBox>
                    <asp:Label ID="vldBrgName" runat="server" CssClass="alert-danger" Text=""></asp:Label>
                </div>
            </div>
            <div class="col-md-4">
                <div class="position-relative form-group">
                    <asp:Label ID="lblSupName" runat="server" Text="Supplier Name"></asp:Label>
                    <asp:DropDownList ID="ddlSupName" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-4">
                <div class="position-relative form-group">
                    <asp:Label ID="lblQtyBrg" runat="server" Text="Qty Barang"></asp:Label>
                    <asp:TextBox ID="txtQtyBrg" runat="server" placeholder="0" CssClass="form-control" onfocus="this.value=''" onkeypress="return isNumberKey(event);" required="required"></asp:TextBox>
                    <asp:Label ID="vldQtyBrg" runat="server" CssClass="alert-danger" Text=""></asp:Label>
                    <asp:Label ID="lblJmlBox" runat="server" Text="Jumlah Box"></asp:Label>
                    <asp:TextBox ID="txtmlBox" runat="server" placeholder="0" CssClass="form-control" onfocus="this.value=''" onkeypress="return isNumberKey(event);" required="required"></asp:TextBox>
                    <asp:Label ID="vldJmlBox" runat="server" CssClass="alert-danger" Text=""></asp:Label>
                </div>
            </div>
            <div class="col-md-4">
                <div class="position-relative form-group">
                    <asp:Label ID="lblStorage" runat="server" Text="Letak Gudang"></asp:Label>
                    <asp:DropDownList ID="ddlStorage" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
        </div>
        <asp:Button ID="bckBtn" runat="server" CssClass="mt-2 btn btn-danger" Text="Back" UseSubmitBehavior="false" OnClick="bckBtn_Click" />
        <asp:LinkButton ID="btnBrgOK" runat="server" CssClass="mt-2 btn btn-primary" OnClick="btnBrgOK_Click">Submit</asp:LinkButton>
    </div>
    <script type="text/javascript">
        function allowOnlyNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode != 46 && charCode > 31
              && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>
</asp:Content>
