using Posy.V2.Domain.Enums;
using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Common.Conversions;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Entities
{
    public class Topico : EntityBase
    {
        public virtual int ComunidadeId { get; set; }
        public virtual int UsuarioId { get; set; }
        public virtual DateTime DataTopico { get; set; }
        public virtual string Titulo { get; set; }
        public virtual byte[] Descricao { get; set; }
        //[NotMapped]
        public virtual string DescricaoHtml
        {
            get
            {
                return Descricao == null ? "" : Conversion.ByteArrayToStr(Descricao);
            }
        }
        public virtual TipoTopico TipoTopico { get; set; }
        public virtual int? Uer { get; set; }
        public virtual DateTime? Der { get; set; }
        public virtual int? Uerp { get; set; }
        public virtual DateTime? Derp { get; set; }

        public virtual Comunidade Comunidade { get; set; }
        public virtual Usuario Usuario { get; set; }

        public virtual ICollection<TopicoPost> TopicosPost { get; set; }

        protected Topico() { }

        public Topico(int comunidadeId, int usuarioId, string titulo, string descricao, TipoTopico tipoTopico)
        {
            ComunidadeId = comunidadeId;
            UsuarioId = usuarioId;
            Titulo = titulo;
            Descricao = Conversion.StrToByteArray(descricao);
            TipoTopico = tipoTopico;
            DataTopico = ConfigurationBase.DataAtual;
        }

        public virtual void Delete(int usuarioIdExclusao)
        {
            Uer = usuarioIdExclusao;
            Der = ConfigurationBase.DataAtual;
        }

        public virtual void DeletePermanente(int usuarioIdExclusao)
        {
            Uerp = usuarioIdExclusao;
            Derp = ConfigurationBase.DataAtual;
        }
    }
}
