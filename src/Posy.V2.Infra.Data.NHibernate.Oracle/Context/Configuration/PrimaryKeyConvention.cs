using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using Posy.V2.Domain.Entities;
using System;

namespace Posy.V2.Infra.Data.Context
{
    /// <summary>
    /// https://stackoverflow.com/questions/21550899/how-to-override-the-nhibernate-identity-id-name
    /// </summary>
    public class PrimaryKeyConvention : IIdConvention
    {
        public void Apply(IIdentityInstance instance)
        {
            //instance.Column(instance.EntityType.Name + "Id");
            Type type = instance.EntityType;
            var persitentType = typeof(EntityBase);
            var isMappableModel = persitentType.IsAssignableFrom(type);

            if (isMappableModel)
            {
                instance.Column(type.Name + "Id");
                //instance.GeneratedBy.Assigned();
            }
        }
    }
}
