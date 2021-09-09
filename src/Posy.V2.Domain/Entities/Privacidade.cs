using Posy.V2.Infra.CrossCutting.Common.Resources;
using Posy.V2.Infra.CrossCutting.Common.Validations;
using System;

namespace Posy.V2.Domain.Entities
{
    public class Privacidade : EntityBase
    {
        //public Guid PrivacidadeId { get; private set; }
        public Guid UsuarioId { get; private set; }

        public int VerRecado { get; private set; }
        public int EscreverRecado { get; private set; }

        public virtual Usuario Usuario { get; set; }

        protected Privacidade()
        {

        }

        public Privacidade(Guid usuarioId, int verRecado, int escreverRecado)
        {
            //PrivacidadeId = Guid.NewGuid();
            UsuarioId = usuarioId;
            VerRecado = verRecado;
            EscreverRecado = escreverRecado;

            Validate();
        }

        public void Edit(int verRecado, int escreverRecado)
        {
            VerRecado = verRecado;
            EscreverRecado = escreverRecado;

            Validate();
        }

        public void Validate()
        {
            Valid.AssertArgumentNotNull(VerRecado, Errors.ErroConfiguracaoPropriedade);
            Valid.AssertArgumentNotNull(EscreverRecado, Errors.ErroConfiguracaoPropriedade);
        }
    }
}
