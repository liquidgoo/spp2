using FakerProject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;

namespace TestProject1
{
    #region struct A
    struct A
    {
        private A(int D) { s = null; anotherString = null; d = 0; f = 0.0; Console.WriteLine("Private constructor"); }
        public A(int D, double F) { s = "QWERTY"; anotherString = null; d = 0; f = 34.5678; }
        public A(string s) { this.s = s; anotherString = "asdf"; d = 0; f = 0.0; }
        public A(double t) { s = null; anotherString = null; d = 0; f = 12.0; }

        public string s;
        public string anotherString;
        public int d;
        public double f;
    }
    #endregion

    #region other structs
    struct B
    {
        public string str;
        public int a;
        public double d;
        DateTime date;
    }

    struct C
    {
        public short d;
        public ushort b;
        public B b__;
    }

    struct D
    {
        public C c;
        public B b;
    }

    struct E
    {
        public D d;
    }


    #endregion

    #region other classes
    class AA
    {
        public AA(int x) { MyIntProperty = x; }
        public AA(int x, int y) { MyIntProperty = x + y; }
        public BB bb { get; set; }
        public int MyIntProperty { get; set; }
    }

    class BB
    {
        public CC cc { get; set; }
    }

    class CC
    {
        public AA aa { get; set; }
    }
    #endregion
    [TestClass]
    public class UnitTest1
    {

        private static void printPrintList<T>(IList<T> list) where T : IList
        {
            foreach (T var in list)
            {
                foreach (object inner in var)
                {
                    Console.Write(inner + " ");
                }
                Console.WriteLine();
            }
        }

        private static void printList<T>(IList<T> list)
        {
            foreach (object var in list)
            {
                Console.Write(var + " ");
            }
            Console.WriteLine();
        }

        [TestMethod]
        public void TestCycle()
        {
            Faker faker = new Faker();

            AA a = faker.Create<AA>();
            Assert.IsTrue(a.MyIntProperty >= int.MinValue && a.MyIntProperty <= int.MaxValue, "Int init");
            Assert.IsNull(a.bb.cc.aa, "Cycle end");

            Console.WriteLine(a.bb);
            Console.WriteLine(a.MyIntProperty);
            Console.WriteLine(a.bb.cc.aa == null ? "Null" : "Not null");
        }

        [TestMethod]
        public void TestEnclosedStructs()
        {
            Faker faker = new Faker();

            E e = faker.Create<E>();
            Assert.IsNotNull(e.d.b.str, "String init");
            Assert.IsTrue(e.d.b.a >= 0 && e.d.b.a <= int.MaxValue, "Int init");
            Assert.IsTrue(e.d.b.d >= 0.0 && e.d.b.d <= double.MaxValue, "Double init");
            Assert.IsTrue(e.d.c.d >= 0.0 && e.d.c.d <= short.MaxValue, "Short init");
            Assert.IsTrue(e.d.c.b >= 0.0 && e.d.c.b <= ushort.MaxValue, "UShort init");

            Console.WriteLine(e.d.b.str);
            Console.WriteLine(e.d.b.a);
            Console.WriteLine(e.d.b.d);
            Console.WriteLine(e.d.c.d);
            Console.WriteLine(e.d.c.b);
            Console.WriteLine("==========================================");
        }

        [TestMethod]
        public void TestStructs()
        {
            Faker faker = new Faker();

            A obj1 = faker.Create<A>();

            Assert.AreEqual("QWERTY", obj1.s, "S field of A is supposed to be QWERTY!");
            Assert.IsNotNull(obj1.anotherString, "AnotherString field of A should have been initialized!");
            Assert.IsTrue(obj1.f >= 0.0 && obj1.f <= double.MaxValue, "Double should have been initialized!");
            Assert.IsTrue(obj1.d >= 0.0 && obj1.d <= int.MaxValue, "Int should have been initialized");

            Console.WriteLine("S = " + obj1.s);
            Console.WriteLine("AnotherString = " + obj1.anotherString);
            Console.WriteLine("F = " + obj1.f);
            Console.WriteLine("D = " + obj1.d);
            Console.WriteLine("==========================================");
        }

        [TestMethod]
        public void TestListType()
        {
            Faker faker = new Faker();

            List<int> intList = faker.Create<List<int>>();
            List<double> doubleList = faker.Create<List<double>>();
            List<string> stringList = faker.Create<List<string>>();
            List<DateTime> dateList = faker.Create<List<DateTime>>();
            List<List<string>> listList = faker.Create<List<List<string>>>();

            Assert.IsNotNull(intList, "IntegerList went wrong!");
            Assert.IsNotNull(doubleList, "DoubleList went wrong!");
            Assert.IsNotNull(stringList, "StringList went wrong!");
            Assert.IsNotNull(dateList, "DateList went wrong!");
            Assert.IsNotNull(listList, "ListList went wrong!");

            printList(intList);
            printList(doubleList);
            printList(stringList);
            printList(dateList);
            printPrintList(listList);
            Console.WriteLine("==========================================");
        }

        [TestMethod]
        public void TestDateType()
        {
            Faker faker = new Faker();

            DateTime dt = faker.Create<DateTime>();
            Assert.IsNotNull(dt, "DateTime went wrong!");

            Console.WriteLine(dt);
            Console.WriteLine("==========================================");
        }

        [TestMethod]
        public void TestStringType()
        {
            Faker faker = new Faker();

            string str = faker.Create<string>();
            Assert.IsNotNull(str, "String went wrong!");

            Console.WriteLine("String = " + str);
            Console.WriteLine("==========================================");
        }

        [TestMethod]
        public void TestRealTypes()
        {
            Faker faker = new Faker();

            float fl = faker.Create<float>();
            double d = faker.Create<double>();

            Assert.IsTrue(fl >= 0 && fl < float.MaxValue, "Float went wrong!");
            Assert.IsTrue(d >= 0 && d < double.MaxValue, "Double went wrong!");

            Console.WriteLine("Float = " + fl);
            Console.WriteLine("Double = " + d);
            Console.WriteLine("==========================================");
        }

        [TestMethod]
        public void TestIntTypes()
        {
            Faker faker = new Faker();

            byte b = faker.Create<byte>();
            sbyte sb = faker.Create<sbyte>();
            ushort us = faker.Create<ushort>();
            int i = faker.Create<int>();
            short sh = faker.Create<short>();

            Assert.IsTrue(b >= 0 && b < byte.MaxValue, "Byte went wrong!");
            Assert.IsTrue(sb >= 0 && sb < sbyte.MaxValue, "Short Byte went wrong!");
            Assert.IsTrue(us >= 0 && us < ushort.MaxValue, "Unsigned short went wrong!");
            Assert.IsTrue(i >= 0 && i < int.MaxValue, "Int went wrong!");
            Assert.IsTrue(sh >= 0 && sh < short.MaxValue, "Short went wrong!");

            Console.WriteLine("Byte = " + b);
            Console.WriteLine("Short Byte = " + sb);
            Console.WriteLine("Unsigned short = " + us);
            Console.WriteLine("Int = " + i);
            Console.WriteLine("Short = " + sh);
            Console.WriteLine("==========================================");
        }
    }
}
