<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/MasterPage.master" AutoEventWireup="true" CodeFile="UsuarioIncluir.aspx.cs" Inherits="Paginas_UsuarioIncluir" %>




<asp:Content ID="Conteudo" ContentPlaceHolderID="Conteudo" Runat="Server">
    
    <div class="card">
         <div class="card-header">
                <h5 class="card-title">Inclusão de Usuários</h5>
              </div>
        <div class="row">
                            <div class="col-md">
                                <asp:Label ID="lblMensagem" role="alert" Width="100%" runat="server" CssClass="alert alert-danger"></asp:Label>
                            </div>
                        </div>
         <div class="card-body">
                
                    <div class="row">
                    <div class="col-md-5 pr-1">
                      <div class="form-group">
                        <label>Nome *</label>
                        <asp:TextBox ID="txtNome"
                                    class="form-control" type="text" MaxLength="100" size="12" name="txtNome" style='text-transform:uppercase' runat="server"></asp:TextBox>
                      </div>
                    </div>
                    <div class="col-md-3 px-1">
                      <div class="form-group">
                        <label>CPF *</label>
                        <asp:TextBox onkeypress='Formatar(this, "###.###.###-##")' ID="txtCpf"
                                    class="form-control" type="text" MaxLength="14" size="12" name="txtCpf" runat="server"></asp:TextBox>
                      </div>
                    </div>
                        <div class="col-md-4 pl-1">
                      <div class="form-group">
                        <label>RG *</label>
                        <asp:TextBox ID="txtRG"
                                    class="form-control" type="text" MaxLength="11" size="12" name="txtRG" runat="server"></asp:TextBox>
                      </div>
                    </div>
                  </div>
                  <div class="row">
                    <div class="col-md-5 pr-1">
                      <div class="form-group">
                        <label>E-mail *</label>
                        <asp:TextBox ID="txtEmail"
                                    class="form-control" type="email" MaxLength="100" size="12" name="txtEmail" style='text-transform:uppercase' runat="server"></asp:TextBox>
                      </div>
                    </div>
                    <div class="col-md-3 px-1">
                      <div class="form-group">
                        <label>Senha *</label>
                        <asp:TextBox ID="txtSenha"
                                    class="form-control" type="password" MaxLength="15" size="12" name="txtEmail"  runat="server"></asp:TextBox>
                      </div>
                    </div>
                    <div class="col-md-4 pl-1">
                      <div class="form-group">
                        <label>Confirmação de Senha *</label>
                        <asp:TextBox ID="txtConfirmacaoSenha"
                                    class="form-control" type="password" MaxLength="15" size="12" name="txtConfirmacaoSenha" runat="server"></asp:TextBox>
                      </div>
                    </div>
                  </div>
                  
                  <div class="row">
                    <div class="col-md-5 pr-1">
                      <div class="form-group">
                        <label>Endereço *</label>
                        <asp:TextBox ID="txtEndereco"
                                    class="form-control" type="text" MaxLength="100" size="12" name="txtEndereco" style='text-transform:uppercase' runat="server"></asp:TextBox>
                      </div>
                    </div>
                        <div class="col-md-3 px-1">
                      <div class="form-group">
                        <label>Complemento</label>
                        <asp:TextBox ID="txtComplemento"
                                    class="form-control" type="text" MaxLength="100" size="12" name="txtEndereco" style='text-transform:uppercase' runat="server"></asp:TextBox>
                      </div>
                    </div>

                      <div class="col-md-4 pl-1">
                                        <div class="form-group">
                                            <label>Bairro *</label>
                                            <asp:TextBox ID="txtBairro"
                                                class="form-control" type="text" MaxLength="50" size="12" name="txtBairro" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                  </div>
                  <div class="row">
                    <div class="col-md-4 pr-1">
                      <div class="form-group">
                        <label>Estado *</label>
                        <asp:DropDownList ID="drpIsnEstado" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpIsnEstado_SelectedIndexChanged">
                                </asp:DropDownList>
                      </div>
                    </div>
                    <div class="col-md-4 px-1">
                      <div class="form-group">
                        <label>Cidade *</label>
                        <asp:DropDownList ID="drpCidade" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpCidade_SelectedIndexChanged">
                                </asp:DropDownList>
                      </div>
                    </div>
                    <div class="col-md-4 pl-1">
                      <div class="form-group">
                        <label>CEP *</label>
                        <asp:TextBox onkeypress='Formatar(this, "#####-###")' ID="txtNumCep"
                                    class="form-control" type="text" MaxLength="9" size="12" name="txtNumCep" runat="server"></asp:TextBox>
                      </div>
                    </div>
                  </div>
                  <div class="row">
                     <div class="col-md-4 pr-1">
                      <div class="form-group">
                         <label>É sócio de algum clube?</label>
                        <asp:RadioButton 
                ID="radioBtnSocioSim" 
                runat="server" 
                Text="Sim" 
                AutoPostBack="true"
                OnCheckedChanged="radioBtnSocioSim_CheckedChanged"
                />
                <asp:RadioButton 
                ID="radioBtnSocioNao" 
                runat="server" 
                Text="Não" 
                Checked ="True"
                AutoPostBack="true"
                OnCheckedChanged="radioBtnSocioNao_CheckedChanged"
                />
                        <asp:DropDownList ID="drpIsnClube" CssClass="form-control" Visible="false" runat="server">
                                </asp:DropDownList>
                      </div>
                         
                    </div>
                      <div class="col-md-4 pr-1">
                      <div class="form-group">
                          <div class="form-group">
                        <label>Possui canil?</label>
                        <asp:RadioButton 
                ID="radioBtnSim" 
                runat="server" 
                Text="Sim" 
                AutoPostBack="true"
                OnCheckedChanged="radioBtnSim_CheckedChanged"
                />
                <asp:RadioButton 
                ID="radioBtnNao" 
                runat="server" 
                Text="Não" 
                Checked ="True"
                AutoPostBack="true"
                OnCheckedChanged="radioBtnNao_CheckedChanged"
                />

                              <div id="divNomeCanil" class="form-group" visible="false" runat="server">
                                  <asp:TextBox ID="txtNomeCanil" AutoPostBack="true"
                                    class="form-control" type="text" MaxLength="50" size="12" placeholder="Nome do Canil (obrigatório)" runat="server"></asp:TextBox>
                                  <asp:TextBox ID="txtCodCanil" AutoPostBack="true"
                                    class="form-control" type="text" MaxLength="10" size="10"  placeholder="Código do Canil (opcional)" runat="server"></asp:TextBox>
                      </div>
                              

                          </div>

                              </div>

                          
                          </div>
                      <div class="col-md-4 pr-1">
                      <div class="form-group">
                        <label>Tipo de Acesso *</label>
                        <asp:DropDownList ID="drpTipoAcesso" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                      </div>
                    </div>

                      </div>


                      <div class="row">
                        <div class="update ml-auto mr-auto">
                          <%--<button type="submit" class="btn btn-primary btn-round">Salvar Dados</button>--%>
                            <asp:Button CssClass="btn btn-success btn-round" ID="btnIncluir" runat="server" Text="Salvar Dados" OnClick="btnSalvar_Click"></asp:Button>
                            
                    </div>
                  </div>
            </div>
    </div>
       
        
              
              
        
    
</asp:Content>

