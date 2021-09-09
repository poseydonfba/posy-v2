using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Tool.hbm2ddl;
using Posy.V2.Domain.Entities;
using Posy.V2.Infra.Data.EntityConfig;
using Posy.V2.Infra.Data.EntityConfig.Overrides;
using Posy.V2.Infra.Data.NHibernate.Context.Configuration;
using System;
using System.IO;
using Environment = NHibernate.Cfg.Environment;

namespace Posy.V2.Infra.Data.Context
{
    public class DatabaseContext : IDisposable
    {
        private readonly ISessionFactory _sessionFactory;
        private ITransaction _transaction;

        public ISession Session { get; private set; }

        public DatabaseContext()
        {
            #region FUNCIONAL

            //var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            //var fluentConfig = Fluently.Configure()
            //    .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString))
            //    .Mappings(m => m.AutoMappings
            //                .Add(AutoMap.AssemblyOf<Usuario>(new StoreConfiguration())
            //                .IgnoreBase<EntityBase>()
            //                .UseOverridesFromAssemblyOf<UsuarioOverride>()));

            #endregion

            FluentConfiguration fluentConfig = Fluently.Configure(/*configuration*/)
                .Database(() =>
                          MsSqlConfiguration.MsSql2012       /// Use MsSql2012 Dialect
                              .ConnectionString(             /// Use a string de conexão do app/web.config
                                    c => c.FromConnectionStringWithKey("DefaultConnection"))
                              .ShowSql()   /// Exibir SQL gerado pelo NH no console.
                              .FormatSql() /// Formatar / Tabify SQL no console para facilitar a leitura.
                                           /// Auto escape nomes de tabelas para nomeação segura.
                                           ///    In SQL Server, this uses '[', and ']' not quotes.
                                           /// Vale a pena mencionar: 'Raw' nos permite adicionar valores de configuração 
                                           ///    como strings de chave / valor como mapeamentos XML,
                                           ///   e ambiente Hbm2DDLKeyWords são classes NH não FNH
                                           ///.Raw(Environment.Hbm2ddlKeyWords, Hbm2DDLKeyWords.AutoQuote.ToString())
                )
                .Mappings(
                    m => m.AutoMappings /// Use o mapeamento por convenção.
                                        /// Mapeie todos os tipos no ASSEMBLY ATUAL
                                        ///     usando as opções padrão da classe StoreConfiguration.
                             .Add(AutoMap.AssemblyOf<Usuario>(new StoreConfiguration())
                                      /// Ignora uma classe dentro do namespace 
                                      /// das entidades
                                      //.IgnoreBase<GlobalUser>()
                                      .IgnoreBase<EntityBase>()
                                      //.Conventions.Add(FluentNHibernate.Conventions.Helpers.DefaultLazy.Never())
                                      .UseOverridesFromAssemblyOf<UsuarioOverride>())
                             /// Display the mappings XML in the console.
                             /// Typically you should do little XML stuff to output that.
                             /// It can also output to text file by specifying path.
                             .ExportTo(/*@"schema"*/Console.Out));

            var cfg = fluentConfig.BuildConfiguration();

            /**
            * Cria as entidades no banco de dados
            **/
            //var exporter = new SchemaExport(cfg);
            //exporter.Execute(true, true, false);

            /// Tudo isso foi feito usando o objeto Fluent NHibernate para construir a configuração
            /// Então, agora, construa o objeto de Configuração do NHibernate.
            _sessionFactory = cfg.BuildSessionFactory();

            Session = _sessionFactory.OpenSession();
        }

        public void BeginTransaction()
        {
            _transaction = Session.BeginTransaction();
        }

        public int SaveChanges(GlobalUser userAudit)
        {
            try
            {
                /**
                * Commit transaction if there is one active
                **/
            if (_transaction != null && _transaction.IsActive)
                    _transaction.Commit();
            }
            catch
            {
                /**
                * Rollback if there was an exception
                **/
                // 
                if (_transaction != null && _transaction.IsActive)
                    _transaction.Rollback();

                throw;
            }

            return -1;
        }

        public void Dispose()
        {
            Session.Dispose();
            _sessionFactory.Dispose();

            if (_transaction != null)
                _transaction.Dispose();
        }
    }
}

