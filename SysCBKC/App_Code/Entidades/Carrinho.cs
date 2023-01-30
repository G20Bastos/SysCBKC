using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Repositorio;


public class Carrinho
{
    public int isnCarrinho { get; set; }
    public int isnSolicitacao { get; set; }
    public int isnPessoa { get; set; }
    public int qtdeFilhotes { get; set; }
    public int isnRaca { get; set; }
    public DateTime dataNacimentoNinhada { get; set; }
    public string msgTransferencia { get; set; }
    public string nomPropOrigem { get; set; }
    public string nomPropDestino { get; set; }
    public string enderecoPropDestino { get; set; }
    public string emailPropDestino { get; set; }
    public string nomCao { get; set; }
    public string rgCao { get; set; }
    
    

    public Carrinho()
    {
        //
        // TODO: Adicionar lógica do construtor aqui
        //
    }

    DbSYSCBKCEntities DbCBKC;

    public void Insert(CAR_CARRINHO carrinho)
    {

        try
        {
            DbCBKC = new DbSYSCBKCEntities();
            DbCBKC.Database.Connection.Open();
            DbCBKC.CAR_CARRINHO.Add(carrinho);
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


    public List<Carrinho> ListarDadosCompraNinhada()
    {
        DbCBKC = new DbSYSCBKCEntities();
        List<Carrinho> ListaDeDadosCompraNinhada = new List<Carrinho>();

        try
        {
            DbCBKC.Database.Connection.Open();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectAux"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                con.Open();
                string commando = "";



                commando = "";
                commando += "SELECT         CAR.ISN_CARRINHO,                                   ";
                commando += "               NIN.DAT_NASCIMENTO,                                 ";
                commando += "               NIN.ISN_RACA,                                       ";
                commando += "               SOL.ISN_SOLICITACAO,                                       ";
                commando += " COUNT(FIL.ISN_FILHOTE) AS QTDE_FILHOTES                           ";
                commando += " FROM CAR_CARRINHO CAR                                             ";
                commando += " INNER JOIN SOL_SOLICITACAO SOL ON SOL.ISN_SOLICITACAO = CAR.ISN_SOLICITACAO ";
                commando += " INNER JOIN NIN_NINHADA NIN ON SOL.ISN_NINHADA = NIN.ISN_NINHADA ";
                commando += " INNER JOIN FIL_FILHOTE FIL ON NIN.ISN_NINHADA = FIL.ISN_NINHADA ";
                commando += " WHERE CAR.ISN_PESSOA = @isnPessoa ";
                commando += " AND SOL.ISN_NINHADA IS NOT NULL ";
                commando += " AND SOL.ISN_TRANSFERENCIA IS NULL ";
                commando += " GROUP BY CAR.ISN_CARRINHO, NIN.DAT_NASCIMENTO, NIN.ISN_RACA, SOL.ISN_SOLICITACAO ";

               




                using (SqlCommand command = new SqlCommand(commando, con))
                {
                    
                        command.Parameters.Add("@isnPessoa", SqlDbType.Int);
                        command.Parameters["@isnPessoa"].Value = this.isnPessoa;
                   
                  

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            ListaDeDadosCompraNinhada.Add(new Carrinho()
                            {
                                isnCarrinho = Int32.Parse(reader["ISN_CARRINHO"].ToString()),
                                qtdeFilhotes = Int32.Parse(reader["QTDE_FILHOTES"].ToString()),
                                isnRaca = Int32.Parse(reader["ISN_RACA"].ToString()),
                                dataNacimentoNinhada = DateTime.Parse(reader["DAT_NASCIMENTO"].ToString()),
                                isnSolicitacao = Int32.Parse(reader["ISN_SOLICITACAO"].ToString())


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
        return ListaDeDadosCompraNinhada;


    }

    public void ListarCarrinhoIsn()
    {
       

        if (this != null && this.isnCarrinho != 0)
        {
            CAR_CARRINHO car_carrinho = new CAR_CARRINHO();
            try
            {
                DbCBKC = new DbSYSCBKCEntities();
                DbCBKC.Database.Connection.Open();


                car_carrinho = DbCBKC.CAR_CARRINHO.First(r => r.ISN_CARRINHO == this.isnCarrinho);
                this.isnSolicitacao = car_carrinho.ISN_SOLICITACAO;
                this.isnPessoa = car_carrinho.ISN_PESSOA;
                
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }


        }


        public List<Carrinho> ListarDadosCompraTransferencia()
    {
        DbCBKC = new DbSYSCBKCEntities();
        List<Carrinho> ListaDeDadosCompraTransferencia = new List<Carrinho>();

        try
        {
            DbCBKC.Database.Connection.Open();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectAux"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                con.Open();
                string commando = "";



                commando = "";
                commando += "SELECT CAR.ISN_CARRINHO, ";
                commando += "       SOL.ISN_SOLICITACAO, ";
                commando += "       TRA.NOM_PROP_ORIGEM, ";
                commando += "       TRA.CPF_ORIGEM,";
                commando += "       TRA.ENDERECO_ORIGEM,";
                commando += "       TRA.NOM_PROP_DESTINO,";
                commando += "       TRA.CPF_DESTINO,";
                commando += "       TRA.ENDERECO_DESTINO,";
                commando += "       TRA.NOM_CAO,";
                commando += "       TRA.RG_CAO,";
                commando += "       TRA.NUM_CEP_ORIGEM,";
                commando += "       TRA.NUM_CEP_DESTINO,";
                commando += "       TRA.DSC_COMPLEMENTO,";
                commando += "       TRA.DSC_COMPLEMENTO_DESTINO,";
                commando += "       TRA.DSC_EMAIL, ";
                commando += "       TRA.DSC_EMAIL_DESTINO";

                commando += "   FROM   CAR_CARRINHO CAR     ";
                commando += "  INNER JOIN SOL_SOLICITACAO SOL ON SOL.ISN_SOLICITACAO = CAR.ISN_SOLICITACAO   ";
                commando += "  INNER JOIN TRA_TRANSFERENCIA TRA ON SOL.ISN_TRANSFERENCIA = TRA.ISN_TRANSFERENCIA   ";
                commando += "  WHERE CAR.ISN_PESSOA = @isnPessoa ";
                commando += "  AND SOL.ISN_NINHADA IS NULL ";
                commando += "  AND SOL.ISN_TRANSFERENCIA IS NOT NULL; ";






                using (SqlCommand command = new SqlCommand(commando, con))
                {

                    command.Parameters.Add("@isnPessoa", SqlDbType.Int);
                    command.Parameters["@isnPessoa"].Value = this.isnPessoa;



                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            ListaDeDadosCompraTransferencia.Add(new Carrinho()
                            {

                                isnCarrinho = Int32.Parse(reader["ISN_CARRINHO"].ToString()),
                                nomPropOrigem =  reader["NOM_PROP_ORIGEM"].ToString(), 
                                nomPropDestino = reader["NOM_PROP_DESTINO"].ToString() ,
                                nomCao = reader["NOM_CAO"].ToString(),
                                rgCao = reader["RG_CAO"].ToString(),
                                enderecoPropDestino = reader["ENDERECO_DESTINO"].ToString() + ". " + reader["DSC_COMPLEMENTO_DESTINO"].ToString(),
                                emailPropDestino = reader["DSC_EMAIL_DESTINO"].ToString(),
                                isnSolicitacao = Int32.Parse(reader["ISN_SOLICITACAO"].ToString())
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
        return ListaDeDadosCompraTransferencia;


    }



    public void Delete()
    {

        try
        {

            if (this != null && this.isnCarrinho != 0)
            {
                DbCBKC = new DbSYSCBKCEntities();
                DbCBKC.Database.Connection.Open();
                short delete_pkey = Convert.ToInt16(this.isnCarrinho);
                CAR_CARRINHO carrinho = DbCBKC.CAR_CARRINHO.FirstOrDefault(a => a.ISN_CARRINHO == delete_pkey);
                DbCBKC.CAR_CARRINHO.Remove(carrinho);
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

    

   


    public int NextId()
    {

        int chave;

        try
        {
            DbCBKC = new DbSYSCBKCEntities();

            if (!DbCBKC.CAR_CARRINHO.Any())
            {
                chave = 1;
            }
            else
            {
                chave = DbCBKC.CAR_CARRINHO.Max(r => r.ISN_CARRINHO) + 1;
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

            CAR_CARRINHO update_carrinho = DbCBKC.CAR_CARRINHO.FirstOrDefault(r => r.ISN_CARRINHO == this.isnCarrinho);

            update_carrinho.ISN_CARRINHO = this.isnCarrinho;
            update_carrinho.ISN_SOLICITACAO = this.isnSolicitacao;
            update_carrinho.ISN_PESSOA = this.isnPessoa;
            


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