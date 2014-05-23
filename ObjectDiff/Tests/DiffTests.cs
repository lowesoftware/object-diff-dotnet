using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace ObjectDiff.Tests
{
    [TestFixture]
    public class DiffTests
    {
        [Test]
        public void NoChange()
        {
            ObjectDifference diff = new ObjectDifference();

            A a1 = new A();

            a1.i1 = 1;
            a1.s1 = "Hello World";
            a1.a2.d1 = 3.14;
            a1.a2.s1 = "Foo";
            a1.a2.s2 = "Bar";



            A a2 = new A();

            a2.i1 = 1;
            a2.s1 = "Hello World";
            a2.a2.d1 = 3.14;
            a2.a2.s1 = "Foo";
            a2.a2.s2 = "Bar";



            string patch = diff.Difference(a1, a2);


            A anew = diff.Patch(a1, patch) as A;

            Assert.AreEqual(a2.i1, anew.i1);
        }

        [Test]
        public void ChangeLevel1()
        {
            ObjectDifference diff = new ObjectDifference();

            A a1 = new A();

            a1.i1 = 1;
            a1.s1 = "Hello World";
            a1.a2.d1 = 3.14;
            a1.a2.s1 = "Foo";
            a1.a2.s2 = "Bar";



            A a2 = new A();

            a2.i1 = 2;
            a2.s1 = "Hello World";
            a2.a2.d1 = 3.14;
            a2.a2.s1 = "Foo";
            a2.a2.s2 = "Bar";



            string patch = diff.Difference(a1, a2);


            A anew = diff.Patch(a1, patch) as A;

            Assert.AreEqual(anew.i1, 2);
        }

        [Test]
        public void ChangeLevel2()
        {
            ObjectDifference diff = new ObjectDifference();

            A a1 = new A();

            a1.i1 = 1;
            a1.s1 = "Hello World";
            a1.a2.d1 = 3.14;
            a1.a2.s1 = "Foo";
            a1.a2.s2 = "Bar";



            A a2 = new A();

            a2.i1 = 1;
            a2.s1 = "Hello World";
            a2.a2.d1 = 1.23;
            a2.a2.s1 = "Foo";
            a2.a2.s2 = "Bar";



            string patch = diff.Difference(a1, a2);


            A anew = diff.Patch(a1, patch) as A;

            Assert.AreEqual(anew.a2.d1, 1.23);
        }

        [Test]
        public void ListChange()
        {
            ObjectDifference diff = new ObjectDifference();

            A a1 = new A();

            a1.i1 = 1;
            a1.s1 = "Hello World";
            a1.a2.d1 = 3.14;
            a1.a2.s1 = "Foo";
            a1.a2.s2 = "Bar";
            a1.stringlist.Add("Hello");
            a1.a2list.Add(new A2() { d1=123 });

            A a2 = new A();

            a2.i1 = 1;
            a2.s1 = "Hello World";
            a2.a2.d1 = 3.14;
            a2.a2.s1 = "Foo";
            a2.a2.s2 = "Bar";
            a2.stringlist.Add("World");
            a2.a2list.Add(new A2() { d1 = 456 });



            string patch = diff.Difference(a1, a2);


            A anew = diff.Patch(a1, patch) as A;

            Assert.AreEqual(anew.stringlist.Exists(a=>a=="Hello"), false);
            Assert.AreEqual(anew.stringlist.Exists(a => a == "World"), true);


            Assert.AreEqual(anew.a2list.Exists(a => a.d1==123), false);
            Assert.AreEqual(anew.a2list.Exists(a => a.d1 == 456), true);
        }
    }



    public class A
    {
        public int i1;
        public string s1;

        public A2 a2 = new A2();

        public List<string> stringlist = new List<string>();

        public List<A2> a2list = new List<A2>();
    }

    public class A2
    {
        public double d1;
        public string s1;
        public string s2;
    }
}
