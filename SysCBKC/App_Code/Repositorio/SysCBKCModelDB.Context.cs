//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Repositorio
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DbSYSCBKCEntities : DbContext
    {
        public DbSYSCBKCEntities()
            : base("name=DbSYSCBKCEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ACE_ACESSO> ACE_ACESSO { get; set; }
        public virtual DbSet<CLB_CLUBE> CLB_CLUBE { get; set; }
        public virtual DbSet<CRC_COR_RACA> CRC_COR_RACA { get; set; }
        public virtual DbSet<DAR_DOMINIO_ATRIBUTO> DAR_DOMINIO_ATRIBUTO { get; set; }
        public virtual DbSet<EST_ESTADO> EST_ESTADO { get; set; }
        public virtual DbSet<FIL_FILHOTE> FIL_FILHOTE { get; set; }
        public virtual DbSet<NIN_NINHADA> NIN_NINHADA { get; set; }
        public virtual DbSet<PES_PESSOA> PES_PESSOA { get; set; }
        public virtual DbSet<PRS_PRECO_SERVICO> PRS_PRECO_SERVICO { get; set; }
        public virtual DbSet<RAC_RACA> RAC_RACA { get; set; }
        public virtual DbSet<REG_REGIAO> REG_REGIAO { get; set; }
        public virtual DbSet<SER_SERVICO> SER_SERVICO { get; set; }
        public virtual DbSet<SOL_SOLICITACAO> SOL_SOLICITACAO { get; set; }
        public virtual DbSet<VAR_VARIEDADE_RACA> VAR_VARIEDADE_RACA { get; set; }
        public virtual DbSet<PRD_PARAMETRO_DESCONTO> PRD_PARAMETRO_DESCONTO { get; set; }
        public virtual DbSet<FAT_FATURA> FAT_FATURA { get; set; }
        public virtual DbSet<TRA_TRANSFERENCIA> TRA_TRANSFERENCIA { get; set; }
        public virtual DbSet<ATR_ANEXO_TRANSFERENCIA> ATR_ANEXO_TRANSFERENCIA { get; set; }
        public virtual DbSet<ANI_ANEXO_NINHADA> ANI_ANEXO_NINHADA { get; set; }
        public virtual DbSet<BAN_BANDEIRA> BAN_BANDEIRA { get; set; }
        public virtual DbSet<OPG_OPCAO_PAGAMENTO> OPG_OPCAO_PAGAMENTO { get; set; }
        public virtual DbSet<CAR_CARRINHO> CAR_CARRINHO { get; set; }
        public virtual DbSet<CID_CIDADE> CID_CIDADE { get; set; }
        public virtual DbSet<CUP_CUPOM> CUP_CUPOM { get; set; }
        public virtual DbSet<ITF_ITEM_FATURA> ITF_ITEM_FATURA { get; set; }
        public virtual DbSet<CAN_CANIL> CAN_CANIL { get; set; }
    }
}
