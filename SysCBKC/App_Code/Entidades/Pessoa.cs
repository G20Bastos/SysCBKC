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
public class Pessoa
{
    public int isnPessoa { get; set; }
    public string nomPessoa { get; set; }
    public string dscEmail { get; set; }
    public long cpf { get; set; }
    public long rg { get; set; }
    public string dscEndereco { get; set; }
    public int isnClube { get; set; }
    public int isnCidade { get; set; }
    public int isnCanil { get; set; }
    public int codCanil { get; set; }
    public string nomCanil { get; set; }
    public int isnEstado { get; set; }
    public int isnAcesso { get; set; }
    public string dscAcesso { get; set; }
    public string dscCidade { get; set; }
    public string dscSenha { get; set; }
    public string dscClube { get; set; }
    public string dscComplemento { get; set; }
    public string dscEstado { get; set; }
    public string cep { get; set; }
    public string bairro { get; set; }

    /// <summary>
    /// Instancia de Util da Techway.dll para utilizar o método EncriptaSenha
    /// </summary>
    private Techway.Util util = new Techway.Util();




    public Pessoa()
    {
        //
        // TODO: Adicionar lógica do construtor aqui
        //

    }

    DbSYSCBKCEntities DbCBKC;

    public void Insert(PES_PESSOA pessoa)
    {

        try
        {
            DbCBKC = new DbSYSCBKCEntities();
            DbCBKC.Database.Connection.Open();
            DbCBKC.PES_PESSOA.Add(pessoa);
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

            if (this != null && this.isnPessoa != 0)
            {
                DbCBKC = new DbSYSCBKCEntities();
                DbCBKC.Database.Connection.Open();
                short delete_pkey = Convert.ToInt16(this.isnPessoa);
                PES_PESSOA pessoa = DbCBKC.PES_PESSOA.FirstOrDefault(a => a.ISN_PESSOA == delete_pkey);
                DbCBKC.PES_PESSOA.Remove(pessoa);
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

    public List<Pessoa> ListarPessoa()
    {
        DbCBKC = new DbSYSCBKCEntities();
        List<Pessoa> ListaDePessoas = new List<Pessoa>();
        try
        {
            DbCBKC.Database.Connection.Open();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectAux"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                con.Open();
                string commando = "";

                if (this.nomPessoa == "" && this.isnClube == 0)
                {

                    commando = "";
                    commando += "SELECT         PES.ISN_PESSOA,                                    ";
                    commando += "               PES.NOM_PESSOA,                                    ";
                    commando += "               PES.DSC_EMAIL,                                     ";
                    commando += "               PES.CPF,                                           ";
                    commando += "               PES.RG,                                            ";
                    commando += "               PES.DSC_ENDERECO,                                  ";
                    commando += "               PES.ISN_CLUBE,                                     ";
                    commando += "               PES.ISN_CANIL,                                     ";
                    commando += "               PES.NOM_CANIL,                                     ";
                    commando += "               PES.ISN_ESTADO,                                    ";
                    commando += "               PES.ISN_ACESSO,                                    ";
                    commando += "               EST.DSC_ESTADO,                                    ";
                    commando += "               CID.DSC_CIDADE,                                    ";

                    commando += "               ACE.DSC_ACESSO,                                    ";
                    commando += "               CLB.DSC_CLUBE,                                    ";
                    commando += "               PES.NUM_CEP,                                    ";
                    commando += "               EST.DSC_ESTADO                                     ";
                    commando += "FROM           PES_PESSOA PES                                     ";
                    commando += "INNER JOIN     ACE_ACESSO ACE                                     ";
                    commando += "ON             PES.ISN_ACESSO = ACE.ISN_ACESSO                    ";
                    commando += "INNER JOIN     EST_ESTADO EST                                     ";
                    commando += "ON             PES.ISN_ESTADO = EST.ISN_ESTADO                    ";
                    commando += "LEFT JOIN      CID_CIDADE CID                                     ";
                    commando += "ON             PES.ISN_CIDADE = CID.ISN_CIDADE                    ";
                    commando += "LEFT JOIN      CLB_CLUBE CLB                                      ";
                    commando += "ON             PES.ISN_CLUBE = CLB.ISN_CLUBE                      ";
                    commando += "ORDER BY       PES.NOM_PESSOA                                     ";

                }
                else
                {

                    commando = "";
                    commando += "SELECT         PES.ISN_PESSOA,                                    ";
                    commando += "               PES.NOM_PESSOA,                                    ";
                    commando += "               PES.DSC_EMAIL,                                     ";
                    commando += "               PES.CPF,                                           ";
                    commando += "               PES.RG,                                            ";
                    commando += "               PES.DSC_ENDERECO,                                  ";
                    commando += "               PES.ISN_CLUBE,                                     ";
                    commando += "               PES.ISN_CANIL,                                     ";
                    commando += "               PES.NOM_CANIL,                                     ";
                    commando += "               PES.ISN_ESTADO,                                    ";
                    commando += "               PES.ISN_ACESSO,                                    ";
                    commando += "               EST.DSC_ESTADO,                                    ";
                    commando += "               CID.DSC_CIDADE,                                    ";
                    commando += "               CLB.DSC_CLUBE,                                     ";
                    commando += "               PES.NUM_CEP,                                    ";

                    commando += "               ACE.DSC_ACESSO,                                    ";
                    commando += "               EST.DSC_ESTADO                                     ";
                    commando += "FROM           PES_PESSOA PES                                     ";
                    commando += "INNER JOIN     ACE_ACESSO ACE                                     ";
                    commando += "ON             PES.ISN_ACESSO = ACE.ISN_ACESSO                    ";
                    commando += "INNER JOIN     EST_ESTADO EST                                     ";
                    commando += "ON             PES.ISN_ESTADO = EST.ISN_ESTADO                    ";
                    commando += "LEFT JOIN      CID_CIDADE CID                                     ";
                    commando += "ON             PES.ISN_CIDADE = CID.ISN_CIDADE                    ";
                    commando += "LEFT JOIN      CLB_CLUBE CLB                                      ";
                    commando += "ON             PES.ISN_CLUBE = CLB.ISN_CLUBE                      ";
                    commando += "WHERE          1 = 1                                              ";

                    if (this.nomPessoa != "")
                    {
                        commando += "AND          PES.NOM_PESSOA LIKE @nomPessoa                     ";
                    }

                    if (this.isnClube != 0)
                    {
                        commando += "AND          PES.ISN_CLUBE = @isnClube                     ";
                    }

                    commando += "ORDER BY       PES.NOM_PESSOA                                     ";


                }


                using (SqlCommand command = new SqlCommand(commando, con))
                {
                    if (this.nomPessoa != "")
                    {

                        command.Parameters.AddWithValue("@nomPessoa", "%" + this.nomPessoa.Trim() + "%");
                    }

                    if (this.isnClube != 0)
                    {
                        command.Parameters.Add("@isnClube", SqlDbType.Int);
                        command.Parameters["@isnClube"].Value = this.isnClube;
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            ListaDePessoas.Add(new Pessoa()
                            {
                                isnPessoa = Int32.Parse(reader["ISN_PESSOA"].ToString()),
                                nomPessoa = reader["NOM_PESSOA"].ToString(),
                                dscEmail = reader["DSC_EMAIL"].ToString(),
                                cpf = long.Parse(reader["CPF"].ToString()),
                                rg = long.Parse(reader["RG"].ToString()),
                                dscEndereco = reader["DSC_ENDERECO"].ToString(),
                                //isnClube = Int32.Parse(reader["ISN_CLUBE"].ToString()),
                                nomCanil = reader["NOM_CANIL"].ToString(),
                                isnEstado = Int32.Parse(reader["ISN_ESTADO"].ToString()),
                                isnAcesso = Int32.Parse(reader["ISN_ACESSO"].ToString()),
                                dscAcesso = reader["DSC_ACESSO"].ToString(),
                                dscCidade = reader["DSC_CIDADE"].ToString(),
                                dscClube = reader["DSC_CLUBE"].ToString(),
                                dscEstado = reader["DSC_ESTADO"].ToString(),
                                cep = reader["NUM_CEP"].ToString()

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
        return ListaDePessoas;


    }

    public List<PES_PESSOA> ListarPessoa(String nomPessoa)
    {
        DbCBKC = new DbSYSCBKCEntities();
        return DbCBKC.PES_PESSOA.Where(r => r.NOM_PESSOA == nomPessoa).ToList();
    }


    public void obterPessoaRedefinicaoSenha()
    {


        
        try
        {
            DbCBKC = new DbSYSCBKCEntities();
            DbCBKC.Database.Connection.Open();

            String cpfTratado = string.Format("{0:00000000000}", this.cpf);

            var usuarioBase = DbCBKC.PES_PESSOA.Where(r => r.DSC_EMAIL == this.dscEmail && r.CPF == cpfTratado).FirstOrDefault();
            if (usuarioBase != null)
            {
                
                this.isnPessoa = usuarioBase.ISN_PESSOA;
                this.nomPessoa = usuarioBase.NOM_PESSOA;
                this.dscEmail = usuarioBase.DSC_EMAIL;
                this.cpf = long.Parse(usuarioBase.CPF);
                this.rg = long.Parse(usuarioBase.RG);
                this.dscEndereco = usuarioBase.DSC_ENDERECO;
                if (usuarioBase.ISN_CLUBE != null)
                {
                    this.isnClube = (int)usuarioBase.ISN_CLUBE;
                }

                this.isnEstado = (int)usuarioBase.ISN_ESTADO;
                this.nomCanil = usuarioBase.NOM_CANIL;
                this.isnEstado = (int)usuarioBase.ISN_ESTADO;
                this.isnCidade = (int)usuarioBase.ISN_CIDADE;
                this.isnAcesso = (int)usuarioBase.ISN_ACESSO;
                this.dscCidade = usuarioBase.DSC_CIDADE;
                this.dscSenha = usuarioBase.DSC_SENHA;
                this.cep = usuarioBase.NUM_CEP;
                this.dscComplemento = usuarioBase.DSC_COMPLEMENTO;

            }


        }
        catch (Exception Ex)
        {
            throw Ex;
        }

    }

    public void ListarPessoaIsn()
    {

        if (this != null && this.isnPessoa != 0)
        {


            PES_PESSOA pes_pessoa = new PES_PESSOA();
            try
            {
                DbCBKC = new DbSYSCBKCEntities();
                DbCBKC.Database.Connection.Open();


                pes_pessoa = DbCBKC.PES_PESSOA.First(r => r.ISN_PESSOA == this.isnPessoa);
                this.nomPessoa = pes_pessoa.NOM_PESSOA;
                this.dscEmail = pes_pessoa.DSC_EMAIL;
                this.cpf = long.Parse(pes_pessoa.CPF);
                this.rg = long.Parse(pes_pessoa.RG);
                this.dscEndereco = pes_pessoa.DSC_ENDERECO;
                if (pes_pessoa.ISN_CLUBE != null)
                {
                    this.isnClube = (int)pes_pessoa.ISN_CLUBE;
                }

                this.isnEstado = (int)pes_pessoa.ISN_ESTADO;
                if(pes_pessoa.ISN_CANIL != null)
                {
                    this.isnCanil = (int)pes_pessoa.ISN_CANIL;
                }
                
                this.nomCanil = pes_pessoa.NOM_CANIL;
                this.isnEstado = (int)pes_pessoa.ISN_ESTADO;
                if (pes_pessoa.ISN_CIDADE != null)
                {
                    this.isnCidade = (int)pes_pessoa.ISN_CIDADE;
                }
                
                this.isnAcesso = (int)pes_pessoa.ISN_ACESSO;
                this.dscCidade = pes_pessoa.DSC_CIDADE;
                this.dscSenha = pes_pessoa.DSC_SENHA;
                this.cep = pes_pessoa.NUM_CEP;
                this.dscComplemento = pes_pessoa.DSC_COMPLEMENTO;

                if (pes_pessoa.DSC_BAIRRO != null)
                {
                    this.bairro = pes_pessoa.DSC_BAIRRO;
                }



            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }


    }

    public bool EfetuarLogin()
    {

        if (this != null && this.cpf != 0 && this.dscSenha != "")
        {
            String cpfTratado = string.Format("{0:00000000000}", this.cpf);

            string senhaEncriptada = util.EncriptaSenha(cpfTratado, this.dscSenha);

            try
            {
                DbCBKC = new DbSYSCBKCEntities();
                //PES_PESSOA pes_pessoa = null;


                var usuarioBase = DbCBKC.PES_PESSOA.Where(m => m.CPF == cpfTratado && m.DSC_SENHA == senhaEncriptada).FirstOrDefault();



                if (usuarioBase != null)
                {
                    // pes_pessoa = DbCBKC.PES_PESSOA.First(r => r.DSC_EMAIL == this.dscEmail && r.DSC_SENHA == this.dscSenha);
                    this.isnPessoa = usuarioBase.ISN_PESSOA;
                    this.nomPessoa = usuarioBase.NOM_PESSOA;
                    this.dscEmail = usuarioBase.DSC_EMAIL;
                    this.cpf = long.Parse(usuarioBase.CPF);
                    this.rg = long.Parse(usuarioBase.RG);
                    this.dscEndereco = usuarioBase.DSC_ENDERECO;
                    if (usuarioBase.ISN_CLUBE != null)
                    {
                        this.isnClube = (int)usuarioBase.ISN_CLUBE;
                    }

                    if (usuarioBase.ISN_CANIL != null)
                    {
                        this.isnCanil = (int)usuarioBase.ISN_CANIL;
                    }

                    this.isnEstado = (int)usuarioBase.ISN_ESTADO;
                    this.nomCanil = usuarioBase.NOM_CANIL;
                    this.isnEstado = (int)usuarioBase.ISN_ESTADO;
                    this.isnAcesso = (int)usuarioBase.ISN_ACESSO;
                    this.dscCidade = usuarioBase.DSC_CIDADE;
                    this.cep = usuarioBase.NUM_CEP;

                    return true;

                }
                else
                {
                    return false;
                }


            }
            catch (Exception Ex)
            {

                throw Ex;

            }

        }
        else
        {

            return false;
        }


    }


    public int NextId()
    {
        int chave;

        try
        {
            DbSYSCBKCEntities dbContext = new DbSYSCBKCEntities();

            if (!dbContext.PES_PESSOA.Any())
            {
                chave = 1;
            }
            else
            {
                chave = dbContext.PES_PESSOA.Max(r => r.ISN_PESSOA) + 1;
            }


        }
        catch (Exception ex)
        {
            throw ex;
        }

        return chave;
    }



    public void UpdateData(bool redefineSenha)
    {


        try
        {
            DbCBKC = new DbSYSCBKCEntities();
            DbCBKC.Database.Connection.Open();

            PES_PESSOA update_pessoa = DbCBKC.PES_PESSOA.FirstOrDefault(r => r.ISN_PESSOA == this.isnPessoa);

            update_pessoa.ISN_PESSOA = this.isnPessoa;
            update_pessoa.NOM_PESSOA = this.nomPessoa;
            update_pessoa.DSC_EMAIL = this.dscEmail;
            if (redefineSenha)
            {
                update_pessoa.DSC_SENHA = this.dscSenha;
            }

            String cpfTratado = string.Format("{0:00000000000}", this.cpf);

            update_pessoa.CPF = cpfTratado;
            update_pessoa.RG = this.rg.ToString();
            update_pessoa.DSC_ENDERECO = this.dscEndereco;
            if (this.isnClube != 0)
            {
                update_pessoa.ISN_CLUBE = this.isnClube;
            }
            else
            {
                update_pessoa.ISN_CLUBE = null;
            }

            if (this.isnCanil != 0)
            {
                update_pessoa.ISN_CANIL = this.isnCanil;
            }
            else
            {
                update_pessoa.ISN_CANIL = null;
            }

            update_pessoa.ISN_ESTADO = this.isnEstado;
            update_pessoa.ISN_CIDADE = this.isnCidade;
            update_pessoa.NOM_CANIL = this.nomCanil;
            update_pessoa.ISN_ACESSO = this.isnAcesso;
            update_pessoa.DSC_CIDADE = this.dscCidade;
            update_pessoa.NUM_CEP = this.cep;
            update_pessoa.DSC_BAIRRO = this.bairro;


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