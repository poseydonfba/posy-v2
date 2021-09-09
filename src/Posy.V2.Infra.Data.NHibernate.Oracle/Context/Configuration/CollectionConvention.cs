using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Posy.V2.Infra.Data.Context
{
    /// <summary>
    /// Como mencionado, as convenções são aplicadas implementando interfaces I… .Convention, 
    /// e há muitas delas para sobrescrever qualquer padrão que você queira. Neste exemplo, 
    /// se quiséssemos que nossa convenção de coleta fosse apenas um subconjunto de coleções, 
    /// poderíamos também ter implementado o “ICollectionConventionAcceptance”. Você pode 
    /// implementar tantas I<SomeCriteria>Convention e <SomeCriteria>ConventionAcceptance
    /// na mesma classe que desejar.
    /// https://www.gurustop.net/blog/2011/04/17/nh-nhibernate-mapping-jose-romaniello-ef-conform-domain-using-fnh-fluentnhibernate/
    /// </summary>
    public class CollectionConvention : ICollectionConvention
    {
        #region ICollectionConvention Members

        public void Apply(ICollectionInstance instance)
        {
            instance.AsSet();
            instance.Cascade.All();
        }

        #endregion
    }
}
