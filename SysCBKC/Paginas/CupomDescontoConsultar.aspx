<%@ Page Language="C#" MasterPageFile="~/Paginas/MasterPage.master" AutoEventWireup="true" CodeFile="CupomDescontoConsultar.aspx.cs" Inherits="Paginas_CupomDescontoConsultar" %>




<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" runat="Server">
    <div class="card">

        <div class="container-fluid box">
            <div class="card-header">
                <h5 class="card-title">Gestão de Cupons</h5>
            </div>


            <div class="row">
                <div class="col-md" id="divMsg" runat="server">
                    <asp:Label ID="lblMensagem" role="alert" Width="100%" runat="server" CssClass="alert alert-danger"></asp:Label>
                </div>
            </div>
            <div class="card-body">
                <div class="row">
                    <div class="form-group col-4">
                        <label>Código do Cupom</label>


                        <asp:TextBox ID="txtCodCupom" CssClass="form-control" MaxLength="20" style='text-transform:uppercase' runat="server"></asp:TextBox>
                    </div>

                    <div class="form-group col-4">
                        <label>Nome do Cliente Reembolsado</label>


                        <asp:TextBox ID="txtNomClienteReembolso" CssClass="form-control" MaxLength="100" style='text-transform:uppercase' runat="server"></asp:TextBox>
                    </div>
                    <div class="form-group col-3">
                        <label>
                            <asp:Label ID="lblStatus" runat="server" Text="Status do Cupom"></asp:Label></label>
                        <asp:DropDownList ID="drpStatus" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>

                </div>
                <div class="row">
                    <div class="col-md-4 pr-1">
                        <label>Período de Inclusão</label>
                        <div class="row">
                            <div class="col-md-4 pr-1">
                                <input class="form-control" onkeypress='Formatar(this, "##/##/####")' id="txtDatCadastroIni"
                                    type="text" maxlength="10" autocomplete="off" name="txtDatCadastroIni" runat="server" />
                                <script type="text/javascript">
                                    DatepickerUtils.createDatepicker($('#txtDatCadastroIni'))
                                </script>
                                <a href="javascript:void(0)" id="calDatCadastroIni" runat="server"></a>
                            </div>
                            <div class="col-md-4 pr-1">
                                <input class="form-control" onkeypress='Formatar(this, "##/##/####")' id="txtDatCadastroFim"
                                    type="text" maxlength="10" autocomplete="off" name="txtDatCadastroFim" runat="server" />
                                <a href="javascript:void(0)" id="calDatFim" runat="server"></a>
                                <script type="text/javascript">
                                    DatepickerUtils.createDatepicker($('#txtDatCadastroFim'))
                                </script>
                                <a href="javascript:void(0)" id="calDatCadastroFim" runat="server"></a>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4 pr-1">
                        <label>Período de Utilização</label>
                        <div class="row">
                            <div class="col-md-4 pr-1">
                                <input class="form-control" onkeypress='Formatar(this, "##/##/####")' id="txtDatUtilizacaoIni"
                                    type="text" maxlength="10" autocomplete="off" name="txtDatUtilizacaoIni" runat="server" />
                                <script type="text/javascript">
                                    DatepickerUtils.createDatepicker($('#txtDatUtilizacaoIni'))
                                </script>
                                <a href="javascript:void(0)" id="calDatUtilizacaoIni" runat="server"></a>
                            </div>
                            <div class="col-md-4 pr-1">
                                <input class="form-control" onkeypress='Formatar(this, "##/##/####")' id="txtDatUtilizacaoFinal"
                                    type="text" maxlength="10" autocomplete="off" name="txtDatUtilizacaoFinal" runat="server" />

                                <script type="text/javascript">
                                    DatepickerUtils.createDatepicker($('#txtDatUtilizacaoFinal'))
                                </script>
                                <a href="javascript:void(0)" id="calDatUtilizacaoFinal" runat="server"></a>
                            </div>
                        </div>
                    </div>
                </div>



                <div>
                    <asp:Button CssClass="btn btn-primary btn-round" ID="btnConsultar" runat="server" Text="Consultar" OnClick="btnConsultar_Click"></asp:Button>
                    <asp:Button CssClass="btn btn-primary btn-round" ID="btnIncluir" runat="server" Text="Incluir" OnClick="btnIncluir_Click"></asp:Button>
                </div>

                <div class="row">
                    <div class="col">
                        <asp:GridView ID="dtgCupom" Width="100%" runat="server" AllowPaging="True" CssClass="table table-striped table-bordered table-hover table-padrao" PageSize="30"
                            OnRowDataBound="dtgCupom_RowDataBound" OnPageIndexChanging="dtgCupom_PageIndexChanging">
                            <PagerStyle HorizontalAlign="Center" CssClass="PagerStyle" />
                            <HeaderStyle CssClass="bg-primary" HorizontalAlign="Left" />
                            <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" FirstPageText="Primeira"
                                LastPageText="Última" PreviousPageText="Anterior"
                                NextPageText="Próxima" />
                        </asp:GridView>
                    </div>
                </div>
            </div>


<%--            <input id="datepicker" width="276" />
    <script>
        $('#datepicker').datepicker({
            uiLibrary: 'bootstrap4'
        });
    </script>--%>
        </div>
    </div>

</asp:Content>


