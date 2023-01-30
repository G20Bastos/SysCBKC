<%@ Page Language="C#" MasterPageFile="~/Paginas/MasterPage.master" AutoEventWireup="true" CodeFile="NinhadaRegistrar.aspx.cs" Inherits="Paginas_NinhadaRegistrar" %>





<asp:Content ID="Conteudo" ContentPlaceHolderID="Conteudo" runat="Server">
    <div class="card" id="divDadosBasicos" runat="server">
        <div class="card-header" id="lblCabecalhoDadosBasicos" runat="server">
            <h5 class="card-title">Dados da Ninhada</h5>
        </div>
        <div class="row">
            <div class="col-md">
                <asp:Label ID="lblMensagem" role="alert" Width="100%" runat="server" CssClass="alert alert-danger"></asp:Label>
            </div>
        </div>
        <div class="card-body">

            <div>
                <div class="row">
                    <div class="col-md-3 pr-1">

                        <label>Nome do Padreador (macho) *</label>
                        <input id="txtNomePadreador"
                            class="form-control" type="text" maxlength="100" size="12" name="txtNome" style='text-transform:uppercase'  runat="server"></input>

                    </div>
                    <div class="col-md-3 px-1">

                        <label>RG do Padreador *</label>
                        <input id="txtRGPadreador"
                            class="form-control" type="text" maxlength="100" size="12" name="txtNome" style='text-transform:uppercase'  runat="server"></input>

                    </div>



                </div>
                <div class="row">
                    <div class="col-md-3 pr-1">
                        <label>Nome da Matriz (fêmea) *</label>
                        <input id="txtNomeMatriz"
                            class="form-control" type="text" maxlength="100" size="12" name="txtNome" style='text-transform:uppercase'  runat="server"></input>

                    </div>
                    <div class="col-md-3 px-1">

                        <label>RG da Matriz *</label>
                        <input id="txtRGMatriz"
                            class="form-control" type="text" maxlength="100" size="12" name="txtNome" style='text-transform:uppercase'  runat="server"></input>

                    </div>




                </div>
                <div class="row">
                    <div class="col-md-3 pr-1">

                        <label>Data de Nascimento da Ninhada *</label>
                        <input class="form-control" onkeypress='Formatar(this, "##/##/####")' id="txtDatNascimento"
                            type="text" maxlength="10" autocomplete="off" name="txtDatNascimento" runat="server" />
                        <script type="text/javascript" src="../Scripts/Utils/DatepickerUtils.js">
                                                    DatepickerUtils.createDatepicker($('#txtDatNascimento'))
                        </script>
                        <a href="javascript:void(0)" id="calDatIni" runat="server"></a>

                    </div>

                    <div class="col-md-3 px-1">
                        <label>Raça *</label>
                        <asp:DropDownList ID="drpRaca" runat="server" CssClass="form-control">
                        </asp:DropDownList>

                    </div>

                    <div class="col-md-4 pl-1" id="divDeclaracao">
                        <label>Declaração de Propriedade (máximo 5MB) - 
                            <asp:LinkButton ID="lkbTermoNinhada" runat="server" OnClick="lkbTermoNinhada_Click" Text="clique para baixar"></asp:LinkButton></label>
                        <asp:FileUpload ID="anexoDeclaracao" CssClass="form-control-file" runat="server" />
                        <label>
                            <asp:Label ID="lblArquivoSelecionado" runat="server" Text="Teste" Visible="false"></asp:Label></label>
                    </div>

                </div>
                <br />
                <div class="row">
                    <div class="col-md">
                        <asp:CheckBox ID="ckbTermos" runat="server"></asp:CheckBox>
                        <asp:LinkButton ID="lkbTermos" runat="server" OnClick="lkbTermos_Click" Text="Li e Concordo com os termos"></asp:LinkButton>

                    </div>
                </div>
                <div class="col-md-3 pr-1">
                    <div class="row">
                        <asp:Button CssClass="btn btn-primary btn-round" ID="btnDefinirDados" runat="server" Text="Definir Dados" OnClick="btnDefinirDados_Click"></asp:Button>

                    </div>
                </div>

            </div>



            <div id="divDadosVariaveis" runat="server">
                <div class="row">
                    <div class="col-md-3 pr-1">
                        <div class="form-group">
                            <label>Nome do Filhote *</label>
                            <input id="txtNomeFilhote"
                                class="form-control" type="text" maxlength="40" size="12" name="txtNome" style='text-transform:uppercase'  runat="server"></input>
                        </div>
                    </div>
                    <div class="col-md-3 pr-1">
                        <div class="form-group">
                            <label>Microchip</label>
                            <input id="txtMicrochip"
                                class="form-control" type="text" maxlength="20" size="12" name="txtMicrochip" style='text-transform:uppercase'  runat="server"></input>
                        </div>
                    </div>
                    <div class="col-md-3 px-1">
                        <div class="form-group">
                            <label>Variedade</label>
                            <asp:DropDownList ID="drpVariedade" runat="server" CssClass="form-control" OnSelectedIndexChanged="drpVariedade_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="col-md-2 pl-1">
                        <div class="form-group">
                            <label>Cor *</label>
                            <asp:DropDownList ID="drpCor" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-1 px-1">
                        <label>Sexo *</label>
                        <div class="row">
                            <div class="col-md1 px-1">
                                <asp:RadioButton
                                    ID="radioBtnMacho"
                                    runat="server"
                                    Text="Macho"
                                    AutoPostBack="true"
                                    OnCheckedChanged="radioBtnMacho_CheckedChanged" />
                            </div>

                        </div>
                        <div class="row">
                            <div class="col-md1 px-1">
                                <asp:RadioButton
                                    ID="radioBtnFemea"
                                    runat="server"
                                    Text="Fêmea"
                                    AutoPostBack="true"
                                    OnCheckedChanged="radioBtnFemea_CheckedChanged" />
                            </div>

                        </div>

                    </div>
                    <div class="col-md pr-1">
                        <div class="row">
                            <asp:Button CssClass="btn btn-success btn-round" ID="btnAdicionarFilhote" runat="server" Text="Adicionar" OnClick="btnAdicionarFilhote_Click"></asp:Button>
                            <asp:Button CssClass="btn btn-danger btn-round" ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click"></asp:Button>
                        </div>
                    </div>




                </div>
                <div class="row">
                    <div class="col" align="right">
                        <asp:LinkButton ID="lkbExcluirSelecionados" class="badge badge-danger" runat="server" OnClick="lkbExcluirSelecionados_Click">Excluir Selecionados</asp:LinkButton>
                    </div>
                </div>

                <div class="table-responsive" style="width: 100%; height: 300; overflow: auto;">
                    <asp:GridView ID="dtgFilhotes" Width="100%" runat="server" CssClass="table table-striped table-bordered table-hover table-padrao"
                        AutoGenerateColumns="False" OnRowDataBound="dtgFilhotes_RowDataBound" OnRowCreated="dtgFilhotes_RowCreated">
                        <PagerStyle HorizontalAlign="Center" CssClass="PagerStyle" />
                        <HeaderStyle CssClass="bg-primary" HorizontalAlign="Left" />
                        <Columns>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                <%-- <HeaderTemplate>
                                                        <asp:CheckBox ID="chkCabecalho" runat="server" />
                                                    </HeaderTemplate>--%>
                                <ItemTemplate>

                                    <asp:CheckBox ID="chkItem" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="identificadorRaca" HeaderText="identificadorRaca" />
                            <asp:BoundField DataField="identificadorVariedade" HeaderText="identificadorVariedade" />
                            <asp:BoundField DataField="identificadorCor" HeaderText="identificadorCor" />
                            <asp:BoundField DataField="Número" HeaderText="Número" />
                            <asp:BoundField DataField="Nome" HeaderText="Nome" />
                            <asp:BoundField DataField="Microchip" HeaderText="Microchip" />
                            <asp:BoundField DataField="Sexo" HeaderText="Sexo" />
                            <asp:BoundField DataField="Raça" HeaderText="Raça" />
                            <asp:BoundField DataField="Variedade" HeaderText="Variedade" />
                            <asp:BoundField DataField="Cor" HeaderText="Cor" />
                        </Columns>
                        <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" FirstPageText="Primeira"
                            LastPageText="Última" />
                    </asp:GridView>
                </div>
                <div class="col-md-3 pr-1">
                    <div class="row">
                        <asp:Button CssClass="btn btn-success btn-round" ID="btnIncluirNinhada" runat="server" Text="Registrar Ninhada" OnClick="btnIncluirNinhada_Click"></asp:Button>

                    </div>
                </div>


            </div>
        </div>
    </div>


    <div class="card" id="divResumo" runat="server">

        <div class="container-fluid box">

            <div class="card-header">
                <h5 class="card-title">
                    <asp:Label ID="lblTopo" runat="server" Text="Resumo do Pedido"></asp:Label></h5>
            </div>
            <div class="card-body">
                <div class="row">
                    <div class="col-md-3 pr-1">
                        <div class="card-header">
                            <h6 class="card-title">Valor por Item</h6>
                            <asp:Label ID="lblValorPorItem" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="col-md-3 pr-1">

                        <div class="card-header">
                            <h6 class="card-title">Quantidade de Filhotes</h6>
                            <asp:Label ID="lblQtdeFilhotes" runat="server"></asp:Label>
                        </div>


                    </div>

                    <div class="col-md-4 pr-1">
                        <div class="card-header">
                            <h6 class="card-title">Valor Total (sem descontos)</h6>
                            <asp:Label ID="lblVlrSemDesconto" runat="server"></asp:Label>
                        </div>
                    </div>

                </div>


                <div class="row">

                    <div class="col-md-3 pr-1">

                        <div class="card-header">
                            <h6 class="card-title">Desconto por Associados</h6>
                            <asp:Label ID="lblDescontoAssociados" runat="server"></asp:Label>
                        </div>


                    </div>

                    <div class="col-md-3 pr-1">

                        <div class="card-header">
                            <h6 class="card-title">Desconto por Raça</h6>
                            <asp:Label ID="lblDescontoRaca" runat="server"></asp:Label>
                        </div>


                    </div>

                    <div class="col-md-4 pr-1">
                        <div class="card-header">
                            <h6 class="card-title">Valor a Pagar</h6>
                            <b>
                                <asp:Label ID="lblValorAPagar" runat="server"></asp:Label></b>
                        </div>
                    </div>

                </div>


            </div>


        </div>

        <div class="row">
            <div class="col-md-4 pr-2">
                <asp:Button CssClass="btn btn-success btn-round" ID="btnAdicionarAoCarrinho" runat="server" Text="Adicionar ao Carrinho" OnClick="btnAddCarrinho_Click"></asp:Button>
            </div>


        </div>


    </div>


    <div class="card" id="divItemAdicionado" runat="server" visible="false">
        <div class="card-body">
            <asp:Label ID="Label2" role="alert" Width="100%" runat="server" CssClass="alert alert-success"><center>Item adicionado ao carrinho!&nbsp;</center></asp:Label>
            <a href="./Carrinho.aspx"><i class="nc-icon nc-cart-simple"></i>&nbsp;&nbsp;Ir para o carrinho</a></label>
        </div>
    </div>


    <div id="divComprovante" runat="server">
        <div class="card">
            <div class="card-header">
                <h5 class="card-title">Comprovante de Pagamento</h5>
            </div>
            <div class="row">
                <div class="col-md">
                    <asp:Label ID="Label1" role="alert" Width="100%" runat="server" CssClass="alert alert-success"><center>Pagamento realizado com sucesso!</center></asp:Label>
                </div>
            </div>
            <div class="card-header">
                <h6 class="card-title">Resumo do Pedido</h6>
            </div>
            <div class="card-body">
                <div class="row">
                    <div class="col-md-4 pr-1">
                        <div class="card-header">
                            <h6 class="card-title">Valor por Item</h6>
                            <asp:Label ID="lblVlrPorItemComprovante" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="col-md-3 pr-1">

                        <div class="card-header">
                            <h6 class="card-title">Quantidade de Filhotes</h6>
                            <asp:Label ID="lblQtdeFilhoteComprovante" runat="server"></asp:Label>
                        </div>


                    </div>

                    <div class="col-md-4 pr-1">
                        <div class="card-header">
                            <h6 class="card-title">Valor Total (sem descontos)</h6>
                            <asp:Label ID="lblVlrTotalSemDescontosComprovante" runat="server"></asp:Label>
                        </div>
                    </div>

                </div>


                <div class="row">

                    <div class="col-md-4 pr-1">

                        <div class="card-header">
                            <h6 class="card-title">Desconto por Associados</h6>
                            <asp:Label ID="lblDescontoAssociadosComprovante" runat="server"></asp:Label>
                        </div>


                    </div>

                    <div class="col-md-3 pr-1">

                        <div class="card-header">
                            <h6 class="card-title">Desconto por Raça</h6>
                            <asp:Label ID="lblDescontoRacaComprovante" runat="server"></asp:Label>
                        </div>


                    </div>

                    <div class="col-md-4 pr-1">
                        <div class="card-header">
                            <h6 class="card-title">Valor a Pagar</h6>
                            <b>
                                <asp:Label ID="lblVlrPagarComprovante" runat="server"></asp:Label></b>
                        </div>
                    </div>

                </div>
                <div class="row">
                    <div class="card-header">
                        <h6 class="card-title">Dados de Pagamento</h6>
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






                <br />





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


