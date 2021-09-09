using Microsoft.VisualStudio.TestTools.UnitTesting;
using Posy.V2.Domain.ValueObject;
using System;

namespace Posy.V2.Domain.Tests.ValueObject
{
    [TestClass]
    public class EmailTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Email_New_Email_Em_Branco()
        {
            var email = new Email("");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Email_New_Email_Null()
        {
            var email = new Email(null);
        }

        [TestMethod]
        public void Email_New_Email_Valido()
        {
            var endereco = "admin@gmail.com";
            var email = new Email(endereco);
            Assert.AreEqual(endereco, email.Endereco);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Email_New_Email_Invalido()
        {
            var email = new Email("sdfgsdbglsdkjbgsdlkgb");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Email_New_Email_Erro_MaxLength()
        {
            var endereco = "admin@gmail.com.br";
            while (endereco.Length < Email.EnderecoMaxLength + 1)
            {
                endereco = endereco + "admin@gmail.com.br";
            }

            new Email(endereco);
        }
    }
}
