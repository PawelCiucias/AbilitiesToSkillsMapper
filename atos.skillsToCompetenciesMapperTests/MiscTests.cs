using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace atos.skillsToCompetenciesMapperTests
{
    class Person : IEquatable<Person>
    {
        public string Name { get; set; }
        public Person(string Name) => this.Name = Name;
        public bool Equals(Person other) => this.Name == other.Name;

        public static IEqualityComparer<Person> GetEqualityComparer() => new EqualityComparer();

        private class EqualityComparer : IEqualityComparer<Person>
        {
            public bool Equals(Person x, Person y) => x.Equals(y);
            public int GetHashCode(Person person) => person.Name.GetHashCode();
        }
    }

    //[TestClass]
    //public class MyTestClass
    //{
    //    [TestMethod]
    //    public void MyTestMethod()
    //    {
    //        var TestData = new []{
    //            new Person("Pav Ciucias"),
    //            new Person("Pav Ciucias") };

    //        var hs = new HashSet<Person>(TestData, Person.GetEqualityComparer());
    //        Assert.IsTrue(hs.Count == 2);
    //    }
    //}
}