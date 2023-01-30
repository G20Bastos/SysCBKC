<%@ Page Language="C#" MasterPageFile="~/Paginas/MasterPage.master" AutoEventWireup="true" CodeFile="PrecoEditar.aspx.cs" Inherits="Paginas_PrecoEditar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" runat="Server">
     <div class="card">
         <div class="container-fluid box">
              <div class="card-header">
                <h5 class="card-title"><asp:Label ID="lblTopo" runat="server" Text=""></asp:Label></h5>
              </div>
         <div class="row">
                            <div class="col-md">
                                <asp:Label ID="lblMensagem" role="alert" Width="100%" runat="server" CssClass="alert alert-danger"></asp:Label>
                            </div>
                        </div>
             <div class="card-body">
                 <div class="row">
                            <div class="form-group col-6">
                                <label>Valor do Serviço</label>
                                <asp:TextBox ID="txtValorServico" CssClass="form-control" MaxLength="200" runat="server"></asp:TextBox>
                            </div>
            <div class="form-group col-3">
                                 <label>Período de Vigência</label>
                                        <div class="row">
                                            <div class="form-group col-6">
                                                <input class="form-control" onkeypress='Formatar(this, "##/##/####")' id="txtDatIni"
                                                    type="text" maxlength="10" autocomplete="off" name="txtDatInicial" runat="server" />
                                                <script type="text/javascript">
                                                    DatepickerUtils.createDatepicker($('#txtDatIni'))
                                                </script>
                                                <a href="javascript:void(0)" id="calDatIni" runat="server"></a>
                                            </div>
                                            <div class="form-group col-6">
                                                <input class="form-control" onkeypress='Formatar(this, "##/##/####")' id="txtDatFim"
                                                    type="text" maxlength="10" autocomplete="off" name="txtDatFinal" runat="server" />
                                                <a href="javascript:void(0)" id="calDatFim" runat="server"></a>
                                                <script type="text/javascript">
                                                    DatepickerUtils.createDatepicker($('#txtDatFim'))
                                                </script>
                                            </div>
                                        </div>
                            </div>
                        </div>
        <div>
            <asp:Button CssClass="btn btn-success btn-round" ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"></asp:Button>
            <asp:Button CssClass="btn btn-danger btn-round" ID="Button1" runat="server" Text="Voltar" OnClick="btnVoltar_Click"></asp:Button>
            
        </div>
                 </div>
        
        </div>
         </div>
    
    </asp:Content>
        
   