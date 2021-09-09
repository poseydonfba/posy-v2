using System.ComponentModel;

namespace Posy.V2.Infra.CrossCutting.Common.Enums
{
    public enum EstadoCivil
    {
        SOLTEIRO = 1,
        CASADO = 2,
        DIVORCIADO = 3,
        [Description("VIÚVO")]
        VIUVO = 4,
        SEPARADO = 5
    }
}
