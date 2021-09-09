using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.AspNet.Identity;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Posy.V2.Infra.CrossCutting.Identity.NHibernate.Configuration;
using System;
using System.Configuration;

namespace Posy.V2.Infra.CrossCutting.Identity.NHibernate.Context
{
    public class ApplicationDbContext
    {
        private readonly ISessionFactory sessionFactory;

        public ApplicationDbContext()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var fluentConfig = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(connectionString))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ApplicationDbContext>());

            var cfg = fluentConfig.BuildConfiguration();

            //var exporter = new SchemaExport(cfg);
            //exporter.Execute(true, true, false);

            sessionFactory = cfg.BuildSessionFactory();
        }
        public ISession MakeSession()
        {
            return sessionFactory.OpenSession();
        }

        public IUserStore<User, int> Users
        {
            get { return new IdentityStore(MakeSession()); }
        }
    }
}
