using System;
using IGeneratorNamespace;
using System.Collections.Generic;

namespace FakerProject
{
    
    public class Faker : IFaker
    {

        private List<IGenerator> generators;
        private Random rand;
        public Faker()
        {
            rand = new Random();
            generators = new List<IGenerator>();
            generators.Add(new IntGenerator());
            generators.Add(new StringGenerator());
            generators.Add(new RealGenerator());
            generators.Add(new DateGenerator());
            generators.Add(new ListGenerator());
        }

        public bool AddGenerator(IGenerator generator)
        {
            foreach(IGenerator gen in generators)
            {
                if (gen.GetType().Equals(generator.GetType())) return false;
            }
            generators.Add(generator);
            return true;
        }

        public T Create<T>()
        {    
            return (T) Create(typeof(T));
        }
        
        
        private object Create(Type t)
        {
            foreach(IGenerator generator in generators) { 
                if (generator.CanGenerate(t))
                {
                    return generator.Generate(new GeneratorContext(rand, t, this));
                }
            }
            
            object generatedObject = null;

            return generatedObject;
        }
    }
}
