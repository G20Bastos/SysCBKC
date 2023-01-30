<%@ Page Language="C#" MasterPageFile="~/Paginas/MasterPage.master" AutoEventWireup="true" CodeFile="MenuParametros.aspx.cs" Inherits="Paginas_MenuParametros" %>



<asp:Content ID="Conteudo" ContentPlaceHolderID="Conteudo" Runat="Server">
    
        <%--<div class="container-fluid box">--%>

        <div class="card">
            <div class="card-body">
                 <div class="table-responsive">
                <asp:GridView ID="dtgParametros" Width="100%" runat="server" AllowPaging="True" CssClass="table table-striped table-bordered table-hover table-padrao" PageSize="30"
                    OnRowDataBound="dtgParametros_RowDataBound">
                    <PagerStyle HorizontalAlign="Center" CssClass="PagerStyle" />
                    <HeaderStyle CssClass="bg-primary" HorizontalAlign="Left" />
                    <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" FirstPageText="Primeira"
                        LastPageText="Última" />
                </asp:GridView>
            </div>
            </div>
          
        </div>
    <%--</div>--%>

       
</asp:Content>


