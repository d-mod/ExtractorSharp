using System.Linq;

namespace System.Text {
    public static class Strings {
        #region 基本重写

        public static string[] Split(this string str, params string[] pattern) {
            return str.Split(pattern, StringSplitOptions.RemoveEmptyEntries);
        }

        #endregion

        #region 前缀和后缀相关

        /// <summary>
        ///     去除字符串中"/"和"\"前面的内容
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetSuffix(this string str) {
            return str.LastSubstring('\\', '/');
        }


        public static string RemoveSuffix(this string s) {
            var i = s.IndexOf(".", StringComparison.Ordinal);
            if(i < 0) {
                i = s.Length;
            }
            return s.Substring(0, i);
        }

        /// <summary>
        ///     移除指定开头的后缀名
        /// </summary>
        /// <param name="c"></param>
        public static string RemoveSuffix(this string s, string c) {
            var i = s.LastIndexOf(c, StringComparison.Ordinal);
            return i > 0 ? s.Substring(0, i) : s;
        }

        /// <summary>
        ///     移除指定结尾的前缀
        /// </summary>
        /// <param name="c"></param>
        public static string RemovePrefix(this string s, string c) {
            var i = s.IndexOf(c, StringComparison.Ordinal);
            return i > 0 ? s.Substring(i) : s;
        }

        /// <summary>
        ///     补全字符串,会去除相同的部分
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static string Complete(this string s1, string s2) {
            var cs1 = s1.ToCharArray();
            var cs2 = s2.ToCharArray();
            var r1 = string.Empty;
            var r2 = new StringBuilder();
            for(int i = cs1.Length - 1, j = 0; i > 0 && j < cs2.Length; i--, j++) {
                r1 = cs1[i] + r1;
                r2.Append(cs2[j]);
                if(!r1.Equals(r2.ToString())) {
                    continue;
                }
                s2 = s2.Substring(j + 1);
                break;
            }
            return s1 + s2;
        }

        public static string LastSubstring(this string str, params char[] split) {
            var index = split.Select(c => str.LastIndexOf(c)).Aggregate(-1,
                (current, index2) => (current > index2 || index2 == -1 ? current : index2));
            return str.Substring(index + 1);
        }


        public static bool EqualsIgnoreCase(this string s1, string s2) {
            return string.Equals(s1?.ToLower(), s2?.ToLower());
        }

        #endregion
    }
}