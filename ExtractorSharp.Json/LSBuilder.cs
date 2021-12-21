using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace ExtractorSharp.Json {
    public class LSBuilder {

        private readonly LSParser parser;


        public LSBuilder() {
            this.parser = new LSParser();
        }

        public Encoding Encoding { set; get; } = Encoding.UTF8;

        public LSObject Get(string url) {
            try {
                var request = WebRequest.Create(url);
                request.Method = "GET";
                request.Timeout = 5000;
                using(var response = request.GetResponse())
                using(var stream = response.GetResponseStream()) {
                    var obj = this.Read(stream);
                    return obj;
                }
            } catch(Exception) {
                return null;
            }
        }

        public LSObject Get(string url, IDictionary<string, object> dataMap) {
            var builder = new StringBuilder();
            builder.Append(url);
            builder.Append("?");
            foreach(var entry in dataMap) {
                builder.Append($"{entry.Key}={entry.Value}&");
            }

            url = builder.ToString();
            return this.Get(url);
        }

        public LSObject Post(string url) {
            return this.Post(url, new Dictionary<string, object>());
        }

        public LSObject Post(string url, IDictionary<string, object> dataMap) {
            var builder = new StringBuilder();
            builder.Append(url);
            foreach(var entry in dataMap) {
                builder.Append($"&{entry.Key}={entry.Value}");
            }

            var data = this.Encoding.GetBytes(builder.ToString());
            var request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            using(var stream = request.GetRequestStream()) {
                stream.Write(data, 0, data.Length);
            }
            using(var response = request.GetResponse() as HttpWebResponse) {
                using(var stream = response.GetResponseStream()) {
                    var obj = this.Read(stream);
                    return obj;
                }
            }
        }

        public LSObject Post(string url, LSObject obj) {
            var data = this.Encoding.GetBytes(obj.ToString());
            var request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            using(var stream = request.GetRequestStream()) {
                stream.Write(data, 0, data.Length);
            }
            using(var response = request.GetResponse() as HttpWebResponse) {
                using(var stream = response.GetResponseStream()) {
                    var rs = this.Read(stream);
                    return rs;
                }
            }
        }

        public LSObject Post(string url, object obj) {
            return this.Post(url, new LSObject {
                Value = obj
            });
        }

        public LSObject Read(string file) {
            using(var fs = new FileStream(file, FileMode.Open)) {
                return this.Read(fs);
            }
        }

        public LSObject Read(Stream stream) {
            using(var sr = new StreamReader(stream)) {
                return this.ReadJson(sr.ReadToEnd());
            }
        }

        public LSObject ReadJson(string source) {
            if(source.Trim().StartsWith("<")) {
                var doc = new XmlDocument();
                doc.LoadXml(source);
                return this.ReadXml(doc);
            }
            return this.parser.Decode(source);
        }

        public LSObject ReadXml(string path) {
            var doc = new XmlDocument();
            doc.Load(path);
            return this.ReadXml(doc);
        }

        public LSObject ReadXml(XmlDocument doc) {
            var obj = new LSObject();
            this.AddXml(obj, doc.DocumentElement);
            obj = obj[0];
            obj = obj.Clone() as LSObject;
            return obj;
        }

        private void AddXml(LSObject parent, XmlNode doc) {
            var obj = new LSObject();
            if(doc.Attributes != null) {
                foreach(XmlAttribute attr in doc.Attributes) {
                    obj.Add(attr.Name, attr.Value.Parse());
                }
            }
            foreach(XmlNode node in doc.ChildNodes) {
                if(node.NodeType == XmlNodeType.Element) {
                    this.AddXml(obj, node);
                }
            }
            obj.Name = doc.Name;
            if(obj.Count == 0) {
                obj.Value = doc.InnerText.Trim().Parse();
            }

            parent.Add(obj);
        }

        public LSObject ReadProperties(string properties) {
            var obj = new LSObject();
            var props = properties.Split("\r\n");
            foreach(var p in props) {
                var i = p.IndexOf("//"); //过滤注释部分
                var tp = i > -1 ? p.Substring(0, i) : p;
                if(tp.Length > 0) {
                    var entry = tp.Split("=");
                    var key = entry[0];
                    var value = entry[1];
                    obj.Add(key, value.Parse());
                }
            }
            return obj;
        }

        public void Write(LSObject root, string file) {
            using(var fs = new FileStream(file, FileMode.Create)) {
                this.Write(root, fs);
            }
        }

        public void Write(LSObject root, Stream stream) {
            using(var fw = new StreamWriter(stream)) {
                fw.Write(root);
            }
        }

        public void WriteObject(object obj, string file) {
            using(var fs = new FileStream(file, FileMode.Create)) {
                this.WriteObject(obj, fs);
            }
        }

        public void WriteObject(object obj, Stream stream) {
            var root = new LSObject {
                Value = obj
            };
            this.Write(root, stream);
        }
    }
}