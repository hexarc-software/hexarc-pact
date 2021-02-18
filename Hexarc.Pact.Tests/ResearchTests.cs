using System;
using System.Linq;
using System.Reflection;
using Hexarc.Pact.Client;
using NUnit.Framework;

namespace Hexarc.Pact.Tests
{
    [TestFixture]
    public class ResearchTests
    {
        [Test]
        public void GetAllTypeMembers()
        {
            Console.WriteLine(ObjectDumper.Dump(typeof(ControllerBase)
                .GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Select(x => x.Name)));
        }
    }
}
