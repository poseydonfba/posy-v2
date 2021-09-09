using Posy.V2.Domain.Enums;
using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Common.Conversions;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using Posy.V2.Infra.CrossCutting.Common.Validations;
using System;

namespace Posy.V2.Domain.Entities
{
    public class Comunidade : EntityBase
    {
        public Guid ComunidadeId { get; private set; }
        public Guid UsuarioId { get; private set; }
        public string Alias { get; private set; }
        public string Nome { get; private set; }
        public TipoComunidade TipoComunidade { get; private set; }
        public int CategoriaId { get; private set; }
        public byte[] DescricaoPerfil { get; private set; }
        //[NotMapped]
        public string PerfilHtml
        {
            get
            {
                return DescricaoPerfil == null ? "" : Conversion.ByteArrayToStr(DescricaoPerfil);
            }
        } 
        //[NotMapped]
        public string Foto { get; set; }

        public DateTime Dir { get; set; }
        public Guid Uar { get; set; }
        public DateTime Dar { get; set; }
        public Guid? Uer { get; set; }
        public DateTime? Der { get; set; }

        public virtual Usuario Usuario { get; set; }
        public virtual Categoria Categoria { get; set; }

        protected Comunidade()
        {
            ComunidadeId = Guid.NewGuid();
            Dir = DateTime.Now;
            Dar = Dir;
        }

        public Comunidade(Guid usuarioId, string nome, int categoriaId, string descricaoPerfil)
        {
            ComunidadeId = Guid.NewGuid();
            UsuarioId = usuarioId;
            Alias = ComunidadeId.ToString();
            Nome = nome;
            TipoComunidade = TipoComunidade.Privada;
            CategoriaId = categoriaId;
            DescricaoPerfil = Conversion.StrToByteArray(descricaoPerfil);
            Dir = ConfigurationBase.DataAtual;
            Uar = usuarioId;
            Dar = Dir;

            Validate();
        }

        public void Edit(string alias, string nome, int categoriaId, string descricaoPerfil, Guid uar)
        {
            Alias = string.IsNullOrEmpty(alias) ? ComunidadeId.ToString() : alias;
            Nome = nome;
            CategoriaId = categoriaId;
            DescricaoPerfil = Conversion.StrToByteArray(descricaoPerfil);            
            Uar = uar;
            Dar = ConfigurationBase.DataAtual;

            Validate();
        }

        public void SetPrivacidade(TipoComunidade tipoComunidade, Guid uar)
        {
            TipoComunidade = tipoComunidade;
            Uar = uar;
            Dar = ConfigurationBase.DataAtual;

            Validate();
        }

        public void Delete(Guid usuarioIdExclusao)
        {
            Uer = usuarioIdExclusao;
            Der = ConfigurationBase.DataAtual;
        }

        public bool IsDono(Guid usuarioId)
        {
            return UsuarioId == usuarioId;
        }

        public void Validate()
        {
            Valid.AssertArgumentNotNull(UsuarioId, Errors.UsuarioInvalido);
            Valid.AssertArgumentNotNull(Alias, Errors.AliasInvalido);
            Valid.AssertArgumentNotNull(Nome, Errors.NomeInvalido);
            Valid.AssertArgumentNotEmpty(Alias, Errors.AliasInvalido);
            Valid.AssertArgumentNotNull(TipoComunidade, Errors.TipoInvalido);
            Valid.AssertArgumentNotNull(CategoriaId, Errors.CategoriaInvalida);
        }
    }
}
