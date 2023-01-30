<%@ Page Language="C#" MasterPageFile="~/Paginas/MasterPage.master" AutoEventWireup="true" CodeFile="Carrinho.aspx.cs" Inherits="Paginas_Carrinho" %>





<asp:Content ID="Conteudo" ContentPlaceHolderID="Conteudo" runat="Server">



    <div class="card" id="divResumo" runat="server">

        <div class="container-fluid box">

            <div class="card-header">
                <h5 class="card-title"><i class="nc-icon nc-cart-simple"></i>&nbsp;&nbsp;<asp:Label ID="lblTopo" runat="server" Text="Meu Carrinho"></asp:Label></h5>
            </div>
            <div class="row">
                <div class="col-md">
                    <asp:Label ID="lblMensagem" role="alert" Width="100%" runat="server" CssClass="alert alert-danger"></asp:Label>
                </div>
            </div>
            <div class="col-md-3 pr-1">
                <div class="card-header">
                    <h6 class="card-title">
                        <asp:Label ID="lblCarrinhoVazio" runat="server" Text="O carrinho está vazio!"></asp:Label></h6>
                </div>
            </div>


            <div class="card-body">

                <div id="divNinhada" runat="server">

                    <div class="col-md-4 pr-1">
                        <div class="card-header">
                            <h6 class="card-title"><i class="nc-icon nc-single-copy-04"></i>&nbsp;&nbsp;Mapas de Ninhadas</h6>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <asp:GridView ID="dtgCarrinhoNinhada" Width="100%" runat="server" AllowPaging="True" CssClass="table table-borderless" PageSize="100"
                                OnRowDataBound="dtgCarrinhoNinhada_RowDataBound">
                                <PagerStyle HorizontalAlign="Center" CssClass="PagerStyle" />
                                <HeaderStyle CssClass="bg-primary" HorizontalAlign="Left" />
                                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" FirstPageText="Primeira"
                                    LastPageText="Última" PreviousPageText="Anterior"
                                    NextPageText="Próxima" />
                            </asp:GridView>
                        </div>

                    </div>

                    <div class="card-header" style="text-align: right">
                        <h6 class="card-title">Valor Total Mapas de Ninhadas</h6>

                        <asp:Label ID="lblTotalNinhada" runat="server"></asp:Label>
                    </div>

                </div>




                <div class="row">

                    <div class="col-md-12 pr-1" id="divDadosTransferencia" runat="server">
                        <div class="card-header">
                            <h6 class="card-title"><i class="nc-icon nc-refresh-69"></i>&nbsp;&nbsp;Transferência de Propriedade</h6>
                            <asp:Label ID="Label3" runat="server"></asp:Label>
                        </div>


                        <div class="row">
                            <div class="col-md-12">
                                <asp:GridView ID="dtgTransferencia" Width="100%" runat="server" AllowPaging="True" CssClass="table table-borderless" PageSize="100"
                                    OnRowDataBound="dtgTransferencia_RowDataBound">
                                    <PagerStyle HorizontalAlign="Center" CssClass="PagerStyle" />
                                    <HeaderStyle CssClass="bg-primary" HorizontalAlign="Left" />
                                    <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" FirstPageText="Primeira"
                                        LastPageText="Última" PreviousPageText="Anterior"
                                        NextPageText="Próxima" />
                                </asp:GridView>
                            </div>

                        </div>
                        <div class="card-header" style="text-align: right">
                            <h6 class="card-title">Valor Total Transferência de Propriedade</h6>

                            <asp:Label ID="lblTotalTransferencia" runat="server"></asp:Label>
                        </div>

                    </div>


                </div>


                <div class="row">

                    <div class="col-md-12 pr-1" id="divTaxaEnvio" runat="server">
                        <div class="card-header">
                            <h5 class="card-title"><i class="nc-icon nc-delivery-fast"></i>&nbsp;&nbsp;Envio da documentação</h5>
                            <asp:Label ID="Label5" runat="server"></asp:Label>
                        </div>

                        <div class="row">
                            <div class="card-header">

                                <h6 class="card-title">Como deseja receber a documentação?</h6>
                                <h6 class="card-title">
                                    <asp:CheckBox ID="ckbEnvioDocFisico" runat="server" AutoPostBack="true" OnCheckedChanged="ckbEnvioDocFisico_CheckedChanged" Text="&nbsp;&nbsp;Física, via correio e arquivo PDF, via e-mail."></asp:CheckBox></h6>
                                <h6 class="card-title">
                                    <asp:CheckBox ID="ckbEnvioDocEmail" runat="server" AutoPostBack="true" OnCheckedChanged="ckbEnvioDocEmail_CheckedChanged" Text="&nbsp;&nbsp;Apenas arquivo PDF, via e-mail."></asp:CheckBox></h6>

                            </div>
                        </div>

                        <div class="card-header" style="text-align: right">
                            <h6 class="card-title">Valor do envio</h6>

                            <asp:Label ID="lblValorEnvio" runat="server"></asp:Label>
                        </div>

                    </div>


                </div>

                <div class="row">
                    <div id="divCupomDesconto" runat="server">
                        <div class="card-header">
                            <h5 class="card-title"><i class="nc-icon nc-layout-11"></i>&nbsp;&nbsp;<asp:Label ID="lblCupomDesconto" runat="server" Text="Cupom de Desconto"></asp:Label></h5>

                        </div>

                         <div class="col-md">
                <asp:Label ID="lblMensagemCupomInvalido" role="alert" Width="100%" runat="server" CssClass="alert alert-danger" Text="Cupom Inválido"></asp:Label>
            </div>
                           <div class="col-md">
                <asp:Label ID="lblMensagemCupomValido" role="alert" Width="100%" runat="server" CssClass="alert alert-success" Text="Cupom Validado!"></asp:Label>
            </div>

                        <div class="col-md pr-1">
                            <input id="txtCodCupom" placeholder="Código" class="form-control" type="text" maxlength="50" size="12" name="txtCodCupom" runat="server"></input>
                            <asp:Button CssClass="btn btn-success btn-round" ID="btnValidarCupom" runat="server" Text="Validar Cupom" OnClick="btnValidarCupom_Click"></asp:Button>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div id="divSubtotal" runat="server">
                        <div class="card-header">
                            <h5 class="card-title"><i class="nc-icon nc-money-coins"></i>&nbsp;&nbsp;<asp:Label ID="lblTotalAPagar" runat="server" Text="Opções de Pagamento"></asp:Label></h5>
                            <h6 class="card-title">Débito:&nbsp;<asp:Label ID="lblValorAVista" runat="server" Text="R$ 40,00"></asp:Label></h6>
                            <h6 class="card-title">Crédito:&nbsp;<asp:Label ID="lblValorAPrazo" runat="server" Text="R$ 40,00"></asp:Label></h6>

                        </div>


                    </div>
                </div>

            </div>
        </div>

    </div>



    <div id="divPagamento" runat="server" class="card">

        <div class="container-fluid box">
            <div class="card-header">
                <h5 class="card-title">
                    <asp:Label ID="lbl" runat="server" Text="Dados do Pagamento"></asp:Label></h5>
            </div>
            <div class="row">
                <div class="col-md">
                    <asp:Label ID="lblMensagemPagamento" role="alert" Width="100%" runat="server" CssClass="alert alert-danger"></asp:Label>
                </div>
            </div>
            <div class="card-body">
                <div class="row">
                    <div class="col-md-3 pr-1">

                        <label>Nome do Titular *</label>
                        <input id="txtNomeTitular"
                            class="form-control" type="text" maxlength="40" size="12" name="txtNome" runat="server"></input>

                    </div>

                    <div class="col-md-3 pl-1">
                        <label>Nº do Cartão *</label>
                        <input id="txtNumCartao"
                            class="form-control" type="number" maxlength="16" size="12" name="txtNome" runat="server"></input>

                    </div>
                    <div class="col-md-2 pl-1">
                        <label>Data de Validade *</label>
                        <input class="form-control" onkeypress='Formatar(this, "##/####")' id="txtValidade"
                            type="text" maxlength="7" placeholder="MM/AAAA" autocomplete="off" name="txtDatNascimento" runat="server" />
                        <script type="text/javascript" src="../Scripts/Utils/DatepickerUtils.js">
                                                    DatepickerUtils.createDatepicker($('#txtValidade'))
                        </script>
                        <a href="javascript:void(0)" id="A1" runat="server"></a>
                    </div>

                    <div class="col-md-1 pl-1">
                        <label>CVV *</label>
                        <input id="txtCVV"
                            class="form-control" type="number" maxlenght="3" size="8" name="txtCVV" runat="server"></input>
                    </div>

                    <div class="col-md-2 pl-1">
                        <label>Bandeira *</label>
                        <asp:DropDownList ID="drpBandeira" runat="server" CssClass="form-control">
                        </asp:DropDownList>

                    </div>
                    <div class="col-md-3 pr-1">

                        <label>Opção de Pagamento *</label>
                        <asp:DropDownList ID="drpOpcaoPagamento" runat="server" CssClass="form-control" OnSelectedIndexChanged="drpOpcaoPagamento_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>

                    </div>



                    <div class="col-md-3 pr-1">

                        <div class="row">
                            <div class="col-md">
                                <asp:CheckBox ID="ckbTermos" runat="server"></asp:CheckBox>
                                <asp:LinkButton ID="lkbTermos" runat="server" OnClick="lkbTermos_Click" Text="Li e Concordo com os termos de pagamento"></asp:LinkButton>
                            </div>
                        </div>

                    </div>

                    <div class="col-md-3 px-1" id="divQtdeParcelas" runat="server">

                        <label>Quantidade de Parcelas *</label>
                        <asp:DropDownList ID="drpParcelas" runat="server" CssClass="form-control">
                        </asp:DropDownList>

                    </div>

                </div>

                <div class="row">
                    <asp:Button CssClass="btn btn-success btn-round" ID="btnRealizarPagamento" runat="server" Text="Realizar Pagamento" OnClick="btnRealizarPagamento_Click"></asp:Button>

                </div>
            </div>

        </div>


    </div>



    <div id="divComprovante" runat="server" class="card">

        <div class="container-fluid box">

            <div class="card-header">
                <h5 class="card-title">Comprovante de Pagamento</h5>
            </div>
            <div class="row">
                <div class="col-md">
                    <asp:Label ID="Label1" role="alert" Width="100%" runat="server" CssClass="alert alert-success"><center>Pagamento realizado com sucesso!</center></asp:Label>
                </div>
            </div>

            <div class="card-body">



                <div class="row">



                    <div class="container-fluid box">

                        <div class="card-header">
                            <h5 class="card-title"><i class="nc-icon nc-paper"></i>&nbsp;&nbsp;<asp:Label ID="Label2" runat="server" Text="Dados do Pedido"></asp:Label></h5>
                        </div>


                        <div class="col-md-12 pr-1" id="divNinhadaComprovante" runat="server" visible="false">

                            <div class="col-md-4 pr-1">
                                <div class="card-header">
                                    <h6 class="card-title"><i class="nc-icon nc-single-copy-04"></i>&nbsp;&nbsp;Mapas de Ninhadas</h6>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-12">
                                    <asp:GridView ID="dtgNinhadaComprovante" Width="100%" runat="server" AllowPaging="True" CssClass="table table-borderless" PageSize="100"
                                        OnRowDataBound="dtgNinhadaComprovante_RowDataBound">
                                        <PagerStyle HorizontalAlign="Center" CssClass="PagerStyle" />
                                        <HeaderStyle CssClass="bg-primary" HorizontalAlign="Left" />
                                        <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" FirstPageText="Primeira"
                                            LastPageText="Última" PreviousPageText="Anterior"
                                            NextPageText="Próxima" />
                                    </asp:GridView>
                                </div>

                            </div>

                            <div class="card-header" style="text-align: right">
                                <h6 class="card-title">Valor Total Mapas de Ninhadas</h6>

                                <asp:Label ID="lblTotalNinhadaComprovante" runat="server"></asp:Label>
                            </div>

                        </div>










                        <div class="row">

                            <div class="col-md-12 pr-1" id="divTrasnferenciaComprovante" runat="server" visible="false">
                                <div class="card-header">
                                    <h6 class="card-title"><i class="nc-icon nc-refresh-69"></i>&nbsp;&nbsp;Transferência de Propriedade</h6>
                                    <asp:Label ID="Label7" runat="server"></asp:Label>
                                </div>


                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:GridView ID="dtgTransferenciaComprovante" Width="100%" runat="server" AllowPaging="True" CssClass="table table-borderless" PageSize="100"
                                            OnRowDataBound="dtgTransferenciaComprovante_RowDataBound">
                                            <PagerStyle HorizontalAlign="Center" CssClass="PagerStyle" />
                                            <HeaderStyle CssClass="bg-primary" HorizontalAlign="Left" />
                                            <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" FirstPageText="Primeira"
                                                LastPageText="Última" PreviousPageText="Anterior"
                                                NextPageText="Próxima" />
                                        </asp:GridView>
                                    </div>

                                </div>
                                <div class="card-header" style="text-align: right">
                                    <h6 class="card-title">Valor Total Transferência de Propriedade</h6>

                                    <asp:Label ID="lblTotalTransfComprovante" runat="server"></asp:Label>
                                </div>

                            </div>


                        </div>







                        <div class="row">

                            <div class="col-md-12 pr-1" id="divTaxaPagamentoComprovante" runat="server">
                                <div class="card-header">
                                    <h5 class="card-title"><i class="nc-icon nc-delivery-fast"></i>&nbsp;&nbsp;Envio da documentação</h5>
                                    <asp:Label ID="Label6" runat="server"></asp:Label>
                                </div>

                                <div class="row">
                                    <div class="card-header">

                                        <h6 class="card-title">A documentação será enviada:</h6>
                                        <h6 class="card-title">
                                            <asp:Label ID="lblEnvioCorreioComprovante" runat="server" Text="Física, via correio e arquivo PDF, via e-mail."></asp:Label></h6>
                                        <h6 class="card-title">
                                            <asp:Label ID="lblEnvioEmailComprovante" runat="server" Text="Arquivo PDF, via e-mail."></asp:Label></h6>

                                    </div>
                                </div>

                                <div class="card-header" style="text-align: right">
                                    <h6 class="card-title">Valor do envio</h6>

                                    <asp:Label ID="lblValorEnvioComprovante" runat="server"></asp:Label>
                                </div>

                            </div>


                        </div>



                         <div class="row">
                    <div id="divCupomComprovante" runat="server">
                        <div class="card-header">
                            <h5 class="card-title"><i class="nc-icon nc-layout-11"></i>&nbsp;&nbsp;<asp:Label ID="Label8" runat="server" Text="Cupom de Desconto"></asp:Label></h5>

                        </div>

                      
                           <div class="col-md">
                <asp:Label ID="lblMsgCupomComprovante" role="alert" Width="100%" runat="server" CssClass="alert alert-success" Text="Cupom Validado!"></asp:Label>
            </div>

                    </div>
                </div>



                        <div class="row">
                            <div id="div4" runat="server">
                                <div class="card-header">
                                    <h5 class="card-title"><i class="nc-icon nc-money-coins"></i>&nbsp;&nbsp;<asp:Label ID="lblTotalAPagarComprovante" runat="server" Text="Total a Pagar"></asp:Label></h5>
                                </div>


                            </div>
                        </div>

                        <div class="row">
                            <div class="card-header">
                                <h5 class="card-title"><i class="nc-icon nc-credit-card"></i>&nbsp;&nbsp;<asp:Label ID="Label4" runat="server" Text="Dados do Pagamento"></asp:Label></h5>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4 pr-1">

                                <div class="card-header">
                                    <h6 class="card-title">Nome do Pagador</h6>
                                    <asp:Label ID="lblNomTitularComprovante" runat="server"></asp:Label>
                                </div>

                            </div>

                            <div class="col-md-4 pr-1">
                                <div class="card-header">
                                    <h6 class="card-title">Nº do Cartão</h6>
                                    <asp:Label ID="lblNumCartaoComprovante" runat="server"></asp:Label>
                                </div>
                            </div>

                            <div class="col-md-4 pr-1">

                                <div class="card-header">
                                    <h6 class="card-title">Tipo de Pagamento</h6>
                                    <asp:Label ID="lblTipoPagamentoComprovante" runat="server"></asp:Label>
                                </div>


                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4 pr-1">

                                <div class="card-header">
                                    <h6 class="card-title">Bandeira</h6>
                                    <asp:Label ID="lblBandeiraComprovante" runat="server"></asp:Label>
                                </div>


                            </div>

                            <div class="col-md-4 pr-1">
                                <div class="card-header">
                                    <h6 class="card-title">Data do Pagamento</h6>
                                    <asp:Label ID="lblDataPagamento" runat="server"></asp:Label>
                                </div>



                            </div>

                            <div class="col-md-2 pr-1">

                                <div class="card-header">
                                    <h6 class="card-title">Valor</h6>
                                    <asp:Label ID="lblValorComprovante" runat="server"></asp:Label>
                                </div>
                            </div>



                        </div>



                        <div class="row">
                            <div class="col-md-4 pr-1">

                                <div class="card-header">
                                    <h6 class="card-title">
                                        <asp:Label ID="lblRotuloQtdeParcelasComprovante" runat="server">Quantidade de Parcelas</asp:Label></h6>
                                    <asp:Label ID="lblQuantidadeParcelasComprovante" runat="server"></asp:Label>
                                </div>


                            </div>

                        </div>
                    </div>





                    <br />





                </div>

            </div>

        </div>



    </div>

    <div class="row">
        <div class="col-md pr-1" id="divBotoes" runat="server">
            <asp:Button CssClass="btn btn-primary btn-round" ID="btnImprimir" runat="server" Text="Imprimir" OnClientClick="printDiv('Conteudo_divComprovante')"></asp:Button>
            <asp:Button CssClass="btn btn-success btn-round" ID="btnConcluir" runat="server" Text="Concluir" OnClick="btnConcluir_Click"></asp:Button>
        </div>
    </div>

    <script>
        function printDiv(divName) {
            var printContents = document.getElementById(divName).innerHTML;
            var originalContents = document.body.innerHTML;

            document.body.innerHTML = printContents;

            window.print();

            document.body.innerHTML = originalContents;

        }
    </script>

</asp:Content>


