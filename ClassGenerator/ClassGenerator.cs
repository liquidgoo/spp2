using System;
using System.Collections.Generic;
using System.Reflection;
using IGeneratorNamespace;
namespace ClassGeneratorNamespace
{
    public class ClassGenerator : IGenerator
    {
        const int maxNesting = 4;
        private List<string> previous = new List<string>();
        public override object Generate(GeneratorContext context)
        {
            Type targetType = context.TargetType;
            string typeName = targetType.FullName;

            if (CanGenerate(targetType))
            {
                if (DoesCauseCycle(typeName))
                {
                    return null;
                }
                previous.Add(typeName);

                object toReturn = InitializeObject(context, targetType);
                InitializeFields(toReturn, context);
                InitializeProperties(toReturn, context);

                previous.RemoveAt(previous.Count - 1);
                return toReturn;
            }
            else
                return null;
        }

        public override bool CanGenerate(Type type)
        {
            return type.IsClass && !type.IsGenericType;
        }

        private bool DoesCauseCycle(string name)
        {
            int count = 0;
            foreach (string prevType in previous)
            {
                if (prevType.Equals(name))
                {
                    count++;
                    if (count >= maxNesting) return true;
                }
            }
            //return previous.Contains(name);
            return false;
        }

        private object InitializeObject(GeneratorContext context, Type targetType)
        {
            ConstructorInfo[] p = targetType.GetConstructors();
            if (p.Length != 0)
            {
                bool notInstantiated = true;
                while (notInstantiated)
                {
                    int maxParam = -1, maxInd = -1;
                    for (int i = 0; i < p.Length; ++i)
                    {
                        if (p[i]?.GetParameters().Length > maxParam)
                        {
                            maxParam = p[i].GetParameters().Length;
                            maxInd = i;
                        }
                    }

                    if (maxInd == -1) break;

                    ParameterInfo[] parametersInfo = p[maxInd].GetParameters();
                    object[] parameters = new object[parametersInfo.Length];


                    for (int i = 0; i < parameters.Length; ++i)
                    {
                        Type paramType = parametersInfo[i].ParameterType;
                        parameters[i] = GetGenericCreate(paramType).Invoke(context.Faker, null);
                    }

                    object toReturn = null;
                    try
                    {
                        toReturn = p[maxInd].Invoke(parameters);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        toReturn = null;
                    }

                    if (toReturn != null) { notInstantiated = false; return toReturn; }
                    p[maxInd] = null;
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        private void InitializeFields(object obj, GeneratorContext context)
        {
            FieldInfo[] fieldInfos = context.TargetType.GetFields();
            foreach (var field in fieldInfos)
            {
                if (IsFieldInitializable(field, obj))
                {
                    field.SetValue(obj, GetGenericCreate(field.FieldType).Invoke(context.Faker, null));
                }
            }
        }

        private bool IsFieldInitializable(FieldInfo info, object obj)
        {
            if (!info.IsInitOnly)
            {
                return (info.GetValue(obj) == null) || info.GetValue(obj).Equals(GetDefaultValue(info.FieldType));
            }
            else
                return false;
        }

        private void InitializeProperties(object obj, GeneratorContext context)
        {
            PropertyInfo[] propertyInfo = context.TargetType.GetProperties();
            foreach (var property in propertyInfo)
            {
                if (IsPropertyInitializable(property, obj))
                {
                    property.SetValue(obj, GetGenericCreate(property.PropertyType).Invoke(context.Faker, null));
                }
            }
        }

        private bool IsPropertyInitializable(PropertyInfo info, object obj)
        {
            if (info.CanRead && info.CanWrite)
            {
                return (info.GetValue(obj) == null) || info.GetValue(obj).Equals(GetDefaultValue(info.PropertyType));
            }
            else
                return false;
        }
    }
}
