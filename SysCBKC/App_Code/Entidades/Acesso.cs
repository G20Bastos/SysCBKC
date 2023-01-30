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
public class Acesso
{
    

    public Acesso()
    {
        //
        // TODO: Adicionar lógica do construtor aqui
        //
        
    }

    public string ObterAcesso(int tipoAcesso)
    {
        if (tipoAcesso == 1)
        {
            return "ADMIN";
        }
        else if (tipoAcesso == 2)
        {
            return "USUARIO";
        }
        else if (tipoAcesso == 3)
        {
            return "FUNCIONARIO";
        }
        else
        {
            return "USUARIO";
        }
    }
}

    

