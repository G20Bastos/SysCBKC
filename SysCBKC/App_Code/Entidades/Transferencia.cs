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
public class Transferencia
{
    public int isnTransferencia { get; set; }
    public string nomPropOrigem { get; set; }
    public string nomPropDestino { get; set; }
    public string cpfOrigem { get; set; }
    public string cpfDestino { get; set; }
    public string enderecoOrigem { get; set; }
    public string enderecoDestino { get; set; }
    public string cepOrigem { get; set; }
    public string cepDestino { get; set; }
    public string nomCao { get; set; }
    public string rgCao { get; set; }
    public string complementoOrigem { get; set; }
    public string complementoDestino { get; set; }
    public string dscEmailOrigem { get; set; }
    public string dscEmailDestino { get; set; }

    public Transferencia()
    {
        //
        // TODO: Adicionar lógica do construtor aqui
        //
    }

    DbSYSCBKCEntities DbCBKC;

    public void Insert(TRA_TRANSFERENCIA transferencia)
    {

        try
        {
            DbCBKC = new DbSYSCBKCEntities();
            DbCBKC.Database.Connection.Open();
            DbCBKC.TRA_TRANSFERENCIA.Add(transferencia);
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

            if (this != null && this.isnTransferencia != 0)
            {
                DbCBKC = new DbSYSCBKCEntities();
                DbCBKC.Database.Connection.Open();
                short delete_pkey = Convert.ToInt16(this.isnTransferencia);
                TRA_TRANSFERENCIA transferencia = DbCBKC.TRA_TRANSFERENCIA.FirstOrDefault(a => a.ISN_TRANSFERENCIA == delete_pkey);
                DbCBKC.TRA_TRANSFERENCIA.Remove(transferencia);
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
            if (!DbCBKC.TRA_TRANSFERENCIA.Any())
            {
                chave = 1;

            } else
            {
                chave = DbCBKC.TRA_TRANSFERENCIA.Max(r => r.ISN_TRANSFERENCIA) + 1;
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

            TRA_TRANSFERENCIA update_tranferencia = DbCBKC.TRA_TRANSFERENCIA.FirstOrDefault(r => r.ISN_TRANSFERENCIA == this.isnTransferencia);

            update_tranferencia.ISN_TRANSFERENCIA = this.isnTransferencia;
            update_tranferencia.NOM_PROP_ORIGEM = this.nomPropOrigem;
            update_tranferencia.NOM_PROP_DESTINO = this.nomPropDestino;
            update_tranferencia.NUM_CEP_ORIGEM = this.cepOrigem;
            update_tranferencia.NUM_CEP_DESTINO = this.cepDestino;
            update_tranferencia.ENDERECO_ORIGEM = this.enderecoOrigem;
            update_tranferencia.ENDERECO_DESTINO = this.enderecoDestino;
            update_tranferencia.CPF_ORIGEM = this.cpfOrigem;
            update_tranferencia.CPF_DESTINO = this.cpfDestino;
            update_tranferencia.NOM_CAO = this.nomCao;
            update_tranferencia.RG_CAO = this.rgCao;
            update_tranferencia.DSC_COMPLEMENTO = this.complementoOrigem;
            update_tranferencia.DSC_COMPLEMENTO_DESTINO = this.complementoDestino;
            update_tranferencia.DSC_EMAIL = this.dscEmailOrigem;
            update_tranferencia.DSC_EMAIL_DESTINO = this.dscEmailDestino;


          



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


    public Transferencia listarTransferenciaIsn()
    {


        try
        {
            DbCBKC = new DbSYSCBKCEntities();
            DbCBKC.Database.Connection.Open();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectAux"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                con.Open();
                string commando = "";

                if (isnTransferencia != 0)
                {

                    commando = "";
                    commando += "SELECT         TRA.NOM_PROP_ORIGEM,                                 ";
                    commando += "               TRA.CPF_ORIGEM,                                        ";
                    commando += "               TRA.ENDERECO_ORIGEM,                                        ";
                    commando += "               TRA.NOM_PROP_DESTINO,                                        ";
                    commando += "               TRA.CPF_DESTINO,                                        ";
                    commando += "               TRA.ENDERECO_DESTINO,                                        ";
                    commando += "               TRA.DSC_COMPLEMENTO,                                        ";
                    commando += "               TRA.DSC_COMPLEMENTO_DESTINO,                                        ";
                    commando += "               TRA.DSC_EMAIL,                                        ";
                    commando += "               TRA.DSC_EMAIL_DESTINO,                                        ";
                    commando += "               TRA.NOM_CAO,                                        ";
                    commando += "               TRA.RG_CAO,                                        ";
                    commando += "               TRA.NUM_CEP_ORIGEM,                                        ";
                    commando += "               TRA.NUM_CEP_DESTINO                                         ";
                    commando += "               FROM TRA_TRANSFERENCIA TRA                                         ";
                    
                    commando += "WHERE          TRA.ISN_TRANSFERENCIA = @isnTransferencia                       ";

                }



                using (SqlCommand command = new SqlCommand(commando, con))
                {
                    if (this.isnTransferencia != 0)
                    {

                        command.Parameters.Add("@isnTransferencia", SqlDbType.Int);
                        command.Parameters["@isnTransferencia"].Value = this.isnTransferencia;
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            
                            this.nomPropOrigem = reader["NOM_PROP_ORIGEM"].ToString();
                            this.cpfOrigem = reader["CPF_ORIGEM"].ToString();
                            this.nomPropDestino = reader["NOM_PROP_DESTINO"].ToString();
                            this.cpfDestino = reader["CPF_DESTINO"].ToString();
                            this.enderecoOrigem = reader["ENDERECO_ORIGEM"].ToString();
                            this.enderecoDestino = reader["ENDERECO_DESTINO"].ToString();
                            this.nomCao = reader["NOM_CAO"].ToString();
                            this.rgCao = reader["RG_CAO"].ToString();
                            this.cepOrigem = reader["NUM_CEP_ORIGEM"].ToString();
                            this.cepDestino = reader["NUM_CEP_DESTINO"].ToString();
                            this.dscEmailOrigem = reader["DSC_EMAIL"].ToString();
                            this.dscEmailDestino = reader["DSC_EMAIL_DESTINO"].ToString();
                            this.complementoOrigem = reader["DSC_COMPLEMENTO"].ToString();
                            this.complementoDestino = reader["DSC_COMPLEMENTO_DESTINO"].ToString();
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

        return this;

    }

}