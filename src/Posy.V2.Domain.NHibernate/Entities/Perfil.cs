using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Common.Conversions;
using Posy.V2.Infra.CrossCutting.Common.Enums;
using Posy.V2.Infra.CrossCutting.Common.Resources;
using Posy.V2.Infra.CrossCutting.Common.Validations;
using System;

namespace Posy.V2.Domain.Entities
{
    public class Perfil : EntityBase
    {
        public const int NomeMaxLength = 15;
        public virtual string Nome { get; set; }

        public const int SobrenomeMaxLength = 15;
        public virtual string Sobrenome { get; set; }

        public const int AliasMaxLength = 36;
        public virtual string Alias { get; set; }

        public virtual string PaisId { get; set; }

        public virtual DateTime DataNascimento { get; set; }

        public virtual Sexo Sexo { get; set; }

        public virtual EstadoCivil EstadoCivil { get; set; }

        public virtual byte[] FrasePerfil { get; set; }

        //private Usuario _usuario;

        //public virtual Usuario Usuario
        //{
        //    get { return _usuario; }
        //    set
        //    {
        //        _usuario = value;
        //        _usuario.Perfil = this;
        //    }
        //}
        public virtual Usuario Usuario { get; set; }

        //[NotMapped]
        public virtual string FraseHtml
        {
            get
            {
                return FrasePerfil == null ? string.Empty : Conversion.ByteArrayToStr(FrasePerfil);
            }
        }
        public virtual byte[] DescricaoPerfil { get; set; }

        //[NotMapped]
        public virtual string PerfilHtml
        {
            get
            {
                return DescricaoPerfil == null ? string.Empty : Conversion.ByteArrayToStr(DescricaoPerfil);
            }
        }

        //[NotMapped]
        public virtual string Foto { get; set; }

        public virtual DateTime Dar { get; set; }


        protected Perfil()
        {

        }

        public Perfil(int id, string nome, string sobrenome, string alias, DateTime dataNascimento, Sexo sexo, EstadoCivil estadoCivil, string paisId)
        {
            Id = id;
            Nome = nome;
            Sobrenome = sobrenome;
            DataNascimento = dataNascimento;
            Sexo = sexo;
            EstadoCivil = estadoCivil;
            Alias = alias;
            PaisId = paisId; // 1058;
            Dar = ConfigurationBase.DataAtual;
            Foto = "1";

            Validate();
        }

        public virtual void Edit(string nome, string sobrenome, string alias, DateTime dataNascimento, Sexo sexo, EstadoCivil estadoCivil, string frasePerfil, string descricaoPerfil, string paisId)
        {
            Nome = nome;
            Sobrenome = sobrenome;
            Alias = alias;
            DataNascimento = dataNascimento;
            Sexo = sexo;
            EstadoCivil = estadoCivil;

            if (!string.IsNullOrEmpty(frasePerfil))
                FrasePerfil = Conversion.StrToByteArray(frasePerfil);

            if (!string.IsNullOrEmpty(descricaoPerfil))
                DescricaoPerfil = Conversion.StrToByteArray(descricaoPerfil);

            PaisId = paisId;

            Validate();
        }

        public virtual void Validate()
        {
            Valid.AssertArgumentNotNull(Nome, Errors.NomeInvalido);
            Valid.AssertArgumentNotEmpty(Nome, Errors.NomeInvalido);
            Valid.AssertArgumentLength(Nome, NomeMaxLength, Errors.NomeInvalidoTamanho);

            Valid.AssertArgumentNotNull(Sobrenome, Errors.SobrenomeInvalido);
            Valid.AssertArgumentNotEmpty(Sobrenome, Errors.SobrenomeInvalido);
            Valid.AssertArgumentLength(Sobrenome, SobrenomeMaxLength, Errors.SobrenomeInvalidoTamanho);

            Valid.AssertArgumentNotNull(Alias, Errors.AliasInvalido);
            Valid.AssertArgumentNotEmpty(Alias, Errors.AliasInvalido);
            Valid.AssertArgumentLength(Alias, AliasMaxLength, Errors.AliasInvalido);

            Valid.AssertArgumentNotNull(PaisId, Errors.PaisInvalido);
            Valid.AssertArgumentNotEmpty(PaisId, Errors.PaisInvalido);

            Valid.AssertArgumentNotNull(DataNascimento, Errors.DataNascimentoInvalida);
            Valid.AssertArgumentNotNull(Sexo, Errors.SexoInvalido);
            Valid.AssertArgumentNotNull(EstadoCivil, Errors.EstadoCivilInvalido);
        }
    }
}
