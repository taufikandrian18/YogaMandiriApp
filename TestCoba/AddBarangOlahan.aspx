<%@ Page Title="" Language="C#" MasterPageFile="~/Visitor.Master" AutoEventWireup="true" CodeBehind="AddBarangOlahan.aspx.cs" Inherits="TestCoba.AddBarangOlahan" %>
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
                    <asp:Label ID="lblHeaderPage" runat="server" Text="Tambah Barang Olahan"></asp:Label>
                    <div class="page-title-subheading">Halaman Ini Untuk Menambahkan / Mengubah Barang Olahan Yang Terbuat Dari Barang Yang Telah Ada.
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
        <asp:Label ID="lblStatusPage" runat="server" Text="Tambah Barang Olahan"></asp:Label></h5>
        <div class="form-row">
            <div class="col-md-4">
                <div class="position-relative form-group">
                    <asp:Label ID="lblItemCode" runat="server" Text="Item Code"></asp:Label>
                    <asp:TextBox ID="txtItemCode" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-4">
                <div class="position-relative form-group">
                    <asp:Label ID="lblNamaBrgOlahan" runat="server" Text="Nama Barang"></asp:Label>
                    <asp:TextBox ID="txtNamaBrgOlahan" runat="server" placeholder="Nama Barang Olahan" CssClass="form-control text-uppercase" required="required"></asp:TextBox>
                    <asp:Label ID="vldNoInvoice" runat="server" CssClass="alert-danger" Text=""></asp:Label>
                </div>
            </div>
            <div class="col-md-2">
                <div class="position-relative form-group">
                    <asp:Label ID="lblGudang" runat="server" Text="Nama Gudang"></asp:Label>
                    <asp:DropDownList ID="ddlNamaGudang" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlNamaGudang_SelectedIndexChanged"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2">
                <div class="position-relative form-group">
                    <asp:Label ID="lblSupName" runat="server" Text="Supplier Name"></asp:Label>
                    <asp:DropDownList ID="ddlSupName" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlSupName_SelectedIndexChanged"></asp:DropDownList>
                </div>
            </div>
        </div>
        <hr />
        <div id="table_box_bootstrap" class="table table-responsive table-light table-hover">
            <table class="mb-0 table">
                <asp:Repeater ID="rptBrgOlahan" runat="server">
                    <HeaderTemplate>
                        <tr>
                            <td style="width:550px">
                                <asp:Label ID="lblItemName" runat="server" Text="Item Name"></asp:Label>
                            </td>
                            <td style="width:120px">
                                <asp:Label ID="lblItemQty" runat="server" Text="Qty / Kg"></asp:Label>
                            </td>
                            <td style="width:120px">
                                <asp:Label ID="lblTemBox" runat="server" Text="Box"></asp:Label>
                            </td>
                            <td colspan="2" style="width:200px">&nbsp;</td>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtItemCode" runat="server" Enabled="false" CssClass="form-control" Visible="false"></asp:TextBox>
                                <asp:TextBox ID="txtItemName" runat="server" placeholder="Item Name" CssClass="form-control text-uppercase" Visible="false" required="required"></asp:TextBox>
                                <asp:DropDownList ID="ddlNamaBarang" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemQty" runat="server" placeholder="0" CssClass="form-control" AutoPostBack="true" onfocus="this.value=''" onkeypress="return isNumberKey(event);" required="required" OnTextChanged="txtItemQty_TextChanged"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemBox" runat="server" placeholder="0" CssClass="form-control" AutoPostBack="true" onfocus="this.value=''"  onkeypress="return allowOnlyNumber(event);" required="required" OnTextChanged="txtItemBox_TextChanged"></asp:TextBox>
                            </td>
                            <td colspan="2" style="text-align:center">
                                <asp:LinkButton ID="btnadd" runat="server" Text="+" CssClass="btn btn-success" OnClick="btnadd_Click"></asp:LinkButton>
                                <asp:LinkButton ID="btndel" runat="server" Text="-" CssClass="btn btn-danger left" OnClick="btndel_Click"></asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <tr>
                    <td colspan="4">
                        <%--<asp:Button ID="backButton" runat="server" CssClass="mt-2 btn btn-danger" Text="Back" OnClientClick="JavaScript:window.history.back(1);return false;" />--%>
                        <asp:Button ID="bckBtn" runat="server" CssClass="mt-2 btn btn-danger" Text="Back" UseSubmitBehavior="false" OnClick="bckBtn_Click" />
                    </td>
                    <td style="text-align:center">
                        <div class="position-relative form-group">
                            <asp:Button ID="btnSubmit" runat="server" CssClass="mt-2 btn btn-primary" UseSubmitBehavior="true" Text="Submit" OnClick="btnSubmit_Click" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
