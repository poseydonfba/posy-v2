using Microsoft.VisualStudio.TestTools.UnitTesting;
using Posy.V2.Domain.ValueObject;
using System;

namespace Posy.V2.Domain.Tests.ValueObject
{
    [TestClass]
    public class TelefoneTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Telefone_New_DDD_Em_Branco()
        {
            var telefone = new Telefone("", "988887777");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Telefone_New_DDD_Null()
        {
            var telefone = new Telefone(null, "988887777");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Telefone_New_DDD_MaxLength()
        {
            var ddd = "098";
            while (ddd.Length < Telefone.DDDMaxLength + 1)
            {
                ddd = ddd + "098";
            }

            new Telefone("9874", "988887777");
        }

        [TestMethod]
        public void Telefone_New_DDD_Valido()
        {
            var ddd = "098";
            var telefone = new Telefone(ddd, "988887777");

            Assert.AreEqual(ddd, telefone.DDD);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Telefone_New_Numero_Em_Branco()
        {
            var telefone = new Telefone("098", "");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Telefone_New_Numero_Null()
        {
            var telefone = new Telefone("098", null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Telefone_New_Numero_MaxLength()
        {
            var numero = "988887777";
            while (numero.Length < Telefone.NumeroMaxLength + 1)
            {
                numero = numero + "988887777";
            }

            new Telefone("098", numero);
        }

        [TestMethod]
        public void Telefone_New_Numero_Valido()
        {
            var numero = "988887777";
            var telefone = new Telefone("098", numero);

            Assert.AreEqual(numero, telefone.Numero);
        }
    }
}
