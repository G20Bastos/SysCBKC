using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Repositorio;

/// <summary>
/// Descrição resumida de Estado
/// </summary>
public class Estado
{
    public Estado()
    {
     
    }

    public List<EST_ESTADO> Lista()
    {
        var contexto = new DbSYSCBKCEntities();
        return contexto.EST_ESTADO.ToList();
    }

    public void Incluir(EST_ESTADO estado)
    {
        var contexto = new DbSYSCBKCEntities();
        contexto.EST_ESTADO.Add(estado);
        contexto.SaveChanges();

    }
}