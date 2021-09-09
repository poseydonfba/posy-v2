using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Posy.V2.Domain.Entities;

namespace Posy.V2.Infra.Data.EntityConfig.Overrides
{
    public class PostOcultoOverride : IAutoMappingOverride<PostOculto>
    {
        public void Override(AutoMapping<PostOculto> mapping)
        {
            mapping.Table("PostOculto");
            mapping.Id(x => x.Id);

            mapping.HasOne(x => x.Usuario);
            mapping.HasOne(x => x.PostPerfil);
        }
    }
}