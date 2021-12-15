using System;
using IGeneratorNamespace;

namespace FakerProject
{
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
}

