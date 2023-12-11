using BackEnd;
using Eonix.DB;
using Eonix.Define;
using LitJson;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Eonix.Util
{
    public static class SerializationUtil
    {
        public static T JsonToObject<T>(JsonData json, DeserializeType deserializeType)
            where T : class
        {
            T data = (T)Activator.CreateInstance(typeof(T));
            var fields = typeof(T).GetFields();

            foreach (var field in fields)
            {
                var identifier = field.Name;

                var isExists = json.Keys.Contains(identifier);

                if (!isExists)
                {
                    GameManager.Log($"### {identifier} Field of {typeof(T).Name} is Not Exist ###");
                    continue;
                }

                try
                {
                    string subKey = GetSubKey(field.FieldType, deserializeType);

                    var fieldData = subKey != null ?
                        json[identifier][subKey] : json[identifier];

                    TryParse(data, field, fieldData);
                }
                catch (Exception e)
                {
                    GameManager.Log($"### Access a Key in {field.Name} Field in {typeof(T).Name} Failed ###\n{e}");
                }
            }
            return data;
        }

        public static Param DtoToParam<T>(T dtoData) where T : DtoBase
        {
            var param = new Param();

            var fields = typeof(T).GetFields();

            foreach(var field in fields)
            {
                var fieldType = field.GetType();

                object value = null;

                if (fieldType.IsEnum)
                    value = field.GetValue(dtoData).ToString();
                else
                    value = field.GetValue(dtoData);

                param.Add(field.Name, value);
            }

            return param;
        }

        private static void TryParse<T>(T data, FieldInfo field, JsonData fieldData)
            where T : class
        {
            var type = field.FieldType;

            try
            {
                if(type == typeof(int))
                {
                    field.SetValue(data, int.Parse(fieldData.ToString()));
                }
                else if (type == typeof(string))
                {
                    field.SetValue(data, fieldData.ToString());
                }
                else if(type == typeof(float))
                {
                    field.SetValue(data, float.Parse(fieldData.ToString()));
                }
                else if (type == typeof(double))
                {
                    field.SetValue(data, double.Parse(fieldData.ToString()));
                }
                else if (type == typeof(bool))
                {
                    field.SetValue(data, bool.Parse(fieldData.ToString()));
                }
                else if (type == typeof(DateTime))
                {
                    field.SetValue(data, DateTime.Parse(fieldData.ToString()));
                }
                else if (type.IsEnum)
                {
                    field.SetValue(data, Enum.Parse(type, fieldData.ToString()));
                }
                else if (type.IsArray)
                {
                    var arrayData = fieldData.ToString().Split(',');
                    var elementType = type.GetElementType();

                    if (elementType == typeof(int))
                    {
                        field.SetValue(data, Array.ConvertAll(arrayData, _ => int.Parse(_)));
                    }
                    else if (elementType == typeof(float))
                    {
                        field.SetValue(data, Array.ConvertAll(arrayData, _ => float.Parse(_)));
                    }
                }
                else if (type.IsGenericType)
                {
                    var listData = new List<string>();
                    for (int i = 0; i < fieldData.Count; ++i)
                    {
                        var key = GetListElementKey(fieldData[i]);

                        if (key != null)
                            listData.Add(fieldData[i][key].ToString());
                    }
                    var elementType = type.GetGenericArguments()[0];

                    if (elementType == typeof(int))
                    {
                        field.SetValue(data, listData.ConvertAll(_ => int.Parse(_)));
                    }
                    else if (elementType == typeof(float))
                    {
                        field.SetValue(data, listData.ConvertAll(_ => float.Parse(_)));
                    }
                }
            }
            catch(Exception e)
            {
                GameManager.Log($"### Parse {field.Name} Field in {typeof(T).Name} Failed ###\n{e}");
            }
        }

        private static string GetListElementKey(JsonData data)
        {
            if (data.Keys.Contains("S")) // string
                return "S";
            else if (data.Keys.Contains("N")) // number
                return "N";
            else if (data.Keys.Contains("M")) // map
                return "M";
            else if (data.Keys.Contains("L")) // list
                return "L";
            else if (data.Keys.Contains("BOOL")) // boolean
                return "BOOL";
            else if (data.Keys.Contains("NULL")) // null
                return "NULL";
            else
                return null;
        }

        private static string GetSubKey(Type fieldType, DeserializeType deserializeType)
        {


            switch (deserializeType)
            {
                case DeserializeType.SD:
                    return "S";
                case DeserializeType.DTO:
                    if (fieldType == typeof(string))
                        return "S";
                    else if (fieldType.IsGenericType)
                        return "L";
                    else if (fieldType == typeof(bool))
                        return "BOOL";
                    else if (fieldType == typeof(int))
                        return "N";
                    else
                        return "N";
                case DeserializeType.DefinedDtoByBackend:
                    return null;
            }

            return null;
        }

    }
}
