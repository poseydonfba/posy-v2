using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Common.Conversions;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using Posy.V2.Infra.CrossCutting.Common.Validations;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Entities
{
    public class PostPerfil : EntityBase
    {
        public virtual int UsuarioId { get; set; }
        public virtual byte[] DescricaoPost { get; set; }
        public virtual DateTime DataPost { get; set; }
        public virtual DateTime? Der { get; set; }

        //[NotMapped]
        public virtual bool IsOculto
        {
            get
            {
                //var postOculto = PostsOculto.Where(x => x.StatusPost == PostOcultoFlag.Oculto).Any();
                //return PostsOculto.Where(x => x.StatusPost == PostOcultoFlag.Oculto).Any();

                return false;
            }
        }
        
        public virtual string PostHtml
        {
            get
            {
                return DescricaoPost == null ? "" : Conversion.ByteArrayToStr(DescricaoPost);
            }
        }

        public virtual Usuario Usuario { get; set; }

        public virtual ICollection<PostPerfilComentario> PostsPerfilComentario { get; set; }
        public virtual ICollection<PostOculto> PostsOculto { get; set; }

        protected PostPerfil()
        {
            DataPost = ConfigurationBase.DataAtual;
        }

        public PostPerfil(int usuarioId, string descricaoPost) // : base()
        {
            DataPost = ConfigurationBase.DataAtual;

            UsuarioId = usuarioId;
            DescricaoPost = Conversion.StrToByteArray(descricaoPost);

            Validate();
        }

        public virtual void Delete()
        {
            Der = ConfigurationBase.DataAtual;
        }

        public virtual void Validate()
        {
            Valid.AssertArgumentNotNull(UsuarioId, Errors.UsuarioInvalido);
            Valid.AssertArgumentNotNull(UsuarioId, Errors.UsuarioInvalido);
            Valid.AssertArgumentNotNull(DescricaoPost, Errors.NenhumPostInformado);
        }
    }
}
