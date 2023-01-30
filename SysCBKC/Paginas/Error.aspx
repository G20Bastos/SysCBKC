<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="Paginas_Erro" %>

<%@ Register Src="../Modelos/Dependencias.ascx" TagName="Dependencias" TagPrefix="uc1" %>
<!DOCTYPE html>

<html>
<head runat="server">
    <title>SysCBKC</title>
    <uc1:Dependencias ID="Dependencias" runat="server" />
    <script src="../Scripts/Funcoes.js" type="text/javascript"></script>
    <script src="../Scripts/Analytics.js" type="text/javascript"></script>
    <link rel="icon" type="image/png" href="../Estilos/assets/img/favicon.png">
    
</head>
<body>
    <form id="form1" runat="server">

        <div class="container-fluid box">
            <h5 class="modal-title">
                <b>Erro
                </b>
            </h5>
        
        <div class="row">
            <div class="col-md" id="divMsg" runat="server">
                <asp:Label ID="lblMensagem" role="alert" Width="100%" runat="server" CssClass="alert alert-danger"></asp:Label>
            </div>
        </div>
            
        <div>
            <asp:Button CssClass="btn btn-primary" ID="btnOk" runat="server" Text="OK" OnClick="btnOk_Click"></asp:Button>
            
        </div>
            </div>



    </form>
</body>
</html>
