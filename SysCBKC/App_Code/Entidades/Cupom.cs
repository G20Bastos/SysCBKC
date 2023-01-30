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
public class Cupom
{
    public int isnCupom { get; set; }
    public string codCupom { get; set; }
    public decimal valorCupom { get; set; }
    public int isnPessoaReembolso { get; set; }
    public string nomePessoaReembolso { get; set; }
    public int isnPessoaCadastroCupom { get; set; }
    public string nomPessoaCadastroCupom { get; set; }
    public DateTime dataCadastro { get; set; }
    public DateTime dataInicialCadastro { get; set; }
    public DateTime dataFinalCadastro { get; set; }
    public DateTime dataUtilizacao { get; set; }
    public DateTime dataInicialUtilizacao { get; set; }
    public DateTime dataFinalUtilizacao { get; set; }
    public int isnSolicitacao { get; set; }
    public int statusCupom { get; set; }

    /// <summary>
    /// Instancia de Util da Techway.dll para utilizar o método EncriptaSenha
    /// </summary>
    private Techway.Util util = new Techway.Util();




    public Cupom()
    {
        //
        // TODO: Adicionar lógica do construtor aqui
        //

    }

    DbSYSCBKCEntities DbCBKC;

    public void Insert(CUP_CUPOM cupom)
    {

        try
        {
            DbCBKC = new DbSYSCBKCEntities();
            DbCBKC.Database.Connection.Open();
            DbCBKC.CUP_CUPOM.Add(cupom);
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

            if (this != null && this.isnCupom != 0)
            {
                DbCBKC = new DbSYSCBKCEntities();
                DbCBKC.Database.Connection.Open();
                short delete_pkey = Convert.ToInt16(this.isnCupom);
                CUP_CUPOM cupom = DbCBKC.CUP_CUPOM.FirstOrDefault(a => a.ISN_CUPOM == delete_pkey);
                DbCBKC.CUP_CUPOM.Remove(cupom);
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

    public List<Cupom> ListarCupons()
    {
        DbCBKC = new DbSYSCBKCEntities();
        List<Cupom> ListaDeCupons = new List<Cupom>();

        //Variáveis utilizadas para tratar dados nulos da consulta
        DateTime dataUtilizacaoTratamento = DateTime.Parse("01/01/0001");
        int isnSolicitacaoTratamento = 0;

        try
        {
            DbCBKC.Database.Connection.Open();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectAux"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                con.Open();
                string commando = "";
                

                    commando = "";
                    commando += " SELECT        CUP.ISN_CUPOM,                                    ";
                    commando += "               CUP.COD_CUPOM,                                    ";
                    commando += "               CUP.VLR_CUPOM,                                     ";
                    commando += "               CUP.ISN_PESSOA_REEMBOLSO,                                           ";
                    commando += "               CUP.NOM_PESSOA_REEMBOLSO,                                            ";
                    commando += "               CUP.ISN_PESSOA_CADASTRO_CUPOM,                                  ";
                    commando += "               CUP.NOM_PESSOA_CADASTRO_CUPOM,                                     ";
                    commando += "               CUP.DAT_CADASTRO,                                     ";
                    commando += "               CUP.DAT_UTILIZACAO,                                     ";
                    commando += "               CUP.ISN_SOLICITACAO,                                     ";
                    commando += "               CUP.STA_CUPOM                                     ";
                    commando += " FROM          CUP_CUPOM CUP                                     ";
                    commando += " INNER JOIN    DAR_DOMINIO_ATRIBUTO DAR                             ";
                    commando += " ON            CUP.STA_CUPOM = DAR.VAL_ATRIBUTO AND DAR.NOM_ATRIBUTO = 'STA_CUPOM'  ";
                    

                    commando += "WHERE          1 = 1                                              ";

                //Código do Cupom
                if (this.codCupom != null)
                {
                    if (this.codCupom != "")
                    {
                        commando += "AND          CUP.COD_CUPOM LIKE @codCupom                     ";
                    }
                }


                //Nome do beneficiário
                if (this.nomePessoaReembolso != null)
                {
                    if (this.nomePessoaReembolso != "")
                    {
                        commando += "AND          CUP.NOM_PESSOA_REEMBOLSO LIKE @nomePessoaReembolso                     ";
                    }
                }


                //Nome da pessoa que criou o cupom
                if (this.nomPessoaCadastroCupom != null)
                {
                    if (this.nomPessoaCadastroCupom != "")
                    {
                        commando += "AND          CUP.NOM_PESSOA_CADASTRO_CUPOM LIKE @nomPessoaCadastroCupom                     ";
                    }
                }
                    

                //Data de cadastro do cupom
                if (this.dataInicialCadastro != DateTime.Parse("01/01/0001") && this.dataFinalCadastro != DateTime.Parse("01/01/0001"))
                {
                    commando += "AND          CUP.DAT_CADASTRO between @dataInicialCadastro AND @dataFinalCadastro                    ";
                }

                //Data de utilização do cupom
                if (this.dataInicialUtilizacao != DateTime.Parse("01/01/0001") && this.dataFinalUtilizacao != DateTime.Parse("01/01/0001"))
                {
                    commando += "AND          CUP.DAT_UTILIZACAO between @dataInicialUtilizacao AND @dataFinalUtilizacao                    ";
                }

                //Status do cupom
                if (this.statusCupom != -1)
                {
                    commando += "AND          CUP.STA_CUPOM = @staCupom                    ";
                }

                   

                commando += "ORDER BY       CUP.ISN_CUPOM DESC                                    ";


                


                using (SqlCommand command = new SqlCommand(commando, con))
                {
                    //Passagem de parâmetros

                    //Código do Cupom
                    if (this.codCupom != null)
                    {
                        if (this.codCupom != "")
                        {

                            command.Parameters.AddWithValue("@codCupom", "%" + this.codCupom.Trim() + "%");
                        }
                    }


                    //Nome do beneficiário
                    if (this.nomePessoaReembolso != null)
                    {
                        if (this.nomePessoaReembolso != "")
                        {
                            command.Parameters.AddWithValue("@nomePessoaReembolso", "%" + this.nomePessoaReembolso.Trim() + "%");
                        }
                    }


                    //Nome da pessoa que criou o cupom
                    if (this.nomPessoaCadastroCupom != null)
                    {
                        if (this.nomPessoaCadastroCupom != "")
                        {
                            command.Parameters.AddWithValue("@nomPessoaCadastroCupom", "%" + this.nomPessoaCadastroCupom.Trim() + "%");
                        }
                    }

                    //Data de cadastro do cupom
                    if (this.dataInicialCadastro != DateTime.Parse("01/01/0001") && this.dataFinalCadastro != DateTime.Parse("01/01/0001"))
                    {
                        command.Parameters.Add("@dataInicialCadastro", SqlDbType.DateTime);
                        command.Parameters["@dataInicialCadastro"].Value = this.dataInicialCadastro;

                        command.Parameters.Add("@dataFinalCadastro", SqlDbType.DateTime);
                        command.Parameters["@dataFinalCadastro"].Value = this.dataFinalCadastro;

                    }

                    //Data de utilização do cupom
                    if (this.dataInicialUtilizacao != DateTime.Parse("01/01/0001") && this.dataFinalUtilizacao != DateTime.Parse("01/01/0001"))
                    {
                        command.Parameters.Add("@dataInicialUtilizacao", SqlDbType.DateTime);
                        command.Parameters["@dataInicialUtilizacao"].Value = this.dataInicialUtilizacao;

                        command.Parameters.Add("@dataFinalUtilizacao", SqlDbType.DateTime);
                        command.Parameters["@dataFinalUtilizacao"].Value = this.dataFinalUtilizacao;

                    }

                    //Status do cupom
                    if (this.statusCupom != -1)
                    {
                        command.Parameters.Add("@staCupom", SqlDbType.Int);
                        command.Parameters["@staCupom"].Value = this.statusCupom;
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            //Verificando os dados que podem vir nulos da consulta

                            //Data de utilização do cupom
                            if (DateTime.TryParse(reader["DAT_UTILIZACAO"].ToString(), out DateTime dt))
                            {
                                dataUtilizacaoTratamento = DateTime.Parse(reader["DAT_UTILIZACAO"].ToString());
                            } else
                            {
                                dataUtilizacaoTratamento = DateTime.Parse("01/01/0001");
                            }

                            //Identificador da solicitação do cupom
                            if (Int32.TryParse(reader["ISN_SOLICITACAO"].ToString(), out int x))
                            {
                                isnSolicitacaoTratamento = Int32.Parse(reader["ISN_SOLICITACAO"].ToString());
                            }

                            ListaDeCupons.Add(new Cupom()
                            {
                                isnCupom = Int32.Parse(reader["ISN_CUPOM"].ToString()),
                                codCupom = reader["COD_CUPOM"].ToString(),
                                valorCupom = Decimal.Parse(reader["VLR_CUPOM"].ToString()),
                                isnPessoaReembolso = Int32.Parse(reader["ISN_PESSOA_REEMBOLSO"].ToString()),
                                nomePessoaReembolso = reader["NOM_PESSOA_REEMBOLSO"].ToString(),
                                isnPessoaCadastroCupom = Int32.Parse(reader["ISN_PESSOA_CADASTRO_CUPOM"].ToString()),
                                nomPessoaCadastroCupom = reader["NOM_PESSOA_CADASTRO_CUPOM"].ToString(),
                                dataCadastro = DateTime.Parse(reader["DAT_CADASTRO"].ToString()),
                                dataUtilizacao = dataUtilizacaoTratamento,
                                isnSolicitacao = isnSolicitacaoTratamento,
                                statusCupom = Int32.Parse(reader["STA_CUPOM"].ToString())

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
        return ListaDeCupons;


    }

    public List<CUP_CUPOM> ListarCupom(String codigoCupom)
    {
        DbCBKC = new DbSYSCBKCEntities();
        return DbCBKC.CUP_CUPOM.Where(r => r.COD_CUPOM == codigoCupom).ToList();
    }

    public void ListarCupomCodigo()
    {

        if (this != null && this.codCupom != "")
        {


            CUP_CUPOM cup_cupom = new CUP_CUPOM();
            try
            {
                DbCBKC = new DbSYSCBKCEntities();
                DbCBKC.Database.Connection.Open();


                cup_cupom = DbCBKC.CUP_CUPOM.First(r => r.COD_CUPOM == this.codCupom.Trim());

                this.codCupom = cup_cupom.COD_CUPOM;
                this.isnCupom = cup_cupom.ISN_CUPOM;
                this.valorCupom = (decimal)cup_cupom.VLR_CUPOM;
                this.isnPessoaReembolso = (int)cup_cupom.ISN_PESSOA_REEMBOLSO;
                this.nomePessoaReembolso = cup_cupom.NOM_PESSOA_REEMBOLSO;
                this.isnPessoaCadastroCupom = (int)cup_cupom.ISN_PESSOA_CADASTRO_CUPOM;
                this.nomPessoaCadastroCupom = cup_cupom.NOM_PESSOA_CADASTRO_CUPOM;
                this.dataCadastro = (DateTime)cup_cupom.DAT_CADASTRO;

                if (cup_cupom.DAT_UTILIZACAO != null)
                {
                    this.dataUtilizacao = (DateTime)cup_cupom.DAT_UTILIZACAO;
                }

                if (cup_cupom.ISN_SOLICITACAO != null)
                {
                    this.isnSolicitacao = (int)cup_cupom.ISN_SOLICITACAO;
                }

                this.statusCupom = (int)cup_cupom.STA_CUPOM;

            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }

    }

    public void ListarCupomIsn()
    {

        if (this != null && this.isnCupom != 0)
        {


            CUP_CUPOM cup_cupom = new CUP_CUPOM();
            try
            {
                DbCBKC = new DbSYSCBKCEntities();
                DbCBKC.Database.Connection.Open();

                
                cup_cupom = DbCBKC.CUP_CUPOM.First(r => r.ISN_CUPOM == this.isnCupom);

                this.codCupom = cup_cupom.COD_CUPOM;
                this.valorCupom = (decimal)cup_cupom.VLR_CUPOM;
                this.isnPessoaReembolso = (int)cup_cupom.ISN_PESSOA_REEMBOLSO;
                this.nomePessoaReembolso = cup_cupom.NOM_PESSOA_REEMBOLSO;
                this.isnPessoaCadastroCupom = (int)cup_cupom.ISN_PESSOA_CADASTRO_CUPOM;
                this.nomPessoaCadastroCupom = cup_cupom.NOM_PESSOA_CADASTRO_CUPOM;
                this.dataCadastro = (DateTime)cup_cupom.DAT_CADASTRO;

                if (cup_cupom.DAT_UTILIZACAO != null)
                {
                    this.dataUtilizacao = (DateTime)cup_cupom.DAT_UTILIZACAO;
                }

                if (cup_cupom.ISN_SOLICITACAO != null)
                {
                    this.isnSolicitacao = (int)cup_cupom.ISN_SOLICITACAO;
                }
                
                this.statusCupom = (int)cup_cupom.STA_CUPOM;
                
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
            DbSYSCBKCEntities dbContext = new DbSYSCBKCEntities();

            if (!dbContext.CUP_CUPOM.Any())
            {
                chave = 1;
            }
            else
            {
                chave = dbContext.CUP_CUPOM.Max(r => r.ISN_CUPOM) + 1;
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

            CUP_CUPOM update_cupom = DbCBKC.CUP_CUPOM.FirstOrDefault(r => r.ISN_CUPOM == this.isnCupom);

            update_cupom.ISN_CUPOM = this.isnCupom;
            update_cupom.COD_CUPOM = this.codCupom;
            update_cupom.VLR_CUPOM = this.valorCupom;
            update_cupom.ISN_PESSOA_REEMBOLSO = this.isnPessoaReembolso;
            update_cupom.NOM_PESSOA_REEMBOLSO = this.nomePessoaReembolso;
            update_cupom.ISN_PESSOA_CADASTRO_CUPOM = this.isnPessoaCadastroCupom;
            update_cupom.NOM_PESSOA_CADASTRO_CUPOM = this.nomPessoaCadastroCupom;
            update_cupom.DAT_CADASTRO = this.dataCadastro;
            if (this.dataUtilizacao != DateTime.Parse("01/01/0001"))
            {
                update_cupom.DAT_UTILIZACAO = this.dataUtilizacao;
            } else
            {
                update_cupom.DAT_UTILIZACAO = null;
            }
            
            update_cupom.ISN_SOLICITACAO = null;
            update_cupom.STA_CUPOM = (byte)this.statusCupom;
          


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