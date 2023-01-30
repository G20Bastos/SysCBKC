<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="Cadastre-se.aspx.cs" Inherits="Paginas_Cadastrese" %>

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
    <form id="form1" runat="server" autocomplete="off">
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
                    <div class="col-md-12" id="divPrincipal" runat="server">
                        <div class="card">
                            <div class="card-header">
                                <h5 class="card-title">Cadastre-se</h5>
                            </div>
                            <div class="row">
                                <div class="col-md">
                                    <asp:Label ID="lblMensagem" role="alert" Width="100%" runat="server" CssClass="alert alert-danger"></asp:Label>
                                    <asp:Label ID="lblMensagemExito" role="alert" Width="100%" runat="server" CssClass="alert alert-success"></asp:Label>
                                </div>
                            </div>

                            <div class="card-body">

                                <div class="row">
                                    <div class="col-md-5 pr-1">
                                        <div class="form-group">
                                            <label>Nome *</label>
                                            <asp:TextBox ID="txtNome"
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
                                    <div class="col-md-4 pl-1">
                                        <div class="form-group">
                                            <label>RG *</label>
                                            <asp:TextBox ID="txtRG"
                                                class="form-control" type="text" MaxLength="11" size="12" name="txtRG" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-5 pr-1">
                                        <div class="form-group">
                                            <label>E-mail *</label>
                                            <asp:TextBox ID="txtEmail"
                                                class="form-control" type="email" MaxLength="100" size="12" name="txtEmail" style='text-transform:uppercase' runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3 px-1">
                                        <div class="form-group">
                                            <label>Senha *</label>
                                            <asp:TextBox ID="txtSenha"
                                                class="form-control" type="password" MaxLength="15" size="12" name="txtEmail" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-4 pl-1">
                                        <div class="form-group">
                                            <label>Confirmação de Senha *</label>
                                            <asp:TextBox ID="txtConfirmacaoSenha"
                                                class="form-control" type="password" MaxLength="15" size="12" name="txtConfirmacaoSenha" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-5 pr-1">
                                        <div class="form-group">
                                            <label>Endereço *</label>
                                            <asp:TextBox ID="txtEndereco"
                                                class="form-control" type="text" MaxLength="100" size="12" name="txtEndereco" style='text-transform:uppercase' runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-md-3 px-1">
                                        <div class="form-group">
                                            <label>Complemento</label>
                                            <asp:TextBox ID="txtComplemento"
                                                class="form-control" type="text" MaxLength="100" size="12" name="txtEndereco" style='text-transform:uppercase' runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-md-4 pl-1">
                                        <div class="form-group">
                                            <label>Bairro *</label>
                                            <asp:TextBox ID="txtBairro"
                                                class="form-control" type="text" MaxLength="50" size="12" name="txtBairro" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>



                                <div class="row">
                                    <div class="col-md-4 pr-1">
                                        <div class="form-group">
                                            <label>Estado *</label>
                                            <asp:DropDownList ID="drpIsnEstado" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpIsnEstado_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-4 px-1">
                                        <div class="form-group">
                                            <label>Cidade *</label>
                                            <asp:DropDownList ID="drpCidade" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpCidade_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-4 pl-1">
                                        <div class="form-group">
                                            <label>CEP *</label>
                                            <asp:TextBox onkeypress='Formatar(this, "#####-###")' ID="txtNumCep"
                                                class="form-control" type="text" MaxLength="9" size="12" name="txtNumCep" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4 pr-1">
                                        <div class="form-group">
                                            <label>É sócio de algum clube?</label>
                                            <asp:RadioButton
                                                ID="radioBtnSocioSim"
                                                runat="server"
                                                Text="Sim"
                                                AutoPostBack="true"
                                                OnCheckedChanged="radioBtnSocioSim_CheckedChanged" />
                                            <asp:RadioButton
                                                ID="radioBtnSocioNao"
                                                runat="server"
                                                Text="Não"
                                                Checked="True"
                                                AutoPostBack="true"
                                                OnCheckedChanged="radioBtnSocioNao_CheckedChanged" />

                                            <asp:DropDownList ID="drpIsnClube" CssClass="form-control" Visible="false" runat="server" OnSelectedIndexChanged="drpIsnClube_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </div>

                                    </div>
                                    <div class="col-md-4 pr-1">
                                        <div class="form-group">
                                            <div class="form-group">
                                                <label>Possui canil?</label>
                                                <asp:RadioButton
                                                    ID="radioBtnSim"
                                                    runat="server"
                                                    Text="Sim"
                                                    AutoPostBack="true"
                                                    OnCheckedChanged="radioBtnSim_CheckedChanged" />
                                                <asp:RadioButton
                                                    ID="radioBtnNao"
                                                    runat="server"
                                                    Text="Não"
                                                    Checked="True"
                                                    AutoPostBack="true"
                                                    OnCheckedChanged="radioBtnNao_CheckedChanged" />

                                                <div id="divNomeCanil" class="form-group" visible="false" runat="server">
                                                    <asp:DropDownList ID="drpIsnCanil" CssClass="form-control" runat="server" OnSelectedIndexChanged="drpIsnCanil_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </div>
                                                

                                            </div>

                                        </div>


                                    </div>
                                    <div id="divNaoEncontrouCanil" class="col-md-4 pr-1" visible="false" runat="server">
                                        <br />
                                                        <asp:Button CssClass="btn btn-primary btn-round" ID="btnNaoEncontrouCanil" runat="server" Text="Não encontrei o meu canil" OnClick="btnNaoEncontrouCanil_Click"></asp:Button>
                                                        </div>

                                </div>


                                <div class="row">
                                    <div>
                                        <div class="update ml-auto mr-auto">

                                            <asp:Button CssClass="btn btn-success btn-round" ID="btnIncluir" runat="server" Text="Cadastrar-se" OnClick="btnSalvar_Click"></asp:Button>
                                        </div>


                                    </div>
                                </div>
                            </div>
                        </div>



                    </div>
                    <div class="card" id="divConfirmacaoCanil" runat="server">
                    <div class="card-body">
                        <div class="card-header">
                            <h5 class="card-title">Confirmação de propriedade do canil</h5>
                        </div>

                        <div class="row">
                     <div class="col-md-3 pr-1">
                <div class="card-header">
                <h6 class="card-title">Canil</h6>
                    <asp:label ID="lblNomeCanil" runat="server"></asp:label>
              </div>
                
                
                
            </div>
                    <div class="col-md-3 pr-1">
                      
                        <div class="card-header">
                <h6 class="card-title">Proprietário</h6>
                            <asp:label ID="lblNomeProprietario" runat="server"></asp:label>
              </div>
                        
                      
                    </div>
                    <div class="col-md-3 pr-1">
                         <div class="card-header">
                <h6 class="card-title">Co-Proprietário</h6>
                            <asp:label ID="lblNomeCoProprietario" runat="server" ></asp:label>
              </div>
                      </div>
                    </div>
                      <div class="row">
                          <div class="col-md-12 pl-1">
                         <div class="card-header">
                <h6 class="card-title">&nbsp;&nbsp;Este é o seu canil?</h6>
                            <asp:Button CssClass="btn btn-success btn-round" ID="btnSim" runat="server" Text="Sim" OnClick="btnSim_Click"></asp:Button>
                            <asp:Button CssClass="btn btn-danger btn-round" ID="btnNao" runat="server" Text="Não" OnClick="btnNao_Click"></asp:Button>
                            
              </div>
                      </div>
                      </div>
                        
                      
                   
          
        

                    </div>

                </div>

                </div>

                
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


