using Posy.V2.Infra.CrossCutting.Common.Resources;
using Posy.V2.Infra.CrossCutting.Common.Validations;
using System;

namespace Posy.V2.Domain.Entities
{
    public class Privacidade : EntityBase
    {
        public virtual int VerRecado { get; set; }
        public virtual int EscreverRecado { get; set; }

        public virtual Usuario Usuario { get; set; }

        protected Privacidade()
        {

        }

        public Privacidade(int id, int verRecado, int escreverRecado)
        {
            Id = id;
            VerRecado = verRecado;
            EscreverRecado = escreverRecado;

            Validate();
        }

        public virtual void Edit(int verRecado, int escreverRecado)
        {
            VerRecado = verRecado;
            EscreverRecado = escreverRecado;

            Validate();
        }

        public virtual void Validate()
        {
            Valid.AssertArgumentNotNull(VerRecado, Errors.ErroConfiguracaoPropriedade);
            Valid.AssertArgumentNotNull(EscreverRecado, Errors.ErroConfiguracaoPropriedade);
        }
    }
}
