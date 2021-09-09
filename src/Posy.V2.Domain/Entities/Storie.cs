using Posy.V2.Domain.Enums;
using Posy.V2.Infra.CrossCutting.Common;
using Posy.V2.Infra.CrossCutting.Common.Validations;
using System;

namespace Posy.V2.Domain.Entities
{
    public class Storie : EntityBase
    {
        public Guid StorieId { get; private set; }
        public Guid UsuarioId { get; private set; }
        public StorieType StorieType { get; private set; }
        public int Length { get; private set; }
        public string Src { get; private set; }
        public string Preview { get; private set; }
        public string Link { get; private set; }
        public string LinkText { get; private set; }
        public string Seen { get; private set; }
        public string Time { get; private set; }
        public DateTime DataStorie { get; private set; }

        public virtual Usuario Usuario { get; set; }

        protected Storie()
        {

        }

        public Storie(Guid usuarioId, StorieType storieType, int length, string src, string preview, string link, string linkText, string seen, string time)
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

        public void Validate()
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
