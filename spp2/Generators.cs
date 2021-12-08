using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FakerProject
{

    public class IntGenerator : IGenerator
    {
        public override object Generate(GeneratorContext context)
        {
            Type targetType = context.TargetType;
            if (CanGenerate(targetType))
            {
                int maxValue = GetUnderlyingTypeMaxValue(GetUnderlyingTypeCode(targetType));
                object toReturn = Convert.ChangeType(context.Random.Next(0, maxValue), targetType);
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
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.Int16:
                case TypeCode.Int32:
                    return true;
                default:
                    return false;
            }
        }

        private int GetUnderlyingTypeMaxValue(TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.Byte:
                    return byte.MaxValue;
                case TypeCode.SByte:
                    return sbyte.MaxValue;
                case TypeCode.UInt16:
                    return ushort.MaxValue;
                case TypeCode.UInt32:
                    return int.MaxValue;
                case TypeCode.Int16:
                    return short.MaxValue;
                case TypeCode.Int32:
                    return int.MaxValue;
                default:
                    throw new ArgumentException("Passed in TypeCode was not an integral type!");
            }
        }
    }

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
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }

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

    public class DateGenerator : IGenerator
    {
        public override object Generate(GeneratorContext context)
        {
            Type targetType = context.TargetType;
            if (CanGenerate(targetType))
            {
                object toReturn = GetRandomDate(context.Random);
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
                case TypeCode.DateTime:
                    return true;
                default:
                    return false;
            }
        }

        private DateTime GetRandomDate(Random random)
        {
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(random.Next(range))
                   .AddHours(random.Next(0, 23))
                   .AddMinutes(random.Next(0, 59))
                   .AddSeconds(random.Next(0, 59));
        }
    }

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

