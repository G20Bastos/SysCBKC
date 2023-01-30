<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="RedefinirSenha.aspx.cs" Inherits="Paginas_RedefinirSenha" %>

<!doctype html>
<html>

<head>
    <meta charset="utf-8" />
    <link rel="apple-touch-icon" sizes="76x76" href="../Estilos/assets/img/apple-icon.png">
    <link rel="icon" type="image/png" href="../Estilos/assets/img/favicon.png">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <script src="../Scripts/Funcoes.js" type="text/javascript"></script>
    <title>SysCBKC
    </title>
    <meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0, shrink-to-fit=no' name='viewport' />
    <!--     Fonts and icons     -->
    <link href="https://fonts.googleapis.com/css?family=Montserrat:400,700,200" rel="stylesheet" />
    <link href="https://maxcdn.bootstrapcdn.com/font-awesome/latest/css/font-awesome.min.css" rel="stylesheet">
    <!-- CSS Files -->
    <link href="../Estilos/assets/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Estilos/assets/css/paper-dashboard.css?v=2.0.1" rel="stylesheet" />
    <!-- CSS Just for demo purpose, don't include it in your project -->
    <link href="../Estilos/assets/demo/demo.css" rel="stylesheet" />

</head>

<body class="">
    <form id="form1" runat="server">
        <div class="wrapper ">
            <div class="sidebar" data-color="white" data-active-color="primary">
                <div class="logo">
                    <a href="#" class="simple-text logo-mini">

                        <!--<div class="logo-image-small">
				<img src="../Estilos/assets/img/logo-small.png">
			  </div> -->
                        <!-- <p>CT</p> -->
                    </a>
                    <a href="https://cbkc.org/" class="simple-text logo-normal">
                        <div class="logo-image-big">
                            <img src="../Estilos/assets/img/LogoCBKC.png">
                        </div>
                    </a>
                </div>
                <div class="sidebar-wrapper">
                    <ul class="nav">
                        <li>
                            <a href="./Login.aspx">
                                <i class="nc-icon nc-shop"></i>
                                <p>Retornar ao Login</p>
                            </a>
                        </li>


                    </ul>
                </div>
            </div>
            <div class="main-panel" style="height: 100vh;">
                <!-- Navbar -->
                <nav class="navbar navbar-expand-lg navbar-absolute fixed-top navbar-transparent">
                    <div class="container-fluid">
                        <div class="navbar-wrapper">
                            <div class="navbar-toggle">
                                <button type="button" class="navbar-toggler">
                                    <span class="navbar-toggler-bar bar1"></span>
                                    <span class="navbar-toggler-bar bar2"></span>
                                    <span class="navbar-toggler-bar bar3"></span>
                                </button>
                            </div>
                            <a class="navbar-brand" href="javascript:;">
                                <asp:Label ID="lblBoasVindas" runat="server"></asp:Label></a>
                        </div>
                        <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navigation" aria-controls="navigation-index" aria-expanded="false" aria-label="Toggle navigation">
                            <span class="navbar-toggler-bar navbar-kebab"></span>
                            <span class="navbar-toggler-bar navbar-kebab"></span>
                            <span class="navbar-toggler-bar navbar-kebab"></span>
                        </button>

                    </div>
                </nav>
                <!-- End Navbar -->
                <div class="content">
                    <div class="col-md-12">
                        <div class="card">
                            <div class="card-header">
                                <h5 class="card-title">Redefinir Senha</h5>
                            </div>
                            <div class="row">
                                <div class="col-md">
                                    <asp:Label ID="lblMensagem" role="alert" Width="100%" runat="server" CssClass="alert alert-danger"></asp:Label>
                                    <asp:Label ID="lblMensagemExito" role="alert" Width="100%" runat="server" CssClass="alert alert-success"></asp:Label>
                                </div>
                            </div>

                            <div class="card-body">
                                <div id="divDadosParaEnvio" runat="server">
                                    <div class="row">


                                        <div class="col-md-3 pr-1">
                                            <div class="form-group">
                                                <label>E-mail *</label>
                                                <asp:TextBox ID="txtEmail"
                                                    class="form-control" type="text" MaxLength="100" size="12" runat="server" style='text-transform:uppercase' autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3 px-1">
                                            <div class="form-group">
                                                <label>CPF *</label>
                                                <asp:TextBox onkeypress='Formatar(this, "###.###.###-##")' ID="txtCpf"
                                                    class="form-control" type="text" MaxLength="14" size="12" name="txtCpf" runat="server"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="col-md-2 pl-1">
                                            <div class="form-group">
                                                <label></label>
                                                <asp:Button CssClass="btn btn-primary btn-round" ID="btnEnviarEmail" runat="server" Text="Enviar Código" OnClick="btnEnviarEmail_Click"></asp:Button>
                                            </div>

                                        </div>
                                    </div>
                                </div>


                                <div id="divCodigoRecuperacao" runat="server">
                                    <div class="row">

                                        <div class="col-md-3 pr-1">
                                            <div class="form-group">
                                                <label>Código de Recuperação *</label>
                                                <asp:TextBox ID="txtCodRecuperacao"
                                                    class="form-control" type="text" MaxLength="100" size="12" runat="server" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="col-md-2 pl-1">
                                            <div class="form-group">
                                                <label></label>
                                                <asp:Button CssClass="btn btn-primary btn-round" ID="btnValidarCodigo" runat="server" Text="Validar Código" OnClick="btnValidarCodigo_Click"></asp:Button>
                                            </div>

                                        </div>
                                    </div>




                                </div>





                                <div id="divDadosRedefinicao" runat="server">

                                    <div class="row">
                                        <div class="col-md-3 pr-1">
                                            <div class="form-group">
                                                <label>Nova Senha *</label>
                                                <asp:TextBox ID="txtSenha"
                                                    class="form-control" type="password" MaxLength="16" size="12" runat="server" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="col-md-3 pl-1">
                                            <div class="form-group">
                                                <label>Confirmar Senha *</label>
                                                <asp:TextBox ID="txtConfirmacao"
                                                    class="form-control" type="password" MaxLength="16" size="12" runat="server" autocomplete="off"></asp:TextBox>
                                            </div>

                                        </div>

                                        <div class="row">

                                            <div class="col-md-2 pl-1">
                                                <div class="form-group">
                                                    <label></label>
                                                    <asp:Button CssClass="btn btn-success btn-round" ID="btnRedefinirSenha" runat="server" Text="Redefinir Senha" OnClick="btnRedefinirSenha_Click"></asp:Button>
                                                </div>

                                            </div>
                                        </div>
                                    </div>


                                </div>






                            </div>
                        </div>




                    </div>





                </div>

            </div>
            <footer class="footer" style="position: absolute; bottom: 0; width: -webkit-fill-available;">
                <div class="container-fluid">
                    <div class="row">
                        <nav class="footer-nav">
                            <ul>
                                <li><a></a></li>
                            </ul>
                        </nav>
                        <div class="credits ml-auto">
                            <span class="copyright">Copyright © CBKC 2020
                            </span>
                        </div>
                    </div>
                </div>
            </footer>
        </div>
        </div>
	  <!--   Core JS Files   -->
        <script src="../Estilos/assets/js/core/jquery.min.js"></script>
        <script src="../Estilos/assets/js/core/popper.min.js"></script>
        <script src="../Estilos/assets/js/core/bootstrap.min.js"></script>
        <script src="../Estilos/assets/js/plugins/perfect-scrollbar.jquery.min.js"></script>
        <!--  Google Maps Plugin    -->
        <script src="https://maps.googleapis.com/maps/api/js?key=YOUR_KEY_HERE"></script>
        <!-- Chart JS -->
        <script src="../Estilos/assets/js/plugins/chartjs.min.js"></script>
        <!--  Notifications Plugin    -->
        <script src="../Estilos/assets/js/plugins/bootstrap-notify.js"></script>
        <!-- Control Center for Now Ui Dashboard: parallax effects, scripts for the example pages etc -->
        <script src="../Estilos/assets/js/paper-dashboard.min.js" type="text/javascript"></script>

    </form>
</body>

</html>


