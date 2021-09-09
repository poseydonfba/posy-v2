using FluentNHibernate.Automapping;
using Posy.V2.Domain.Entities;
using System;

namespace Posy.V2.Infra.Data.Context
{
    public class AutomappingConfiguration : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            return type.GetInterface(typeof(EntityBase).FullName) != null;
        }
    }
}
