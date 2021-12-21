using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ExtractorSharp.Composition.Control {

    public class CommandContext : IEnumerable<KeyValuePair<string, object>> {

        private readonly IDictionary<string, object> parameters = new Dictionary<string, object>();

        public string CommandName { set; get; }

        public const string DEFUALT_KEY = "@";

        public bool IsCancel { set; get; } = false;

        public object this[string key] {
            set => this.parameters[key] = value;
            get => this.Get<object>(key);
        }

        public CommandContext() : this(null) {

        }

        public CommandContext(object defaultValue) {
            this.Add(DEFUALT_KEY, defaultValue);
        }

        public T Get<T>() {
            return this.Get<T>(DEFUALT_KEY);
        }

        public void Get<T>(out T t) {
            t = this.Get<T>();
        }

        public void Get<T>(string key, out T t) {
            t = this.Get<T>(key);
        }

        public T Get<T>(string key) {
            return this.parameters.TryGetValue(key.ToUpper(), out var o) ? (T)o : default;
        }

        public void Add(string key, object value) {
            switch(key) {
                case DEFUALT_KEY:
                    break;
                default:
                    key = key.ToUpper();
                    break;
            }
            this.parameters[key] = value;
        }

        public void Add(IEnumerable<KeyValuePair<string,object>> context) {
            foreach(var item in context.ToList()) {
                this.parameters[item.Key] = item.Value;
            }
        }

        public void Export(object obj) {
            var type = obj.GetType();
            var members = type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach(var member in members) {

                if(Attribute.GetCustomAttribute(member, typeof(CommandParameter)) is CommandParameter commandParameter) {
                    var name = (commandParameter.Name ?? member.Name).ToUpper();

                    if(!this.parameters.ContainsKey(name)) {
                        if(commandParameter.IsDefault) {
                            name = DEFUALT_KEY;
                        } else {
                            if(commandParameter.IsRequired) {
                                throw new ArgumentException($"`{name.ToLower()}` is required");
                            }
                            continue;
                        }
                    }

                    var value = this.parameters[name];
                    if(member is PropertyInfo prop && prop.CanWrite) {
                        prop.SetValue(obj, value);
                    } else if(member is FieldInfo field) {
                        field.SetValue(obj, value);
                    }

                }
            }

        }

        public void Import(object obj) {
            var type = obj.GetType();
            var members = type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);


            foreach(var member in members) {

                if(Attribute.GetCustomAttribute(member, typeof(CommandParameter)) is CommandParameter commandParameter) {
                    var name = (commandParameter.Name ?? member.Name).ToUpper();

                    if(commandParameter.IsDefault) {
                        name = DEFUALT_KEY;
                    }

                    object value = null;
                    if(member is PropertyInfo prop && prop.CanRead) {
                        value = prop.GetValue(obj);
                    } else if(member is FieldInfo field) {
                        value = field.GetValue(obj);
                    }

                    this.parameters[name] = value;

                }
            }
        }

        public static CommandContext CreateFrom(object obj) {
            var context = new CommandContext();
            context.Import(obj);
            return context;
        }


        public void Transfer<T>(ref T t) {
            if(t == null) {

            }

            t = default;
        }




        public void Cancel() {

        }



        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator() {
            return this.parameters.GetEnumerator();
        }

        public IEnumerator GetEnumerator() {
            return this.parameters.GetEnumerator();
        }
    }
}
