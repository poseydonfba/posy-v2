using Posy.V2.Domain.Enums;
using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Common.Validations;
using System;

namespace Posy.V2.Domain.Entities
{
    public class Storie : EntityBase
    {
        public virtual int UsuarioId { get; set; }
        public virtual StorieType StorieType { get; set; }
        public virtual int Length { get; set; }
        public virtual string Src { get; set; }
        public virtual string Preview { get; set; }
        public virtual string Link { get; set; }
        public virtual string LinkText { get; set; }
        public virtual string Seen { get; set; }
        public virtual string Time { get; set; }
        public virtual DateTime DataStorie { get; set; }

        public virtual Usuario Usuario { get; set; }

        protected Storie()
        {

        }

        public Storie(int usuarioId, StorieType storieType, int length, string src, string preview, string link, string linkText, string seen, string time)
        {
            UsuarioId = usuarioId;
            StorieType = storieType;
            Length = length;
            Src = src;
            Preview = preview;
            Link = link;
            LinkText = linkText;
            Seen = seen;
            Time = time;
            DataStorie = ConfigurationBase.DataAtual;

            Validate();
        }

        public virtual void Validate()
        {
            Valid.AssertArgumentNotNull(StorieType, "Erro");
            //Valid.AssertArgumentLength(Nome, NomeMaxLength, Errors.NomeInvalidoTamanho);

            Valid.AssertArgumentNotNull(Length, "Erro");
            Valid.AssertArgumentNotEmpty(Length.ToString(), "Erro");

            Valid.AssertArgumentNotNull(Src, "Erro");
            Valid.AssertArgumentNotEmpty(Src, "Erro");

            Valid.AssertArgumentNotNull(Preview, "Erro");
            Valid.AssertArgumentNotEmpty(Preview, "Erro");

            Valid.AssertArgumentNotNull(Time, "Erro");
            Valid.AssertArgumentNotEmpty(Time, "Erro");
        }
    }
}
