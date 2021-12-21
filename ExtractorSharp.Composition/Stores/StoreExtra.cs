using System;

namespace ExtractorSharp.Composition.Core {
    public partial class Store {

        /// <summary>
        /// 获得某个项的值并设置新值
        /// </summary>

        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public Store Use<T>(string key, Func<T, T> func) {
            return this.Use(key, func, default);
        }

        /// <summary>
        /// 获得某个项的值并设置新值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public Store Use<T>(string key, Func<T, T> func, T defaultValue) {
            var value = this.Get<T>(key);
            if(value == null) {
                value = defaultValue;
            }
            return this.Set(key, func.Invoke(value));
        }

        public T Get<T>(string key) {
            return this.Get(key, default(T));
        }


        public Store Get<T>(string key, Action<T> func) {
            func?.Invoke(this.Get<T>(key));
            return this;
        }

        public Store Get<T>(string key, out T result) {
            result = this.Get<T>(key);
            return this;
        }

        public Store Get<T>(string key, out T result, T defaultValue) {
            result = this.Get<T>(key);
            if(result == null) {
                result = defaultValue;
            }
            return this;
        }

        public Store Set<T>(string key, T value) {
            return this.Set(key, value, null);
        }

        /// <summary>
        /// 只读数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Store ReadOnly<T>(string key, T value) {
            return this.Compute(key, () => value);
        }

        /// <summary>
        /// 添加一个getter
        /// </summary>
        /// <param name="key"></param>
        /// <param name="getter"></param>
        public Store Compute(string key, Getter getter) {
            return this.Compute(key, getter, null);
        }



        /// <summary>
        /// 添加一个setter/getter
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fuc"></param>
        public Store Compute<T>(string key, Getter getter, Setter<T> setter) {
            return this.Compute(key, getter, _ => setter((T)_));
        }

        public Store Watch<T>(string key, Setter<T> changed) {
            return this.Watch(key, value => changed((T)value));
        }

        public Store Register(string key, Action action) {
            return this.Watch(key, e => action?.Invoke());
        }

        public Store Dispatch(string key) {
            return this.Set(key, DateTime.UtcNow);
        }
    }
}
