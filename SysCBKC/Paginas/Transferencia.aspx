<%@ Page Language="C#" MasterPageFile="~/Paginas/MasterPage.master" AutoEventWireup="true" CodeFile="Transferencia.aspx.cs" Inherits="Paginas_Transferencia" %>



<asp:Content ID="Conteudo" ContentPlaceHolderID="Conteudo" runat="Server">



    <div class="card" id="divDadosProprietarios" runat="server">
        <div class="card-header">
            <h5 class="card-title">Transferência de Propriedade</h5>
        </div>
        <div class="col-md-4 pr-1">
            <h6 class="card-title">Sou proprietário:</h6>
            <asp:RadioButton
                ID="radioBtnOrigem"
                runat="server"
                Text="&nbsp;Origem&nbsp;"
                Checked="True"
                AutoPostBack="true"
                OnCheckedChanged="radioBtnOrigem_CheckedChanged" />
            <asp:RadioButton
                ID="radioBtnDestino"
                runat="server"
                Text="&nbsp;Destino&nbsp;"
                Checked="False"
                AutoPostBack="true"
                OnCheckedChanged="radioBtnDestino_CheckedChanged" />
        </div>

        <div class="row">
            <div class="col-md">
                <asp:Label ID="lblMensagem" role="alert" Width="100%" runat="server" CssClass="alert alert-danger"></asp:Label>
            </div>
        </div>
        <div class="card-header">
            <h6 class="card-title">Proprietário Origem</h6>
        </div>
        <div class="card-body">
            <div class="row">
                <div class="col-md-4 pr-1">
                    <label>Nome do Proprietário (origem) *</label>


                    <asp:TextBox ID="txtNomPropietarioOrigem" CssClass="form-control" MaxLength="200" Style='text-transform: uppercase' runat="server"></asp:TextBox>
                </div>
                <div class="col-md-2 pr-1">

                    <label>CPF</label>
                    <asp:TextBox onkeypress='Formatar(this, "###.###.###-##")' ID="txtCpf"
                        CssClass="form-control" type="text" MaxLength="14" size="12" name="txtCpf" runat="server"></asp:TextBox>

                </div>

                <div class="col-md-4 pr-1">
                    <label>E-mail</label>
                    <asp:TextBox ID="txtEmailOrigem"
                        CssClass="form-control" type="text" MaxLength="100" size="12" name="txtEndereco" Style='text-transform: uppercase' runat="server"></asp:TextBox>
                </div>

                <div class="col-md pr-1">

                    <label>CEP</label>
                    <asp:TextBox onkeypress='Formatar(this, "#####-###")' ID="txtNumCepOrigem"
                        CssClass="form-control" type="text" MaxLength="9" size="12" name="txtNumCep" runat="server"></asp:TextBox>

                </div>

            </div>
            <div class="row">
                <div class="col-md-6 pr-1">
                    <label>Endereço</label>
                    <asp:TextBox ID="txtEndereco"
                        CssClass="form-control" type="text" MaxLength="100" size="12" name="txtEndereco" Style='text-transform: uppercase' runat="server"></asp:TextBox>
                </div>
                <div class="col-md pr-1">
                    <label>Complemento</label>
                    <asp:TextBox ID="txtComplementoOrigem"
                        CssClass="form-control" type="text" MaxLength="100" size="12" name="txtEndereco" Style='text-transform: uppercase' runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4 pr-1">
                    <label>Bairro</label>
                    <asp:TextBox ID="txtBairroOrigem"
                        CssClass="form-control" type="text" MaxLength="100" size="12" name="txtEndereco" Style='text-transform: uppercase' runat="server"></asp:TextBox>
                </div>
                <div class="col-md-2 pr-1">

                    
                        <label>Estado *</label>
                        <asp:DropDownList ID="drpEstadoOrigem" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpEstadoOrigem_SelectedIndexChanged">
                        </asp:DropDownList>
                    

                </div>
                <div class="col-md-4 pr-1">

                    
                        <label>Cidade *</label>
                        <asp:DropDownList ID="drpCidadeOrigem" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpCidadeOrigem_SelectedIndexChanged">
                        </asp:DropDownList>
                    

                </div>

                <div class="col-md pr-1">

                    
                        <label>País *</label>
                        <asp:DropDownList ID="drpPaisOrigem" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpPaisOrigem_SelectedIndexChanged">
                        </asp:DropDownList>
                    

                </div>
                
            </div>
    <div class="row">
                   <div class="card-header">
                <h6 class="card-title">Proprietário Destino</h6>
              </div>
               </div> 
                    <div class="row">
                     <div class="col-md-4 pr-1">
                <label>Nome do Proprietário (destino) *</label>
                
                
                <asp:TextBox ID="txtNomPropDestino" CssClass="form-control" MaxLength="200" style='text-transform:uppercase'  runat="server"></asp:TextBox>
            </div>
                    <div class="col-md-2 pr-1">
                      
                        <label>CPF *</label>
                        <asp:TextBox onkeypress='Formatar(this, "###.###.###-##")' ID="txtCpfDestino"
                                    CssClass="form-control" type="text" MaxLength="14" size="12" name="txtCpf" runat="server"></asp:TextBox>
                      
                    </div>

                        <div class="col-md-4 pr-1">
                        <label>E-mail *</label>
                        <asp:TextBox ID="txtEmailDestino"
                                    CssClass="form-control" type="text" MaxLength="100" size="12" name="txtEndereco" style='text-transform:uppercase'  runat="server"></asp:TextBox>
                      </div>
                   
                    <div class="col-md pr-1">
                      
                        <label>CEP *</label>
                        <asp:TextBox onkeypress='Formatar(this, "#####-###")' ID="txtNumCepDestino"
                                    CssClass="form-control" type="text" MaxLength="9" size="12" name="txtNumCep" runat="server"></asp:TextBox>
                      
                    </div>
          
        </div>
                <div class="row">
                     <div class="col-md-6 pr-1">
                        <label>Endereço *</label>
                        <asp:TextBox ID="txtEnderecoDestino"
                                    CssClass="form-control" type="text" MaxLength="100" size="12" name="txtEndereco" style='text-transform:uppercase'  runat="server"></asp:TextBox>
                      </div>
                    <div class="col-md-6 pr-1">
                        <label>Complemento</label>
                        <asp:TextBox ID="txtComplementoDestino"
                                    CssClass="form-control" type="text" MaxLength="100" size="12" name="txtEndereco" style='text-transform:uppercase'  runat="server"></asp:TextBox>
                      </div>
                </div>
            <div class="row">
                <div class="col-md-4 pr-1">
                    <label>Bairro</label>
                    <asp:TextBox ID="txtBairroDestino"
                        CssClass="form-control" type="text" MaxLength="100" size="12" name="txtEndereco" Style='text-transform: uppercase' runat="server"></asp:TextBox>
                </div>
                
                <div class="col-md-2 pr-1">

                    
                        <label>Estado *</label>
                        <asp:DropDownList ID="drpEstadoDestino" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpEstadoDestino_SelectedIndexChanged">
                        </asp:DropDownList>
                    

                </div>
                <div class="col-md-4 pr-1">

                    
                        <label>Cidade *</label>
                        <asp:DropDownList ID="drpCidadeDestino" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpCidadeDestino_SelectedIndexChanged">
                        </asp:DropDownList>
                    

                </div>

                <div class="col-md pr-1">

                    
                        <label>País *</label>
                        <asp:DropDownList ID="drpPaisDestino" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpPaisDestino_SelectedIndexChanged">
                        </asp:DropDownList>
                    

                </div>
            </div>
            <div class="row">
                  <div class="col-md-4 pr-1">
                <label>Nome do Co-Proprietário</label>
                
                
                <asp:TextBox ID="txtCoProprietario" CssClass="form-control" MaxLength="200" style='text-transform:uppercase'  runat="server"></asp:TextBox>
            </div>
                </div>
                 <div class="row">
                   <div class="card-header">
                <h6 class="card-title">Dados do Cão</h6>
              </div>
               </div> 
                    <div class="row">
                     <div class="col-md-4 pr-1">
                <label>Nome do Cão *</label>
                
                
                <asp:TextBox ID="txtNomCao" CssClass="form-control" MaxLength="200" style='text-transform:uppercase'  runat="server"></asp:TextBox>
            </div>
                    <div class="col-md-2 pr-1">
                      
                        <label>RG*</label>
                        <asp:TextBox ID="txtRgCao"
                                    class="form-control" type="text" MaxLength="15" size="12" name="txtRG" style='text-transform:uppercase'  runat="server"></asp:TextBox>
                     
                    </div>
                        
                    <div class="col-md-4 pl-1">
                                <label>Termo de Transferência (máximo 5MB) -  <asp:LinkButton ID="lkbTermoTransferencia" runat="server" OnClick="lkbTermoTransferencia_Click" Text="clique para baixar"></asp:LinkButton></label>
                                <asp:FileUpload ID="anexoDeclaracao" CssClass="form-control-file" runat="server" />
                            </div>
                        <div class="col-md pl-1" id="divAnexoComum" visible="false" runat="server">
                            <label>Anexar arquivo</label>
                                <asp:FileUpload ID="anexoComum" CssClass="form-control-file" runat="server" />
                            </div>
                    
          
        </div>
                <br />
                <div class="row">
                    <div class="col-md">
                                <asp:CheckBox ID="ckbTermos" runat="server"></asp:CheckBox>
                              <asp:LinkButton ID="lkbTermos" runat="server" OnClick="lkbTermos_Click" Text="Li e Concordo com os termos"></asp:LinkButton>
                            </div>
                    </div>

               
              <br />
                <div class="row">
                            <div class="col-md px-1">
                        <asp:Button CssClass="btn btn-success btn-round" ID="btnAvancar" runat="server" Text="Avançar" OnClick="btnAvancar_Click"></asp:Button>
                      </div>
                        </div>
                
                
              
          
            </div>
          
        </div>
    
    <div id="divDadosPagamentoTransferencia" runat="server">
        <div class="card">
            <div class="card-header">
                <h5 class="card-title">Dados do Pedido e Pagamento do Serviço de Transferência</h5>
            </div>
            <div class="row">
                <div class="col-md">
                    <asp:Label ID="lblMensagemPagamento" role="alert" Width="100%" runat="server" CssClass="alert alert-danger"></asp:Label>
                </div>
            </div>
            <div class="card-header">
                <h6 class="card-title">Resumo do Pedido</h6>
            </div>
            <div class="card-body">
                <div class="row">
                    <div class="col-md pr-1">
                        <asp:Label ID="lblResumo" runat="server"></asp:Label>

                    </div>



                </div>



                <br />
                <div class="row">
                    <div class="col-md px-1">
                        <asp:Button CssClass="btn btn-success btn-round" ID="btnAddCarrinho" runat="server" Text="Adicionar ao Carrinho" OnClick="btnAddCarrinho_Click"></asp:Button>
                        <asp:Button CssClass="btn btn-danger btn-round" ID="btnVoltar" runat="server" Text="Voltar" OnClick="btnVoltar_Click"></asp:Button>
                    </div>
                </div>




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
                    <div class="col-md pr-1">
                        <asp:Label ID="lblResumoPedidoPagamentoComprovante" runat="server"></asp:Label>

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

                    <div class="col-md pr-1">

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


