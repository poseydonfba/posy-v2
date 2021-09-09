using FluentNHibernate.Automapping;
using Posy.V2.Domain.Entities;
using System;
using System.Linq;

namespace Posy.V2.Infra.Data.Context
{
    /// <summary>
    /// O que esse método faz é especificar quais classes representam entidades do banco de dados 
    /// e devem ser mapeadas automaticamente. Ele informa ao FNH para mapear todos os tipos no namespace 
    /// da classe EntityBase, exceto Enums e exceto EntityBase.
    /// https://www.gurustop.net/blog/2011/04/17/nh-nhibernate-mapping-jose-romaniello-ef-conform-domain-using-fnh-fluentnhibernate/
    /// </summary>
    public class StoreConfiguration : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            var persitentType = typeof(EntityBase);
            return persitentType.IsAssignableFrom(type);
        }
        //public override bool IsConcreteBaseType(Type type)
        //{
        //    return type == typeof(EntityBase);
        //}

        //public override bool ShouldMap(Type type)
        //{
        //    return type.Namespace == typeof(EntityBase).Namespace
        //           && type.IsEnum == false
        //           && type != typeof(EntityBase);
        //}
    }
}
