using Posy.V2.Domain.Enums;
using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Common.Conversions;
using System;

namespace Posy.V2.Domain.Entities
{
    public class Topico : EntityBase
    {
        public Guid TopicoId { get; private set; }
        public Guid ComunidadeId { get; private set; }
        public Guid UsuarioId { get; private set; }
        public DateTime DataTopico { get; private set; }
        public string Titulo { get; private set; }
        public byte[] Descricao { get; private set; }
        //[NotMapped]
        public string DescricaoHtml
        {
            get
            {
                return Descricao == null ? "" : Conversion.ByteArrayToStr(Descricao);
            }
        }
        public TipoTopico TipoTopico { get; private set; }
        public Guid? Uer { get; private set; }
        public DateTime? Der { get; private set; }
        public Guid? Uerp { get; private set; }
        public DateTime? Derp { get; private set; }

        public virtual Comunidade Comunidade { get; set; }
        public virtual Usuario Usuario { get; set; }

        protected Topico() { }

        public Topico(Guid comunidadeId, Guid usuarioId, string titulo, string descricao, TipoTopico tipoTopico)
        {
            TopicoId = Guid.NewGuid();
            ComunidadeId = comunidadeId;
            UsuarioId = usuarioId;
            Titulo = titulo;
            Descricao = Conversion.StrToByteArray(descricao);
            TipoTopico = tipoTopico;
            DataTopico = ConfigurationBase.DataAtual;
        }

        public void Delete(Guid usuarioIdExclusao)
        {
            Uer = usuarioIdExclusao;
            Der = ConfigurationBase.DataAtual;
        }

        public void DeletePermanente(Guid usuarioIdExclusao)
        {
            Uerp = usuarioIdExclusao;
            Derp = ConfigurationBase.DataAtual;
        }
    }
}
