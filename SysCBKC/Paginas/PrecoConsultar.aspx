<%@ Page Language="C#" MasterPageFile="~/Paginas/MasterPage.master" AutoEventWireup="true" CodeFile="PrecoConsultar.aspx.cs" Inherits="Paginas_PrecoConsultar" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" runat="Server">
    <div class="card">
<div class="container-fluid box">
         <div class="card-header">
                <h5 class="card-title">Preços</h5>
              </div>
        
        <div class="row">
            <div class="col-md" id="divMsg" runat="server">
                <asp:Label ID="lblMensagem" role="alert" Width="100%" runat="server" CssClass="alert alert-danger"></asp:Label>
            </div>
        </div>
         <div class="card-body">
            
        <div>
          
            <asp:Button CssClass="btn btn-primary btn-round" ID="btnIncluir" runat="server" Text="Incluir" OnClick="btnIncluir_Click"></asp:Button>
        </div>

        <div class="row">
            <div class="col">
                <asp:GridView ID="dtgRaca" Width="100%" runat="server" AllowPaging="True" CssClass="table table-striped table-bordered table-hover table-padrao" PageSize="30"
                    OnRowDataBound="dtgRaca_RowDataBound">
                    <PagerStyle HorizontalAlign="Center" CssClass="PagerStyle" />
                    <HeaderStyle CssClass="bg-primary" HorizontalAlign="Left" />
                    <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" FirstPageText="Primeira"
                        LastPageText="Última" />
                </asp:GridView>
            </div>
        </div>
             </div>
        
            </div>
    </div>
     
    
    </asp:Content>

       