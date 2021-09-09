using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Common.Conversions;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using Posy.V2.Infra.CrossCutting.Common.Validations;
using System;

namespace Posy.V2.Domain.Entities
{
    public class Video : EntityBase
    {
        public Guid VideoId { get; private set; }
        public Guid UsuarioId { get; private set; }
        public string Url { get; private set; }
        public byte[] NomeVideo { get; private set; }
        //[NotMapped]
        public string NomeHtml
        {
            get
            {
                return NomeVideo == null ? "" : Conversion.ByteArrayToStr(NomeVideo);
            }
        }
        public DateTime DataVideo { get; private set; }
        public DateTime? Der { get; private set; }

        public virtual Usuario Usuario { get; set; }
        //public virtual ICollection<VideoComentario> VideoComentarios { get; set; }

        protected Video()
        {
            VideoId = Guid.NewGuid();
            //VideoComentarios = new List<VideoComentario>();
        }

        public Video(Guid usuarioId, string url, string nomeVideo)
        {
            VideoId = Guid.NewGuid();
            UsuarioId = usuarioId;
            Url = url;
            NomeVideo = Conversion.StrToByteArray(nomeVideo);
            DataVideo = ConfigurationBase.DataAtual;

            //VideoComentarios = new List<VideoComentario>();

            validate();
        }

        public void Delete()
        {
            Der = ConfigurationBase.DataAtual;
        }

        public void validate()
        {
            Valid.AssertArgumentNotNull(UsuarioId, Errors.UsuarioInvalido);
            Valid.AssertArgumentNotNull(Url, Errors.UrlInvalida);
        }
    }
}
