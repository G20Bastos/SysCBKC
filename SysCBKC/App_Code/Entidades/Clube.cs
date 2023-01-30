using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Repositorio;

/// <summary>
/// Descrição resumida de Raca
/// </summary>
public class Clube
{
    public int isnClube { get; set; }
    public string dscClube { get; set; }
    public int isnEstado { get; set; }
    public int isnRegiao { get; set; }
    public int isnCidade { get; set; }
    public int aderiuSistema { get; set; }
    public string dscEndereco { get; set; }
    public string numCep { get; set; }
    public string numTelefone1 { get; set; }
    public string numTelefone2 { get; set; }
    public string dscEmail { get; set; }
    public string dscSite { get; set; }
    public string nomCidade { get; set; }
    public string nomEstado { get; set; }
    public string dscBairro { get; set; }
    public string dscRegiao { get; set; }

    public Clube()
    {
        //
        // TODO: Adicionar lógica do construtor aqui
        //
    }

    DbSYSCBKCEntities DbCBKC;

    public void Insert(CLB_CLUBE clube)
    {

        try
        {
            DbCBKC = new DbSYSCBKCEntities();
            DbCBKC.Database.Connection.Open();
            DbCBKC.CLB_CLUBE.Add(clube);
            DbCBKC.SaveChanges();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            DbCBKC.Database.Connection.Close();
        }

    }



    public void Delete()
    {

        try
        {

            if (this != null && this.isnClube != 0)
            {
                DbCBKC = new DbSYSCBKCEntities();
                DbCBKC.Database.Connection.Open();
                short delete_pkey = Convert.ToInt16(this.isnClube);
                CLB_CLUBE clube = DbCBKC.CLB_CLUBE.FirstOrDefault(a => a.ISN_CLUBE == delete_pkey);
                DbCBKC.CLB_CLUBE.Remove(clube);
                DbCBKC.SaveChanges();
            }
          
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            DbCBKC.Database.Connection.Close();
        }

    }

    public List<Clube> ListarClube()
    {
        DbCBKC = new DbSYSCBKCEntities();
        List<Clube> ListaClubes = new List<Clube>();
        try
        {
            DbCBKC.Database.Connection.Open();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectAux"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                con.Open();
                string commando = "";

                if (this.dscClube == "" && this.isnEstado == 0 && this.isnRegiao == 0)
                {
                    
                    commando = "";
                    commando += "SELECT         CLB.ISN_CLUBE,                                 ";
                    commando += "               CLB.DSC_CLUBE,                                 ";
                    commando += "               CLB.DSC_ENDERECO,                              ";
                    commando += "               CLB.DSC_BAIRRO,                                ";
                    commando += "               EST.DSC_ESTADO,                                ";
                    commando += "               CID.DSC_CIDADE,                                ";
                    commando += "               REG.DSC_REGIAO,                                ";
                    commando += "               CLB.NUM_TELEFONE_1,                            ";
                    commando += "               CLB.DSC_EMAIL                                  ";
                    
                    commando += "FROM           CLB_CLUBE CLB                                  ";
                    commando += "INNER JOIN     EST_ESTADO EST                                 ";
                    commando += "ON             CLB.ISN_ESTADO = EST.ISN_ESTADO                ";
                    commando += "LEFT JOIN     CID_CIDADE CID                                 ";
                    commando += "ON             CLB.ISN_CIDADE = CID.ISN_CIDADE                ";
                    commando += "LEFT JOIN      REG_REGIAO REG                                 ";
                    commando += "ON             CLB.ISN_REGIAO = REG.ISN_REGIAO                ";
                    commando += "ORDER BY       CLB.DSC_CLUBE                                  ";

                } else
                {

                    commando = "";
                    commando += "SELECT         CLB.ISN_CLUBE,                               ";
                    commando += "               CLB.DSC_CLUBE,                               ";
                    commando += "               CLB.DSC_ENDERECO,                            ";
                    commando += "               CLB.DSC_BAIRRO,                              ";
                    commando += "               EST.DSC_ESTADO,                              ";
                    commando += "               CID.DSC_CIDADE,                              ";
                    commando += "               REG.DSC_REGIAO,                              ";
                    commando += "               CLB.NUM_TELEFONE_1,                          ";
                    commando += "               CLB.DSC_EMAIL                                ";

                    commando += "FROM           CLB_CLUBE CLB                                ";
                    commando += "INNER JOIN     EST_ESTADO EST                               ";
                    commando += "ON             CLB.ISN_ESTADO = EST.ISN_ESTADO              ";
                    commando += "LEFT JOIN     CID_CIDADE CID                                 ";
                    commando += "ON             CLB.ISN_CIDADE = CID.ISN_CIDADE                ";
                    commando += "LEFT JOIN      REG_REGIAO REG                               ";
                    commando += "ON             CLB.ISN_REGIAO = REG.ISN_REGIAO              ";
                    commando += "WHERE          1 = 1                                        ";



                    if(this.dscClube != "")
                    {
                        commando += " AND          CLB.DSC_CLUBE LIKE @dscClube               ";
                    }

                    if (this.isnEstado != 0)
                    {
                        commando += " AND          CLB.ISN_ESTADO = @isnEstado               ";
                    }

                    if (this.isnRegiao != 0)
                    {
                        commando += " AND          CLB.ISN_REGIAO = @isnRegiao               ";
                    }

                    
                    commando += "ORDER BY       CLB.DSC_CLUBE                                ";


                }
                

                using (SqlCommand command = new SqlCommand(commando, con))
                {
                    if(this.dscClube != "")
                    {
                       
                        command.Parameters.AddWithValue("@dscClube", "%" + this.dscClube.Trim() + "%");
                    }

                    if (this.isnEstado != 0)
                    {

                       
                        command.Parameters.Add("@isnEstado", SqlDbType.Int);
                        command.Parameters["@isnEstado"].Value = this.isnEstado;
                    }

                    if (this.isnRegiao != 0)
                    {

                        command.Parameters.Add("@isnRegiao", SqlDbType.Int);
                        command.Parameters["@isnRegiao"].Value = this.isnRegiao;
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            ListaClubes.Add(new Clube()
                            {
                                isnClube = Int32.Parse(reader["ISN_CLUBE"].ToString()),
                                dscClube = reader["DSC_CLUBE"].ToString(),
                                dscEndereco = reader["DSC_ENDERECO"].ToString(),
                                dscBairro = reader["DSC_BAIRRO"].ToString(),
                                nomEstado = reader["DSC_ESTADO"].ToString(),
                                dscRegiao = reader["DSC_REGIAO"].ToString(),
                                nomCidade = reader["DSC_CIDADE"].ToString(),
                                numTelefone1 = reader["NUM_TELEFONE_1"].ToString(),
                                dscEmail = reader["DSC_EMAIL"].ToString()
                               
                            });
                        }
                    }
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            DbCBKC.Database.Connection.Close();
        }
        return ListaClubes;


    }


    public bool ClubeCidadeAderiu()
    {
        DbCBKC = new DbSYSCBKCEntities();
        List<Clube> ListaClubes = new List<Clube>();
        try
        {
            DbCBKC.Database.Connection.Open();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectAux"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                con.Open();
                string commando = "";

                if (this.isnCidade != 0)
                {

                    commando = "";
                    commando += "SELECT         1                                ";
                    

                    commando += "FROM           CLB_CLUBE CLB                                  ";
                    commando += "WHERE          CLB.ISN_CIDADE = @isnCidade                                        ";
                    commando += "AND            CLB.STA_ADESAO = 1                                        ";

                }
                




                using (SqlCommand command = new SqlCommand(commando, con))
                {
                    if (this.isnCidade != 0)
                    {

                        command.Parameters.Add("@isnCidade", SqlDbType.Int);
                        command.Parameters["@isnCidade"].Value = this.isnCidade;
                    }

                  
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            return true;
                        }
                    }
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            DbCBKC.Database.Connection.Close();
        }
        return false;


    }



    public void CarregaDropdownClubesAderiram(ref DropDownList drp, string isnCampoTabelaFilha, string dscCampoTabelaFilha, string dscTabelaPai, string aliasTabelaPai, string dscTabelaFilha, string aliasTabelaFilha, string valorCampo, string isnCampoTabelaPai)
    {

        DbSYSCBKCEntities DbSysCBKC = new DbSYSCBKCEntities();

        try
        {
            DbSysCBKC.Database.Connection.Open();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectAux"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                con.Open();
                string commando = "";



                commando += " SELECT '' " + ",          ";
                commando += "       0  AS  ISN_CLUBE ,  ";
                commando += "       '' AS  DSC_CLUBE    ";

                commando += " UNION ALL ";


                commando += "  SELECT '' " + ",         ";
                commando += "       CLB.ISN_CLUBE, ";
                commando += "       CLB.DSC_CLUBE  ";
                commando += " FROM   CLB_CLUBE CLB  ";
                commando += " INNER JOIN EST_ESTADO EST  ";
                commando += " ON CLB.ISN_ESTADO=EST.ISN_ESTADO  ";
                commando += " WHERE EST.ISN_ESTADO  = " + valorCampo + " ";
                commando += " AND CLB.STA_ADESAO = 1 ";
                commando += " ORDER BY DSC_CLUBE";





                using (SqlCommand command = new SqlCommand(commando, con))
                {


                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        drp.DataSource = reader;
                        drp.DataValueField = isnCampoTabelaFilha;
                        drp.DataTextField = dscCampoTabelaFilha;

                        drp.DataBind();

                        reader.Close();
                    }
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            DbSysCBKC.Database.Connection.Close();
        }




    }





    public bool CidadeNaoTemClube()
    {
        DbCBKC = new DbSYSCBKCEntities();
        List<Clube> ListaClubes = new List<Clube>();
        try
        {
            DbCBKC.Database.Connection.Open();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectAux"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                con.Open();
                string commando = "";

                if (this.isnCidade != 0)
                {

                    commando = "";
                    commando += "SELECT         1                                ";


                    commando += "FROM           CLB_CLUBE CLB                                  ";
                    commando += "WHERE          CLB.ISN_CIDADE = @isnCidade                                        ";

                }





                using (SqlCommand command = new SqlCommand(commando, con))
                {
                    if (this.isnCidade != 0)
                    {

                        command.Parameters.Add("@isnCidade", SqlDbType.Int);
                        command.Parameters["@isnCidade"].Value = this.isnCidade;
                    }


                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            return false;
                        }
                    }
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            DbCBKC.Database.Connection.Close();
        }
        return true;


    }

    public bool ClubeEstadoAderiu()
    {
        DbCBKC = new DbSYSCBKCEntities();
        List<Clube> ListaClubes = new List<Clube>();
        try
        {
            DbCBKC.Database.Connection.Open();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectAux"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                con.Open();
                string commando = "";

                if (this.isnEstado != 0)
                {

                    commando = "";
                    commando += "SELECT         1                                ";


                    commando += "FROM           CLB_CLUBE CLB                                  ";
                    commando += "WHERE          CLB.ISN_ESTADO = @isnEstado                                        ";
                    commando += "AND            CLB.STA_ADESAO = 1                                        ";

                }





                using (SqlCommand command = new SqlCommand(commando, con))
                {
                    if (this.isnEstado != 0)
                    {

                        command.Parameters.Add("@isnEstado", SqlDbType.Int);
                        command.Parameters["@isnEstado"].Value = this.isnEstado;
                    }


                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            return true;
                        }
                    }
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            DbCBKC.Database.Connection.Close();
        }
        return false;


    }




    public void ListarClubeIsn()
    {

        if (this != null && this.isnClube != 0)
        {


            CLB_CLUBE clb_clube = new CLB_CLUBE();
            try
            {
                DbCBKC = new DbSYSCBKCEntities();
                DbCBKC.Database.Connection.Open();


                clb_clube = DbCBKC.CLB_CLUBE.First(r => r.ISN_CLUBE == this.isnClube);

                this.dscClube = clb_clube.DSC_CLUBE;
                this.isnEstado = (int)clb_clube.ISN_ESTADO;

                if (clb_clube.ISN_CIDADE != null)
                {
                    this.isnCidade = (int)clb_clube.ISN_CIDADE;
                }
                
                this.isnRegiao = (int)clb_clube.ISN_REGIAO;
                this.dscEndereco = clb_clube.DSC_ENDERECO;
                this.dscBairro = clb_clube.DSC_BAIRRO;
                this.numCep = clb_clube.NUM_CEP;
                this.numTelefone1 = clb_clube.NUM_TELEFONE_1;
                this.numTelefone2 = clb_clube.NUM_TELEFONE_2;
                this.dscEmail = clb_clube.DSC_EMAIL;
                this.dscSite = clb_clube.DSC_SITE;
                this.nomCidade = clb_clube.NOM_CIDADE;
                if(clb_clube.STA_ADESAO != null)
                {
                    this.aderiuSistema = (int)clb_clube.STA_ADESAO;
                }
                


            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }

        
    }

    

    public int NextId()
    {

        int chave;

        try
        {
            DbCBKC = new DbSYSCBKCEntities();

            if (!DbCBKC.CLB_CLUBE.Any())
            {
                chave = 1;
            }
            else
            {
                chave = DbCBKC.CLB_CLUBE.Max(r => r.ISN_CLUBE) + 1;
            }

            
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return chave;
    }

    public void UpdateData()
    {


        try
        {
            DbCBKC = new DbSYSCBKCEntities();
            DbCBKC.Database.Connection.Open();

            CLB_CLUBE update_clube = DbCBKC.CLB_CLUBE.FirstOrDefault(r => r.ISN_CLUBE == this.isnClube);

            update_clube.ISN_CLUBE = this.isnClube;
            update_clube.DSC_CLUBE = this.dscClube;
            update_clube.ISN_ESTADO = this.isnEstado;
            update_clube.ISN_CIDADE = this.isnCidade;
            update_clube.ISN_REGIAO = this.isnRegiao;
            update_clube.DSC_ENDERECO = this.dscEndereco;
            update_clube.NUM_CEP = this.numCep;
            update_clube.NUM_TELEFONE_1 = this.numTelefone1;
            update_clube.NUM_TELEFONE_2 = this.numTelefone2;
            update_clube.DSC_EMAIL = this.dscEmail;
            update_clube.DSC_SITE = this.dscSite;
            update_clube.NOM_CIDADE = this.nomCidade;
            update_clube.DSC_BAIRRO = this.dscBairro;
            update_clube.STA_ADESAO = this.aderiuSistema;


            DbCBKC.SaveChanges();
        }
        catch (Exception ex)
        {

            throw ex;
        }
        finally
        {
            DbCBKC.Database.Connection.Close();
        }
    }


}