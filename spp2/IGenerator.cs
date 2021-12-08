using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FakerProject
{
    public abstract class IGenerator
    {
        public abstract object Generate(GeneratorContext context);
        public abstract bool CanGenerate(Type type);

        public TypeCode GetUnderlyingTypeCode(Type type)
        {
            TypeCode typeCode = Type.GetTypeCode(type);
            return typeCode;
        }

        protected MethodInfo GetGenericCreate(Type paramType)
        {
            MethodInfo method = typeof(IFaker).GetMethods().Single(m => m.Name == "Create" && m.IsGenericMethodDefinition);
            method = method.MakeGenericMethod(paramType);
            return method;
        }


        protected static object GetDefaultValue(Type t)
        {
            if (t.IsValueType)
                return Activator.CreateInstance(t);
            else
                return null;
        }

    }

    public class GeneratorContext
    {
        public Random Random { get; }
        public Type TargetType { get; }
        public IFaker Faker { get; }
        public readonly Type SelfType;
        public GeneratorContext(Random random, Type targetType, IFaker faker)
        {
            Random = random;
            TargetType = targetType;
            Faker = faker;
        }
    }

    public interface IFaker
    {
        T Create<T>();
    }
}
