<%@ Page Title="" Language="C#" MasterPageFile="~/Visitor.Master" AutoEventWireup="true" CodeBehind="CustomerPage.aspx.cs" Inherits="TestCoba.CustomerPage" %>
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
                    <div>List Customer
                        <div class="page-title-subheading">Halaman Ini Memuat Seluruh List Customer yang Telah Membuat Transaksi.
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
        <a id="btnAddVsbl" runat="server" href="AddCustomerPage.aspx" class="btn btn-primary">Add</a>
        <hr />
    <div class="card-body">
        <div class="table table-responsive table-light table-bordered table-hover">
        <table class="mb-0 table">
            <thead>
                <tr style="background-color:white">
                    <th>Customer Name</th>
                    <th>Address</th>
                    <th>Phone</th>
                    <th>Email</th>
                    <th id="thBtnVsbl" runat="server" colspan="2">Action</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="RepeaterListCustomer" runat="server" OnItemDataBound="RepeaterListCustomer_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#Eval("CustomerName")%>
                            </td>
                            <td>
                                <%#Eval("CustomerAddress")%>
                            </td>
                            <td>
                                <%#Eval("CustomerPhone")%>
                            </td>
                            <td>
                                <%#Eval("CustomerEmail")%>
                            </td>
                            <td id="tdVal1" runat="server">
                                <a href="AddCustomerPage.aspx?id=<%#Eval("IdCustomer")%>" /><span class="btn btn-alternate">Update</span>
                            </td>
                            <td id="tdVal2" runat="server">
                                <asp:Button id="BtnDelete" CssClass="btn btn-danger" runat="server" Text="Delete" CommandArgument='<%#Eval("IdCustomer") %>' OnClick="BtnDelete_Click"/>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        </div>
    </div>
</asp:Content>
