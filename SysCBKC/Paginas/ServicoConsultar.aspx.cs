using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Repositorio;

public partial class Paginas_ServicoConsultar : System.Web.UI.Page
{
    string tipoAcesso;
    Acesso acesso = new Acesso();

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            if (!IsPostBack)
            {

                if (Session["isnUsuario"] != null && Session["isnAcesso"] != null && Session["nomeUsuario"] != null)
                {
                    tipoAcesso = acesso.ObterAcesso((int)Session["isnAcesso"]);


                }

                if (tipoAcesso != "ADMIN")
                {
                    Session["erro"] = "Ops! Acesso não permitido :(";
                    Response.Redirect("../Paginas/Error.aspx", false);
                }

                calDatFim.Attributes.Add("onclick", "if(self.gfPop)gfPop.fPopCalendar(document.Form1.txtDatFim);return false;");

                calDatIni.Attributes.Add("onclick", "if(self.gfPop)gfPop.fPopCalendar(document.Form1.txtDatIni);return false;");
                

                Consultar();
            }

            divMsg.Visible = false;
        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
       
    }

    protected void btnConsultar_Click(object sender, EventArgs e)
    {
        try
        {
            Consultar();
        }
        catch (Exception ex)
        {
            Session["erro"] = ex.Message;
            Response.Redirect("../Paginas/Error.aspx");
        }
        

    }

    private void Consultar()
    {
        Servico servico = new Servico();
        ArrayList arlConsulta = new ArrayList();
        NameValueCollection nvcConsulta = new NameValueCollection();
        List<Servico> listaServicos = new List<Servico>();

        try
        {
            //Descrição do Serviço
            servico.dscServico = txtDscServico.Text.Trim();

            //Data inicial da vigência
            if(txtDatIni.Value != "")
            {
                servico.dataInicialVigencia = DateTime.Parse(txtDatIni.Value.Trim());
            }

            //Data final da vigência
            if (txtDatFim.Value != "")
            {
                servico.dataFinalVigencia = DateTime.Parse(txtDatFim.Value.Trim());
            }

            

            listaServicos = servico.ListarServico();

            foreach (Servico currentServico in listaServicos)
            {
                nvcConsulta = new NameValueCollection();
                nvcConsulta.Add("isnServico", currentServico.isnServico.ToString());
                nvcConsulta.Add("Serviço", currentServico.dscServico.ToString());
                nvcConsulta.Add("Valor do Serviço", currentServico.valorServico.ToString());
                nvcConsulta.Add("Ações", "");

                arlConsulta.Add(nvcConsulta);

            }

            dtgServico.DataSource = Util.Validadores.ConverteResultado(arlConsulta);
            dtgServico.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }


    private bool ValidaCampos()
    {
        //Data inicial da vigência
        if (txtDatIni.Value.Trim() != "")
        {
            if (!Util.Validadores.DataValida(txtDatIni.Value.Trim()))
            {
                lblMensagem.Text = "Data inicial de vigência inválida.";
                lblMensagem.Visible = true;
                txtDatIni.Focus();
                return false;
            }
            
            
        }

        //Data final da vigência
        if (txtDatFim.Value.Trim() != "")
        {
            if (!Util.Validadores.DataValida(txtDatFim.Value.Trim()))
            {
                lblMensagem.Text = "Data final de vigência inválida.";
                lblMensagem.Visible = true;
                txtDatFim.Focus();
                return false;
            }


        }

        //Data final menor que inicial

        if(txtDatIni.Value.Trim() != "" && txtDatFim.Value.Trim() != "")
        {
            if(DateTime.Parse(txtDatFim.Value.Trim()) < DateTime.Parse(txtDatIni.Value.Trim()))
            {
                lblMensagem.Text = "Data inicial de vigência deve ser menor ou igual a data final.";
                lblMensagem.Visible = true;
                txtDatFim.Focus();
                return false;
            }
        }


        return true;

    }
    protected void btnIncluir_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas/ServicoIncluir.aspx");
    }

    protected void dtgServico_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    


        if (e.Row.RowType != DataControlRowType.Pager)
        {
            
            e.Row.Cells[0].Visible = false;

        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

          

            HyperLink editAction = new HyperLink();
            //editAction.CssClass = "btn btn-sm btn-circle";
            editAction.Text = "<center><i class='fa fa-pencil'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</i>";
            editAction.ToolTip = "Editar";
            editAction.NavigateUrl = "ServicoEditar.aspx?edit=" + e.Row.Cells[0].Text.Trim();

            HyperLink deleteAction = new HyperLink();
           // deleteAction.CssClass = "btn btn-sm btn-circle";
            deleteAction.Text = "<i class='fa fa-trash-o'></i></center>";
            deleteAction.ToolTip = "Excluir";
            deleteAction.NavigateUrl = "ServicoExcluir.aspx?deleting=" + e.Row.Cells[0].Text.Trim();


            e.Row.Cells[e.Row.Cells.Count -1].Controls.Add(editAction);
            e.Row.Cells[e.Row.Cells.Count -1].Controls.Add(deleteAction);
        }


    }
}