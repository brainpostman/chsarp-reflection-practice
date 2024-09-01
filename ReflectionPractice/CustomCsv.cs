using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionPractice
{
    public static class CustomCsv
    {
        public static string Serialize(object obj)
        {
            Type objType = obj.GetType();
            PropertyInfo[] props = objType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            string[] headers = props.Select(x => x.Name).ToArray();
            string[] values = new string[props.Length];
            for (int i = 0; i < props.Length; i++)
            {
                var val = props[i].GetValue(obj);
                values[i] = val == null ? string.Empty : val.ToString()!;
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < headers.Length; i++)
            {
                sb.Append($"{headers[i]}");
                if (i != headers.Length - 1) sb.Append(',');
            }
            sb.AppendLine(string.Empty);
            for (int i = 0; i < values.Length; i++)
            {
                string val = values[i];
                if (val.Contains(',')) val = $"\"{val}\"";
                sb.Append(val);
                if (i != values.Length - 1) sb.Append(',');
            }
            return sb.ToString();
        }

        public static object? Deserialize(Type targetType, string value, char delimiter)
        {
            string[] rows = value.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            string[] props = rows[0].Split(delimiter);
            string[] values = rows[1].Split(delimiter);

            object? obj = Activator.CreateInstance(targetType)!;

            if (obj != null)
            {
                for (int i = 0; i < props.Length; i++)
                {
                    PropertyInfo? prop = targetType.GetProperty(props[i], BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                    if (prop != null && prop.PropertyType == typeof(int))
                    {
                        if (values[i] != string.Empty)
                        {
                            prop.SetValue(obj, int.Parse(values[i]));
                        }
                        else
                        {
                            prop.SetValue(obj, 0);
                        }
                    }
                }
            }

            return obj;
        }
    }
}
