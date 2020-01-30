using NUnit.Framework;
using FindFriends.Controllers;
using Microsoft.AspNetCore.Mvc;


namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            ValuesController v = new ValuesController();
            
            string b = new string[] { "value1", "value2" }.ToString();

            string a = v.Teste().ToString();

            StringAssert.AreEqualIgnoringCase(a, b);
        }
    }
}