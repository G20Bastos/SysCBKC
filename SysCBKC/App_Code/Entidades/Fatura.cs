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
public class Fatura
{
    public int isnFatura { get; set; }
    public int isnPessoa { get; set; }
    public int isnSolicitacao { get; set; }
    public string dscFormaPagamento { get; set; }
    public decimal valor { get; set; }
    public int qtdePArcelas { get; set; }
    public int isnCupom { get; set; }
    public decimal valorCupom { get; set; }
    public decimal valorTaxa { get; set; }

    public Fatura()
    {
        //
        // TODO: Adicionar lógica do construtor aqui
        //
    }

    DbSYSCBKCEntities DbCBKC;

    public void Insert(FAT_FATURA fatura)
    {

        try
        {
            DbCBKC = new DbSYSCBKCEntities();
            DbCBKC.Database.Connection.Open();
            DbCBKC.FAT_FATURA.Add(fatura);
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



    

    public int NextId()
    {

        int chave;

        try
        {
            DbCBKC = new DbSYSCBKCEntities();

            if (!DbCBKC.FAT_FATURA.Any())
            {
                chave = 1;
            }
            else
            {
                chave = DbCBKC.FAT_FATURA.Max(r => r.ISN_FATURA) + 1;
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

            FAT_FATURA update_fatura = DbCBKC.FAT_FATURA.FirstOrDefault(r => r.ISN_FATURA == this.isnFatura);

            update_fatura.ISN_SOLICITACAO = null;
            if (this.isnCupom != 0)
            {
                update_fatura.ISN_CUPOM = this.isnCupom;
            } else
            {
                update_fatura.ISN_CUPOM = null;
            }

            if (this.valorCupom != 0)
            {
                update_fatura.VLR_CUPOM = this.valorCupom;
            } else
            {
                update_fatura.VLR_CUPOM = null;
            }
            
            
            update_fatura.VLR_TAXA = this.valorTaxa;


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