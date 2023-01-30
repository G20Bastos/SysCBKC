<%@ Page Language="C#" MasterPageFile="~/Paginas/MasterPage.master" AutoEventWireup="true" CodeFile="PrecoIncluir.aspx.cs" Inherits="Paginas_PrecoIncluir" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" runat="Server">
      <div class="card">
           <div class="container-fluid box">
                       <div class="card-header">
                <h5 class="card-title">Inclusão de Preços</h5>
              </div>
         <div class="row">
                            <div class="col-md">
                                <asp:Label ID="lblMensagem" role="alert" Width="100%" runat="server" CssClass="alert alert-danger"></asp:Label>
                            </div>
                        </div>
               <div class="card-body">
                     <div class="row">
                            <div class="form-group col-6">
                                <label>Valor do Serviço *</label>
                                <asp:TextBox ID="txtValor" CssClass="form-control" MaxLength="200" runat="server"></asp:TextBox>
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

        
       
