using Posy.V2.Infra.CrossCutting.Common.Resources;
using Posy.V2.Infra.CrossCutting.Common.Validations;

namespace Posy.V2.Domain.ValueObject
{
    public class Telefone
    {
        public const int NumeroMaxLength = 20;
        public string Numero { get; private set; }

        public const int DDDMaxLength = 3;
        public string DDD { get; private set; }

        protected Telefone()
        {

        }

        public Telefone(string ddd, string numero)
        {
            SetTelefone(numero);
            SetDDD(ddd);
        }

        private void SetTelefone(string numero)
        {
            Valid.AssertArgumentNotNull(numero, Errors.TelefoneInvalido);
            Valid.AssertArgumentNotEmpty(numero, Errors.TelefoneInvalido);
            Valid.AssertArgumentLength(numero, NumeroMaxLength, Errors.TelefoneInvalido);

            Numero = numero;
        }

        private void SetDDD(string ddd)
        {
            Valid.AssertArgumentNotNull(ddd, Errors.DDDInvalido);
            Valid.AssertArgumentNotEmpty(ddd, Errors.DDDInvalido); 
            Valid.AssertArgumentLength(ddd, DDDMaxLength, Errors.DDDInvalido);

            DDD = ddd;
        }

        public string GetTelefoneCompleto()
        {
            return DDD + Numero;
        }
    }
}
