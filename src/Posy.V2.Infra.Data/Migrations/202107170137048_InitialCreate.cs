namespace Posy.V2.Infra.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AmizadeAudit",
                c => new
                    {
                        AmizadeAuditId = c.Guid(nullable: false),
                        AmizadeId = c.Guid(nullable: false),
                        SolicitadoPorId = c.Guid(nullable: false),
                        SolicitadoParaId = c.Guid(nullable: false),
                        DataSolicitacao = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        DataResposta = c.DateTime(precision: 7, storeType: "datetime2"),
                        StatusSolicitacao = c.Int(nullable: false),
                        Uer = c.Guid(),
                        Der = c.DateTime(precision: 7, storeType: "datetime2"),
                        AuditStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditStartOperation = c.String(maxLength: 100),
                        AuditStartUserID = c.Guid(nullable: false),
                        AuditStartUsername = c.String(maxLength: 100),
                        AuditStartTransactionGUID = c.Guid(nullable: false),
                        AuditEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditEndOperation = c.String(maxLength: 100),
                        AuditEndUserID = c.Guid(nullable: false),
                        AuditEndUsername = c.String(maxLength: 100),
                        AuditEndTransactionGUID = c.Guid(nullable: false),
                        ChangedColumns = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.AmizadeAuditId);
            
            CreateTable(
                "dbo.ComunidadeAudit",
                c => new
                    {
                        ComunidadeAuditId = c.Guid(nullable: false),
                        ComunidadeId = c.Guid(nullable: false),
                        UsuarioId = c.Guid(nullable: false),
                        Alias = c.String(maxLength: 100),
                        Nome = c.String(maxLength: 100),
                        TipoComunidade = c.Int(nullable: false),
                        CategoriaId = c.Int(nullable: false),
                        DescricaoPerfil = c.Binary(),
                        Dir = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Uar = c.Guid(nullable: false),
                        Dar = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Uer = c.Guid(),
                        Der = c.DateTime(precision: 7, storeType: "datetime2"),
                        AuditStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditStartOperation = c.String(maxLength: 100),
                        AuditStartUserID = c.Guid(nullable: false),
                        AuditStartUsername = c.String(maxLength: 100),
                        AuditStartTransactionGUID = c.Guid(nullable: false),
                        AuditEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditEndOperation = c.String(maxLength: 100),
                        AuditEndUserID = c.Guid(nullable: false),
                        AuditEndUsername = c.String(maxLength: 100),
                        AuditEndTransactionGUID = c.Guid(nullable: false),
                        ChangedColumns = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.ComunidadeAuditId);
            
            CreateTable(
                "dbo.ConnectionAudit",
                c => new
                    {
                        ConnectionAuditId = c.Guid(nullable: false),
                        ConnectionId = c.String(maxLength: 100),
                        UsuarioId = c.Guid(nullable: false),
                        UserAgent = c.String(maxLength: 100),
                        Connected = c.Boolean(nullable: false),
                        DataConnected = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        DataDisconnected = c.DateTime(precision: 7, storeType: "datetime2"),
                        TipoDesconexao = c.Int(),
                        AuditStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditStartOperation = c.String(maxLength: 100),
                        AuditStartUserID = c.Guid(nullable: false),
                        AuditStartUsername = c.String(maxLength: 100),
                        AuditStartTransactionGUID = c.Guid(nullable: false),
                        AuditEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditEndOperation = c.String(maxLength: 100),
                        AuditEndUserID = c.Guid(nullable: false),
                        AuditEndUsername = c.String(maxLength: 100),
                        AuditEndTransactionGUID = c.Guid(nullable: false),
                        ChangedColumns = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.ConnectionAuditId);
            
            CreateTable(
                "dbo.DepoimentoAudit",
                c => new
                    {
                        DepoimentoAuditId = c.Guid(nullable: false),
                        DepoimentoId = c.Guid(nullable: false),
                        EnviadoPorId = c.Guid(nullable: false),
                        EnviadoParaId = c.Guid(nullable: false),
                        DescricaoDepoimento = c.Binary(),
                        DataDepoimento = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        StatusDepoimento = c.Int(nullable: false),
                        DataResposta = c.DateTime(precision: 7, storeType: "datetime2"),
                        Uer = c.Guid(),
                        Der = c.DateTime(precision: 7, storeType: "datetime2"),
                        AuditStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditStartOperation = c.String(maxLength: 100),
                        AuditStartUserID = c.Guid(nullable: false),
                        AuditStartUsername = c.String(maxLength: 100),
                        AuditStartTransactionGUID = c.Guid(nullable: false),
                        AuditEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditEndOperation = c.String(maxLength: 100),
                        AuditEndUserID = c.Guid(nullable: false),
                        AuditEndUsername = c.String(maxLength: 100),
                        AuditEndTransactionGUID = c.Guid(nullable: false),
                        ChangedColumns = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.DepoimentoAuditId);
            
            CreateTable(
                "dbo.MembroAudit",
                c => new
                    {
                        MembroAuditId = c.Guid(nullable: false),
                        MembroId = c.Guid(nullable: false),
                        ComunidadeId = c.Guid(nullable: false),
                        UsuarioMembroId = c.Guid(nullable: false),
                        DataSolicitacao = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        DataResposta = c.DateTime(precision: 7, storeType: "datetime2"),
                        UsuarioRespostaId = c.Guid(),
                        StatusSolicitacao = c.Int(nullable: false),
                        Uer = c.Guid(),
                        Der = c.DateTime(precision: 7, storeType: "datetime2"),
                        AuditStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditStartOperation = c.String(maxLength: 100),
                        AuditStartUserID = c.Guid(nullable: false),
                        AuditStartUsername = c.String(maxLength: 100),
                        AuditStartTransactionGUID = c.Guid(nullable: false),
                        AuditEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditEndOperation = c.String(maxLength: 100),
                        AuditEndUserID = c.Guid(nullable: false),
                        AuditEndUsername = c.String(maxLength: 100),
                        AuditEndTransactionGUID = c.Guid(nullable: false),
                        ChangedColumns = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.MembroAuditId);
            
            CreateTable(
                "dbo.ModeradorAudit",
                c => new
                    {
                        ModeradorAuditId = c.Guid(nullable: false),
                        ModeradorId = c.Guid(nullable: false),
                        ComunidadeId = c.Guid(nullable: false),
                        UsuarioModeradorId = c.Guid(nullable: false),
                        UsuarioOperacaoId = c.Guid(nullable: false),
                        DataOperacao = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Uer = c.Guid(),
                        Der = c.DateTime(precision: 7, storeType: "datetime2"),
                        AuditStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditStartOperation = c.String(maxLength: 100),
                        AuditStartUserID = c.Guid(nullable: false),
                        AuditStartUsername = c.String(maxLength: 100),
                        AuditStartTransactionGUID = c.Guid(nullable: false),
                        AuditEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditEndOperation = c.String(maxLength: 100),
                        AuditEndUserID = c.Guid(nullable: false),
                        AuditEndUsername = c.String(maxLength: 100),
                        AuditEndTransactionGUID = c.Guid(nullable: false),
                        ChangedColumns = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.ModeradorAuditId);
            
            CreateTable(
                "dbo.PerfilAudit",
                c => new
                    {
                        PerfilAuditId = c.Guid(nullable: false),
                        UsuarioId = c.Guid(nullable: false),
                        Nome = c.String(maxLength: 100),
                        Sobrenome = c.String(maxLength: 100),
                        Alias = c.String(maxLength: 100),
                        PaisId = c.String(maxLength: 100),
                        DataNascimento = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Sexo = c.Int(nullable: false),
                        EstadoCivil = c.Int(nullable: false),
                        FrasePerfil = c.Binary(),
                        DescricaoPerfil = c.Binary(),
                        Dar = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditStartOperation = c.String(maxLength: 100),
                        AuditStartUserID = c.Guid(nullable: false),
                        AuditStartUsername = c.String(maxLength: 100),
                        AuditStartTransactionGUID = c.Guid(nullable: false),
                        AuditEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditEndOperation = c.String(maxLength: 100),
                        AuditEndUserID = c.Guid(nullable: false),
                        AuditEndUsername = c.String(maxLength: 100),
                        AuditEndTransactionGUID = c.Guid(nullable: false),
                        ChangedColumns = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.PerfilAuditId);
            
            CreateTable(
                "dbo.PostOcultoAudit",
                c => new
                    {
                        PostOcultoAuditId = c.Guid(nullable: false),
                        PostOcultoId = c.Guid(nullable: false),
                        UsuarioId = c.Guid(nullable: false),
                        PostPerfilId = c.Guid(nullable: false),
                        Data = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        StatusPost = c.Int(nullable: false),
                        AuditStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditStartOperation = c.String(maxLength: 100),
                        AuditStartUserID = c.Guid(nullable: false),
                        AuditStartUsername = c.String(maxLength: 100),
                        AuditStartTransactionGUID = c.Guid(nullable: false),
                        AuditEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditEndOperation = c.String(maxLength: 100),
                        AuditEndUserID = c.Guid(nullable: false),
                        AuditEndUsername = c.String(maxLength: 100),
                        AuditEndTransactionGUID = c.Guid(nullable: false),
                        ChangedColumns = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.PostOcultoAuditId);
            
            CreateTable(
                "dbo.PostPerfilAudit",
                c => new
                    {
                        PostPerfilAuditId = c.Guid(nullable: false),
                        PostPerfilId = c.Guid(nullable: false),
                        UsuarioId = c.Guid(nullable: false),
                        DescricaoPost = c.Binary(),
                        DataPost = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Der = c.DateTime(precision: 7, storeType: "datetime2"),
                        AuditStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditStartOperation = c.String(maxLength: 100),
                        AuditStartUserID = c.Guid(nullable: false),
                        AuditStartUsername = c.String(maxLength: 100),
                        AuditStartTransactionGUID = c.Guid(nullable: false),
                        AuditEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditEndOperation = c.String(maxLength: 100),
                        AuditEndUserID = c.Guid(nullable: false),
                        AuditEndUsername = c.String(maxLength: 100),
                        AuditEndTransactionGUID = c.Guid(nullable: false),
                        ChangedColumns = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.PostPerfilAuditId);
            
            CreateTable(
                "dbo.PostPerfilBloqueadoAudit",
                c => new
                    {
                        PostPerfilBloqueadoAuditId = c.Guid(nullable: false),
                        PostPerfilBloqueadoId = c.Guid(nullable: false),
                        UsuarioId = c.Guid(nullable: false),
                        UsuarioIdBloqueado = c.Guid(nullable: false),
                        DataBloqueio = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Uer = c.Guid(),
                        Der = c.DateTime(precision: 7, storeType: "datetime2"),
                        AuditStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditStartOperation = c.String(maxLength: 100),
                        AuditStartUserID = c.Guid(nullable: false),
                        AuditStartUsername = c.String(maxLength: 100),
                        AuditStartTransactionGUID = c.Guid(nullable: false),
                        AuditEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditEndOperation = c.String(maxLength: 100),
                        AuditEndUserID = c.Guid(nullable: false),
                        AuditEndUsername = c.String(maxLength: 100),
                        AuditEndTransactionGUID = c.Guid(nullable: false),
                        ChangedColumns = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.PostPerfilBloqueadoAuditId);
            
            CreateTable(
                "dbo.PostPerfilComentarioAudit",
                c => new
                    {
                        PostPerfilComentarioAuditId = c.Guid(nullable: false),
                        PostPerfilComentarioId = c.Guid(nullable: false),
                        PostPerfilId = c.Guid(nullable: false),
                        UsuarioId = c.Guid(nullable: false),
                        Comentario = c.Binary(),
                        Data = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Uer = c.Guid(),
                        Der = c.DateTime(precision: 7, storeType: "datetime2"),
                        AuditStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditStartOperation = c.String(maxLength: 100),
                        AuditStartUserID = c.Guid(nullable: false),
                        AuditStartUsername = c.String(maxLength: 100),
                        AuditStartTransactionGUID = c.Guid(nullable: false),
                        AuditEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditEndOperation = c.String(maxLength: 100),
                        AuditEndUserID = c.Guid(nullable: false),
                        AuditEndUsername = c.String(maxLength: 100),
                        AuditEndTransactionGUID = c.Guid(nullable: false),
                        ChangedColumns = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.PostPerfilComentarioAuditId);
            
            CreateTable(
                "dbo.PrivacidadeAudit",
                c => new
                    {
                        PrivacidadeAuditId = c.Guid(nullable: false),
                        UsuarioId = c.Guid(nullable: false),
                        VerRecado = c.Int(nullable: false),
                        EscreverRecado = c.Int(nullable: false),
                        AuditStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditStartOperation = c.String(maxLength: 100),
                        AuditStartUserID = c.Guid(nullable: false),
                        AuditStartUsername = c.String(maxLength: 100),
                        AuditStartTransactionGUID = c.Guid(nullable: false),
                        AuditEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditEndOperation = c.String(maxLength: 100),
                        AuditEndUserID = c.Guid(nullable: false),
                        AuditEndUsername = c.String(maxLength: 100),
                        AuditEndTransactionGUID = c.Guid(nullable: false),
                        ChangedColumns = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.PrivacidadeAuditId);
            
            CreateTable(
                "dbo.RecadoComentarioAudit",
                c => new
                    {
                        RecadoComentarioAuditId = c.Guid(nullable: false),
                        RecadoComentarioId = c.Guid(nullable: false),
                        RecadoId = c.Guid(nullable: false),
                        UsuarioId = c.Guid(nullable: false),
                        DescricaoComentario = c.Binary(),
                        DataComentario = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Uer = c.Guid(),
                        Der = c.DateTime(precision: 7, storeType: "datetime2"),
                        AuditStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditStartOperation = c.String(maxLength: 100),
                        AuditStartUserID = c.Guid(nullable: false),
                        AuditStartUsername = c.String(maxLength: 100),
                        AuditStartTransactionGUID = c.Guid(nullable: false),
                        AuditEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditEndOperation = c.String(maxLength: 100),
                        AuditEndUserID = c.Guid(nullable: false),
                        AuditEndUsername = c.String(maxLength: 100),
                        AuditEndTransactionGUID = c.Guid(nullable: false),
                        ChangedColumns = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.RecadoComentarioAuditId);
            
            CreateTable(
                "dbo.RecadoAudit",
                c => new
                    {
                        RecadoAuditId = c.Guid(nullable: false),
                        RecadoId = c.Guid(nullable: false),
                        EnviadoPorId = c.Guid(nullable: false),
                        EnviadoParaId = c.Guid(nullable: false),
                        DescricaoRecado = c.Binary(),
                        DataRecado = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        StatusRecado = c.Int(nullable: false),
                        DataLeitura = c.DateTime(precision: 7, storeType: "datetime2"),
                        Uer = c.Guid(),
                        Der = c.DateTime(precision: 7, storeType: "datetime2"),
                        AuditStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditStartOperation = c.String(maxLength: 100),
                        AuditStartUserID = c.Guid(nullable: false),
                        AuditStartUsername = c.String(maxLength: 100),
                        AuditStartTransactionGUID = c.Guid(nullable: false),
                        AuditEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditEndOperation = c.String(maxLength: 100),
                        AuditEndUserID = c.Guid(nullable: false),
                        AuditEndUsername = c.String(maxLength: 100),
                        AuditEndTransactionGUID = c.Guid(nullable: false),
                        ChangedColumns = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.RecadoAuditId);
            
            CreateTable(
                "dbo.StorieAudit",
                c => new
                    {
                        StorieAuditId = c.Guid(nullable: false),
                        StorieId = c.Guid(nullable: false),
                        UsuarioId = c.Guid(nullable: false),
                        StorieType = c.Int(nullable: false),
                        Length = c.Int(nullable: false),
                        Src = c.String(maxLength: 100),
                        Preview = c.String(maxLength: 100),
                        Link = c.String(maxLength: 100),
                        LinkText = c.String(maxLength: 100),
                        Seen = c.String(maxLength: 100),
                        Time = c.String(maxLength: 100),
                        DataStorie = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditStartOperation = c.String(maxLength: 100),
                        AuditStartUserID = c.Guid(nullable: false),
                        AuditStartUsername = c.String(maxLength: 100),
                        AuditStartTransactionGUID = c.Guid(nullable: false),
                        AuditEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditEndOperation = c.String(maxLength: 100),
                        AuditEndUserID = c.Guid(nullable: false),
                        AuditEndUsername = c.String(maxLength: 100),
                        AuditEndTransactionGUID = c.Guid(nullable: false),
                        ChangedColumns = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.StorieAuditId);
            
            CreateTable(
                "dbo.TopicoAudit",
                c => new
                    {
                        TopicoAuditId = c.Guid(nullable: false),
                        TopicoId = c.Guid(nullable: false),
                        ComunidadeId = c.Guid(nullable: false),
                        UsuarioId = c.Guid(nullable: false),
                        DataTopico = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Titulo = c.String(maxLength: 100),
                        Descricao = c.Binary(),
                        TipoTopico = c.Int(nullable: false),
                        Uer = c.Guid(),
                        Der = c.DateTime(precision: 7, storeType: "datetime2"),
                        Uerp = c.Guid(),
                        Derp = c.DateTime(precision: 7, storeType: "datetime2"),
                        AuditStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditStartOperation = c.String(maxLength: 100),
                        AuditStartUserID = c.Guid(nullable: false),
                        AuditStartUsername = c.String(maxLength: 100),
                        AuditStartTransactionGUID = c.Guid(nullable: false),
                        AuditEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditEndOperation = c.String(maxLength: 100),
                        AuditEndUserID = c.Guid(nullable: false),
                        AuditEndUsername = c.String(maxLength: 100),
                        AuditEndTransactionGUID = c.Guid(nullable: false),
                        ChangedColumns = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.TopicoAuditId);
            
            CreateTable(
                "dbo.TopicoPostAudit",
                c => new
                    {
                        TopicoPostAuditId = c.Guid(nullable: false),
                        TopicoPostId = c.Guid(nullable: false),
                        TopicoId = c.Guid(nullable: false),
                        UsuarioId = c.Guid(nullable: false),
                        DataPost = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Descricao = c.Binary(),
                        Uer = c.Guid(),
                        Der = c.DateTime(precision: 7, storeType: "datetime2"),
                        Uerp = c.Guid(),
                        Derp = c.DateTime(precision: 7, storeType: "datetime2"),
                        AuditStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditStartOperation = c.String(maxLength: 100),
                        AuditStartUserID = c.Guid(nullable: false),
                        AuditStartUsername = c.String(maxLength: 100),
                        AuditStartTransactionGUID = c.Guid(nullable: false),
                        AuditEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditEndOperation = c.String(maxLength: 100),
                        AuditEndUserID = c.Guid(nullable: false),
                        AuditEndUsername = c.String(maxLength: 100),
                        AuditEndTransactionGUID = c.Guid(nullable: false),
                        ChangedColumns = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.TopicoPostAuditId);
            
            CreateTable(
                "dbo.UsuarioAudit",
                c => new
                    {
                        UsuarioAuditId = c.Guid(nullable: false),
                        UsuarioId = c.Guid(nullable: false),
                        Dir = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Der = c.DateTime(precision: 7, storeType: "datetime2"),
                        Email = c.String(maxLength: 100),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(maxLength: 100),
                        SecurityStamp = c.String(maxLength: 100),
                        PhoneNumber = c.String(maxLength: 100),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(precision: 7, storeType: "datetime2"),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(maxLength: 100),
                        CurrencySymbol = c.String(maxLength: 100),
                        Language = c.String(maxLength: 100),
                        LongDateFormat = c.String(maxLength: 100),
                        ShortDateFormat = c.String(maxLength: 100),
                        AuditStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditStartOperation = c.String(maxLength: 100),
                        AuditStartUserID = c.Guid(nullable: false),
                        AuditStartUsername = c.String(maxLength: 100),
                        AuditStartTransactionGUID = c.Guid(nullable: false),
                        AuditEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditEndOperation = c.String(maxLength: 100),
                        AuditEndUserID = c.Guid(nullable: false),
                        AuditEndUsername = c.String(maxLength: 100),
                        AuditEndTransactionGUID = c.Guid(nullable: false),
                        ChangedColumns = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.UsuarioAuditId);
            
            CreateTable(
                "dbo.VideoComentarioAudit",
                c => new
                    {
                        VideoComentarioAuditId = c.Guid(nullable: false),
                        VideoComentarioId = c.Guid(nullable: false),
                        VideoId = c.Guid(nullable: false),
                        UsuarioId = c.Guid(nullable: false),
                        DescricaoComentario = c.Binary(),
                        DataComentario = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Uer = c.Guid(),
                        Der = c.DateTime(precision: 7, storeType: "datetime2"),
                        AuditStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditStartOperation = c.String(maxLength: 100),
                        AuditStartUserID = c.Guid(nullable: false),
                        AuditStartUsername = c.String(maxLength: 100),
                        AuditStartTransactionGUID = c.Guid(nullable: false),
                        AuditEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditEndOperation = c.String(maxLength: 100),
                        AuditEndUserID = c.Guid(nullable: false),
                        AuditEndUsername = c.String(maxLength: 100),
                        AuditEndTransactionGUID = c.Guid(nullable: false),
                        ChangedColumns = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.VideoComentarioAuditId);
            
            CreateTable(
                "dbo.VideoAudit",
                c => new
                    {
                        VideoAuditId = c.Guid(nullable: false),
                        VideoId = c.Guid(nullable: false),
                        UsuarioId = c.Guid(nullable: false),
                        Url = c.String(maxLength: 100),
                        NomeVideo = c.Binary(),
                        DataVideo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Der = c.DateTime(precision: 7, storeType: "datetime2"),
                        AuditStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditStartOperation = c.String(maxLength: 100),
                        AuditStartUserID = c.Guid(nullable: false),
                        AuditStartUsername = c.String(maxLength: 100),
                        AuditStartTransactionGUID = c.Guid(nullable: false),
                        AuditEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditEndOperation = c.String(maxLength: 100),
                        AuditEndUserID = c.Guid(nullable: false),
                        AuditEndUsername = c.String(maxLength: 100),
                        AuditEndTransactionGUID = c.Guid(nullable: false),
                        ChangedColumns = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.VideoAuditId);
            
            CreateTable(
                "dbo.VisitantePerfilAudit",
                c => new
                    {
                        VisitantePerfilAuditId = c.Guid(nullable: false),
                        VisitantePerfilId = c.Guid(nullable: false),
                        VisitanteId = c.Guid(nullable: false),
                        VisitadoId = c.Guid(nullable: false),
                        DataVisita = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditStartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditStartOperation = c.String(maxLength: 100),
                        AuditStartUserID = c.Guid(nullable: false),
                        AuditStartUsername = c.String(maxLength: 100),
                        AuditStartTransactionGUID = c.Guid(nullable: false),
                        AuditEndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AuditEndOperation = c.String(maxLength: 100),
                        AuditEndUserID = c.Guid(nullable: false),
                        AuditEndUsername = c.String(maxLength: 100),
                        AuditEndTransactionGUID = c.Guid(nullable: false),
                        ChangedColumns = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.VisitantePerfilAuditId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.VisitantePerfilAudit");
            DropTable("dbo.VideoAudit");
            DropTable("dbo.VideoComentarioAudit");
            DropTable("dbo.UsuarioAudit");
            DropTable("dbo.TopicoPostAudit");
            DropTable("dbo.TopicoAudit");
            DropTable("dbo.StorieAudit");
            DropTable("dbo.RecadoAudit");
            DropTable("dbo.RecadoComentarioAudit");
            DropTable("dbo.PrivacidadeAudit");
            DropTable("dbo.PostPerfilComentarioAudit");
            DropTable("dbo.PostPerfilBloqueadoAudit");
            DropTable("dbo.PostPerfilAudit");
            DropTable("dbo.PostOcultoAudit");
            DropTable("dbo.PerfilAudit");
            DropTable("dbo.ModeradorAudit");
            DropTable("dbo.MembroAudit");
            DropTable("dbo.DepoimentoAudit");
            DropTable("dbo.ConnectionAudit");
            DropTable("dbo.ComunidadeAudit");
            DropTable("dbo.AmizadeAudit");
        }
    }
}
