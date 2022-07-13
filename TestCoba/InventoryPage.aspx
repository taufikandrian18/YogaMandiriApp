<%@ Page Title="" Language="C#" MasterPageFile="~/Visitor.Master" AutoEventWireup="true" CodeBehind="InventoryPage.aspx.cs" Inherits="TestCoba.InventoryPage" %>
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
                    <div>List Gudang
                        <div class="page-title-subheading">Halaman Ini Untuk Memuat Seluruh List Gudang Beserta Dengan Kapasitanya.
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
        <a id="btnAddInvVsbl" runat="server" href="AddInventoryPage.aspx" class="btn btn-primary">Add</a>
        <hr />
    <div class="card-body">
        <div class="table table-responsive table-light table-bordered table-hover">
        <table class="mb-0 table">
            <thead>
                <tr style="background-color:white">
                    <th>Nama Gudang</th>
                    <th>Kapasitas Gudang</th>
                    <th>Kapasistas Sisa Gudang</th>
                    <th id="thUpdateVisbl" runat="server">Action</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="RepeaterListInventory" runat="server" OnItemDataBound="RepeaterListInventory_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#Eval("IvnStorage")%>
                            </td>
                            <td>
                                <%#Eval("IvnCapacity")%> Box
                            </td>
                            <td>
                                <%#Eval("IvnSisaCapacity")%> Box
                            </td>
                            <td id="btnUpdateVisbl" runat="server">
                                <a href="AddInventoryPage.aspx?id=<%#Eval("IdInventory")%>"><span class="btn btn-alternate">Update</span></a>
                                <asp:Button id="BtnDelete" CssClass="btn btn-danger" runat="server" Text="Delete" CommandArgument='<%#Eval("IdInventory") %>' OnClick="BtnDelete_Click"/>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        </div>
    </div>
</asp:Content>
