using Microsoft.VisualStudio.TestTools.UnitTesting;
using Posy.V2.Infra.CrossCutting.Common.Cache;
using System.Collections.Generic;

namespace Posy.V2.Infra.CrossCutting.Common.Tests.Cache
{
    [TestClass]
    public class CacheTests
    {
        ICacheService _cacheProvider;

        [TestInitialize]
        public void Initialize()
        {
            _cacheProvider = new RedisCacheProvider();
        }

        [TestMethod]
        public void Test_SetValue()
        {
            List<Person> people = new List<Person>()
            {
                new Person(1, "Joe", new List<Contact>()
                {
                    new Contact("1", "123456789"),
                    new Contact("2", "234567890")
                })
            };

            _cacheProvider.Set("People", people);
        }

        [TestMethod]
        public void Test_GetValue()
        {
            var people = _cacheProvider.Get<List<Person>>("People");

            Assert.IsNotNull(people);
            Assert.AreEqual(2, people[0].Contacts.Count);

            //var contacts = _cacheProvider.Get<List<Contact>>("People");

            //Assert.IsNotNull(contacts);
            //Assert.AreEqual(2, contacts.Count);
        }
    }

    public class Contact
    {
        public string Type { get; set; }

        public string Value { get; set; }

        public Contact(string type, string value)
        {
            this.Type = type;
            this.Value = value;
        }
    }

    public class Person
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public List<Contact> Contacts { get; set; }

        public Person(long id, string name, List<Contact> contacts)
        {
            this.Id = id;
            this.Name = name;
            this.Contacts = contacts;
        }
    }
}
