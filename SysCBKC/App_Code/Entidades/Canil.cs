using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Repositorio;

/// <summary>
/// Descrição resumida de Raca
/// </summary>
public class Canil
{

    public int isnCanil { get; set; }
    public string codigoFCI { get; set; }
    public string nome { get; set; }
    public int isnClube { get; set; }
    public string nomeProprietario { get; set; }
    public string nomeCoProprietario { get; set; }
    public DateTime anuidade { get; set; }



    DbSYSCBKCEntities DbSYSCBKCEntities;



    public void ListarCanilPeloClubeECanil()
    {

        if (this != null && this.isnClube != 0)
        {


            CAN_CANIL can_canil = new CAN_CANIL();
            try
            {
                DbSYSCBKCEntities = new DbSYSCBKCEntities();
                DbSYSCBKCEntities.Database.Connection.Open();


                can_canil = DbSYSCBKCEntities.CAN_CANIL.First(c => c.ISN_CLUBE == this.isnClube && c.ISN_CANIL == this.isnCanil);

                this.isnCanil = (int)can_canil.ISN_CANIL;
                this.codigoFCI = can_canil.CODIGO_FCI;
                this.nome = can_canil.NOME;
                this.nomeProprietario = can_canil.PROPRIETARIO;
                this.nomeCoProprietario = can_canil.COPROPRIETARIO;
                this.anuidade = (DateTime)can_canil.ANUIDADE;


            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }

    }

    public bool ValidaAnuidade()
    {
        if (this.anuidade.Year == DateTime.Now.Year  ){
            return true;
        } else
        {
           return false;
        }
    }
}