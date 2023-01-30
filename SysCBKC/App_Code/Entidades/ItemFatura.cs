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
public class ItemFatura
{

    public int isnItemFatura { get; set; }
    public int isnFatura { get; set; }
    public int isnSolicitacao { get; set; }
    public decimal valorItem { get; set; }

    public ItemFatura()
    {
        //
        // TODO: Adicionar lógica do construtor aqui
        //
    }

    DbSYSCBKCEntities DbSYSCBKCEntities;

    public void Insert(ITF_ITEM_FATURA itemFatura)
    {

        try
        {
            DbSYSCBKCEntities = new DbSYSCBKCEntities();
            DbSYSCBKCEntities.Database.Connection.Open();
            DbSYSCBKCEntities.ITF_ITEM_FATURA.Add(itemFatura);
            DbSYSCBKCEntities.SaveChanges();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            DbSYSCBKCEntities.Database.Connection.Close();
        }

    }



    public void Delete()
    {

        try
        {

            if (this != null && this.isnItemFatura != 0)
            {
                DbSYSCBKCEntities = new DbSYSCBKCEntities();
                DbSYSCBKCEntities.Database.Connection.Open();
                short delete_pkey = Convert.ToInt16(this.isnItemFatura);
                ITF_ITEM_FATURA raca = DbSYSCBKCEntities.ITF_ITEM_FATURA.FirstOrDefault(a => a.ISN_ITEM_FATURA == delete_pkey);
                DbSYSCBKCEntities.ITF_ITEM_FATURA.Remove(raca);
                DbSYSCBKCEntities.SaveChanges();
            }
          
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            DbSYSCBKCEntities.Database.Connection.Close();
        }

   

        
    }

    

    public int NextId()
    {

        int chave;

        try
        {
            DbSYSCBKCEntities = new DbSYSCBKCEntities();

            if (!DbSYSCBKCEntities.ITF_ITEM_FATURA.Any())
            {
                chave = 1;
            }
            else
            {
                chave = DbSYSCBKCEntities.ITF_ITEM_FATURA.Max(r => r.ISN_ITEM_FATURA) + 1;
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
            DbSYSCBKCEntities = new DbSYSCBKCEntities();
            DbSYSCBKCEntities.Database.Connection.Open();

            ITF_ITEM_FATURA update_item_fatura = DbSYSCBKCEntities.ITF_ITEM_FATURA.FirstOrDefault(r => r.ISN_ITEM_FATURA == this.isnItemFatura);

            update_item_fatura.ISN_ITEM_FATURA = this.isnItemFatura;
            update_item_fatura.ISN_FATURA = this.isnFatura;
            update_item_fatura.ISN_SOLICITACAO = this.isnSolicitacao;
            update_item_fatura.VLR_SOLICITACAO = this.valorItem;


            DbSYSCBKCEntities.SaveChanges();
        }
        catch (Exception ex)
        {

            throw ex;
        }
        finally
        {
            DbSYSCBKCEntities.Database.Connection.Close();
        }
    }


}