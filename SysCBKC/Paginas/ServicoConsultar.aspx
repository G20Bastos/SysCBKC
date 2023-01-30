<%@ Page Language="C#" MasterPageFile="~/Paginas/MasterPage.master" AutoEventWireup="true" CodeFile="ServicoConsultar.aspx.cs" Inherits="Paginas_ServicoConsultar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Conteudo" runat="Server">
    <div class="card">
        <div class="container-fluid box">
            <div class="card-header">
                <h5 class="card-title">Serviços</h5>
            </div>
            <div class="row">
                <div class="col-md" id="divMsg" runat="server">
                    <asp:Label ID="lblMensagem" role="alert" Width="100%" runat="server" CssClass="alert alert-danger"></asp:Label>
                </div>
            </div>
            <div class="card-body">
                <div class="row">
                    <div class="form-group col-6">
                        <label>Descrição do Serviço</label>


                        <asp:TextBox ID="txtDscServico" CssClass="form-control" MaxLength="200" style='text-transform:uppercase' runat="server"></asp:TextBox>
                    </div>
                </div>

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
                            type="text" maxlength="10" autocomplete="off" name="txtDatFim" runat="server" />
                        <a href="javascript:void(0)" id="calDatFim" runat="server"></a>
                        <script type="text/javascript">
                            DatepickerUtils.createDatepicker($('#txtDatFim'))
                        </script>
                    </div>
                </div>

                <div>
                    <asp:Button CssClass="btn btn-primary btn-round" ID="btnConsultar" runat="server" Text="Consultar" OnClick="btnConsultar_Click"></asp:Button>
                    <asp:Button CssClass="btn btn-primary btn-round" ID="btnIncluir" runat="server" Text="Incluir" OnClick="btnIncluir_Click"></asp:Button>
                </div>

                <div class="row">
                    <div class="col">
                        <asp:GridView ID="dtgServico" Width="100%" runat="server" AllowPaging="True" CssClass="table table-striped table-bordered table-hover table-padrao" PageSize="30"
                            OnRowDataBound="dtgServico_RowDataBound">
                            <PagerStyle HorizontalAlign="Center" CssClass="PagerStyle" />
                            <HeaderStyle CssClass="bg-primary" HorizontalAlign="Center" />
                            <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" FirstPageText="Primeira"
                                LastPageText="Última" />
                        </asp:GridView>
                    </div>
                </div>
            </div>

        </div>
    </div>

</asp:Content>




