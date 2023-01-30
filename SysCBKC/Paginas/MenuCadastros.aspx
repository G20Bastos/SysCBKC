<%@ Page Language="C#" MasterPageFile="~/Paginas/MasterPage.master" AutoEventWireup="true" CodeFile="MenuCadastros.aspx.cs" Inherits="Paginas_MenuPedrigree" %>



<asp:Content ID="Conteudo" ContentPlaceHolderID="Conteudo" Runat="Server">
    
        <div class="col-md-12">
           

        <div class="card">
            <div class="card-header">
                <h5 class="card-title">Cadastros</h5>
              </div>
            <div class="card-body">
                <div class="row">
                     <div class="col-md-4 pr-1">
                         <asp:LinkButton ID="lkbRaca" runat="server" OnClick="lkbRaca_Click">Raças</asp:LinkButton>
                         </div>
                    <div class="col-md-4 px-1">
                        <asp:LinkButton ID="lkbVariedade" runat="server" OnClick="lkbVariedade_Click">Variedade de Raças</asp:LinkButton>
                         </div>
                     <div class="col-md-4 pl-1">
                        <asp:LinkButton ID="lkbCores" runat="server" OnClick="lkbCores_Click">Cores das Raças</asp:LinkButton>
                         </div>
                    
                      
                </div>
                      
                <div class="row">
                     <div class="col-md-4 pr-1">
                         <asp:LinkButton ID="lkbServicos" runat="server" OnClick="lkbServicos_Click">Serviços</asp:LinkButton>
                         </div>
                    <div class="col-md-4 px-1">
                        <asp:LinkButton ID="lkbPrecos" runat="server" OnClick="lkbPrecos_Click">Preços dos Serviços</asp:LinkButton>
                         </div>
                     <div class="col-md-4 pl-1">
                        <asp:LinkButton ID="lkbUsuarios" runat="server" OnClick="lkbUsuarios_Click">Usuários</asp:LinkButton>
                         </div>
                    
                    
                      
                </div>
                 <div class="row">
                     <div class="col-md-4 pr-1">
                        <asp:LinkButton ID="lkbClubes" runat="server" OnClick="lkbClubes_Click">Clubes</asp:LinkButton>
                         </div>
                         </div>
                    
                      
                </div>


            </div>
            </div>
          
      
    

       
</asp:Content>


