using Posy.V2.Domain.Audit;
using Posy.V2.Domain.Entities;
using Posy.V2.Domain.Enums;
using Posy.V2.Infra.CrossCutting.Common.Extensions;
using Posy.V2.Infra.Data.EntityConfig;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Posy.V2.Infra.Data.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
            : base("DefaultConnection")
        {
            /**
            * Traz os dados por parte
            **/
            Configuration.LazyLoadingEnabled = false;

            /**
            * Evita erro de serialização json
            **/
            Configuration.ProxyCreationEnabled = false;

            /// Database.SetInitializer<DatabaseContext>(null);
        }

        #region DbSets

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Perfil> Perfis { get; set; }
        public DbSet<Privacidade> Privacidades { get; set; }
        public DbSet<Amizade> Amizades { get; set; }
        public DbSet<VisitantePerfil> VisitantesPerfil { get; set; }
        public DbSet<PostPerfil> PostsPerfil { get; set; }
        public DbSet<PostPerfilBloqueado> PostsPerfilBloqueado { get; set; }
        public DbSet<PostOculto> PostsOculto { get; set; }
        public DbSet<PostPerfilComentario> PostsPerfilComentario { get; set; }
        public DbSet<Recado> Recados { get; set; }
        public DbSet<RecadoComentario> RecadoComentarios { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<VideoComentario> VideoComentarios { get; set; }
        public DbSet<Depoimento> Depoimentos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Comunidade> Comunidades { get; set; }
        public DbSet<Membro> Membros { get; set; }
        public DbSet<Moderador> Moderadores { get; set; }
        public DbSet<Topico> Topicos { get; set; }
        public DbSet<TopicoPost> TopicosPosts { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<Storie> Stories { get; set; }

        #endregion

        #region DbSets Audit

        public DbSet<UsuarioAudit> UsuariosAudit { get; set; }
        public DbSet<PerfilAudit> PerfisAuditAudit { get; set; }
        public DbSet<PrivacidadeAudit> PrivacidadesAudit { get; set; }
        public DbSet<AmizadeAudit> AmizadesAudit { get; set; }
        public DbSet<VisitantePerfilAudit> VisitantesPerfilAudit { get; set; }
        public DbSet<PostPerfilAudit> PostsPerfilAuditAudit { get; set; }
        public DbSet<PostPerfilBloqueadoAudit> PostsPerfilBloqueadoAudit { get; set; }
        public DbSet<PostOcultoAudit> PostsOcultoAudit { get; set; }
        public DbSet<PostPerfilComentarioAudit> PostsPerfilComentarioAudit { get; set; }
        public DbSet<RecadoAudit> RecadosAudit { get; set; }
        public DbSet<RecadoComentarioAudit> RecadoComentariosAudit { get; set; }
        public DbSet<VideoAudit> VideosAudit { get; set; }
        public DbSet<VideoComentarioAudit> VideoComentariosAudit { get; set; }
        public DbSet<DepoimentoAudit> DepoimentosAudit { get; set; }
        public DbSet<ComunidadeAudit> ComunidadesAudit { get; set; }
        public DbSet<MembroAudit> MembrosAudit { get; set; }
        public DbSet<ModeradorAudit> ModeradoresAudit { get; set; }
        public DbSet<TopicoAudit> TopicosAudit { get; set; }
        public DbSet<TopicoPostAudit> TopicosPostsAudit { get; set; }
        public DbSet<ConnectionAudit> ConnectionsAudit { get; set; }
        public DbSet<StorieAudit> StoriesAudit { get; set; }

        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /**
            * Não cria tabela no plural
            * Evita deletar em cascata
            * Evita deletar em cascata
            **/
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            /**
            * Força a configurar quando uma propriedade for alguma coisa mais Id no final como chave primaria
            **/
            modelBuilder.Properties()
                .Where(x => x.Name == x.ReflectedType.Name + "Id")
                .Configure(x => x.IsKey());

            /**
            * Muda valor padrão de uma string para varchar(100)
            **/
            modelBuilder.Properties<string>()
                .Configure(x => x.HasColumnType("nvarchar"));

            modelBuilder.Properties<string>()
                .Configure(x => x.IsUnicode(true));

            ///modelBuilder.Properties<DateTime>()
            ///    .Configure(x => x.HasColumnType("datetime2"));

            modelBuilder.Properties<string>()
                .Configure(x => x.HasMaxLength(100));

            ///modelBuilder.Properties<byte[]>()
            ///    .Configure(x => x.HasColumnType("varbinary(MAX)"));

            #region CONFIG ENTITIES

            modelBuilder.Configurations.Add(new UsuarioConfig());
            modelBuilder.Configurations.Add(new PerfilConfig());
            modelBuilder.Configurations.Add(new PrivacidadeConfig());
            modelBuilder.Configurations.Add(new AmizadeConfig());
            modelBuilder.Configurations.Add(new VisitantePerfilConfig());
            modelBuilder.Configurations.Add(new PostPerfilConfig());
            modelBuilder.Configurations.Add(new PostPerfilBloqueadoConfig());
            modelBuilder.Configurations.Add(new PostOcultoConfig());
            modelBuilder.Configurations.Add(new RecadoConfig());
            modelBuilder.Configurations.Add(new RecadoComentarioConfig());
            modelBuilder.Configurations.Add(new VideoConfig());
            modelBuilder.Configurations.Add(new VideoComentarioConfig());
            modelBuilder.Configurations.Add(new DepoimentoConfig());
            modelBuilder.Configurations.Add(new CategoriaConfig());
            modelBuilder.Configurations.Add(new ComunidadeConfig());
            modelBuilder.Configurations.Add(new MembroConfig());
            modelBuilder.Configurations.Add(new ModeradorConfig());
            modelBuilder.Configurations.Add(new TopicoConfig());
            modelBuilder.Configurations.Add(new TopicoPostConfig());
            modelBuilder.Configurations.Add(new ConnectionConfig());
            modelBuilder.Configurations.Add(new StorieConfig());

            //modelBuilder.Ignore<Usuario>();

            #endregion
        }

        /**
        * Obtem o DbSet da entidade de auditoria para insert, update ou delete
        **/
        private DbSet dbSet;

        /// <summary>
        /// Isso é sobrescrito para impedir que alguém chame SaveChanges sem especificar o usuário que fez a alteração
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            /**
            * Erro com update-database, usar apenas em produção
            **/
            //throw new InvalidOperationException(Errors.OIDDoUsuarioDeveSerFornecido);

            return base.SaveChanges();
        }

        /// <summary>
        /// SaveChanges para que possamos chamar o novo método AuditEntities
        /// </summary>
        /// <returns></returns>
        public int SaveChanges(GlobalUser userAudit)
        {
            this.AuditEntities(userAudit);

            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entidade do tipo \"{0}\" no estado \"{1}\" tem os seguintes erros de validação:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Erro: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        #region AUDITORIA

        /// <summary>
        /// Método que irá definir as propriedades de Auditoria para cada Entidade 
        /// adicionada ou modificada marcada com o IAuditable interface.
        /// Verificar depois essa possibilidade 
        /// https://stackoverflow.com/questions/24534607/entity-framework-snapshot-history
        /// Opção com colunas alteradas
        /// https://jmdority.wordpress.com/2011/07/20/using-entity-framework-4-1-dbcontext-change-tracking-for-audit-logging/
        /// https://www.codeproject.com/Articles/34491/Implementing-Audit-Trail-using-Entity-Framework-Pa
        /// </summary>
        private void AuditEntities(GlobalUser userAudit)
        {
            /**
            * Prevenindo exclusão de entidades de auditoria 
            * TESTAR
            **/
            //var DeletedEntities = ChangeTracker.Entries<Auditable>().Where(E => E.State == EntityState.Deleted).ToList();

            //DeletedEntities.ForEach(E =>
            //{
            //    E.State = EntityState.Unchanged;
            //});

            /**
            * Para cada entidade alterada marcada como IAuditable, defina os valores para as propriedades de auditoria 
            **/
            ///ObjectStateManager objectStateManager = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager;
            ///objectStateManager.GetObjectStateEntries(
            ///    EntityState.Added | EntityState.Modified | EntityState.Deleted)
            ChangeTracker
                .Entries<EntityBase>()
                .Where(p => p.State == EntityState.Added || p.State == EntityState.Modified || p.State == EntityState.Deleted)
                .ToList().ForEach(entry =>
                {
                    var entity = entry.Entity;
                    var entityName = entity.GetType().Name;
                    var entityAssembly = entity.GetType().GetTypeInfo().Assembly;

                    /**
                    * Obtem o type contido no nome da entidade de auditoria
                    **/
                    var type = entityAssembly.DefinedTypes.FirstOrDefault(t => t.Name == entityName + "Audit");
                    if (type != null)
                    {
                        /**
                        * Obtem a chave primaria da entidade
                        **/
                        var pk = GetPrimaryKeyValue(entry);

                        /**
                        * Cria uma instancia da entidade de auditoria
                        **/
                        var instance = Activator.CreateInstance(type);

                        /**
                        * Copia os dados da enetidade para a respectiva entidade de auditoria
                        **/
                        CloneProperties(entity, instance);

                        /**
                        * Obtem o DbSet da entidade de auditoria para add
                        **/
                        dbSet = Set(instance.GetType());

                        var auditDate = DateTime.UtcNow;
                        var auditTransactionGUID = Guid.NewGuid();

                        if (entry.State == EntityState.Added)
                        {
                            /**
                            * Criando novas revisões para registros que foram inseridos ou atualizados
                            **/
                            AudityInsert(instance, auditDate, auditTransactionGUID, userAudit.UsuarioId, userAudit.Nome, "*");
                        }
                        if (entry.State == EntityState.Modified)
                        {
                            /**
                            * Obtendo apenas as propriedades que foram alteradas
                            **/
                            #region OTHERS
                            //IEnumerable<string> modifiedProperties = entry.GetModifiedProperties();

                            //var modifiedProperties = entry.CurrentValues.PropertyNames
                            //    .Where(propertyName => entry.Property(propertyName).IsModified).ToList();

                            //ObjectStateManager objectStateManager = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager;
                            //var originalValues = objectStateManager.GetObjectStateEntry(entity).GetModifiedProperties();

                            //var modifiedProperties = entry.CurrentValues.PropertyNames
                            //    .Where(propertyName => 
                            //        entry.CurrentValues).ToList();
                            #endregion
                            var colunas = new List<string>();
                            foreach (string propertyName in entry.OriginalValues.PropertyNames)
                            {
                                var typeEntry = entry.OriginalValues[propertyName].GetType();
                                var ori = entry.OriginalValues.GetValue<object>(propertyName);
                                var cur = entry.CurrentValues.GetValue<object>(propertyName);

                                /**
                                * Na verificação de byte[] não estava funcionando com GetHashCode()
                                **/
                                var equals = typeEntry.Name.Equals("Byte[]") ?
                                    entry.OriginalValues.GetValue<byte[]>(propertyName).ByteEquals(entry.CurrentValues.GetValue<byte[]>(propertyName)) :
                                    ori.GetHashCode() == cur.GetHashCode();

                                if (!equals)
                                    colunas.Add(propertyName);
                            }

                            var changedColumns = string.Join(",", colunas.ToArray());

                            /**
                            * Obtendo o ultimo registro de auditoria para alteração
                            **/
                            var entityAudity = AuditRegister(instance, pk);

                            /**
                            * Fechando o tempo de vida das revisões atuais (EndDate = infinito) 
                            * para registros que foram atualizados ou excluídos
                            **/
                            AudityUpdate(entityAudity, auditDate, auditTransactionGUID, userAudit.UsuarioId, userAudit.Nome);

                            /**
                            * Criando novas revisões para registros que foram inseridos ou atualizados
                            **/
                            AudityInsert(instance, auditDate, auditTransactionGUID, userAudit.UsuarioId, userAudit.Nome, changedColumns);
                        }
                        if (entry.State == EntityState.Deleted)
                        {
                            /**
                            * Obtendo o ultimo registro de auditoria para alteração
                            **/
                            var entityAudity = AuditRegister(instance, pk);

                            /**
                            * Fechando o tempo de vida das revisões atuais (EndDate = infinito) 
                            * para registros que foram atualizados ou excluídos
                            **/
                            AudityDelete(entityAudity, auditDate, auditTransactionGUID, userAudit.UsuarioId, userAudit.Nome);
                        }
                    }
                });
        }

        /// <summary>
        /// Metodo para atualizar os campos de auditoria para insert
        /// </summary>
        private void AudityInsert<T>(T instance, DateTime auditDate, Guid auditStartTransactionGUID, Guid auditUser, string auditName, string changedColumns)
        {
            var destinationType = instance.GetType();
            destinationType.GetProperty("AuditStartDate").SetValue(instance, auditDate, null);
            destinationType.GetProperty("AuditEndDate").SetValue(instance, new DateTime(9999, 12, 31), null);
            destinationType.GetProperty("AuditStartOperation").SetValue(instance, AuditActions.I.ToString(), null);
            destinationType.GetProperty("AuditStartUserID").SetValue(instance, auditUser, null);
            destinationType.GetProperty("AuditStartUsername").SetValue(instance, auditName, null);
            destinationType.GetProperty("AuditStartTransactionGUID").SetValue(instance, auditStartTransactionGUID, null);
            destinationType.GetProperty("ChangedColumns").SetValue(instance, changedColumns, null);

            dbSet.Add(instance);
        }

        /// <summary>
        /// Metodo para atualizar os campos de auditoria para update
        /// </summary>
        private void AudityUpdate<T>(T instance, DateTime auditDate, Guid auditStartTransactionGUID, Guid auditUser, string auditName) where T : class
        {
            var destinationType = instance.GetType();
            destinationType.GetProperty("AuditEndDate").SetValue(instance, auditDate, null);
            destinationType.GetProperty("AuditEndOperation").SetValue(instance, AuditActions.U.ToString(), null);
            destinationType.GetProperty("AuditEndUserID").SetValue(instance, auditUser, null);
            destinationType.GetProperty("AuditEndUsername").SetValue(instance, auditName, null);
            destinationType.GetProperty("AuditEndTransactionGUID").SetValue(instance, auditStartTransactionGUID, null);

            Entry<T>(instance).State = EntityState.Modified;
        }

        /// <summary>
        /// Metodo para atualizar os campos de auditoria para delete
        /// </summary>
        private void AudityDelete<T>(T instance, DateTime auditDate, Guid auditStartTransactionGUID, Guid auditUser, string auditName) where T : class
        {
            var destinationType = instance.GetType();
            destinationType.GetProperty("AuditEndDate").SetValue(instance, auditDate, null);
            destinationType.GetProperty("AuditEndOperation").SetValue(instance, AuditActions.D.ToString(), null);
            destinationType.GetProperty("AuditEndUserID").SetValue(instance, auditUser, null);
            destinationType.GetProperty("AuditEndUsername").SetValue(instance, auditName, null);
            destinationType.GetProperty("AuditEndTransactionGUID").SetValue(instance, auditStartTransactionGUID, null);

            Entry<T>(instance).State = EntityState.Modified;
        }

        /// <summary>
        /// Metodo para copiar os valores de uma entidade para a 
        /// sua entidade de auditoria
        /// </summary>
        private void CloneProperties<T>(T original, T destination)
        {
            var originalType = original.GetType();
            var destinationType = destination.GetType();

            PropertyInfo[] props = originalType.GetProperties();
            foreach (var propertyInfo in props)
            {
                var pv = propertyInfo.GetValue(original, null);
                var destinationProperty = destinationType.GetProperty(propertyInfo.Name);
                if (destinationProperty != null)
                    destinationProperty.SetValue(destination, pv, null);
            }
        }

        /// <summary>
        /// Metodo para obter a versão de auditoria atual
        /// </summary>
        private T AuditRegister<T>(T instance, EntityKeyMember pk) where T : class
        {
            var parameter = Expression.Parameter(typeof(T));
            var convert = Expression.Convert(parameter, instance.GetType());
            var equal = Expression.Equal(
                Expression.PropertyOrField(convert, "AuditEndOperation"),
                Expression.Constant(null));
            var predicate = Expression.Lambda<Func<T, bool>>(equal, parameter);

            var parameterPk = Expression.Parameter(typeof(T));
            var convertPk = Expression.Convert(parameterPk, instance.GetType());
            var equalPk = Expression.Equal(
                Expression.PropertyOrField(convertPk, pk.Key),
                Expression.Constant(pk.Value));
            var predicatePk = Expression.Lambda<Func<T, bool>>(equalPk, parameterPk);

            return ((IQueryable<T>)dbSet).Where(predicatePk).Where(predicate).FirstOrDefault();

            #region OTHERS
            //var baseQuery = ((IQueryable<object>)dbSet)
            //    .Where(x => x.GetType().GetProperty("AuditEndOperation").GetValue(x) == null)
            //    .FirstOrDefault();
            //..GetValue(this, null);
            //dynamic q = ((IEnumerable<object>)Queryable.Where((dynamic)dbSet, l)).ToList();


            //PropertyInfo entityProperty = GetType().GetProperties().Where(t => t.Name == entityName + "Audit").Single();
            //var baseQuery = (IQueryable<IAudity>)entityAudit..GetValue(this, null);
            //var result = baseQuery.Where(t => t.);


            //ParameterExpression parameter = Expression.Parameter(instance.GetType(), "x");
            //Expression property = Expression.Property(parameter, "AuditEndOperation");
            //Expression target = Expression.Constant(null);
            //Expression equalsMethod = Expression.Call(property, "Equals", null, target);
            //Expression<Func<object, bool>> lambda =
            //   Expression.Lambda<Func<object, bool>>(equalsMethod, parameter);


            //var parameter = Expression.Parameter(typeof(T), "p");
            //var predicate = Expression.Lambda<Func<T, bool>>(
            //    Expression.Equal(Expression.PropertyOrField(parameter, "AuditEndOperation"), Expression.Constant(null)),
            //    parameter);
            //return Set<T>().Where(predicate).ToList();


            //return Set<T>().Where(p => p.AuditEndOperation == null).FirstOrDefault();

            //var property = instance.GetType().GetProperty("AuditEndOperation");
            //var parameter = Expression.Parameter(property.PropertyType, "p");


            //PropertyInfo propertyInfoObj = instance.GetType().GetProperty("AuditEndOperation");
            //var r = ((IQueryable<T>)dbSet)
            //                .FirstOrDefault(x => propertyInfoObj.GetValue(x) == null);
            //return r;
            #endregion
        }

        /// <summary>
        /// Obtem a chave primaria de uma entidade
        /// </summary>
        /// <returns></returns>
        private EntityKeyMember /*object*/ GetPrimaryKeyValue(DbEntityEntry entry)
        {
            var objectStateEntry = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager.GetObjectStateEntry(entry.Entity);
            var key = objectStateEntry.EntityKey?.EntityKeyValues?[0];//.Value;
            if (key != null)
                return key;

            try
            {
                var setBase = objectStateEntry.EntitySet;
                var keyName = setBase.ElementType.KeyMembers.Select(k => k.Name).FirstOrDefault();
                return new EntityKeyMember(keyName, entry.CurrentValues[keyName]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
