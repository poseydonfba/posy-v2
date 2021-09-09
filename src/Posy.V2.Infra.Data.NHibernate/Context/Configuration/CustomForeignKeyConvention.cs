using FluentNHibernate;
using FluentNHibernate.Conventions;
using System;

namespace Posy.V2.Infra.Data.Context
{
    public class CustomForeignKeyConvention : ForeignKeyConvention
    {
        protected override string GetKeyName(Member property, Type type)
        {
            if (property == null)
                return type.Name + "_FK";

            return property.Name + "_FK";
        }
    }
}
