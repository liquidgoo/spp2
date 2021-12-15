using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using IGeneratorNamespace;
namespace FakerProject
{
    class Program
    {
        public static void Main()
        {
            Faker f = new Faker();

            Assembly classGeneratorDll = Assembly.LoadFrom("D:\\smth\\походу уник\\spp\\spp2\\ClassGenerator\\bin\\Debug\\net5.0\\ClassGenerator.dll");
            Assembly structGeneratorDll = Assembly.LoadFrom("D:\\smth\\походу уник\\spp\\spp2\\StructGenerator\\bin\\Debug\\net5.0\\StructGenerator.dll");
            f.AddGenerator((IGenerator)classGeneratorDll.CreateInstance(classGeneratorDll.GetTypes()[0].FullName));
            f.AddGenerator((IGenerator)structGeneratorDll.CreateInstance(structGeneratorDll.GetTypes()[0].FullName));

            User ff = f.Create<User>();

            //ICollection<int> ff = f.Create<List<int>>();
            Console.WriteLine(JsonConvert.SerializeObject(ff, Formatting.Indented));

            //Console.WriteLine(ff.ToString());


        }
    }

    public class User
    {
        public String name;
        public int age;
        public List<Dog> dogs;
        public long test { get; set; }
        private float money = 10.5f;
        public Profile profile;
    }

    public class Dog
    {
        public string name;
        public User owner;
        public Dog(string name, User owner)
        {
            this.name = name;
            this.owner = owner;
        }

    }

    public class Profile
    {
        public string address;
        public Profile(string address)
        {
            this.address = address;
        }
        public Profile(string address, string hello)
        {
            this.address = address;
            throw new Exception();
        }
    }
}
