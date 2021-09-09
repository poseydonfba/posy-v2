using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Common.Conversions;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using Posy.V2.Infra.CrossCutting.Common.Validations;
using System;

namespace Posy.V2.Domain.Entities
{
    public class PostPerfil : EntityBase
    {
        public Guid PostPerfilId { get; private set; }
        public Guid UsuarioId { get; private set; }
        public byte[] DescricaoPost { get; private set; }
        public DateTime DataPost { get; private set; }
        public DateTime? Der { get; private set; }

        //[NotMapped]
        public bool IsOculto
        {
            get
            {
                //var postOculto = PostsOculto.Where(x => x.StatusPost == PostOcultoFlag.Oculto).Any();
                //return PostsOculto.Where(x => x.StatusPost == PostOcultoFlag.Oculto).Any();

                return false;
            }
        }

        public string PostHtml
        {
            get
            {
                return DescricaoPost == null ? "" : Conversion.ByteArrayToStr(DescricaoPost);
            }
        }

        public virtual Usuario Usuario { get; set; }

        protected PostPerfil()
        {
            PostPerfilId = Guid.NewGuid();
            DataPost = ConfigurationBase.DataAtual;
        }

        public PostPerfil(Guid usuarioId, string descricaoPost) // : base()
        {
            PostPerfilId = Guid.NewGuid();
            DataPost = ConfigurationBase.DataAtual;

            UsuarioId = usuarioId;
            DescricaoPost = Conversion.StrToByteArray(descricaoPost);

            Validate();
        }

        public void Delete()
        {
            Der = ConfigurationBase.DataAtual;
        }

        public void Validate()
        {
            Valid.AssertArgumentNotNull(UsuarioId, Errors.UsuarioInvalido);
            Valid.AssertArgumentNotNull(UsuarioId, Errors.UsuarioInvalido);
            Valid.AssertArgumentNotNull(DescricaoPost, Errors.NenhumPostInformado);
        }
    }
}
