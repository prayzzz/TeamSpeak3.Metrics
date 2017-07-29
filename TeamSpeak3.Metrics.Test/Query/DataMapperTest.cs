using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeamSpeak3.Metrics.Query;

namespace TeamSpeak3.Metrics.Test.Query
{
    [TestClass]
    public class DataMapperTest
    {
        [TestMethod]
        public void TestUnderscoreCase()
        {
            var data = new Dictionary<string, string>
            {
                { "Value_1", "Foo" },
                { "Value_2", "Bar" }
            };

            var mappedObject = DataMapper.Map<DataObject>(new List<Dictionary<string, string>> { data });

            Assert.AreEqual("Foo", mappedObject.Value1);
            Assert.AreEqual("Bar", mappedObject.Value2);
        }

        [TestMethod]
        public void TestSingleToSingle()
        {
            var data = new Dictionary<string, string>
            {
                { "Value1", "Foo" },
                { "Value2", "Bar" }
            };

            var mappedObject = DataMapper.Map<DataObject>(new List<Dictionary<string, string>> { data });

            Assert.AreEqual("Foo", mappedObject.Value1);
            Assert.AreEqual("Bar", mappedObject.Value2);
        }

        [TestMethod]
        public void TestMultipleToSingle()
        {
            var data1 = new Dictionary<string, string>
            {
                { "Value1", "Foo" },
                { "Value2", "Bar" }
            };

            var data2 = new Dictionary<string, string>
            {
                { "Value1", "Foo42" },
                { "Value2", "Bar42" }
            };

            var mappedObject = DataMapper.Map<DataObject>(new List<Dictionary<string, string>> { data1, data2 });

            Assert.AreEqual("Foo", mappedObject.Value1);
            Assert.AreEqual("Bar", mappedObject.Value2);
        }

        [TestMethod]
        public void TestMultipleToMultiple()
        {
            var data1 = new Dictionary<string, string>
            {
                { "Value1", "Foo" },
                { "Value2", "Bar" }
            };

            var data2 = new Dictionary<string, string>
            {
                { "Value1", "Foo42" },
                { "Value2", "Bar42" }
            };

            var mappedObjects = DataMapper.Map<List<DataObject>>(new List<Dictionary<string, string>> { data1, data2 });

            Assert.AreEqual(2, mappedObjects.Count);
            Assert.AreEqual("Foo", mappedObjects[0].Value1);
            Assert.AreEqual("Bar", mappedObjects[0].Value2);
            Assert.AreEqual("Foo42", mappedObjects[1].Value1);
            Assert.AreEqual("Bar42", mappedObjects[1].Value2);
        }

        [TestMethod]
        public void TestSingleToMultiple()
        {
            var data = new Dictionary<string, string>
            {
                { "Value1", "Foo" },
                { "Value2", "Bar" }
            };

            var mappedObjects = DataMapper.Map<List<DataObject>>(new List<Dictionary<string, string>> { data });

            Assert.AreEqual(1, mappedObjects.Count);
            Assert.AreEqual("Foo", mappedObjects[0].Value1);
            Assert.AreEqual("Bar", mappedObjects[0].Value2);
        }

        public class DataObject
        {
            public string Value1 { get; set; }

            public string Value2 { get; set; }
        }
    }
}