using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using Posy.V2.Infra.CrossCutting.Common.Validations;
using System;

namespace Posy.V2.Domain.Entities
{
    public class VisitantePerfil : EntityBase
    {
        public Guid VisitantePerfilId { get; private set; }
        public Guid VisitanteId { get; private set; }
        public Guid VisitadoId { get; private set; }
        public DateTime DataVisita { get; private set; }

        public virtual Usuario Visitante { get; set; }
        public virtual Usuario Visitado { get; set; }

        protected VisitantePerfil()
        {
            VisitantePerfilId = Guid.NewGuid();
        }

        public VisitantePerfil(Guid visitanteId, Guid visitadoId)
        {
            VisitantePerfilId = Guid.NewGuid();
            VisitanteId = visitanteId;
            VisitadoId = visitadoId;
            DataVisita = ConfigurationBase.DataAtual;

            Validate();
        }

        public void Validate()
        {
            Valid.AssertArgumentNotNull(VisitanteId,Errors.PerfilVisitanteInvalido);
            Valid.AssertArgumentNotNull(VisitadoId, Errors.PerfilVisitadoInvalido);
            Valid.AssertArgumentNotEquals(VisitanteId, Guid.Empty, Errors.PerfilVisitanteInvalido);
            Valid.AssertArgumentNotEquals(VisitadoId, Guid.Empty, Errors.PerfilVisitadoInvalido);
            Valid.AssertArgumentNotEquals(VisitanteId, VisitadoId, Errors.PerfilVisitanteIgualPerfilVisitado);
        }
    }
}
