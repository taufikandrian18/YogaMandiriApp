<%@ Page Title="" Language="C#" MasterPageFile="~/Visitor.Master" AutoEventWireup="true" CodeBehind="SupplierPage.aspx.cs" Inherits="TestCoba.SupplierPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
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
                    <div>List Supplier
                        <div class="page-title-subheading">Halaman Untuk Menampilkan Seluruh List Supplier.
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
        <a id="btnAddVsbl" runat="server" href="AddSupplierPage.aspx" class="btn btn-primary">Add</a>
        <hr />
    <div class="card-body">
        <div class="table table-responsive table-light table-bordered table-hover">
        <table class="mb-0 table">
            <thead>
                <tr style="background-color:white">
                    <th>Supplier Name</th>
                    <th>Address</th>
                    <th>Phone</th>
                    <th>Email</th>
                    <th id="thBtnVsbl" runat="server" colspan="2" style="text-align:center">Action</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="RepeaterListSupplier" runat="server" OnItemDataBound="RepeaterListSupplier_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#Eval("SupplierName")%>
                            </td>
                            <td>
                                <%#Eval("SupplierAddress")%>
                            </td>
                            <td>
                                <%#Eval("SupplierPhone")%>
                            </td>
                            <td>
                                <%#Eval("SupplierEmail")%>
                            </td>
                            <td id="tdVal1" runat="server">
                                <a href="AddSupplierPage.aspx?id=<%#Eval("IdSupplier")%>" /><span class="btn btn-alternate">Update</span>
                            </td>
                            <td id="tdVal2" runat="server">
                                <asp:Button id="BtnDelete" CssClass="btn btn-danger" runat="server" Text="Delete" CommandArgument='<%#Eval("IdSupplier") %>' OnClick="BtnDelete_Click"/>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        </div>
    </div>
</asp:Content>
