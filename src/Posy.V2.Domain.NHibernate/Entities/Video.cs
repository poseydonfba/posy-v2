using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Common.Conversions;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using Posy.V2.Infra.CrossCutting.Common.Validations;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Entities
{
    public class Video : EntityBase
    {
        public virtual int UsuarioId { get; set; }
        public virtual string Url { get; set; }
        public virtual byte[] NomeVideo { get; set; }
        //[NotMapped]
        public virtual string NomeHtml
        {
            get
            {
                return NomeVideo == null ? "" : Conversion.ByteArrayToStr(NomeVideo);
            }
        }
        public virtual DateTime DataVideo { get; set; }
        public virtual DateTime? Der { get; set; }

        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<VideoComentario> VideosComentario { get; set; }

        protected Video()
        {
        }

        public Video(int usuarioId, string url, string nomeVideo)
        {
            UsuarioId = usuarioId;
            Url = url;
            NomeVideo = Conversion.StrToByteArray(nomeVideo);
            DataVideo = ConfigurationBase.DataAtual;

            //VideoComentarios = new List<VideoComentario>();

            validate();
        }

        public virtual void Delete()
        {
            Der = ConfigurationBase.DataAtual;
        }

        public virtual void validate()
        {
            Valid.AssertArgumentNotNull(UsuarioId, Errors.UsuarioInvalido);
            Valid.AssertArgumentNotNull(Url, Errors.UrlInvalida);
        }
    }
}
