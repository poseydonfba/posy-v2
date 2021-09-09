using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Posy.V2.Infra.CrossCutting.Common.Validations;

namespace Posy.V2.Infra.CrossCutting.Common.Tests.Validations
{
    [TestClass]
    public class ValidTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Valid_AssertArgumentEquals_Erro_Quando_Diferente()
        {
            Valid.AssertArgumentEquals("A", "", "Os valores não podem ser diferente");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Valid_AssertArgumentNotEmpty_Erro_Quando_Em_Branco()
        {
            Valid.AssertArgumentNotEmpty("", "Valor não pode ser vazio");
        }
    }
}
