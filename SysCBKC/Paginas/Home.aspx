<%@ Page Language="C#" MasterPageFile="~/Paginas/MasterPage.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Paginas_Home" %>



<asp:Content ID="Conteudo" ContentPlaceHolderID="Conteudo" Runat="Server">
    
        <%--<div class="container-fluid box">--%>
    <div class="col-md-12">
        <div class="row">
            <div class="col-md-4 pl-1">
 <div class="card">
        <div class="card-header">

            <div class="col-md">
                      <div class="row">
                         
                           <br />
                      </div>
                <div class="row">
                    <h5><a href="./NinhadaRegistrar.aspx">
                        <center><i class="nc-icon nc-bullet-list-67"></i></center>

                        <center>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Mapas de Ninhadas</center>
                        <br />
                        </a></h5>
                </div>
                
                    </div>
               
              </div>
        <div class="card-body">
            </div>
        </div>
    </div>

    <div class="col-md-4 px-1">
 <div class="card">
        <div class="card-header">

            <div class="col-md">
                      <div class="row">
                         
                           <br />
                      </div>
                <div class="row">
                    <h5><a href="./Transferencia.aspx">
                        <center><i class="nc-icon nc-refresh-69"></i></center>
                      
                        <center>Transferência de Propriedade</center></a></h5>
                </div>
                
                    </div>
               
              </div>
        <div class="card-body">
            </div>
        </div>
    </div>

  <div class="col-md-4 pr-1">
 <div class="card">
        <div class="card-header">

            <div class="col-md">
                      <div class="row">
                         
                           <br />
                      </div>
                <div class="row">
                    <h5>
                        <center><asp:Label ID="lblQtdeSolicitacoes" runat="server"></asp:Label></center>
                      
                        <center>Solicitações Realizadas à CBKC</center>
                    </h5>
                </div>
                
                    </div>
               
              </div>
        <div class="card-body">
            </div>
        </div>
    </div>
        </div>
    </div>
    
   

        <div class="card">
            <div class="card-header">
                <h5 class="card-title">Solicitações</h5>
              </div>
            <div class="card-body">
                <div class="row">
                     <div class="col-md-4 pr-1">
                <label>Nome do Solicitante</label>
                
                
                <asp:TextBox ID="txtNomPessoa" CssClass="form-control" MaxLength="200" style='text-transform:uppercase'   runat="server"></asp:TextBox>
            </div>

                    
                    <div class="col-md-3 pr-1">
                                <label><asp:Label ID="Label2" runat="server" Text="Status do Pagamento"></asp:Label></label>
                                <asp:DropDownList ID="drpStatusPagamento" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                    <div class="col-md-3 pr-1">
                                <label><asp:Label ID="Label3" runat="server" Text="Status da Solicitação"></asp:Label></label>
                                <asp:DropDownList ID="drpStatusSolicitacao" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                    
          
        </div>
                <div class="row">
                     <div class="col-md-4 pr-1">
                                <label><asp:Label ID="Label1" runat="server" Text="Tipo de Solicitação"></asp:Label></label>
                                <asp:DropDownList ID="drpTipoSolicitacao" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>

                     <div class="col-md-4 pr-1">
                                        <label>Período</label>
                                        <div class="row">
                                            <div class="col-md-4 pr-1">
                                                <input class="form-control" onkeypress='Formatar(this, "##/##/####")' id="txtDatIni"
                                                    type="text" maxlength="10" autocomplete="off" name="txtDatInicial" runat="server" />
                                                <script type="text/javascript">
                                                    DatepickerUtils.createDatepicker($('#txtDatIni'))
                                                </script>
                                                <a href="javascript:void(0)" id="calDatIni" runat="server"></a>
                                            </div>
                                            <div class="col-md-4 pr-1">
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
                <div class="row">
                    <div class="col-md pr-1">
                        <asp:Button CssClass="btn btn-primary btn-round" ID="btnConsultar" runat="server" Text="Consultar" OnClick="btnConsultar_Click"></asp:Button>
                        </div>
                </div>
                <br />
                
                
                <asp:GridView ID="dtgSolicitacoes" Width="100%" runat="server" AllowPaging="True" CssClass="table table-striped table-bordered table-hover table-padrao" PageSize="20"
                    OnRowDataBound="dtgSolicitacoes_RowDataBound" OnPageIndexChanging="dtgSolicitacoes_PageIndexChanging">
                    <PagerStyle HorizontalAlign="Center" CssClass="PagerStyle" />
                    <HeaderStyle CssClass="bg-primary" HorizontalAlign="Left" />
                    <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" FirstPageText="Primeira"
                        LastPageText="Última" PreviousPageText="Anterior"
                            NextPageText="Próxima" />
                </asp:GridView>
          
            </div>
          
        </div>


    <%--</div>--%>
    
    
</asp:Content>


