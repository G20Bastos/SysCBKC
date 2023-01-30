using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Repositorio;

public partial class Paginas_UsuarioEditar : System.Web.UI.Page
{


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Request["edit"] != "")
                {
                    Exibir(Int32.Parse(Request["edit"]));
                    lblMensagem.Visible = false;
                    lblMsgSucesso.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
    }

    private void Exibir(int identificador)
    {
        Pessoa usuario = new Pessoa();
        usuario.isnPessoa = identificador;

        try
        {
            usuario.ListarPessoaIsn();
            lblTopo.Text = "Editar Usuário - " + usuario.nomPessoa;
            txtNome.Value = usuario.nomPessoa;
            txtEmail.Value = usuario.dscEmail;
            txtCpf.Value = usuario.cpf.ToString().PadLeft(11, Char.Parse("0"));
           
            //txtCpf.Enabled = false;
            divCPF.Disabled = true;
            txtRG.Text = usuario.rg.ToString();
            txtEndereco.Text = usuario.dscEndereco;
            Util.Validadores.CarregaDropdown(ref drpIsnEstado, "ISN_ESTADO", "DSC_ESTADO", "EST_ESTADO");
            drpIsnEstado.SelectedValue = usuario.isnEstado.ToString();
            Util.Validadores.CarregaDropdownPorTabela(ref drpCidade, "ISN_CIDADE", "DSC_CIDADE", "EST_ESTADO", "EST", "CID_CIDADE", "CID", drpIsnEstado.SelectedValue, "ISN_ESTADO");
            drpCidade.SelectedValue = usuario.isnCidade.ToString();
            //Util.Validadores.CarregaDropdownPorTabela(ref drpIsnClube, "ISN_CLUBE", "DSC_CLUBE", "CID_CIDADE", "CID", "CLB_CLUBE", "CLB", drpCidade.SelectedValue, "ISN_CIDADE");
            Util.Validadores.CarregaDropdown(ref drpIsnClube, "ISN_CLUBE", "DSC_CLUBE", "CLB_CLUBE");
            drpIsnClube.SelectedValue = usuario.isnClube.ToString();
            Util.Validadores.CarregaDropdownPorTabela(ref drpIsnCanil, "ISN_CANIL", "NOME", "CLB_CLUBE", "CLB", "CAN_CANIL", "CAN", drpIsnClube.SelectedValue, "ISN_CLUBE");
            drpIsnCanil.SelectedValue = usuario.isnCanil.ToString();
            Util.Validadores.CarregaDropdown(ref drpTipoAcesso, "ISN_ACESSO", "DSC_ACESSO", "ACE_ACESSO");
            drpTipoAcesso.SelectedValue = usuario.isnAcesso.ToString();

            if (drpIsnClube.SelectedValue != "0")
            {
                radioBtnSocioSim.Checked = true;
                radioBtnSocioNao.Checked = false;
                drpIsnClube.Visible = true;
            } else
            {
                radioBtnSocioSim.Checked = false;
                radioBtnSocioNao.Checked = true;
                drpIsnClube.Visible = false;
            }

            if (drpIsnCanil.Text == "0" || drpIsnCanil.Text == "")
            {

                radioBtnSim.Checked = false;
                radioBtnNao.Checked = true;
                drpIsnCanil.Visible = false;

               
            } else
            {
                radioBtnSim.Checked = true;
                radioBtnNao.Checked = false;
                drpIsnCanil.Visible = true;
            }
           

            txtSenha.Text = usuario.dscSenha;
            txtSenha.Enabled = false;
            txtConfirmacaoSenha.Text = usuario.dscSenha;
            txtConfirmacaoSenha.Enabled = false;
            txtNumCep.Text = usuario.cep;
            txtComplemento.Text = usuario.dscComplemento;
            txtBairro.Text = usuario.bairro;

            //Definindo o que usuário/admin podem visualizar
            if (Session["isnAcesso"].ToString() == "2")
            {
                lblTipoAcesso.Visible = false;
                drpTipoAcesso.Visible = false;
                txtCpf.Disabled = true;
            }

           


        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    

    protected void radioBtnSim_CheckedChanged(object sender, EventArgs e)
    {
        radioBtnNao.Checked = false;
        divNomeCanil.Visible = true;
        drpIsnCanil.Visible = true;
    }

    protected void radioBtnNao_CheckedChanged(object sender, EventArgs e)
    {
        radioBtnSim.Checked = false;
        divNomeCanil.Visible = false;
    }

    protected void drpIsnEstado_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMensagem.Visible = false;
        if (drpIsnEstado.SelectedValue != "0")
        {
            //Carregar o drop das cidades se pelo menos um kennel daquele estado tenha aderido ao sistema
            Clube clube = new Clube();
            clube.isnEstado = Int32.Parse(drpIsnEstado.SelectedValue);

            //Se pelo menos um clube daquele estado aderiu, preenche as cidades pelo estado
            if (clube.ClubeEstadoAderiu())
            {
                Util.Validadores.CarregaDropdownPorTabela(ref drpCidade, "ISN_CIDADE", "DSC_CIDADE", "EST_ESTADO", "EST", "CID_CIDADE", "CID", drpIsnEstado.SelectedValue, "ISN_ESTADO");
            }
            else
            {
                lblMensagem.Text = "Seu estado não aderiu ao sistema.";
                lblMensagem.Visible = true;

            }
        }
    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        try
        {
            if (ValidaCampos())
            {
                lblMensagem.Visible = false;
                Salvar();
                
            }

        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
    }

    private void Salvar()
    {
        Pessoa pessoa = new Pessoa();

        try
        {
            pessoa.isnPessoa = Int32.Parse(Request["edit"]);
            pessoa.nomPessoa = txtNome.Value.ToUpper();
            pessoa.dscEmail = txtEmail.Value.ToUpper();
            pessoa.cpf = long.Parse(txtCpf.Value.Replace(".","").Replace("-",""));
            pessoa.rg = long.Parse(txtRG.Text.Replace(".", "").Replace("-", ""));
            pessoa.dscEndereco = txtEndereco.Text.ToUpper();
            pessoa.dscComplemento = txtComplemento.Text.ToUpper();
            pessoa.bairro = txtBairro.Text.ToUpper();

            if (drpIsnClube.SelectedValue != "0")
            {
                pessoa.isnClube = Int32.Parse(drpIsnClube.SelectedValue);
            }
            else
            {
                pessoa.isnClube = 0;
            }
            if (radioBtnSim.Checked)
            {
             
                pessoa.isnCanil = Int32.Parse(drpIsnCanil.SelectedValue);
            } else
            {
                pessoa.nomCanil = "";
                pessoa.codCanil = 0;
            }
            
            pessoa.isnEstado = Int32.Parse(drpIsnEstado.SelectedValue);
            pessoa.isnAcesso = Int32.Parse(drpTipoAcesso.SelectedValue);
            pessoa.isnCidade = Int32.Parse(drpCidade.SelectedValue);
            Techway.Util util = new Techway.Util();

            
            pessoa.cep = txtNumCep.Text;
            pessoa.UpdateData(false);

            if (Session["isnAcesso"].ToString() == "2" || Session["isnAcesso"].ToString() == "3")
            {
                lblMsgSucesso.Text = "Usuário Alterado com Sucesso!";
                lblMsgSucesso.Visible = true;
            }
            else
            {
                Response.Redirect("../Paginas/UsuarioConsultar.aspx", false);
            }

            

        }
        catch (Exception ex)
        {
            throw ex;
        }



    }

    private bool ValidaCampos()
    {

        if (drpCidade.SelectedValue != "0" && drpCidade.SelectedValue != "")
        {
            //Carregar o drop dos clubes se a cidade selecionada tem kennel e este aderiu ao sistema
            Clube clube = new Clube();
            clube.isnCidade = Int32.Parse(drpCidade.SelectedValue);


            //Se a cidade selecionada tem clube, avaliar
            if (!clube.CidadeNaoTemClube())
            {

                //Se a cidade selecionada tem clube e este aderiu, preenche o drop de clubes.
                if (!clube.ClubeCidadeAderiu())
                {
                    drpIsnClube.Items.Clear();
                    lblMensagem.Text = "Seu estado não aderiu ao sistema.";
                    lblMensagem.Visible = true;
                    return false;
                }

            }



        }

        //Nome da Pessoa
        if (txtNome.Value == "")
        {
            lblMensagem.Text = "Favor, inserir o nome do usuário.";
            lblMensagem.Visible = true;
            txtNome.Focus();
            return false;
        }

        //CPF
        if (txtCpf.Value == "")
        {
            lblMensagem.Text = "Favor, inserir o CPF do usuário.";
            lblMensagem.Visible = true;
            txtCpf.Focus();
            return false;
        }

        //CPF válido
        if (txtCpf.Value != "")
        {
            if (!Util.Validadores.IsCpf(txtCpf.Value.Replace(".", "").Replace("-", "").Trim()))
            {
                lblMensagem.Text = "Favor, inserir um CPF válido.";
                lblMensagem.Visible = true;
                txtCpf.Focus();
                return false;
            }
        }

        //RG
        if (txtRG.Text == "")
        {
            lblMensagem.Text = "Favor, inserir o RG do usuário.";
            lblMensagem.Visible = true;
            txtRG.Focus();
            return false;
        }

        //E-mail
        if (txtEmail.Value == "")
        {
            lblMensagem.Text = "Favor, inserir o E-mail do usuário.";
            lblMensagem.Visible = true;
            txtEmail.Focus();
            return false;
        }

      

        //Endereço
        if (txtEndereco.Text == "")
        {
            lblMensagem.Text = "Favor, inserir o endereço do usuário.";
            lblMensagem.Visible = true;
            txtEndereco.Focus();
            return false;
        }


        //Bairro
        if (txtBairro.Text == "")
        {
            lblMensagem.Text = "Favor, inserir o bairro do usuário.";
            lblMensagem.Visible = true;
            txtBairro.Focus();
            return false;
        }


        //Estado
        if (drpIsnEstado.SelectedValue == "")
        {
            lblMensagem.Text = "Favor, selecionar o estado do usuário.";
            lblMensagem.Visible = true;
            drpIsnEstado.Focus();
            return false;
        }

      

        //Cidade
        if (drpCidade.SelectedValue == "" || drpCidade.SelectedValue == "0")
        {
            lblMensagem.Text = "Favor, inserir a cidade do usuário.";
            lblMensagem.Visible = true;
            drpCidade.Focus();
            return false;
        }

        //CEP
        if (txtNumCep.Text == "")
        {
            lblMensagem.Text = "Favor, inserir o CEP do usuário.";
            lblMensagem.Visible = true;
            txtNumCep.Focus();
            return false;
        }

        //Caso possua canil, preencher o nome do canil
        if (radioBtnSim.Checked)
        {
            if(drpIsnCanil.SelectedValue == "0")
            {
                lblMensagem.Text = "Favor, inserir o nome do canil.";
                lblMensagem.Visible = true;
                drpIsnCanil.Focus();
                return false;
            }
           
        }

        //Tipo de acesso
        if (drpTipoAcesso.SelectedValue == "")
        {
            lblMensagem.Text = "Favor, selecionar o tipo de acesso do usuário.";
            lblMensagem.Visible = true;
            drpIsnEstado.Focus();
            return false;
        }

        //Caso seja associado a clube, preencher o drop do clube
        if (radioBtnSocioSim.Checked)
        {
            if (drpIsnClube.SelectedValue == "0")
            {
                lblMensagem.Text = "Favor, informar o clube ao qual é associado.";
                lblMensagem.Visible = true;
                drpIsnClube.Focus();
                return false;
            }

        }

        return true;

    }


    protected void drpCidade_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMensagem.Visible = false;

        if (drpCidade.SelectedValue != "0")
        {
            //Carregar o drop dos clubes se a cidade selecionada tem kennel e este aderiu ao sistema
            Clube clube = new Clube();
            clube.isnCidade = Int32.Parse(drpCidade.SelectedValue);


            //Se a cidade selecionada tem clube, avaliar
            if (clube.CidadeNaoTemClube())
            {
                clube.CarregaDropdownClubesAderiram(ref drpIsnClube, "ISN_CLUBE", "DSC_CLUBE", "EST_ESTADO", "EST", "CLB_CLUBE", "CLB", drpIsnEstado.SelectedValue, "ISN_ESTADO");
            }
            else
            {
                //Se a cidade selecionada tem clube e este aderiu, preenche o drop de clubes.
                if (clube.ClubeCidadeAderiu())
                {
                    Util.Validadores.CarregaDropdownPorTabela(ref drpIsnClube, "ISN_CLUBE", "DSC_CLUBE", "CID_CIDADE", "CID", "CLB_CLUBE", "CLB", drpCidade.SelectedValue, "ISN_CIDADE");
                }
                //Se a cidade tem clube mas este não aderiu
                else
                {
                    drpIsnClube.Items.Clear();
                    lblMensagem.Text = "Seu estado não aderiu ao sistema.";
                    lblMensagem.Visible = true;

                }
            }



        }

    }

    protected void radioBtnSocioSim_CheckedChanged(object sender, EventArgs e)
    {
        radioBtnSocioNao.Checked = false;
        drpIsnClube.Visible = true;
    }

    protected void radioBtnSocioNao_CheckedChanged(object sender, EventArgs e)
    {
        radioBtnSocioSim.Checked = false;
        drpIsnClube.Visible = false;
        drpIsnClube.SelectedValue = "0";
    }

    protected void drpIsnCanil_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void drpIsnClube_SelectedIndexChanged(object sender, EventArgs e)
    {
        Util.Validadores.CarregaDropdownPorTabela(ref drpIsnCanil, "ISN_CANIL", "NOME", "CLB_CLUBE", "CLB", "CAN_CANIL", "CAN", drpIsnClube.SelectedValue, "ISN_CLUBE");
    }
}