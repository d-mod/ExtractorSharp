using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ExtractorSharp.Install.Loose {
    public static class LSTools {
        private static readonly Dictionary<char, char> escapes;
        private static readonly Dictionary<char, string> reescapes;

        static LSTools() {
            escapes = new Dictionary<char, char> {
                {'t', '\t'},
                {'r', '\r'},
                {'f', '\f'},
                {'n', '\n'},
                {'b', '\b'},
                {'\\', '\\'},
                {'\'', '\''},
                {'"', '"'}
            };
            reescapes = new Dictionary<char, string> {
                {'\\', @"\\"},
                {'\'', @"\'"},
                {'\t', @"\t"},
                {'\r', @"\r"},
                {'\f', @"\f"},
                {'\n', @"\n"},
                {'\b', @"\b"}
            };
        }

        public static string Format(this string s) {
            foreach (var c in escapes.Keys) {
                s = s.Replace(c, escapes[c]);
            }
            return s;
        }

        public static string ReFormat(this string s) {
            foreach (var c in reescapes.Keys) {
                s = s.Replace(c + "", reescapes[c]);
            }
            return s;
        }

        /// <summary>
        ///     将字符串解析为bool,number,string三种类型的数据
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static object Parse(this string s) {
            if (bool.TryParse(s, out var b)) {
                return b;
            }
            if (decimal.TryParse(s, out var d)) {
                return d;
            }
            return s;
        }


        public static string Substring(this char[] cs, int start, int length) {
            return new string(cs.Sub(start, length));
        }

        public static T[] Sub<T>(this T[] array, int start, int length) {
            var newArray = new T[length];
            Array.ConstrainedCopy(array, start, newArray, 0, length);
            return newArray;
        }

        public static string[] Split(this string s, string c) {
            return s.Split(c.ToArray(), StringSplitOptions.RemoveEmptyEntries);
        }

        public static string ToUpperFrist(this string s) {
            if (s.Length > 0) {
                return char.ToUpper(s[0]) + s.Substring(1, s.Length - 1);
            }
            return s;
        }

        public static bool EqualsIgnoreCase(this string s1, string s2) {
            return s1.ToLower().Equals(s2?.ToLower());
        }


        public static void AppendTab(this StringBuilder sb, int count) {
            while (--count > 0) {
                sb.Append('\t');
            }
        }

        public static bool IsNumber(this object obj) {
            return obj is byte || obj is short || obj is int || obj is long || obj is float || obj is double ||
                   obj is decimal;
        }

        public static object CreateInstance(this Type type, int count) {
            if (typeof(Array).IsAssignableFrom(type)) {
                return Array.CreateInstance(type.GetElementType(), count);
            }
            return type.CreateInstance();
        }

        public static object CreateInstance(this Type type, params object[] args) {
            var ass = type.Assembly;
            return ass.CreateInstance(type.FullName, false, BindingFlags.CreateInstance, null, args, null, null);
        }

        public static object CreateInstanceByPoint(this Type type, string pointname, params object[] args) {
            if (pointname == null || pointname == "" || pointname.Equals("constructor")) {
                return type.CreateInstance(args);
            }
            var method = type.GetMethod(pointname, Type.GetTypeArray(args));
            return method.Invoke(null, args);
        }

        public static bool NotIgnore(this MemberInfo element) {
            var attr_type = typeof(LSIgnoreAttribute);
            if (Attribute.IsDefined(element, attr_type)) return false;
            Type class_type = null;
            if (element is FieldInfo f) class_type = f.FieldType;
            if (element is PropertyInfo p) class_type = p.PropertyType;
            if (class_type == null || Attribute.IsDefined(class_type, attr_type)) return false;
            return true;
        }
    }
}