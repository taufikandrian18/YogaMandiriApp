<%@ Page Title="" Language="C#" MasterPageFile="~/Visitor.Master" AutoEventWireup="true" CodeBehind="AddEmployeePage.aspx.cs" Inherits="TestCoba.AddEmployeePage" %>
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
                    <div>Tambah / Edit Pegawai
                        <div class="page-title-subheading">Halaman Ini Dipergunakan Untuk Menambahkan Atau Merubah Data Pegawai.
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
        <asp:Label ID="lblStatusPage" runat="server" Text="Add Employee"></asp:Label></h5>
            <div class="form-row">
                <div class="col-md-6">
                    <div class="position-relative form-group">
                        <asp:Label ID="lblEmployeeNm" runat="server" Text="Employee Name"></asp:Label>
                        <asp:TextBox ID="txtEmployeeNm" runat="server" placeholder="Employee Name" CssClass="form-control" required="required"></asp:TextBox>
                        <asp:Label ID="vldEmployeeNm" runat="server" CssClass="alert-danger" Text=""></asp:Label>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="position-relative form-group">
                        <asp:Label ID="lblEmployeeUsr" runat="server" Text="Employee Username"></asp:Label>
                        <asp:TextBox ID="txtEmployeeUsr" runat="server" placeholder="Employee Username" CssClass="form-control" required="required"></asp:TextBox>
                        <asp:Label ID="vldEmployeeUsr" runat="server" CssClass="alert-danger" Text=""></asp:Label>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="position-relative form-group">
                        <asp:Label ID="lblEmployeePass" runat="server" Text="Employee Password"></asp:Label>
                        <asp:TextBox ID="txtEmployeePass" runat="server" placeholder="Employee Password" TextMode="Password" CssClass="form-control" required="required"></asp:TextBox>
                        <asp:Label ID="vldEmployeePass" runat="server" CssClass="alert-danger" Text=""></asp:Label>
                    </div>
                    <div class="position-relative form-group">
                        <asp:Label ID="lblEmployeePassCnfrm" runat="server" Text="Confirm Password"></asp:Label>
                        <asp:TextBox ID="txtEmployeePassCnfrm" runat="server" placeholder="Confirm Password" TextMode="Password" CssClass="form-control" required="required"></asp:TextBox>
                        <asp:Label ID="vldEmployeePassCnfrm" runat="server" CssClass="alert-danger" Text=""></asp:Label>
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
            <div class="position-relative form-group">
                <asp:Label ID="lblRole" runat="server" Text="Role"></asp:Label>
                <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
            <%--<asp:Button ID="backButton" runat="server" CssClass="mt-2 btn btn-danger" Text="Back" OnClientClick="JavaScript:window.history.back(1);return false;" />--%>
            <asp:Button ID="bckBtn" runat="server" CssClass="mt-2 btn btn-danger" Text="Back" UseSubmitBehavior="false" OnClick="bckBtn_Click" />
            <asp:LinkButton ID="btnEmpOK" runat="server" CssClass="mt-2 btn btn-primary" OnClick="btnEmpOK_Click">Submit</asp:LinkButton>
    </div>
</asp:Content>
