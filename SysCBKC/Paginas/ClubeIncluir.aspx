<%@ Page Language="C#" MasterPageFile="~/Paginas/MasterPage.master" AutoEventWireup="true" CodeFile="ClubeIncluir.aspx.cs" Inherits="Paginas_ClubeIncluir" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" runat="Server">
      <div class="card">

           <div class="container-fluid box">
                  <div class="card-header">
                <h5 class="card-title">Inclusão de Clubes</h5>
              </div>
         <div class="row">
                            <div class="col-md">
                                <asp:Label ID="lblMensagem" role="alert" Width="100%" runat="server" CssClass="alert alert-danger"></asp:Label>
                            </div>
                        </div>
                                 <div class="card-body">
                                      <div class="row">
                            <div class="form-group col-md-6">
                                <label for="txtNomeTipo">Nome do Clube*</label>
                                <asp:TextBox ID="txtNomClube" CssClass="form-control" style='text-transform:uppercase' runat="server" MaxLength="100"></asp:TextBox>
                            </div>
                            
                            <div class="form-group col-md-3">
                                <label><asp:Label ID="lblNumCep" runat="server" Text="CEP"></asp:Label></label>
                                <asp:TextBox onkeypress='Formatar(this, "#####-###")' ID="txtNumCep"
                                    class="form-control" type="text" MaxLength="9" size="12" name="txtNumCep" runat="server"></asp:TextBox>
                            </div>
                                        
                        </div>
                        <div class="row">
                            <div class="form-group col-md-6">
                                <label><asp:Label ID="lblDscEndereco" runat="server" Text="Endereço*"></asp:Label></label>
                                <asp:TextBox ID="txtDscEndereco" CssClass="form-control" style='text-transform:uppercase' runat="server" MaxLength="100"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-md-6">
                                <label><asp:Label ID="lblDscBairro" runat="server" Text="Bairro*"></asp:Label></label>
                                <asp:TextBox ID="txtDscBairro" CssClass="form-control"  style='text-transform:uppercase' runat="server" MaxLength="25"></asp:TextBox>
                            </div>

                            <div class="form-group col-md-6">
                                <label><asp:Label ID="lblNomCidade" runat="server" Text="Cidade*"></asp:Label></label>
                                <asp:DropDownList ID="drpCidade" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-md-3">
                                <label><asp:Label ID="lblIsnEstado" runat="server" Text="Estado*"></asp:Label></label>
                                <asp:DropDownList ID="drpIsnEstado" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                                 </div>
                                  <div class="form-group col-md-3">
                                <label><asp:Label ID="lblRegiao" runat="server" Text="Região*"></asp:Label></label>
                                <asp:DropDownList ID="drpRegiao" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-md-3">
                                <label><asp:Label ID="lblNumFone1" runat="server" Text="Telefone Principal*"></asp:Label></label>
                                <input id="txtNumFone1" class="form-control" type="text" maxlength="15"
                                    size="12" name="txtNumFone1" runat="server" />
                            </div>
                            <div class="form-group col-md-3">
                                <label><asp:Label ID="lblNumFone2" runat="server" Text="Telefone Adicional"></asp:Label></label>
                                <input id="txtNumFone2" class="form-control" type="text" maxlength="15"
                                    size="12" name="txtNumFone2" runat="server" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-md-6">
                                <label><asp:Label ID="lblDscEmail" runat="server" Text="E-mail*"></asp:Label></label>
                                <asp:TextBox ID="txtDscEmail" CssClass="form-control" style='text-transform:uppercase' runat="server" MaxLength="60"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label><asp:Label ID="lblDscEmailAdicional" runat="server" Text="Site"></asp:Label></label>
                                <asp:TextBox ID="txtDscSite" CssClass="form-control" style='text-transform:uppercase' runat="server" MaxLength="60"></asp:TextBox>
                            </div>
                        </div>
                                     <div class="row">
                                          <div class="form-group col-md">
                        <asp:CheckBox ID="ckbAdesao" runat="server"></asp:CheckBox>
                        Aderiu ao Sistema
                    </div>
                                     </div>
                         
                    <div>
                        <asp:Button ID="btnSalvar" CssClass=" btn btn-success btn-round" runat="server" Text="Salvar" OnClick="btnSalvar_Click"></asp:Button>
                        <asp:Button ID="btnVoltar" CssClass="btn btn-danger btn-round" runat="server" Text="Voltar" OnClick="btnVoltar_Click"></asp:Button>
                    </div>
             
                                     </div>
                       
            </div>
     
</div>

             
    </asp:Content>
       