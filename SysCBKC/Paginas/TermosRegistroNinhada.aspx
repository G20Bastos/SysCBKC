<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TermosRegistroNinhada.aspx.cs" Inherits="Paginas_TermosRegistroNinhada" %>

<%@ Register Src="../Modelos/Dependencias.ascx" TagName="Dependencias" TagPrefix="uc1" %>
<!DOCTYPE html>

<html>
<head runat="server">
    <title>SysCBKC</title>
    <uc1:Dependencias ID="Dependencias" runat="server" />
    <script src="../Scripts/Funcoes.js" type="text/javascript"></script>
    <script src="../Scripts/Analytics.js" type="text/javascript"></script>
    
</head>
<body>
    <form id="form1" runat="server">

        <div class="container-fluid box">
            <h5 class="modal-title">
                <b>Termos CBKC
                </b>
            </h5>
        
        <div class="row">
            <div class="col-md" id="divMsg" runat="server">
                <asp:Label ID="lblMensagem" Width="100%" runat="server" ></asp:Label>
            </div>
        </div>
            
       
            </div>



    </form>
</body>
</html>
