<%@ Page Language="C#" MasterPageFile="~/Paginas/MasterPage.master" AutoEventWireup="true" CodeFile="UsuarioConsultar.aspx.cs" Inherits="Paginas_UsuarioConsultar" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" runat="Server">

     
    <div class="card">
            <div class="container-fluid box">
            <div class="card-header">
                <h5 class="card-title">Usuários</h5>
              </div>
        
        <div class="row">
            <div class="col-md" id="divMsg" runat="server">
                <asp:Label ID="lblMensagem" role="alert" Width="100%" runat="server" CssClass="alert alert-danger"></asp:Label>
            </div>
        </div>
                <div class="card-body">
                    <div class="row">
            <div class="col-md-4 pr-1">
                <label>Nome do Usuário</label>
                
                
                <asp:TextBox ID="txtNomPessoa" CssClass="form-control" MaxLength="200" style='text-transform:uppercase' runat="server"></asp:TextBox>
            </div>
                    <div class="form-group col-3">
                                <label><asp:Label ID="lblClube" runat="server" Text="Clube"></asp:Label></label>
                                <asp:DropDownList ID="drpClube" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
        </div>
        <div>
            <asp:Button CssClass="btn btn-primary btn-round" ID="btnConsultar" runat="server" Text="Consultar" OnClick="btnConsultar_Click"></asp:Button>
            <asp:Button CssClass="btn btn-primary btn-round"     ID="btnIncluir" runat="server" Text="Incluir" OnClick="btnIncluir_Click"></asp:Button>
        </div>

        <div class="row">
            <div class="col">
                <asp:GridView ID="dtgClube" Width="100%" runat="server" AllowPaging="True" CssClass="table table-striped table-bordered table-hover table-padrao" PageSize="30"
                    OnRowDataBound="dtgUsuario_RowDataBound" OnPageIndexChanging="dtgClube_PageIndexChanging">
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
        

    </div>

    </asp:Content>


       
 