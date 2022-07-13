<%@ Page Title="" Language="C#" MasterPageFile="~/Visitor.Master" AutoEventWireup="true" CodeBehind="AddPembelianPage.aspx.cs" Inherits="TestCoba.AddPembelianPage" Culture="en-US" UICulture="en-US" %>
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
                        <asp:Label ID="lblHeaderPage" runat="server" Text="Tambah Pembelian"></asp:Label>
                        <div class="page-title-subheading">Halaman Ini Untuk Menambahkan Atau Me-Retur Data Pembelian.
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
            <asp:Label ID="lblStatusPage" runat="server" Text="Tambah Pembelian"></asp:Label></h5>
            <div class="form-row">
                <div class="col-md-2">
                    <div class="position-relative form-group">
                        <asp:Label ID="lblNoInvoice" runat="server" Text="No Invoice"></asp:Label>
                        <asp:TextBox ID="txtNoInvoice" runat="server" placeholder="No Invoice" CssClass="form-control text-uppercase" required="required"></asp:TextBox>
                        <asp:Label ID="vldNoInvoice" runat="server" CssClass="alert-danger" Text=""></asp:Label>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="position-relative form-group">
                        <asp:Label ID="lblSupName" runat="server" Text="Supplier Name"></asp:Label>
                        <asp:DropDownList ID="ddlSupName" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlSupName_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="position-relative form-group">
                        <asp:Label ID="lblTanggalInvoice" runat="server" Text="Tanggal Pembelian"></asp:Label>
                        <asp:TextBox ID="txtCldBrgBeli" runat="server" placeholder="Tanggal Beli" Text="mm/dd/yyyy" onkeydown="return false;" CssClass="form-control fa-calendar" required="required"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtCldBrgBeli" runat="server" Format="MM/dd/yyyy" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Static" ControlToValidate="txtCldBrgBeli" ErrorMessage="RequiredFieldValidator"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="txtCldBrgBeli" ControlToCompare="txtCldJtuhTempo" Type="Date" Display="Dynamic" EnableClientScript="true" SetFocusOnError="true" ErrorMessage="Tanggal Kurang dari Jatuh Tempo" ForeColor="Red" Operator="LessThanEqual"></asp:CompareValidator>
                        <asp:Label ID="vldTglInv" runat="server" CssClass="alert-danger" Text=""></asp:Label>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="position-relative form-group">
                        <asp:Label ID="lblJenisTagihan" runat="server" Text="Jenis Tagihan"></asp:Label>
                        <asp:DropDownList ID="ddlJenisTagihan" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Kontrabon" Value="KONTRABON"></asp:ListItem>
                            <asp:ListItem Text="Cash" Value="CASH"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="position-relative form-group">
                        <asp:Label ID="lblJtuhTempo" runat="server" Text="Tanggal Jatuh Tempo"></asp:Label>
                        <asp:TextBox ID="txtCldJtuhTempo" runat="server" placeholder="Tanggal Jatuh Tempo" Text="mm/dd/yyyy" onkeydown="return false;" CssClass="form-control fa-calendar" required="required"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtCldJtuhTempo" runat="server" Format="MM/dd/yyyy" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Static" ControlToValidate="txtCldJtuhTempo" ErrorMessage="RequiredFieldValidator"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtCldJtuhTempo" ControlToCompare="txtCldBrgBeli" Type="Date" Display="Dynamic" EnableClientScript="true" SetFocusOnError="true" ErrorMessage="tanggal Harus Lebih Dari Tanggal Beli" ForeColor="Red" Operator="GreaterThanEqual"></asp:CompareValidator>
                        <asp:Label ID="vldJtuhTempo" runat="server" CssClass="alert-danger" Text=""></asp:Label>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="position-relative form-group">
                        <asp:Label ID="lblBrgNameBank" runat="server" Text="Detail Rekening Bank"></asp:Label>
                        <asp:TextBox ID="txtBrgNameBank" runat="server" placeholder="Bank-No Rekening" CssClass="form-control text-uppercase" required="required"></asp:TextBox>
                        <asp:Label ID="vldBrgNameBank" runat="server" CssClass="alert-danger" Text=""></asp:Label>
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
                        <asp:Label ID="lblSisaKapasitasGudang" runat="server" Text="Sisa Kapasitas Gudang"></asp:Label>
                        <asp:TextBox ID="txtSisaKapasitasGudang" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <hr />
            <div id="table_box_bootstrap" class="table table-responsive table-light table-hover">
                <table class="mb-0 table">
                    <asp:Repeater ID="rptPembelianDtl" runat="server">
                    <HeaderTemplate>
                        <tr>
                            <td style="width:300px">
                                <asp:Label ID="lblItemName" runat="server" Text="Item Name"></asp:Label>
                            </td>
                            <td style="width:120px">
                                <asp:Label ID="lblItemQty" runat="server" Text="Qty / Kg"></asp:Label>
                            </td>
                            <td style="width:100px">
                                <asp:Label ID="lblPriceKg" runat="server" Text="Price / Kg"></asp:Label>
                            </td>
                            <td style="width:70px">
                                <asp:Label ID="lblTemBox" runat="server" Text="Box"></asp:Label>
                            </td>
                            <td style="width:230px">
                                <asp:Label ID="lblTotalPrice" runat="server" Text="Total Price"></asp:Label>
                            </td>
                            <td colspan="2" style="width:100px">&nbsp;</td>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtItemCode" runat="server" Enabled="false" CssClass="form-control" Visible="false"></asp:TextBox>
                                <asp:TextBox ID="txtItemName" runat="server" placeholder="Item Name" CssClass="form-control text-uppercase" Visible="false" required="required"></asp:TextBox>
                                <asp:DropDownList ID="ddlNamaBarang" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlNamaBarang_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemQty" runat="server" placeholder="0" CssClass="form-control" AutoPostBack="true" onfocus="this.value=''" onkeypress="return isNumberKey(event);" OnTextChanged="txtItemQty_TextChanged" required="required"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPrice" runat="server" placeholder="0" CssClass="form-control" AutoPostBack="true" onfocus="this.value=''" onkeypress="return allowOnlyNumber(event);" OnTextChanged="txtPrice_TextChanged" required="required"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemBox" runat="server" placeholder="0" CssClass="form-control" AutoPostBack="true" onfocus="this.value=''" onkeypress="return allowOnlyNumber(event);" OnTextChanged="txtItemBox_TextChanged" required="required"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTotalPrice" runat="server" placeholder="Rp0" CssClass="form-control" AutoPostBack="true" Enabled="false" OnTextChanged="txtTotalPrice_TextChanged"></asp:TextBox>
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
                        <asp:Button ID="bckBtn" runat="server" CssClass="mt-2 btn btn-danger" Text="Back" UseSubmitBehavior="false" CausesValidation="false" OnClick="bckBtn_Click" />
                    </td>
                    <td>
                        <div class="position-relative form-group">
                            <asp:Label ID="lblGrandTotal" runat="server" Text="Grand Total : "></asp:Label>
                            <asp:TextBox ID="txtGrandTotal" runat="server" CssClass="form-control" Enabled="false" Text="0"></asp:TextBox>
                        </div>
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
        function CheckDate(sender, args)
        {
            var calendar = document.getElementById("#txtCldBrgBeli");
            if(calendar.SelectedDate != "mm/dd/yyyy")
            {
                args.IsValid = true;
            }
            else
            {
                args.IsValid = false;
            }
        }
    </script>
</asp:Content>
