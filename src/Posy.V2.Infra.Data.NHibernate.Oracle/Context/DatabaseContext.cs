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
            #region OTHERS
            //_sessionFactory = Fluently.Configure()
            //    .Database(MsSqlConfiguration.MsSql2012
            //      .ConnectionString(System.Configuration.ConfigurationManager.AppSettings["DefaultConnectionNHibernate"])
            //                  .ShowSql()
            //    )
            //   .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Perfil>())
            //   .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(false, false))
            //   .BuildSessionFactory();

            //Fluently.Configure().
            //    Database(OracleClientConfiguration.
            //            Oracle10.Dialect<Oracle10gDialect>().
            //            DefaultSchema(defaultSchema)



            //_sessionFactory = Fluently.Configure()
            //    .Database(
            //        OracleClientConfiguration.Oracle10
            //            .ShowSql()
            //            .ConnectionString(x => x.FromConnectionStringWithKey("DefaultConnectionNHibernate"))
            //            .DefaultSchema("POSEYDON"))
            //            //.UseReflectionOptimizer()
            //            //.Provider<NHibernate.Connection.DriverConnectionProvider>()
            //            //.Driver<NHibernate.Driver.OracleManagedDataClientDriver>())
            //    .Mappings(m => {

            //        m.HbmMappings
            //            .AddFromAssemblyOf<Usuario>();

            //        m.FluentMappings
            //            .AddFromAssemblyOf<Perfil>();
            //            //.Conventions.Add<OracleDateTimeTypeConvention>();

            //        m.HbmMappings
            //            .AddFromAssemblyOf<Privacidade>();
            //    })
            //    .ExposeConfiguration(config => new SchemaUpdate(config).Execute(false, true))
            //    .BuildSessionFactory();


            //_sessionFactory = Fluently.Configure()
            //    .Database(
            //        OracleClientConfiguration.Oracle10.ConnectionString(x => x.FromConnectionStringWithKey("DefaultConnectionNHibernate")).DefaultSchema("POSEYDON")
            //        )
            //    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Perfil>())
            //    //.Mappings(x => x.AutoMappings.Add(
            //    //    AutoMap.AssemblyOf<Perfil>(new AutomappingConfiguration()).UseOverridesFromAssemblyOf<PerfilOverride>()))
            //    .ExposeConfiguration(config => new SchemaUpdate(config).Execute(false, true))
            //    .BuildSessionFactory();



            #endregion

            _sessionFactory = ConfigureNHibernate();

            Session = _sessionFactory.OpenSession();
        }

        private ISessionFactory ExemploConfiguracao1()
        {
            //Comenzamos la configuración a través de FluentNHibernate
            var f = Fluently.Configure();

            //Configuramos la cadena de conexión a la base de datos en este caso utilizamos ORACLE XE 11
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionNHibernate"].ConnectionString; //"Data Source=XE;User Id=poseydon;Password=apocalipse1;Min Pool Size=10;Connection Lifetime=240";
            f.Database(OracleClientConfiguration.Oracle10.ConnectionString(connString).DefaultSchema("POSEYDON").ShowSql());

            //Mapeo de clases, con solo hacer una referencia a una clase nos mapeara todas las clases
            //f.Mappings(m => {
            //    //m.FluentMappings.AddFromAssemblyOf<Usuario>();
            //    m.FluentMappings.AddFromAssemblyOf<Perfil>();
            //    //m.FluentMappings.AddFromAssemblyOf<Privacidade>();
            //});
            f.Mappings(m => m.FluentMappings.Add(typeof(PerfilConfig)));

            //Con esta configuración cualquier modificación en nuestro modelo se aplicará automaticamente en 
            f.ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(true, true));
            //Esta linea de código la descomentaremos y comentaremos la anterios si queremos resetear toda nuestra base de datos
            //f.ExposeConfiguration(cfg => new SchemaExport(cfg).Create(true,true));

            //Por último creamos el session factory
            return f.BuildSessionFactory();
        }

        private ISessionFactory ConfigureNHibernate()
        {
            //if (Directory.Exists("schema"))
            //    Directory.Delete("schema", true);
            //Directory.CreateDirectory("schema");

            Configuration configuration = new Configuration();
            // https://weblogs.asp.net/ricardoperes/nhibernate-conventions
            // https://github.com/fluentmigrator/fluentmigrator/issues/305
            configuration.SetNamingStrategy(new CustomNamingStrategy()/*DefaultNamingStrategy.Instance*/ /*ImprovedNamingStrategy.Instance*/);
            /**
            * Começamos a configuração através do FluentNHibernate
            **/
            FluentConfiguration fluentConfig = Fluently.Configure(configuration)
                .Database(() =>
                          OracleClientConfiguration.Oracle10 /// Use Oracle10 Dialect
                              .ConnectionString(             /// Use a string de conexão do app/web.config
                                    c => c.FromConnectionStringWithKey("DefaultConnectionNHibernate"))
                              .DefaultSchema("POSEYDON")
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
                             .Add(AutoMap.AssemblyOf<Amizade>(new StoreConfiguration())
                                      /// Ignora uma classe dentro do namespace 
                                      /// das entidades
                                      //.IgnoreBase<GlobalUser>()
                                      .IgnoreBase<EntityBase>()
                                      //.IgnoreBase<Usuario>()
                                      ///Adicione nossos padrões específicos para Cascade, etc ...
                                      //.Conventions.Add(new PrimaryKeyConvention())
                                      //.Conventions.Add(
                                      //  Table.Is(x => x.EntityType.Name),
                                      //  PrimaryKey.Name.Is(x => x.EntityType.Name + "Id")
                                      //)
                                      //.Conventions.Add<PrimaryKeyConvention>()
                                      //.Conventions.Add(PrimaryKey.Name.Is(x => x.EntityType.Name + "Id"))
                                      //.Conventions.Setup(c =>
                                      //{
                                      //    c.Add<PrimaryKeyConvention>();
                                      //    c.Add<CollectionConvention>();
                                      //    //c.Add<CustomForeignKeyConvention>();
                                      //    //c.Add<DefaultStringLengthConvention>();
                                      //})
                                      //.Conventions.Add<CollectionConvention>()
                                      /// Add 'Order' class exceptions to the convention.
                                      /// The line itself adds any conventions in assembly,
                                      ///    which is not the only option we have.
                                      ///.UseOverridesFromAssembly(typeof(PerfilOverride).Assembly)
                                      .UseOverridesFromAssemblyOf<AmizadeOverride>()
                                      .UseOverridesFromAssemblyOf<CategoriaOverride>()
                                      .UseOverridesFromAssemblyOf<ComunidadeOverride>()
                                      .UseOverridesFromAssemblyOf<ConnectionOverride>()
                                      .UseOverridesFromAssemblyOf<DepoimentoOverride>()
                                      .UseOverridesFromAssemblyOf<MembroOverride>()
                                      .UseOverridesFromAssemblyOf<ModeradorOverride>()
                                      .UseOverridesFromAssemblyOf<PerfilOverride>()
                                      .UseOverridesFromAssemblyOf<PostOcultoOverride>()
                                      .UseOverridesFromAssemblyOf<PostPerfilBloqueadoOverride>()
                                      .UseOverridesFromAssemblyOf<PostPerfilComentarioOverride>()
                                      .UseOverridesFromAssemblyOf<PostPerfilOverride>()
                                      .UseOverridesFromAssemblyOf<PrivacidadeOverride>()
                                      .UseOverridesFromAssemblyOf<RecadoComentarioOverride>()
                                      .UseOverridesFromAssemblyOf<RecadoOverride>()
                                      .UseOverridesFromAssemblyOf<StorieOverride>()
                                      .UseOverridesFromAssemblyOf<TopicoOverride>()
                                      .UseOverridesFromAssemblyOf<TopicoPostOverride>()
                                      .UseOverridesFromAssemblyOf<UsuarioOverride>()
                                      .UseOverridesFromAssemblyOf<VideoComentarioOverride>()
                                      .UseOverridesFromAssemblyOf<VideoOverride>()
                                      .UseOverridesFromAssemblyOf<VisitantePerfilOverride>())
                             /// Display the mappings XML in the console.
                             /// Typically you should do little XML stuff to output that.
                             /// It can also output to text file by specifying path.
                             .ExportTo(/*@"schema"*/Console.Out)
                );
            /// Use Jose's collection factory.
            /// This enables us to use .NET 4.0 HashSet not custom Set types.
            //.CollectionTypeFactory<Net4CollectionTypeFactory>();

            //new SchemaExport(fluentConfig).Drop(false, true);
            //new SchemaExport(fluentConfig).Execute(false, true, false);

            var cfg = fluentConfig.BuildConfiguration();

            //var exporter = new SchemaExport(cfg);
            //exporter.Execute(true, true, false);

            /// Tudo isso foi feito usando o objeto Fluent NHibernate para construir a configuração
            /// Então, agora, construa o objeto de Configuração do NHibernate.
            return cfg.BuildSessionFactory();
        }



        private static Configuration ConfigureNHibernateSqlServer()
        {
            FluentConfiguration fluentConfig = Fluently.Configure()
                .Mappings(
                    m => m.AutoMappings /// Use mapping by convention.
                                        /// Map all types in current assembly
                                        ///     using default options from StoreConfiguration class.
                             .Add(AutoMap.AssemblyOf<DatabaseContext>(new StoreConfiguration())
                                      /// Add our specific defaults for Cascade, etc...
                                      .Conventions.Add<CollectionConvention>()
                                      /// Add 'Order' class exceptions to the convention.
                                      /// The line itself adds any conventions in assembly,
                                      ///    which is not the only option we have.
                                      .UseOverridesFromAssembly(typeof(PerfilOverride).Assembly))
                             /// Display the mappings XML in the console.
                             /// Typically you should do little XML stuff to output that.
                             /// It can also output to text file by specifying path.
                             .ExportTo(Console.Out)
                )
                .Database(() =>
                          MsSqlConfiguration.MsSql2012 /// Use SQL Server 2008 Dialect
                              .ConnectionString(       /// Use connection string from app/web.config
                                    c => c.FromConnectionStringWithKey("DefaultConnectionNHibernate"))
                              .ShowSql()   /// Display SQL generated by NH in console.
                              .FormatSql() /// Format / Tabify SQL in console for readability.
                                           /// Auto escape names of tables for safe naming.
                                           ///    In SQL Server, this uses '[', and ']' not quotes.
                                           /// It's worth mentioning: 'Raw' allows us to add config
                                           ///     values as key/value strings like XML mappings,
                                           ///    and Environment Hbm2DDLKeyWords are NH classes not FNH
                //.Raw(Environment.Hbm2ddlKeyWords, Hbm2DDLKeyWords.AutoQuote.ToString())
                );
            /// Use Jose's collection factory.
            /// This enables us to use .NET 4.0 HashSet not custom Set types.
            //.CollectionTypeFactory<Net4CollectionTypeFactory>();

            /// All this was done using Fluent NHibernate object to build config
            /// So, now build the actual NHibernate Configuration object.
            return fluentConfig.BuildConfiguration();
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

