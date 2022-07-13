<%@ Page Title="" Language="C#" MasterPageFile="~/Visitor.Master" AutoEventWireup="true" CodeBehind="AddBiayaPage.aspx.cs" Inherits="TestCoba.AddBiayaPage" %>
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
                    <asp:Label ID="lblHeaderPage" runat="server" Text="Tambah Biaya"></asp:Label>
                    <div class="page-title-subheading">Halaman Ini Untuk Menambahkan Atau Mengubah Data Biaya.
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
            <asp:Label ID="lblStatusPage" runat="server" Text="Tambah Biaya"></asp:Label>
        </h5>
        <div class="form-row">
            <div class="col-md-4">
                <div class="position-relative form-group">
                    <asp:Label ID="lblJenisBiaya" runat="server" Text="Jenis Biaya"></asp:Label>
                    <asp:DropDownList ID="ddlJenisBiaya" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Biaya Peralatan" Value="Biaya Peralatan"></asp:ListItem>
                        <asp:ListItem Text="Biaya Produksi" Value="Biaya Produksi"></asp:ListItem>
                        <asp:ListItem Text="Biaya Operasional" Value="Biaya Operasional"></asp:ListItem>
                        <asp:ListItem Text="Biaya Pribadi Ibu" Value="Biaya Pribadi Ibu"></asp:ListItem>
                        <asp:ListItem Text="Biaya Pribadi Bapak" Value="Biaya Pribadi Bapak"></asp:ListItem>
                        <asp:ListItem Text="Biaya Pribadi Yoga" Value="Biaya Pribadi Yoga"></asp:ListItem>
                        <asp:ListItem Text="Biaya Pribadi Jihan" Value="Biaya Pribadi Jihan"></asp:ListItem>
                        <asp:ListItem Text="Biaya Pribadi Marcel" Value="Biaya Pribadi Marcel"></asp:ListItem>
                        <asp:ListItem Text="Biaya Bunga Bank" Value="Biaya Bunga Bank"></asp:ListItem>
                        <asp:ListItem Text="Pajak" Value="Pajak"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-4">
                <div class="position-relative form-group">
                    <asp:Label ID="lblTanggalInvoice" runat="server" Text="Tanggal Biaya"></asp:Label>
                    <asp:TextBox ID="txtCldBrgBeli" runat="server" placeholder="Tanggal Biaya" Text="mm/dd/yyyy" onkeydown="return false;" CssClass="form-control fa-calendar" required="required"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtCldBrgBeli" runat="server" Format="MM/dd/yyyy" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic" ControlToValidate="txtCldBrgBeli" ErrorMessage="Tanggal Harus Diisi!" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                    <asp:Label ID="vldTglInv" runat="server" CssClass="alert-danger" Text=""></asp:Label>
                </div>
            </div>
        </div>
        <hr />
        <div id="table_box_bootstrap" class="table table-responsive table-light table-hover">
            <table class="mb-0 table">
                <asp:Repeater ID="rptPembiayaan" runat="server">
                    <HeaderTemplate>
                        <tr>
                            <td style="width:400px">
                                <asp:Label ID="lblItemName" runat="server" Text="Item Name"></asp:Label>
                            </td>
                            <td style="width:400px">
                                <asp:Label ID="lblTotalPrice" runat="server" Text="Total Price"></asp:Label>
                            </td>
                            <td colspan="2" style="width:100px">&nbsp;</td>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtItemCode" runat="server" Enabled="false" CssClass="form-control" Visible="false"></asp:TextBox>
                                <asp:TextBox ID="txtItemName" runat="server" placeholder="Item Name" CssClass="form-control" required="required"></asp:TextBox>
                                <asp:DropDownList ID="ddlNamaBarang" runat="server" Visible="false" CssClass="form-control"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTotalPrice" runat="server" placeholder="Rp0" CssClass="form-control" AutoPostBack="true" onfocus="this.value=''" onkeypress="return allowOnlyNumber(event);" OnTextChanged="txtTotalPrice_TextChanged"></asp:TextBox>
                            </td>
                            <td colspan="2" style="text-align:center">
                                <asp:LinkButton ID="btnadd" runat="server" Text="+" CssClass="btn btn-success" OnClick="btnadd_Click"></asp:LinkButton>
                                <asp:LinkButton ID="btndel" runat="server" Text="-" CssClass="btn btn-danger left" OnClick="btndel_Click"></asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <tr>
                    <td>
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
                            <asp:Button ID="btnSubmit" runat="server" CssClass="mt-2 btn btn-primary" UseSubmitBehavior="true" CausesValidation="true" Text="Submit" OnClick="btnSubmit_Click"/>
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
            var calendar = document.getElementById('<%=txtCldBrgBeli.ClientID %> ');
            if(calendar.val() != "mm/dd/yyyy")
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
