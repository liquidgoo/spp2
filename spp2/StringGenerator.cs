using System;
using IGeneratorNamespace;

namespace FakerProject
{
    public class StringGenerator : IGenerator
    {
        private static readonly int MAX_LENGTH = 35;

        public override object Generate(GeneratorContext context)
        {
            Type targetType = context.TargetType;
            if (CanGenerate(targetType))
            {
                int stringLength = context.Random.Next(0, MAX_LENGTH);
                object toReturn = GetRandomString(stringLength, context.Random);
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
                case TypeCode.String:
                    return true;
                default:
                    return false;
            }
        }

        private String GetRandomString(int length, Random random)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string s ="";
            for (int i =0; i < length; i++)
            s += chars[random.Next(chars.Length)];
            return s;
        }
    }
}

