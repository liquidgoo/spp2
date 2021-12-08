using System;


namespace FakerProject
{
    
    public class Faker : IFaker
    {
        private static readonly int SUPPORTED_TOTAL = 7;
        public Faker()
        {
            rand = new Random();
            generators = new IGenerator[SUPPORTED_TOTAL];
            generators[0] = new IntGenerator();
            generators[1] = new StringGenerator();
            generators[2] = new RealGenerator();
            generators[3] = new DateGenerator();
            generators[4] = new ListGenerator();
            generators[5] = new StructGenerator();
            generators[6] = new ClassGenerator();
        }

        public T Create<T>()
        {    
            return (T) Create(typeof(T));
        }
        
        
        private object Create(Type t)
        {
            for (int i = 0; i < SUPPORTED_TOTAL; ++i)
            {
                if (generators[i].CanGenerate(t))
                {
                    return generators[i].Generate(new GeneratorContext(rand, t, this));
                }
            }
            object generatedObject = null;

            return generatedObject;
        }


        private IGenerator[] generators;
        private Random rand;
    }
}
