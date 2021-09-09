using Posy.V2.Domain.Entities;
using FluentNHibernate.Mapping;
using Posy.V2.Domain.Entities;

namespace Posy.V2.Infra.Data.EntityConfig
{
    public class PerfilConfig : ClassMap<Domain.Entities.Perfil>
    {
        public PerfilConfig()
        {
            Table("Perfil");

            Id(x => x.Id);

            Map(x => x.Nome);
            Map(x => x.Sobrenome);
            Map(x => x.Alias);
            Map(x => x.PaisId);
            Map(x => x.DataNascimento);
            Map(x => x.Sexo);
            Map(x => x.EstadoCivil);
            Map(x => x.FrasePerfil);
            Map(x => x.DescricaoPerfil);
            Map(x => x.Dar);
        }
    }
}
