<%@ Page Language="C#" MasterPageFile="~/Paginas/MasterPage.master" AutoEventWireup="true" CodeFile="CupomDescontoEditar.aspx.cs" Inherits="Paginas_CupomDescontoEditar" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" runat="Server">
    <div class="card">
        <div class="container-fluid box">
            <div class="card-header">
                <h5 class="card-title">Edição de Cupons de Desconto</h5>
            </div>
            <div class="row">
                <div class="col-md">
                    <asp:Label ID="lblMensagem" role="alert" Width="100%" runat="server" CssClass="alert alert-danger"></asp:Label>
                </div>
            </div>
            <div class="card-body">
                <div class="row">
                     <div class="col-md-4 pr-1">
                        <label>Código do Cupom</label>
                        <asp:TextBox ID="txtCodCupom" disable="true" CssClass="form-control" MaxLength="20" runat="server"></asp:TextBox>
                    </div>

                      <div class="col-md-4 pr-1">
                       <label>
                            <asp:Label ID="lblStatusCupom" runat="server" Text="Status"></asp:Label></label>
                        <asp:DropDownList ID="drpStatusCupom" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                        </div>

                    <div class="col-md-4 pr-1">
                        <label>Data de Inclusão</label>
                        <div class="row">
                            <div class="col-md-6 pr-1">
                                <input class="form-control" onkeypress='Formatar(this, "##/##/####")' id="txtDataInclusao"
                                    type="text" enabled="false" maxlength="10" autocomplete="off" name="txtDataInclusao" runat="server" />
                            </div>
                        </div>
                    </div>

                  
                </div>
                <div class="row">
                    <div class="col-md-8 pr-1">
                       <label>
                            <asp:Label ID="lblClient" runat="server" Text="Cliente"></asp:Label></label>
                        <asp:DropDownList ID="drpCliente" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                        </div>

                    <div class="col-md-2 pr-1">
                    <label>Valor</label>
                        <asp:TextBox ID="txtValor"  CssClass="form-control" MaxLength="20" runat="server"></asp:TextBox>
                        </div>
                </div>
                <div>
                    <asp:Button CssClass="btn btn-success btn-round" ID="btnIncluir" runat="server" Text="Salvar" OnClick="btnSalvar_Click"></asp:Button>
                    <asp:Button CssClass="btn btn-danger btn-round" ID="btnVoltar" runat="server" Text="Voltar" OnClick="btnVoltar_Click"></asp:Button>

                </div>
            </div>


        </div>
    </div>


</asp:Content>
