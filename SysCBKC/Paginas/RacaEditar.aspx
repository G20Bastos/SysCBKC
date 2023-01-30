<%@ Page Language="C#" MasterPageFile="~/Paginas/MasterPage.master" AutoEventWireup="true" CodeFile="RacaEditar.aspx.cs" Inherits="Paginas_RacaEditar" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" runat="Server">

    <div class="container-fluid box">

                        <h5 class="modal-title">
                            <b><asp:Label ID="lblTopo" runat="server" Text="" Font-Bold="true"></asp:Label>
                            </b>
                        </h5>
                    
         <div class="row">
                            <div class="col-md">
                                <asp:Label ID="lblMensagem" role="alert" Width="100%" runat="server" CssClass="alert alert-danger"></asp:Label>
                            </div>
                        </div>
        <div class="row">
                            <div class="form-group col-6">
                                <label>Descrição da Raça</label>
                                <asp:TextBox ID="txtRaca" CssClass="form-control" MaxLength="200" runat="server"></asp:TextBox>
                            </div>
            
                        </div>
        <div class="row">
            <div class="col-md">
                                <asp:CheckBox ID="ckbDesconto" runat="server"></asp:CheckBox>
                              Permite Desconto
                            </div>
        </div>
        <div>
            <asp:Button CssClass="btn btn-success btn-round" ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"></asp:Button>
            <asp:Button CssClass="btn btn-danger btn-round" ID="Button1" runat="server" Text="Voltar" OnClick="btnVoltar_Click"></asp:Button>
            
        </div>
        </div>
   

    </asp:Content>
