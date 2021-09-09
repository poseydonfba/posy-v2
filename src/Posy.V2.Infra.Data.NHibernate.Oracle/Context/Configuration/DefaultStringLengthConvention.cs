using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Posy.V2.Infra.Data.Context
{
    public class DefaultStringLengthConvention
      : IPropertyConvention
    {
        public void Apply(IPropertyInstance instance)
        {
            instance.Length(250);
        }
    }
}
