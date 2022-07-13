<%@ Page Title="" Language="C#" MasterPageFile="~/Visitor.Master" AutoEventWireup="true" CodeBehind="StockOpnamePage.aspx.cs" Inherits="TestCoba.StockOpnamePage" %>
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
                <div>Stock Opname Barang
                    <div class="page-title-subheading">Halaman Untuk Menyesuaikan Isi dari List Barang yang Telah Terekam di Sistem.
                    </div>
                </div>
            </div>  
        </div>
    </div>
    <h5 class="card-title"><asp:Label ID="lblHeader1" runat="server" CssClass="font-weight-bold" Text="Arus Data Quantity"></asp:Label></h5>
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
                    <asp:Label ID="Label4" runat="server" CssClass="font-weight-bold" Text="Inventory"></asp:Label>
                    <asp:DropDownList ID="ddlInventory" runat="server" CssClass="form-control" AppendDataBoundItems="true" DataTextField="IvnStorage" DataValueField="IdInventory">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="-------------------" Value=""></asp:ListItem>
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
                        <th>Nama Barang</th>
                        <th>Sisa Barang Qty</th>
                        <th>Barang Olahan Terpakai(-)</th>
                        <th>Total Terjual (-)</th>
                        <th>Retur Penjualan (+)</th>
                        <th>Retur Pembelian (-)</th>
                        <th>Total Pembelian (+)</th>
                        <th id="thVal1" runat="server">Action</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RepeaterListItm" runat="server" OnItemDataBound="RepeaterListItm_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%#Eval("NamaBarang")%>
                                </td>
                                <td>
                                    <%#Eval("BarangQuantity")%>
                                </td>
                                <td>
                                    <%#Eval("BarangOlahan")%>
                                </td>
                                <td>
                                    <%#Eval("Penjualan")%>
                                </td>
                                <td>
                                    <%#Eval("PenjualanRetur")%>
                                </td>
                                <td>
                                    <%#Eval("PembelianRetur")%>
                                </td>
                                <td id="qtyTotalPembelian" runat="server">
                                    <%#Eval("Pembelian")%>
                                </td>
                                <td id="tdVal1" runat="server">
                                    <asp:LinkButton runat="server" ID="lnkUpdate" CommandArgument='<%#Eval("IdBarang")%>' CssClass="btn btn-alternate" Text="Adjust" OnClick="lnkUpdate_Click"></asp:LinkButton>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        `</div>
    </div>
    <hr />
    <h5 class="card-title"><asp:Label ID="lblHeader2" runat="server" CssClass="font-weight-bold" Text="Arus Data Box"></asp:Label></h5>
    <div class="card-body">
        <div class="form-row">
            <div class="col-md-2">
                <div class="position-relative form-group">
                    <asp:Label ID="Label2" runat="server" CssClass="font-weight-bold" Text="Search Box"></asp:Label>
                    <asp:TextBox ID="txtSearch1" runat="server" CssClass="form-control" onkeyup='Filter1(this);'></asp:TextBox>
                </div>
            </div>
            <div class="col-md-2">
                <div class="position-relative form-group">
                    <asp:Label ID="Label6" runat="server" CssClass="font-weight-bold" Text="Inventory"></asp:Label>
                    <asp:DropDownList ID="ddlInventory1" runat="server" CssClass="form-control" AppendDataBoundItems="true" DataTextField="IvnStorage" DataValueField="IdInventory">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="-------------------" Value=""></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2">
                <div class="position-relative form-group">
                    <br />
                    <asp:Button ID="btnFilter1" runat="server" CssClass="btn btn-dark" Text="Search" OnClick="btnFilter1_Click" />
                </div>
            </div>
        </div>
    </div>
    <div class="card-body">
        <div id="table_box_bootstrap1" class="table table-responsive table-light table-bordered table-hover">
            <table class="mb-0 table">
                <thead>
                    <tr style="background-color:white">
                        <th>Nama Barang</th>
                        <th>Sisa Jumlah Box</th>
                        <th>Box Olahan Terpakai(-)</th>
                        <th>Total Terjual (-)</th>
                        <th>Retur Penjualan (+)</th>
                        <th>Retur Pembelian (-)</th>
                        <th>Total Pembelian (+)</th>
                        <th id="thVal2" runat="server">Action</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptBoxUpdate" runat="server" OnItemDataBound="rptBoxUpdate_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%#Eval("NamaBarang")%>
                                </td>
                                <td>
                                    <%#Eval("BarangJmlBox")%>
                                </td>
                                <td>
                                    <%#Eval("BarangOlahan")%>
                                </td>
                                <td>
                                    <%#Eval("Penjualan")%>
                                </td>
                                <td>
                                    <%#Eval("PenjualanRetur")%>
                                </td>
                                <td>
                                    <%#Eval("PembelianRetur")%>
                                </td>
                                <td id="boxTotalPembelian" runat="server">
                                    <%#Eval("Pembelian")%>
                                </td>
                                <td id="tdVal2" runat="server">
                                    <asp:LinkButton runat="server" ID="lnkUpdate1" CommandArgument='<%#Eval("IdBarang")%>' CssClass="btn btn-alternate" Text="Adjust" OnClick="lnkUpdate1_Click"></asp:LinkButton>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        `</div>
    </div>
    <script type="text/javascript">
        function Filter(Obj) {

            var grid = document.getElementsByTagName('tbody');
            var terms = Obj.value.toUpperCase();
            var cellNr = 0; //your grid cellindex like name
            var ele;
            for (var r = 0; r <= grid[1].rows.length; r++) {
                ele = grid[1].rows[r].cells[cellNr].innerHTML.replace(/<[^>]+>/g, "");
                if (ele.toUpperCase().indexOf(terms) >= 0)
                    grid[1].rows[r].style.display = '';
                else grid[1].rows[r].style.display = 'none';
            }
        }
        function Filter1(Obj) {
            var grid = document.getElementsByTagName('tbody')[2];
            var terms = Obj.value.toUpperCase();
            var cellNr = 0; //your grid cellindex like name
            var ele;
            for (var r = 0; r <= grid.rows.length; r++) {
                ele = grid.rows[r].cells[cellNr].innerHTML.replace(/<[^>]+>/g, "");
                if (ele.toUpperCase().indexOf(terms) >= 0)
                    grid.rows[r].style.display = '';
                else grid.rows[r].style.display = 'none';
            }
        }
    </script>
</asp:Content>
