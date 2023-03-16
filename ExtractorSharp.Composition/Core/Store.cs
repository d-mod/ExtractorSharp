using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using ExtractorSharp.Composition.Stores;

namespace ExtractorSharp.Composition.Core {

    public delegate void Setter<T>(T o);

    public delegate object Getter();


    /**
     *  数据仓库
     */
    public partial class Store {

        private Dictionary<string, Field> state { get; }

        private Dictionary<object, Field> bindings { get; }

        private List<Subscriber> subscribers { get; }

        private List<IStoreFilter> filters { get; }

        public Store() {
            this.state = new Dictionary<string, Field>();
            this.bindings = new Dictionary<object, Field>();
            this.subscribers = new List<Subscriber>();
            this.filters = new List<IStoreFilter>();
        }



        /// <summary>
        /// 添加一个setter/getter
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fuc"></param>
        public Store Compute(string key, Getter getter, Setter<object> setter) {
            var field = this.GetField(key);
            if(getter != null) {
                field.Getter = getter;
            }
            if(setter != null) {
                field.ValueChanged += setter;
            }
            return this;
        }

        /// <summary>
        /// 绑定字段
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Store Bind(string key, object obj, string name, Converter<object, object> converter = null) {
            var type = obj.GetType();
            var prop = type.GetProperty(name);
            if(prop != null) {
                var field = this.GetField(key);
                if(prop.CanWrite) {
                    field.ValueChanged += _ => prop.SetValue(obj, _);
                }
                if(prop.CanRead) {
                    field.Getter = () => {
                        var rs = prop.GetValue(obj);
                        if(converter != null) {
                            rs = converter(rs);
                        }
                        return rs;
                    };
                }
                if(obj is INotifyPropertyChanged notify) {
                    notify.PropertyChanged += (o, e) => {
                        if(e.PropertyName == name) {
                            field.Value = prop.GetValue(obj);
                        }
                    };
                }
                this.bindings.Add(obj, field);

            }

            return this;
        }


        public void AddFilter(IStoreFilter filter) {
            this.filters.Add(filter);
        }


        public Store Watch(string key, Setter<object> changed) {
            return this.Compute(key,null,changed);
        }


        public Store Trigger(string key) {
            var field = this.GetField(key);
            field?.Trigger();
            return this;
        }


        public void OnValueChanged(object sender, EventArgs e) {
            if(this.bindings.ContainsKey(sender)) {
                var field = this.bindings[sender];
                field.Notify();
            }
        }


        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Store Subscribe(object obj) {
            var subscriber = new Subscriber(this, obj);
            this.subscribers.Add(subscriber);
            return this;
        }

        public Store UnSubscribe(object obj) {
            var subscriber = this.subscribers.Find(e => object.ReferenceEquals(obj, e.Source));
            if(subscriber != null) {
                subscriber.Dispose();
            }
            return this;
        }


        public Store Set<T>(string key, T value, Setter<T> changed) {
            var field = this.GetField(key);
            field.Value = value;
            if(changed != null) {
                field.ValueChanged += (val) => changed((T)val);
            }
            return this;
        }

        public bool IsNullOrEmpty(string key) {
            if(this.state.ContainsKey(key)) {
                var field = this.state[key];
                if(field.Value is string s) {
                    return s == string.Empty;
                }
                if(field.Value is IEnumerable e) {
                    return !e.GetEnumerator().MoveNext();
                }
                return field.Value == null;
            }
            return true;
        }


        /// <summary>
        /// 初始化,设置默认值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Store Create(string key, object value) {
            this.state[key] = new Field(value);
            return this;
        }


        public T Get<T>(string key, T defaultValue) {
            var field = this.GetField(key);
            return field.Value != null ? (T)field.Value : defaultValue;
        }



        public bool IsLocked(string key) {
            var field = this.GetField(key);
            return field.Locked;
        }

        public Store Lock(string key) {
            var field = this.GetField(key);
            field.Locked = true;
            return this;
        }

        public Store Unlock(string key) {
            var field = this.GetField(key);
            field.Locked = false;
            return this;
        }

        /// <summary>
        /// 根据键名获取字段，如果不存在则创建一个新的字段
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private Field GetField(string key) {
            Field field = null;
            if(!this.state.ContainsKey(key)) {
                this.state[key]= field = new Field();
                var filter = this.filters.Find(e => e.IsMatch(key));
                if(filter != null) {
                    field.Getter = () => filter.Get(key);
                    field.ValueChanged += value => filter.Set(key, value);
                }
            } else {
                field = this.state[key];
            }
            return field;
        }



        private class Field {

            public Field() {
                this.ValueChanged += value => this._value = value;
            }

            public Field(object Value) : this() {
                this.Value = Value;
                if(Value is IBindingList binding) {
                    binding.ListChanged += (o, e) => ValueChanged?.Invoke(binding);
                }
            }

            /// <summary>
            /// 表示数据正在更新中
            /// </summary>
            private bool isUpdating = false;

            /// <summary>
            /// 表示数据正在锁定
            /// </summary>
            public bool Locked { set; get; } = false;

            private object _value;

            /// <summary>
            /// 主动通知数据更改
            /// </summary>
            public void Notify() {
                ValueChanged?.Invoke(this.Value);
            }

            public void Trigger() {
                var oldValue = this._value;
                if(this.Value != oldValue) {
                    this.Notify();
                }
            }

            public object Value {
                set {
                    //防止循环触发数据更新事件
                    if(!this.isUpdating && !this.Locked) {
                        this.isUpdating = true;
                        this._value = value;
                        ValueChanged?.Invoke(value);
                        this.isUpdating = false;
                    }
                }
                get {
                    if(this.Getter != null) {
                        this._value = this.Getter.Invoke();
                    }
                    return this._value;
                }
            }


            public Getter Getter;

            public event Setter<object> ValueChanged;

        }

        private class Subscriber : IDisposable {

            private readonly Dictionary<Field, Setter<object>> ValueChangeds;

            private readonly Dictionary<string, Field> ToWays;

            public object Source { set; get; }

            private Store Store { get; }

            private PropertyChangedEventHandler OnPropertyChanged;

            public Subscriber(Store Store, object Source) {
                this.Store = Store;
                this.Source = Source;
                this.ValueChangeds = new Dictionary<Field, Setter<object>>();
                this.ToWays = new Dictionary<string, Field>();
                this.Initialize();
            }

            private void Initialize() {
                var obj = this.Source;
                var type = this.Source.GetType();
                var props = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);


                //双向绑定
                foreach(var prop in props) {

                    if(Attribute.GetCustomAttribute(prop, typeof(StoreBinding)) is StoreBinding computed) {
                        var key = computed.Key;
                        var field = this.Store.GetField(key);
                        if(prop.CanWrite) {
                            void OnChanged(object value) {
                                prop.SetValue(obj, value);
                            }
                            if(field.Value != null) {
                                OnChanged(field.Value);
                            }
                            field.ValueChanged += OnChanged;
                            this.ValueChangeds.Add(field, OnChanged);
                            //初始化被绑定的属性值
                        }
                        if(computed.ToWay && prop.CanRead) {
                            field.Getter = () => prop.GetValue(obj);
                            this.ToWays.Add(prop.Name, field);
                        }

                    }

                }

                if(obj is INotifyPropertyChanged notify) {
                    this.OnPropertyChanged = (object sender, PropertyChangedEventArgs e) => {
                        if(this.ToWays.ContainsKey(e.PropertyName)) {
                            var prop = type.GetProperty(e.PropertyName);
                            var field = this.ToWays[e.PropertyName];
                            field.Value = prop.GetValue(obj);
                        }
                    };

                    notify.PropertyChanged += this.OnPropertyChanged;
                }

            }


            public void Dispose() {
                foreach(var entry in this.ValueChangeds) {
                    var field = entry.Key;
                    var valueChanged = entry.Value;
                    field.ValueChanged -= valueChanged;
                }

                if(this.Source is INotifyPropertyChanged notify) {
                    notify.PropertyChanged -= this.OnPropertyChanged;
                }
            }
        }
    }
}
