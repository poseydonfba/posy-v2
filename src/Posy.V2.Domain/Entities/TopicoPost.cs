using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Common.Conversions;
using System;

namespace Posy.V2.Domain.Entities
{
    public class TopicoPost : EntityBase
    {
        public Guid TopicoPostId { get; private set; }
        public Guid TopicoId { get; private set; }
        public Guid UsuarioId { get; private set; }
        public DateTime DataPost { get; private set; }
        public byte[] Descricao { get; private set; }
        //[NotMapped]
        public string DescricaoHtml
        {
            get
            {
                return Descricao == null ? "" : Conversion.ByteArrayToStr(Descricao);
            }
        }
        public Guid? Uer { get; private set; }
        public DateTime? Der { get; private set; }
        public Guid? Uerp { get; private set; }
        public DateTime? Derp { get; private set; }

        public virtual Topico Topico { get; set; }
        public virtual Usuario Usuario { get; set; }

        protected TopicoPost() { }

        public TopicoPost(Guid topicoId, Guid usuarioId, string descricao)
        {
            TopicoPostId = Guid.NewGuid();
            TopicoId = topicoId;
            UsuarioId = usuarioId;
            Descricao = Conversion.StrToByteArray(descricao);
            DataPost = ConfigurationBase.DataAtual;
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
