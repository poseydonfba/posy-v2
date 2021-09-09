using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using Posy.V2.Infra.CrossCutting.Common.Validations;
using System;

namespace Posy.V2.Domain.Entities
{
    public class VisitantePerfil : EntityBase
    {
        public virtual int VisitanteId { get; set; }
        public virtual int VisitadoId { get; set; }
        public virtual DateTime DataVisita { get; set; }

        public virtual Usuario Visitante { get; set; }
        public virtual Usuario Visitado { get; set; }

        protected VisitantePerfil()
        {
        }

        public VisitantePerfil(int visitanteId, int visitadoId)
        {
            VisitanteId = visitanteId;
            VisitadoId = visitadoId;
            DataVisita = ConfigurationBase.DataAtual;

            Validate();
        }

        public virtual void Validate()
        {
            Valid.AssertArgumentNotNull(VisitanteId,Errors.PerfilVisitanteInvalido);
            Valid.AssertArgumentNotNull(VisitadoId, Errors.PerfilVisitadoInvalido);
            Valid.AssertArgumentNotEquals(VisitanteId, Guid.Empty, Errors.PerfilVisitanteInvalido);
            Valid.AssertArgumentNotEquals(VisitadoId, Guid.Empty, Errors.PerfilVisitadoInvalido);
            Valid.AssertArgumentNotEquals(VisitanteId, VisitadoId, Errors.PerfilVisitanteIgualPerfilVisitado);
        }
    }
}
