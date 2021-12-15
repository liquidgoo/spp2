using System;
using IGeneratorNamespace;

namespace FakerProject
{
    public class RealGenerator : IGenerator
    {
        private static readonly int MAX_POWER = 10;
        public override object Generate(GeneratorContext context)
        {
            Type targetType = context.TargetType;
            if (CanGenerate(targetType))
            {
                object toReturn =
                Convert.ChangeType(
                    context.Random.NextDouble() * Math.Pow(10, context.Random.Next(0, MAX_POWER)),
                    targetType
                );
                return toReturn;
            }
            else
                return null;
        }

        public override bool CanGenerate(Type type)
        {
            TypeCode typeCode = GetUnderlyingTypeCode(type);
            switch (typeCode)
            {
                case TypeCode.Single:
                case TypeCode.Double:
                    return true;
                default:
                    return false;
            }
        }
    }
}

