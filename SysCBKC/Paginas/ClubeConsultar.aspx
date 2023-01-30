<%@ Page Language="C#" MasterPageFile="~/Paginas/MasterPage.master" AutoEventWireup="true" CodeFile="ClubeConsultar.aspx.cs" Inherits="Paginas_ClubeConsultar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" runat="Server">

    <div class="card">
        <div class="container-fluid box">
            <div class="card-header">
                <h5 class="card-title">Clubes</h5>
              </div>
         <div class="row">
            <div class="col-md" id="divMsg" runat="server">
                <asp:Label ID="lblMensagem" role="alert" Width="100%" runat="server" CssClass="alert alert-danger"></asp:Label>
            </div>
        </div>
            <div class="card-body">
                <div class="row">
             <div class="col-md-4 pr-1">
                <label>Nome do Clube</label>

     
                    <asp:TextBox ID="txtNomClube" CssClass="form-control" MaxLength="200" style='text-transform:uppercase' runat="server"></asp:TextBox>
      
                
                
            </div>
                    <div class="col-md-4 pr-1">
                                <label><asp:Label ID="lblEstado" runat="server" Text="Estado"></asp:Label></label>
                                <asp:DropDownList ID="drpEstado" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                    <div class="col-md-4 pr-1">
                                <label><asp:Label ID="lblRegiao" runat="server" Text="Região"></asp:Label></label>
                                <asp:DropDownList ID="drpRegiao" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
        </div>
        <div>
            <asp:Button CssClass="btn btn-primary btn-round" ID="btnConsultar" runat="server" Text="Consultar" OnClick="btnConsultar_Click"></asp:Button>
            <asp:Button CssClass="btn btn-primary btn-round" ID="btnIncluir" runat="server" Text="Incluir" OnClick="btnIncluir_Click"></asp:Button>
        </div>

        <div class="row">
            <div class="col">
                <asp:GridView ID="dtgClube" Width="100%" runat="server" AllowPaging="True" CssClass="table table-striped table-bordered table-hover table-padrao" PageSize="30"
                    OnRowDataBound="dtgClube_RowDataBound" OnPageIndexChanging="dtgClube_PageIndexChanging">
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


   

    

