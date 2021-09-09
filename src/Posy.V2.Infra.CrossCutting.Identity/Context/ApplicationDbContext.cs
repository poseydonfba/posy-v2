using Microsoft.AspNet.Identity.EntityFramework;
using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Identity.Model;
using System;
using System.Data.Entity;

namespace Posy.V2.Infra.CrossCutting.Identity.Context
{
    public class ApplicationDbContext :
        IdentityDbContext<ApplicationUser, ApplicationRole, Guid, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, IDisposable
    {
        public DbSet<UsuarioCliente> UsuarioCliente { get; set; }

        public DbSet<Claims> Claims { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection"/*, throwIfV1Schema: false*/)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().ToTable("Usuario").Property(p => p.Id).HasColumnName("UsuarioId");
            modelBuilder.Entity<ApplicationUserRole>().ToTable("UsuarioFuncao").Property(p => p.UserId).HasColumnName("UsuarioId");
            modelBuilder.Entity<ApplicationUserRole>().ToTable("UsuarioFuncao").Property(p => p.RoleId).HasColumnName("FuncaoId");
            modelBuilder.Entity<ApplicationUserLogin>().ToTable("UsuarioLogin").Property(p => p.UserId).HasColumnName("UsuarioId");
            modelBuilder.Entity<ApplicationUserClaim>().ToTable("UsuarioClaim").Property(p => p.Id).HasColumnName("UsuarioClaimId");
            modelBuilder.Entity<ApplicationUserClaim>().ToTable("UsuarioClaim").Property(p => p.UserId).HasColumnName("UsuarioId");
            modelBuilder.Entity<ApplicationRole>().ToTable("Funcao").Property(p => p.Id).HasColumnName("FuncaoId");
            //modelBuilder.Entity<UsuarioCliente>().HasRequired(x => x.Usuario);

            // DEFININDO O IDIOMA PADRÃO
            //modelBuilder.Entity<ApplicationUser>(typeBuilder =>
            //{
            //    typeBuilder.Property<string>(u => u.Language)
            //        .HasDefaultValue("en")
            //        .IsRequired();
            //});

            if (ConfigurationBase.ServerDatabase == Common.Enums.ServerDatabase.MYSQL)
            {
                #region INFO

                // PARA EVITAR ERRO NO MYSQL https://wildlyinaccurate.com/mysql-specified-key-was-too-long-max-key-length-is-767-bytes/
                // O MySQL tem uma limitação de prefixo de 767 bytes no InnoDB e 1000 bytes no MYISAM.Isso nunca foi um 
                // problema para mim, até que comecei a usar o UTF - 16 como o conjunto de caracteres de um dos 
                // meus bancos de dados. O UTF-16 pode usar até 4 bytes por caractere, o que significa que em uma tabela 
                // InnoDB você não pode ter mais de 191 caracteres.Tome esta CREATEdeclaração por exemplo:
                //  CREATE TABLE `user` (
                //  `id` int(11) NOT NULL AUTO_INCREMENT,
                //  `username` varchar(32) NOT NULL,
                //  `password` varchar(64) NOT NULL,
                //  `email` varchar(255) NOT NULL,
                //  PRIMARY KEY(`id`),
                //  UNIQUE KEY `UNIQ_8D93D649F85E0677` (`username`),
                //  UNIQUE KEY `UNIQ_8D93D649E7927C74` (`email`)
                // ) ENGINE = InnoDB DEFAULT CHARSET = utf16 AUTO_INCREMENT = 1;
                // Isso falhará com um erro como Specified key was too long; max key length is 767 bytes, porque o 
                // UNIQUE INDEXcampo de email requer pelo menos 1020 bytes(255 * 4).
                // Infelizmente não há solução real para isso.Suas únicas opções são reduzir o tamanho da coluna, usar um conjunto de 
                // caracteres diferente(como UTF-8) ou usar um mecanismo diferente(como MYISAM).Neste caso, mudei o conjunto de caracteres para UTF - 8, 
                // o que aumentou o comprimento máximo da chave para 255 caracteres.

                #endregion

                modelBuilder.Entity<ApplicationRole>().Property(p => p.Name).HasMaxLength(150);
                modelBuilder.Entity<ApplicationUser>().Property(p => p.UserName).HasMaxLength(150);
            }
            else if (ConfigurationBase.ServerDatabase == Common.Enums.ServerDatabase.ORACLE)
            {
                #region INFO

                // Por padrão, o Entity Framework usa o “schema” chamado “dbo“. Ao trabalhar com o Oracle no Entity Framework, 
                // descobri nesta thread do StackOverflow que temos que alterar o schema padrão no contexto
                // de forma que ele fique com o mesmo nome do usuário(no meu caso, “POSEYDON“), maiusculo.
                // Para isso, temos que fazer um override no método “OnModelCreating” do 
                // nosso contexto e, dentro desse método, temos que fazer uma chamada ao método “HasDefaultSchema“, 
                // passando o nome do usuário:

                #endregion

                modelBuilder.HasDefaultSchema("POSEYDON");
            }

            modelBuilder.Properties<string>()
                .Configure(x => x.HasMaxLength(100));
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
