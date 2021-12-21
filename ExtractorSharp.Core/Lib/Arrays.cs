using System.Linq;

namespace System.Collections.Generic {
    public static class Arrays {
        public static T Find<T>(this T[] array, Predicate<T> match) {
            return Array.Find(array, match);
        }

        public static bool Compare<T>(this T[] arr1, T[] arr2) {
            if(arr1.Length != arr2.Length) {
                return false;
            }
            for(var i = 0; i < arr1.Length && i < arr2.Length; i++) {
                if(!Equals(arr1[i], arr2[i])) {
                    return false;
                }
            }
            return true;
        }

        public static T[] Concat<T>(this T[] arr1, T[] arr2) {
            var newArray = new T[arr1.Length + arr2.Length];
            Buffer.BlockCopy(arr1, 0, newArray, 0, arr1.Length);
            Buffer.BlockCopy(arr2, 0, newArray, arr1.Length, arr2.Length);
            return newArray;
        }

        public static T[][] Split<T>(this T[] data, T[] pattern) {
            var last = 0;
            var list = new List<T[]>();
            for(var i = 0; i < data.Length; i++) {
                var j = i;
                while(j < data.Length && j - i < pattern.Length && Equals(data[j], pattern[j - i])) {
                    j++;
                }

                if(j - i == pattern.Length) {
                    var temp = new T[j - last];
                    Buffer.BlockCopy(data, last, temp, 0, temp.Length);
                    list.Add(temp);
                    last = j;
                    continue;
                }
                i = j;
            }
            var arr = new T[data.Length - last];
            Buffer.BlockCopy(data, last, arr, 0, arr.Length);
            list.Add(arr);
            return list.ToArray();
        }

        /// <summary>
        /// 互换位置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        public static void Switch<T>(this IList<T> list, int source, int dest) {
            if(source > -1 && source != dest) {
                var item = list[dest];
                list[dest] = list[source];
                list[source] = item;
            }
        }


        /// <summary>
        ///     安全插入 当插入的位置不在于集合的区间时，改为添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <param name="t"></param>
        public static void InsertAt<T>(this List<T> list, int index, IEnumerable<T> t) {
            if(index > list.Count) {
                list.AddRange(t);
            } else if(index < 0) {
                list.InsertRange(0, t);
            } else {
                list.InsertRange(index, t);
            }
        }

        /// <summary>
        /// 查找两个集合并集的下标
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="part"></param>
        /// <returns></returns>
        public static int[] FindIndices<T>(this IEnumerable<T> source, IEnumerable<T> part) {
            var indices = new List<int>();
            var countA = source.Count();
            var countB = part.Count();
            for(var i = 0; i < countA; i++) {
                var a = source.ElementAt(i);
                for(var j = 0; j < countB; j++) {
                    var b = part.ElementAt(j);
                    if(a.Equals(b)) {
                        indices.Add(i);
                        break;
                    }
                }

            }
            return indices.ToArray();
        }
    }
}