<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>


<!DOCTYPE html>
<html>
<head>
	<title>SysCBKC</title>
	<meta charset="UTF-8">
	<meta name="viewport" content="width=device-width, initial-scale=1">
<!--===============================================================================================-->	
	<link rel="icon" type="image/png" href="../Estilos/login_assets/images/icons/favicon.png"/>
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="../Estilos/login_assets/vendor/bootstrap/css/bootstrap.min.css">
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="../Estilos/login_assets/fonts/font-awesome-4.7.0/css/font-awesome.min.css">
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="../Estilos/login_assets/fonts/Linearicons-Free-v1.0.0/icon-font.min.css">
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="../Estilos/login_assets/vendor/animate/animate.css">
<!--===============================================================================================-->	
	<link rel="stylesheet" type="text/css" href="../Estilos/login_assets/vendor/css-hamburgers/hamburgers.min.css">
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="../Estilos/login_assets/vendor/animsition/css/animsition.min.css">
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="../Estilos/login_assets/vendor/select2/select2.min.css">
<!--===============================================================================================-->	
	<link rel="stylesheet" type="text/css" href="../Estilos/login_assets/vendor/daterangepicker/daterangepicker.css">
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="../Estilos/login_assets/css/util.css">
	<link rel="stylesheet" type="text/css" href="../Estilos/login_assets/css/main.css">
<!--===============================================================================================-->
</head>
<body>
	
	<div class="limiter">
		<div class="container-login100">
			<div class="wrap-login100">
				<form class="login100-form validate-form" id="form1" runat="server">
					<span class="login100-form-title p-b-34">
						SISTEMA DE REGISTROS DA CBKC
					</span>
					
					<div class="wrap-input100 rs1-wrap-input100 validate-input m-b-20" data-validate="Type user name">
						
                        <asp:TextBox onkeypress='Formatar(this, "###.###.###-##")' id="txtCPF" class="input100" type="text" MaxLength="14" size="12" placeholder="CPF" runat="server"></asp:TextBox>
						<span class="focus-input100"></span>
					</div>
					<div class="wrap-input100 rs2-wrap-input100 validate-input m-b-20" data-validate="Type password">
						
                        <asp:TextBox id="txtSenha" class="input100" type="password" runat="server" placeholder="Senha"></asp:TextBox>
						<span class="focus-input100"></span>
					</div>
					
					<div class="container-login100-form-btn">
						<asp:Button id="btnLogar" class="btn btn-primary btn-round" text="Acessar" runat="server" OnClick="btnLogar_Click">
							
						</asp:Button>
					</div>

					<div class="w-full text-center p-t-27 p-b-239">
						<asp:LinkButton class="text-primary" ID="lkbEsqueciMinhaSenha" Text="
							Esqueceu a senha?" runat="server" OnClick="lkbEsqueciMinhaSenha_Click">
						</asp:LinkButton>
                        <br />
                        <br />
                        <asp:Label ID="lblMsg" CssClass="text-danger" Text="E-mail e/ou Senha Incorretos" runat="server"></asp:Label>
					</div>

                   

					<div class="w-full text-center">
                        <span class="txt1">
							Ainda não tem uma conta? 
						</span>
						<asp:LinkButton class="text-primary" Text="Cadastre-se" ID="lkbCadastro" runat="server" OnClick="lkbCadastro_Click">
							
						</asp:LinkButton>
					</div>
				</form>

				<div class="login100-more" style="background-image: url('../Estilos/login_assets/images/background.png');"></div>
			</div>
		</div>
	</div>
	
	

	<div id="dropDownSelect1"></div>
	
<!--===============================================================================================-->
	<script src="../Estilos/login_assets/vendor/jquery/jquery-3.2.1.min.js"></script>
<!--===============================================================================================-->
	<script src="../Estilos/login_assets/vendor/animsition/js/animsition.min.js"></script>
<!--===============================================================================================-->
	<script src="../Estilos/login_assets/vendor/bootstrap/js/popper.js"></script>
	<script src="../Estilos/login_assets/vendor/bootstrap/js/bootstrap.min.js"></script>
<!--===============================================================================================-->
	<script src="vendor/select2/select2.min.js"></script>
	<script>
		$(".selection-2").select2({
			minimumResultsForSearch: 20,
			dropdownParent: $('#dropDownSelect1')
		});
	</script>
<!--===============================================================================================-->
	<script src="../Estilos/login_assets/vendor/daterangepicker/moment.min.js"></script>
	<script src="../Estilos/login_assets/vendor/daterangepicker/daterangepicker.js"></script>
<!--===============================================================================================-->
	<script src="../Estilos/login_assets/vendor/countdowntime/countdowntime.js"></script>
<!--===============================================================================================-->
	<script src="../Estilos/login_assets/js/main.js"></script>

</body>
</html>