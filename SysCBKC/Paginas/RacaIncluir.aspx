<%@ Page Language="C#" MasterPageFile="~/Paginas/MasterPage.master" AutoEventWireup="true" CodeFile="RacaIncluir.aspx.cs" Inherits="Paginas_RacaIncluir" %>



<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" runat="Server">
    <div class="card">
        <div class="container-fluid box">
            <div class="card-header">
                <h5 class="card-title">Inclusão de Raças</h5>
            </div>
            <div class="row">
                <div class="col-md">
                    <asp:Label ID="lblMensagem" role="alert" Width="100%" runat="server" CssClass="alert alert-danger"></asp:Label>
                </div>
            </div>
            <div class="card-body">
                <div class="row">
                    <div class="form-group col-3">
                        <label>Descrição da Raça *</label>
                        <asp:TextBox ID="txtRaca" CssClass="form-control" MaxLength="200" style='text-transform:uppercase'  runat="server"></asp:TextBox>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md">
                        <asp:CheckBox ID="ckbDesconto" runat="server"></asp:CheckBox>
                        Permite Desconto
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




