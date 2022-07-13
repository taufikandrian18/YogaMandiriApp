<%@ Page Title="" Language="C#" MasterPageFile="~/Visitor.Master" AutoEventWireup="true" CodeBehind="EmployeePage.aspx.cs" Inherits="TestCoba.EmployeePage" %>
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
                    <div>List Pegawai
                        <div class="page-title-subheading">Halaman Ini Memuat Seluruh Daftar Pegawai yang Dapat Menggunakan Aplikasi Ini.
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
        <a id="btnValVsbl" runat="server" href="AddEmployeePage.aspx" class="btn btn-primary">Add</a>
        <hr />
    <div class="card-body">
        <div class="table table-responsive table-light table-bordered table-hover">
        <table class="mb-0 table">
            <thead>
                <tr style="background-color:white">
                    <th>Employee Name</th>
                    <th>Username</th>
                    <th>Address</th>
                    <th>Phone</th>
                    <th>Email</th>
                    <th>Role</th>
                    <th id="thVal" runat="server" colspan="2" style="text-align:center">Action</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="RepeaterListEmp" runat="server" OnItemDataBound="RepeaterListEmp_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#Eval("EmpName")%>
                            </td>
                            <td>
                                <%#Eval("EmpUsername")%>
                            </td>
                            <td>
                                <%#Eval("EmpAddress")%>
                            </td>
                            <td>
                                <%#Eval("EmpPhone")%>
                            </td>
                            <td>
                                <%#Eval("EmpEmail")%>
                            </td>
                            <td>
                                <%#Eval("NameRole")%>
                            </td>
                            <td id="tdVal1" runat="server">
                                <a href="AddEmployeePage.aspx?id=<%#Eval("IdEmployee")%>" /><span class="btn btn-alternate">Update</span>
                            </td>
                            <td id="tdVal2" runat="server">
                                <asp:Button id="BtnDelete" CssClass="btn btn-danger" runat="server" Text="Delete" CommandArgument='<%#Eval("IdEmployee") %>' OnClick="BtnDelete_Click"/>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        </div>
    </div>
</asp:Content>
