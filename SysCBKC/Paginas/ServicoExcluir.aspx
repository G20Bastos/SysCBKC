<%@ Page Language="C#" MasterPageFile="~/Paginas/MasterPage.master" AutoEventWireup="true" CodeFile="ServicoExcluir.aspx.cs" Inherits="Paginas_ServicoExcluir" %>

        
<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" runat="Server">
    <div class="container-fluid box">
            <div class="modal-dialog " role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">
                            <asp:Label ID="lblTitulo" runat="server" Text=""></asp:Label>
                        </h5>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="form-group col-md-8">
                                <asp:Label ID="lblMensagem" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="row">
                            <div class="col">
                                <asp:Button ID="btnNao" CssClass="btn btn-danger btn-round" runat="server" Text="Não" OnClick="btnNao_Click"></asp:Button>&nbsp;
						        <asp:Button ID="btnSim" CssClass="btn btn-success btn-round" runat="server" Text="Sim" OnClick="btnSim_Click"></asp:Button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Content>

   