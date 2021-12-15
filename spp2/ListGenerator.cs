using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IGeneratorNamespace;

namespace FakerProject
{

    public class ListGenerator : IGenerator
    {
        private static readonly int MAX_ELEMENTS = 20;
        public override object Generate(GeneratorContext context)
        {
            Type targetType = context.TargetType;
            if (CanGenerate(targetType))
            {
                Type[] innerTypes = targetType.GetGenericArguments();
                Type innerType = innerTypes[0];
                if (innerTypes.Length != 1) throw new NotSupportedException();

                int elementsCount = context.Random.Next(0, MAX_ELEMENTS);
                object toReturn = Activator.CreateInstance(targetType);

                object[] arr = new object[1];
                for (int i = 0; i < elementsCount; ++i)
                {
                    arr[0] = GetGenericCreate(innerType).Invoke(context.Faker, null);
                    targetType.GetMethod("Add").Invoke(toReturn, arr);
                }
                
                return toReturn;
            }
            else
                return null;
        }

        public override bool CanGenerate(Type type)
        {
            if (type.IsValueType) return false;
            if (!type.IsGenericType) return false;
            if (type.GetGenericTypeDefinition() == typeof(List<>)) {  return true; }
            return false;
        }
    }
}

