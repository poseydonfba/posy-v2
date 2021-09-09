using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Common.Conversions;
using System;

namespace Posy.V2.Domain.Entities
{
    public class TopicoPost : EntityBase
    {
        public virtual int TopicoId { get; set; }
        public virtual int UsuarioId { get; set; }
        public virtual DateTime DataPost { get; set; }
        public virtual byte[] Descricao { get; set; }
        //[NotMapped]
        public virtual string DescricaoHtml
        {
            get
            {
                return Descricao == null ? "" : Conversion.ByteArrayToStr(Descricao);
            }
        }
        public virtual int? Uer { get; set; }
        public virtual DateTime? Der { get; set; }
        public virtual int? Uerp { get; set; }
        public virtual DateTime? Derp { get; set; }

        public virtual Topico Topico { get; set; }
        public virtual Usuario Usuario { get; set; }

        protected TopicoPost() { }

        public TopicoPost(int topicoId, int usuarioId, string descricao)
        {
            TopicoId = topicoId;
            UsuarioId = usuarioId;
            Descricao = Conversion.StrToByteArray(descricao);
            DataPost = ConfigurationBase.DataAtual;
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
