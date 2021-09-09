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
        //public const int UsuarioIdMaxLength = 128;
        public Guid UsuarioId { get; private set; }

        public const int NomeMaxLength = 15;
        public string Nome { get; private set; }

        public const int SobrenomeMaxLength = 15;
        public string Sobrenome { get; private set; }

        public const int AliasMaxLength = 36;
        public string Alias { get; private set; }

        public string PaisId { get; set; }

        public DateTime DataNascimento { get; private set; }

        public Sexo Sexo { get; private set; }

        public EstadoCivil EstadoCivil { get; private set; }

        public byte[] FrasePerfil { get; private set; }

        public virtual Usuario Usuario { get; set; }

        //[NotMapped]
        public string FraseHtml
        {
            get
            {
                return FrasePerfil == null ? string.Empty : Conversion.ByteArrayToStr(FrasePerfil);
            }
        }
        public byte[] DescricaoPerfil { get; private set; }

        //[NotMapped]
        public string PerfilHtml
        {
            get
            {
                return DescricaoPerfil == null ? string.Empty : Conversion.ByteArrayToStr(DescricaoPerfil);
            }
        }

        //[NotMapped]
        public string Foto { get; set; }

        public DateTime Dar { get; set; }

               
        protected Perfil()
        {

        }

        // Apenas para o projeto DOS
        public Perfil(Guid usuarioId, string nome, string sobrenome, string alias, DateTime dataNascimento, Sexo sexo, EstadoCivil estadoCivil, string frase, string descricao, string paisId)
        {
            UsuarioId = usuarioId;
            Nome = nome;
            Sobrenome = sobrenome;
            DataNascimento = dataNascimento;
            Sexo = sexo;
            EstadoCivil = estadoCivil;
            Alias = alias;
            PaisId = paisId; // 1058;
            Dar = ConfigurationBase.DataAtual;
            Foto = "1";
            FrasePerfil = Conversion.StrToByteArray(frase);
            DescricaoPerfil = Conversion.StrToByteArray(descricao);

            Validate();
        }

        public Perfil(Guid usuarioId, string nome, string sobrenome, string alias, DateTime dataNascimento, Sexo sexo, EstadoCivil estadoCivil, string paisId)
        {
            UsuarioId = usuarioId;
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

        public void Edit(string nome, string sobrenome, string alias, DateTime dataNascimento, Sexo sexo, EstadoCivil estadoCivil, string frasePerfil, string descricaoPerfil, string paisId)
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

        public void Validate()
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
