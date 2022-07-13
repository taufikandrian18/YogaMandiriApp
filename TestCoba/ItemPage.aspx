<%@ Page Title="" Language="C#" MasterPageFile="~/Visitor.Master" AutoEventWireup="true" CodeBehind="ItemPage.aspx.cs" Inherits="TestCoba.ItemPage" %>
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
                    <div>List Barang
                        <div class="page-title-subheading">Halaman Untuk Memuat Seluruh List Barang yang Telah Terekam di Sistem.
                        </div>
                    </div>
                </div>
                <div class="page-title-actions">
                    <a id="tmbhBrgBtn" runat="server" href="AddItemPage.aspx" class="btn btn-primary">Tambah Barang</a>
                    <a id="tmbhBrgOlahanBtn" runat="server" href="AddBarangOlahan.aspx" class="btn btn-success">Tambah Barang Olahan</a>
                </div>    
            </div>
        </div>
        <hr />
    <asp:Button id="btnCetakGnrt" CssClass="btn btn-warning" runat="server" Text="Cetak Stock" OnClick="btnCetakGnrt_Click" />
    <asp:Button id="btnCetakModal" CssClass="btn btn-info" runat="server" Text="Cetak Modal" OnClick="btnCetakModal_Click" Visible="false" />
    <div class="card-body">
        <div class="form-row">
            <div class="col-md-2">
                <div class="position-relative form-group">
                    <asp:Label ID="Label1" runat="server" CssClass="font-weight-bold" Text="Search Box"></asp:Label>
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" onkeyup='Filter(this);'></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <div class="position-relative form-group">
                    <asp:Label ID="Label2" runat="server" CssClass="font-weight-bold" Text="Supplier"></asp:Label>
                    <asp:DropDownList ID="ddlSupplier" runat="server" CssClass="form-control" AppendDataBoundItems="true" DataTextField="SupplierName" DataValueField="IdSupplier">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="-------------------" Value=""></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2">
                <div class="position-relative form-group">
                    <asp:Label ID="Label3" runat="server" CssClass="font-weight-bold" Text="Inventory"></asp:Label>
                    <asp:DropDownList ID="ddlInventory" runat="server" CssClass="form-control" AppendDataBoundItems="true" DataTextField="IvnStorage" DataValueField="IdInventory">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="-------------------" Value=""></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2">
                <div class="position-relative form-group">
                    <asp:Label ID="Label4" runat="server" CssClass="font-weight-bold" Text="Category"></asp:Label>
                    <asp:DropDownList ID="ddlBrgCategory" runat="server" CssClass="form-control">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="-------------------" Value=""></asp:ListItem>
                        <asp:ListItem Text="PABRIKAN" Value="PABRIKAN"></asp:ListItem>
                        <asp:ListItem Text="OLAHAN" Value="OLAHAN"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2">
                <div class="position-relative form-group">
                    <asp:Label ID="Label5" runat="server" CssClass="font-weight-bold" Text="Sort By"></asp:Label>
                    <asp:DropDownList ID="ddlSortBy" runat="server" CssClass="form-control">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="-------------------" Value=""></asp:ListItem>
                        <asp:ListItem Text="Code" Value="B.BarangItemCode"></asp:ListItem>
                        <asp:ListItem Text="Nama" Value="B.NamaBarang"></asp:ListItem>
                        <asp:ListItem Text="Tanggal" Value="B.BarangTanggalBeli"></asp:ListItem>
                        <asp:ListItem Text="Supplier" Value="S.SupplierName"></asp:ListItem>
                        <asp:ListItem Text="Inventory" Value="INV.IvnStorage"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2">
                <div class="position-relative form-group">
                    <br />
                    <asp:Button ID="btnFilter" runat="server" CssClass="btn btn-dark" Text="Search" OnClick="btnFilter_Click" />
                </div>
            </div>
        </div>
    </div>
    <div class="card-body">
        <div id="table_box_bootstrap" class="table table-responsive table-light table-bordered table-hover">
        <table class="mb-0 table">
            <thead>
                <tr style="background-color:white">
                    <th>Item Code</th>
                    <th>Nama Barang</th>
                    <th>Supplier</th>
                    <th>Tanggal Beli Terakhir</th>
                    <th>Qty</th>
                    <th>Satuan</th>
                    <th>Jumlah Box</th>
                    <th>Nama Penyimpanan</th>
                    <th>Category</th>
                    <th id="thVal" runat="server" colspan="2" style="text-align:center">Action</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="RepeaterListItm" runat="server" OnItemDataBound="RepeaterListItm_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#Eval("BarangItemCode")%>
                            </td>
                            <td>
                                <%#Eval("NamaBarang")%>
                            </td>
                            <td>
                                <%#Eval("SupplierName")%>
                            </td>
                            <td>
                                <%#Eval("BarangTanggalBeli")%>
                            </td>
                            <td>
                                <%#Eval("BarangQuantity")%>
                            </td>
                            <td>
                                <%#Eval("BarangSatuan")%>
                            </td>
                            <td>
                                <%#Eval("BarangJmlBox")%> Box
                            </td>
                            <td>
                                <%#Eval("IvnStorage")%>
                            </td>
                            <td id="brgCategory" runat="server">
                                <%#Eval("BarangCategory")%>
                            </td>
                            <td id="tdVal" runat="server">
                                <a href="AddItemPage.aspx?id=<%#Eval("IdBarang")%>" /><span class="btn btn-alternate">Ubah</span>
                                <asp:LinkButton runat="server" ID="lnkUpdate" CommandArgument='<%#Eval("IdBarang")%>' CssClass="btn btn-alternate" Text="Update Barang Olahan" OnClick="lnkUpdate_Click"></asp:LinkButton>
                            </td>
                            <td>
                                <a href="ViewBarangPage.aspx?id=<%#Eval("IdBarang")%>" /><span class="btn btn-dark">View</span>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        </div>
    </div>
    <script type="text/javascript">
        function Filter(Obj) {

            var grid = document.getElementsByTagName('tbody');
            var terms = Obj.value.toUpperCase();
            var cellNr = 1; //your grid cellindex like name
            var ele;
            for (var r = 0; r <= grid[1].rows.length; r++) {
                ele = grid[1].rows[r].cells[cellNr].innerHTML.replace(/<[^>]+>/g, "");
                if (ele.toUpperCase().indexOf(terms) >= 0)
                    grid[1].rows[r].style.display = '';
                else grid[1].rows[r].style.display = 'none';
            }
        }
    </script>
</asp:Content>
