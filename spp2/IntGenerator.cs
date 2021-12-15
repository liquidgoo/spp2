using System;
using IGeneratorNamespace;

namespace FakerProject
{
    public class IntGenerator : IGenerator
    {
        public override object Generate(GeneratorContext context)
        {
            Type targetType = context.TargetType;
            if (CanGenerate(targetType))
            {
                byte[] bytes = new byte[8];
                context.Random.NextBytes(bytes);

                object toReturn = convertBytes(bytes, targetType);
                return toReturn;
            }
            else
                return null;
        }

        private object convertBytes(byte[] bytes, Type targetType)
        {
            long num;
            num = BitConverter.ToInt64(bytes);
            object result = num;
            switch (GetUnderlyingTypeCode(targetType))
            {
                case TypeCode.Byte:
                    result = (byte)num;
                    break;
                case TypeCode.SByte:
                    result = (sbyte)num;
                    break;
                case TypeCode.UInt16:
                    result = (ushort)num;
                    break;
                case TypeCode.UInt32:
                    result = (uint)num;
                    break;
                case TypeCode.UInt64:
                    result = (ulong)num;
                    break;
                case TypeCode.Int16:
                    result = (short)num;
                    break;
                case TypeCode.Int32:
                    result = (int)num;
                    break;
                case TypeCode.Int64:
                    result = (long)num;
                    break;
            }
            return result;
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
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    return true;
                default:
                    return false;
            }
        }

    }
}

