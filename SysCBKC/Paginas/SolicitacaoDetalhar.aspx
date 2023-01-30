<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/MasterPage.master" AutoEventWireup="true" CodeFile="SolicitacaoDetalhar.aspx.cs" Inherits="Paginas_SolicitacaoDetalhar" %>




<asp:Content ID="Conteudo" ContentPlaceHolderID="Conteudo" Runat="Server">
    
   <div class="card" id="divDadosSolicitacao" runat="server">
            <div class="card-header">
                <h5 class="card-title">Detalhes da Solicitação</h5>
              </div>
            <div class="row">
                            <div class="col-md">
                                <asp:Label ID="lblMensagem" role="alert" Width="100%" runat="server" CssClass="alert alert-danger"></asp:Label>
                            </div>
                        </div>
            <div class="card-body">
                <div class="row">
                     <div class="col-md-3 pr-1">
                <div class="card-header">
                <h6 class="card-title">Número da Solicitação</h6>
                    <asp:label ID="lblNumSolicitacao" runat="server"></asp:label>
              </div>
                
                
                
            </div>
                    <div class="col-md-3 pr-1">
                      
                        <div class="card-header">
                <h6 class="card-title">Data da Solicitação</h6>
                            <asp:label ID="lblDatSolicitacao" runat="server"></asp:label>
              </div>
                        
                      
                    </div>
                    <div class="col-md-3 pr-1">
                         <div class="card-header">
                <h6 class="card-title">Tipo de Solicitação</h6>
                            <asp:label ID="lblTipoSolicitacao" runat="server"></asp:label>
              </div>
                      </div>
                    
                      
                        <div class="col-md-3 pr-1">
                         <div class="card-header">
                <h6 class="card-title">Usuário Solicitante</h6>
                            <asp:label ID="lblUsuariosolicitante" runat="server"></asp:label>
              </div>
                      </div>
                      
                   
          
        </div>
               <div class="row">
                    <div class="col-md-3 pr-1">
                <div class="card-header">
                <h6 class="card-title">Status</h6>
                    <asp:label ID="lblStatusSolicitacao" runat="server"></asp:label>
              </div>
                
                
                
            </div>
                   <div class="col-md-3 pr-1">
                      
                        <div class="card-header">
                <h6 class="card-title">Pagamento</h6>
                            <asp:label ID="lblStatusPagamento" runat="server"></asp:label>
              </div>
                        
                      
                    </div>

                   <div class="col-md pr-1">
                      
                        <div class="card-header">
                <h6 class="card-title">Observações</h6>
                            <asp:label ID="lblObservacoes" runat="server"></asp:label>
              </div>
                        
                      
                    </div>

               </div> 
                
               
                <div class="row">
                     <div class="col-md-12" id="divDadosNinhada" runat="server">
                         <div class="card-header">
                     <div class="row">
                    <h5 class="card-title">Dados do Registro de Mapa de Ninhada</h5>
                </div>
                    </div>
                    <div class="row">
                           <div class="col-md-2 pr-1">
                <div class="card-header">
                <h6 class="card-title">Padreador</h6>
                    <asp:label ID="lblNomePadreador" runat="server" Text="Zeus"></asp:label>
              </div>
            </div>
                         <div class="col-md-3 pr-1">
                <div class="card-header">
                <h6 class="card-title">RG do Padreador</h6>
                    <asp:label ID="lblRgPadreador" runat="server"></asp:label>
              </div>
            </div>
                           <div class="col-md-2 pr-1">
                <div class="card-header">
                <h6 class="card-title">Matriz</h6>
                    <asp:label ID="lblNomeMatriz" runat="server"></asp:label>
              </div>
            </div>
                          <div class="col-md-3 pr-1">
                <div class="card-header">
                <h6 class="card-title">RG da Matriz</h6>
                    <asp:label ID="lblRgMatriz" runat="server"></asp:label>
              </div>
            </div>

                          <div class="col-md-2 pr-1">
                <div class="card-header">
                <h6 class="card-title">Data de Nascimento</h6>
                    <asp:label ID="lblDataNascimento" runat="server"></asp:label>
              </div>
            </div>

                    </div>
                         <br />
                          <div class="row">

                              <div class="col-md-12 pr-1">
                             
                             <asp:GridView ID="dtgDadosNinhada" Width="100%" runat="server" AllowPaging="True" CssClass="table table-striped table-bordered table-hover table-padrao" OnDataBound="dtgDadosNinhada_DataBound">
                    <PagerStyle HorizontalAlign="Center" CssClass="PagerStyle" />
                    <HeaderStyle CssClass="bg-primary" HorizontalAlign="Left" />
                    <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" FirstPageText="Primeira"
                        LastPageText="Última" PreviousPageText="Anterior"
                            NextPageText="Próxima" />
                </asp:GridView>
                         
                         </div>
                              </div>
                         
                        

                </div>
                </div>

                <div class="row">
                    <div class="col-md-12" id="divDadosTransferencia" runat="server">
                        <div class="card-header">
                     <div class="row">
                    <h5 class="card-title">Dados de Transferência de Propriedade</h5>
                </div>
                    </div>

                </div>
                </div>
                <div class="row">
                     <div class="col-md pr-1">
                <div class="card-header">
                    <asp:label ID="lblResumoTransferencia" runat="server"></asp:label>
              </div>
            </div>
                </div>
               
                
                <div class="row">
                    <div class="col-md pr-1">
                      
                        <div class="card-header">
                <h6 class="card-title">Anexos</h6>
                            <asp:LinkButton ID="lkbVisualizar" runat="server" Text="Visualizar Anexos" OnClick="lkbVisualizar_Click"></asp:LinkButton>
              </div>
                        
                      
                    </div>
                </div>
                <br />

                <div class="row">
                     <div class="card-header">
                            <asp:label ID="lblTituloCorrecaoInconsistencias" runat="server"><h5 class="card-title">Correção de Inconsistências</h5></asp:label>
              </div>
                    
        </div>

                <div class="row">
                            <div class="col-md">
                                <asp:Label ID="lblMsgCorrecaoInconsistencia" role="alert" Width="100%" runat="server" CssClass="alert alert-danger"></asp:Label>
                            </div>
                        </div>

                 <div id="divCorrecaoNinhada" runat="server" visible="false">
                <div class="row">
                    <div class="col-md-3 pr-1">
                    
                        <label>Nome do Padreador (macho) *</label>
                        <input ID="txtNomePadreador"
                                    class="form-control" type="text" MaxLength="100" size="12" name="txtNome" runat="server"></input>
                      
                    </div>
                <div class="col-md-3 px-1">
                     
                        <label>RG do Padreador *</label>
                        <input ID="txtRGPadreador"
                                    class="form-control" type="text" MaxLength="100" size="12" name="txtNome" runat="server"></input>
                     
                    </div>

                    <div class="col-md-2 px-1">
                      
                        <label>Data de Nascimento *</label>
                        <input class="form-control" onkeypress='Formatar(this, "##/##/####")' id="txtDatNascimento"
                                                    type="text" maxlength="10" autocomplete="off" name="txtDatNascimento" runat="server" />
                                                <script type="text/javascript" src="../Scripts/Utils/DatepickerUtils.js">
                                                    DatepickerUtils.createDatepicker($('#txtDatNascimento'))
                                                </script>
                                                <a href="javascript:void(0)" id="calDatIni" runat="server"></a>
                     
                    </div>
                
                  </div>
            <div class="row">
                <div class="col-md-3 pr-1">
                        <label>Nome da Matriz (fêmea) *</label>
                        <input ID="txtNomeMatriz"
                                    class="form-control" type="text" MaxLength="100" size="12" name="txtNome" runat="server"></input>
                      
                    </div>
                <div class="col-md-3 px-1">
                      
                        <label>RG da Matriz *</label>
                        <input ID="txtRGMatriz"
                                    class="form-control" type="text" MaxLength="100" size="12" name="txtNome" runat="server"></input>
                     
                    </div>

                

               
            </div>
              
                
            </div>
            


                <div id="divCorrecaoTransferencia" runat="server" visible="false">

                    <div id="divDadosProprietarios" runat="server">
            
            
            <div class="card-header">
                <h6 class="card-title">Proprietário Origem</h6>
              </div>
            <div class="card-body">
                <div class="row">
                     <div class="col-md-4 pr-1">
                <label>Nome do Proprietário (origem) *</label>
                
                
                <asp:TextBox ID="txtNomPropietarioOrigem" CssClass="form-control" MaxLength="200" runat="server"></asp:TextBox>
            </div>
                    <div class="col-md-2 pr-1">
                      
                        <label>CPF *</label>
                        <asp:TextBox onkeypress='Formatar(this, "###.###.###-##")' ID="txtCpf"
                                     CssClass="form-control" type="text" MaxLength="14" size="12" name="txtCpf" runat="server"></asp:TextBox>
                      
                    </div>

                    <div class="col-md-4 pr-1">
                        <label>E-mail *</label>
                        <asp:TextBox ID="txtEmailOrigem"
                                    class="form-control" type="text" MaxLength="100" size="12" name="txtEndereco" runat="server"></asp:TextBox>
                      </div>
                    
                    <div class="col-md pl-1">
                      
                        <label>CEP *</label>
                        <asp:TextBox onkeypress='Formatar(this, "#####-###")' ID="txtNumCepOrigem"
                                    class="form-control" type="text" MaxLength="9" size="12" name="txtNumCep" runat="server"></asp:TextBox>
                      
                    </div>
          
        </div>
                <div class="row">
                    <div class="col-md-4 pr-1">
                        <label>Endereço *</label>
                        <asp:TextBox ID="txtEndereco"
                                    class="form-control" type="text" MaxLength="100" size="12" name="txtEndereco" runat="server"></asp:TextBox>
                      </div>
                    <div class="col-md-6 pr-1">
                        <label>Complemento</label>
                        <asp:TextBox ID="txtComplementoOrigem"
                                    class="form-control" type="text" MaxLength="100" size="12" name="txtEndereco" runat="server"></asp:TextBox>
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
                
                
                <asp:TextBox ID="txtNomPropDestino" CssClass="form-control" MaxLength="200" runat="server"></asp:TextBox>
            </div>
                    <div class="col-md-2 pr-1">
                      
                        <label>CPF *</label>
                        <asp:TextBox onkeypress='Formatar(this, "###.###.###-##")' ID="txtCpfDestino"
                                    class="form-control" type="text" MaxLength="14" size="12" name="txtCpf" runat="server"></asp:TextBox>
                      
                    </div>

                        <div class="col-md-4 pr-1">
                        <label>E-mail *</label>
                        <asp:TextBox ID="txtEmailDestino"
                                    class="form-control" type="text" MaxLength="100" size="12" name="txtEndereco" runat="server"></asp:TextBox>
                      </div>
                   
                    <div class="col-md pl-1">
                      
                        <label>CEP *</label>
                        <asp:TextBox onkeypress='Formatar(this, "#####-###")' ID="txtNumCepDestino"
                                    class="form-control" type="text" MaxLength="9" size="12" name="txtNumCep" runat="server"></asp:TextBox>
                      
                    </div>
          
        </div>
                <div class="row">
                     <div class="col-md-4 pr-1">
                        <label>Endereço *</label>
                        <asp:TextBox ID="txtEnderecoDestino"
                                    class="form-control" type="text" MaxLength="100" size="12" name="txtEndereco" runat="server"></asp:TextBox>
                      </div>
                    <div class="col-md-6 pr-1">
                        <label>Complemento</label>
                        <asp:TextBox ID="txtComplementoDestino"
                                    class="form-control" type="text" MaxLength="100" size="12" name="txtEndereco" runat="server"></asp:TextBox>
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
                
                
                <asp:TextBox ID="txtNomCao" CssClass="form-control" MaxLength="200" runat="server"></asp:TextBox>
            </div>
                    <div class="col-md-2 pr-1">
                      
                        <label>RG*</label>
                        <asp:TextBox ID="txtRgCao"
                                    class="form-control" type="text" MaxLength="15" size="12" name="txtRG" runat="server"></asp:TextBox>
                     
                    </div>
                        
                    
          
        </div>
                
                
            </div>
          
        </div>


                </div>

                 <div class="row">
                     <div class="col-md-4 pr-1">
                <asp:label ID="lblObs" runat="server"><label>Observações</label></asp:label>
                
                
                <asp:TextBox ID="txtObs" CssClass="form-control" MaxLength="200" runat="server" Height="100px" TextMode="MultiLine"></asp:TextBox>
            </div>
            </div>
                <br />
                <div class="row">
                        <div class="col-md-4 pr-1">
                             <asp:label ID="lblDeclaracao" runat="server"><label>Declaração de Responsabilidade -  <a href="http://localhost:50948/Documentos/Termo de Transferência de Propriedade.pdf" target="_blank">clique aqui para baixar</a></label></asp:label>  
                                <asp:FileUpload ID="anexoDeclaracao" CssClass="form-control-file" runat="server" />
                            </div>
                        <div class="col-md pl-1" visible="false">
                            <asp:Label ID="lblAnexar" runat="server"><label>Anexar arquivo</label></asp:Label>
                                <asp:FileUpload ID="anexoComum" CssClass="form-control-file" runat="server" />
                            </div>
                    
                   
                    
        </div>
               
              <br />
                <div class="row">
                            <div class="col-md px-1">
                        <asp:Button CssClass="btn btn-success btn-round" ID="btnAceitar" runat="server" Text="Aceitar" OnClick="btnAceitar_Click" ></asp:Button>
                        <asp:Button CssClass="btn btn-danger btn-round" ID="btnRecusar" runat="server" Text="Recusar" OnClick="btnRecusar_Click" ></asp:Button>
                        <asp:Button CssClass="btn btn-success btn-round" ID="btnSolicitar" runat="server" Text="Solicitar Ajustes" OnClick="btnSolicitar_Click"></asp:Button>
                      </div>
                        </div>
                
                
              
          
            </div>
          
        </div>
              
              
        
    
</asp:Content>

