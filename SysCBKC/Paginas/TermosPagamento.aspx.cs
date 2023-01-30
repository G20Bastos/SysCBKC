using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Paginas_TermosPagamento : System.Web.UI.Page
{

   
    protected void Page_Load(object sender, EventArgs e)
    {

            
            lblMensagem.Text = "VALIDAÇÃO: * Os serviços solicitados serão realizados após a confirmação de pagamento pela instituição financeira. A inexatidão das informações prestadas pelo Declarante, seja por erro, omissão ou dolo, isentará a CBKC de qualquer restituição de valores. Descontos sujeitos a alterações.";
       
        
    }

    
}