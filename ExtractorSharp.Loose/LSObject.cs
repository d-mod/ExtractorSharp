using ExtractorSharp.Loose.Attr;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ExtractorSharp.Loose {
    /// <summary>
    /// Json对象
    /// </summary>
    [Serializable]
    public class LSObject : IEnumerable<LSObject>, ICloneable {
        private const BindingFlags FILED_FLAG = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        public static string Separator { set; get; } = ",";
        public static string Mark { set; get; } = "\"";
        private List<LSObject> List { get; }
        public LSObject this[string name] {
            get {
                return Find(name);
            }
            set {
                var obj = this[name];
                if (this[name] != null)
                    List.Remove(obj);
                value.Name = name;
                List.Add(value);
            }
        }

        public LSObject this[int i] => List[i];

        /// <summary>
        /// 下标
        /// </summary>
        public int Index { get; private set; } = -1;
        /// <summary>
        /// 返回元素总数
        /// </summary>
        public int Count => List.Count;
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 值
        /// </summary>
        public object Value {
            set => SetValue(value);
            get => _value;
        }

        private object _value;

        public LSType ValueType { set; get; } = LSType.Object;

        /// <summary>
        /// 父元素
        /// </summary>
        public LSObject Parent { set; get; }
        /// <summary>
        /// 根元素
        /// </summary>
        public LSObject Root { set; get; }

        public LSObject() {
            List = new List<LSObject>();
            Root = this;
            Parent = this;
        }

        public LSObject(string Name, object Value, LSType Type) : this() {
            this.Name = Name;
            this.Value = Value;
            this.ValueType = Type;
        }


        public void Clear() {
            List.Clear();
            Value = null;
            ValueType = LSType.Object;
        }

        public bool Contains(string name) {
            return List.Exists(e => e.Name == name);
        }

        public bool Contains(string name,object value) {
            return List.Exists(e => e.Name == name && value.Equals(e.Value));
        }

        public LSObject Add(object obj) {
            return Add(null,obj);
        }

        public LSObject Add(string name,object obj) {
            if (obj is LSObject e)
                return Add(name, e);
            var eo = new LSObject();
            eo.Value = obj;
            return Add(name, eo);
        }

        public LSObject Add(string name, LSObject child) {
            if (child == null) 
                child = new LSObject();           
            child.Name = name;
            return Add(child);
        }

        public LSObject Add(LSObject child) {
            if (child != null && !child.Equals(this)) {
                child.Index = List.Count;
                child.Parent = this;
                child.Root = Root;
                List.Add(child);
                return child;
            }
            return null;
        }

        public void Remove(string name) {
            List.RemoveAll(e => name.Equals(e.Name));
        }


        public void CopyTo(LSObject obj) {
            obj.ValueType = ValueType;
            if (obj.ValueType == LSType.Object) {
                obj.List.Clear();
                obj.List.AddRange(List);
            } else
                obj.Value = Value;
        }


        public object Clone() {
            var json = "{"+ToString()+"}";
            return new LSBuilder().ReadJson(json);
        }

        /// <summary>
        /// 根据路径查询
        /// </summary>
        /// <param name="path">元素全路径</param>
        public LSObject Find(string path) {
            var arr = path.Split(".");
            var len = arr.Length;
            LSObject obj = null;
            if (arr.Length > 0) {
                obj = FindByChild(arr);//寻找下级元素
                if (obj == null)//寻找父元素的下级元素
                    obj = FindByParent(arr);
            }
            return obj;
        }


        public LSObject FindByParent(params string[] paths) {
            if (Parent != null)//寻找同级元素及其下级元素
                return Parent.FindByChild(paths);
            return null;
        }

        /// <summary>
        /// 根据多个路径查询子元素
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        public LSObject FindByChild(string[] paths) {
            var obj = this;
            var len = 0;
            while (len < paths.Length && obj != null) {
                var name = paths[len];
                obj = obj.FindByChild(name);
                len++;
            }
            return obj;
        }

        /// <summary>
        /// 查询子元素
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LSObject FindByChild(string name) {
            if (name!=null&&name.Equals(Name))
                return this;
            foreach(var e in List) {
                if (e.Name != null && (e.Name.Equals(name) || Regex.IsMatch(e.Name, name))) {
                    return e;
                }
            }
            return null;
        }

        public override string ToString() => ToString(0);


        /// <summary>
        /// 将数据映射到指定的实例上
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public void GetValue<T>(ref T obj) {
            if (ValueType == LSType.Null) {
                obj = default;
                return;
            }
            if (ValueType != LSType.Object && !typeof(Enum).IsAssignableFrom(obj.GetType())) {
                obj = (T)Value;
                return;
            }
            var type = obj.GetType();
            //获取实例泛型
            var arr = type.GetGenericArguments();
            switch (obj) {
                //键值对,第一个泛型必须是string
                case IDictionary dic:
                    if (arr.Length > 1 && arr[0].Equals(typeof(string))) {
                        type = arr[1];
                        List.ForEach(item => dic.Add(item.Name, item.GetValue(type)));
                    }
                    break;
                //数组
                case Array a:
                    type = type.GetElementType();
                    for (int i = 0; i < a.Length; i++)
                        a.SetValue(List[i].GetValue(type), i);
                    break;
                //列表
                case IList e:
                    if (arr.Length > 0) {
                        type = arr[0];
                        List.ForEach(item => e.Add(item.GetValue(type)));
                    }
                    break;
                case Enum em:
                    obj = (T)Enum.Parse(obj.GetType(), Value?.ToString());
                    break;
                default:
                    //遍历子元素
                    foreach (var child in List) {
                        //根据元素名,给属性赋值
                        var completed = false;
                        var properties = type.GetProperties(FILED_FLAG);
                        foreach (var p in properties) {
                            if (p.CanWrite && p.Name.EqualsIgnoreCase(child.Name) && p.NotIgnore()) {
                                p.SetValue(obj, child.GetValue(p.PropertyType));
                                completed = true;
                                break;
                            }
                        }
                        
                        //判断是否成功映射
                        if (completed)
                            continue;
                        //给字段赋值
                        var fileds = type.GetFields(FILED_FLAG);
                        foreach (var f in fileds) {
                            if (f.Name.EqualsIgnoreCase(child.Name) && f.NotIgnore()) {
                                f.SetValue(obj, child.GetValue(f.FieldType));
                                break;
                            }
                        }
                    }
                    break;
            }
        }

       


        /// <summary>
        /// 映射到指定的类型的实例上
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object GetValue(Type type) {
            if (ValueType == LSType.Object||typeof(Enum).IsAssignableFrom(type)) {
                var obj = type.CreateInstance(List.Count);
                GetValue(ref obj);
                return obj;
            }
            return Value;
        }


        public void SetValue(object value) {
            if (value == null) {
                ValueType = LSType.Null;
                _value = null;
                return;
            }
            switch (value) {
                case IDictionary dic:
                    foreach (var key in dic.Keys) {
                        var child = new LSObject();
                        child.Value = dic[key];
                        child.Name = key.ToString();
                        Add(child);
                    }
                    break;
                case string s:
                case char c:
                    ValueType = LSType.String;
                    _value = value?.ToString();
                    break;
                case LSObject es:
                    CopyTo(es);
                    return;
                case IEnumerable arr:
                    foreach (var a in arr) {
                        var child = new LSObject();
                        child.Value = a;
                        Add(child);
                    }
                    break;
                case bool b:
                    ValueType = LSType.Bool;
                    _value = b;
                    break;
                case Enum e:
                    ValueType = LSType.String;
                    _value = value.ToString();
                    break;
                case byte b:
                case short sh:
                case int i:
                case long l:
                case double dl:
                case float f:
                case decimal d:
                    ValueType = LSType.Number;
                    _value = value;
                    break;
                default:
                    var type = value.GetType();
                    var fileds = type.GetFields(FILED_FLAG);
                    foreach (var filed in fileds) {
                        if (filed.NotIgnore()) {
                            Add(filed.Name, filed.GetValue(value));
                        }
                    }
                    var properties = type.GetProperties(FILED_FLAG);
                    foreach (var prop in properties) {
                        if (prop.NotIgnore() && prop.CanRead) {
                            Add(prop.Name, prop.GetValue(value));
                        }
                    }
                    break;
            }
        }

        public Type GetValueType() => Value?.GetType();

        /// <summary>
        /// 格式化输出
        /// </summary>
        /// <param name="depth"></param>
        /// <returns></returns>
        public string ToString(int depth) {
            var buf = new StringBuilder();
            buf.AppendTab(++depth);
            //属性名
            if (Name != null && !string.Empty.Equals(Name.Trim()))
                buf.Append($"{Mark + Name + Mark} : ");
            if (ValueType == LSType.Object) {
                buf.Append("{");
                buf.AppendLine();
                for (var i = 0; i < List.Count; i++) {
                    if (i != List.Count - 1)
                        buf.AppendLine(List[i].ToString(depth) + Separator);
                    else
                        buf.AppendLine(List[i].ToString(depth));
                }
                buf.AppendTab(depth);
                buf.AppendTab(depth--);
                buf.Append("}");
            } else {
                //字符串
                if (ValueType == LSType.String)
                    //reformat 处理转义字符
                    buf.Append($"{Mark + Value.ToString().ReFormat() + Mark}");
                else if (ValueType == LSType.Null)
                    buf.Append("null");
                else
                    buf.Append(Value);
            }
            return buf.ToString();
        }

        public IEnumerator<LSObject> GetEnumerator() => List.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => List.GetEnumerator();
    }
}
