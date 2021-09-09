using Posy.V2.Domain.Enums;
using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Common.Conversions;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using Posy.V2.Infra.CrossCutting.Common.Validations;
using System;
using System.Collections.Generic;

namespace Posy.V2.Domain.Entities
{
    public class Comunidade : EntityBase
    {
        public virtual int UsuarioId { get; set; }
        public virtual string Alias { get; set; }
        public virtual string Nome { get; set; }
        public virtual TipoComunidade TipoComunidade { get; set; }
        public virtual int CategoriaId { get; set; }
        public virtual byte[] DescricaoPerfil { get; set; }
        //[NotMapped]
        public virtual string PerfilHtml
        {
            get
            {
                return DescricaoPerfil == null ? "" : Conversion.ByteArrayToStr(DescricaoPerfil);
            }
        } 
        //[NotMapped]
        public virtual string Foto { get; set; }

        public virtual DateTime Dir { get; set; }
        public virtual int Uar { get; set; }
        public virtual DateTime Dar { get; set; }
        public virtual int? Uer { get; set; }
        public virtual DateTime? Der { get; set; }

        public virtual Usuario Usuario { get; set; }
        public virtual Categoria Categoria { get; set; }

        public virtual ICollection<Topico> Topicos { get; set; }
        public virtual ICollection<Membro> Membros { get; set; }

        public virtual ICollection<Moderador> Moderadores { get; set; }

        protected Comunidade()
        {
            Dir = DateTime.Now;
            Dar = Dir;
        }

        public Comunidade(int usuarioId, string nome, int categoriaId, string descricaoPerfil)
        {
            UsuarioId = usuarioId;
            Alias = Id.ToString();
            Nome = nome;
            TipoComunidade = TipoComunidade.Privada;
            CategoriaId = categoriaId;
            DescricaoPerfil = Conversion.StrToByteArray(descricaoPerfil);
            Dir = ConfigurationBase.DataAtual;
            Uar = usuarioId;
            Dar = Dir;

            Validate();
        }

        public virtual void Edit(string alias, string nome, int categoriaId, string descricaoPerfil, int uar)
        {
            Alias = string.IsNullOrEmpty(alias) ? Id.ToString() : alias;
            Nome = nome;
            CategoriaId = categoriaId;
            DescricaoPerfil = Conversion.StrToByteArray(descricaoPerfil);            
            Uar = uar;
            Dar = ConfigurationBase.DataAtual;

            Validate();
        }

        public virtual void SetPrivacidade(TipoComunidade tipoComunidade, int uar)
        {
            TipoComunidade = tipoComunidade;
            Uar = uar;
            Dar = ConfigurationBase.DataAtual;

            Validate();
        }

        public virtual void Delete(int usuarioIdExclusao)
        {
            Uer = usuarioIdExclusao;
            Der = ConfigurationBase.DataAtual;
        }

        public virtual bool IsDono(int usuarioId)
        {
            return UsuarioId == usuarioId;
        }

        public virtual void Validate()
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
