using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;

namespace ExtractorSharp.Loose.Unit {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void TestMethod1() {
            LSObject root = new LSObject();
            root.Root = root;
            LSObject child1 = new LSObject();
            child1.Name = "test";
            LSObject child2 = new LSObject();
            child2.Name = "child";
            LSObject child3 = new LSObject();
            child3.Name = "for918";
            child3.Value = "Test";
            LSObject child4 = new LSObject();
            child4.Name = "for1018";
            child4.Value = "HelloWorld";
            LSObject child5 = new LSObject();
            child5.Name = "for1018";
            child5.Value = 10;
            root.Add(child1);
            child1.Add(child2);
            child2.Add(child3);
            child1.Add(child4);
            child2.Add(child5);
            var text = root.ToString();
            var rs = new StringReader(text);
            string line = null;
            while ((line = rs.ReadLine()) != null)
                Console.WriteLine(line);
            rs.Close();
            Console.ReadKey();
        }

        /// <summary>
        /// 赋值测试 
        /// </summary>
        [TestMethod]
        public void Test02() {
            LSObject obj = new LSObject();
            obj.Value = new object[] {"1",
                2,
                true,
                new string[]{"HelloWorld" },
                null};
            Assert.AreEqual(obj[0].ValueType, LSType.String);
            Assert.AreEqual(obj[1].ValueType, LSType.Number);
            Assert.AreEqual(obj[2].ValueType, LSType.Bool);
            Assert.AreEqual(obj[3].ValueType, LSType.Object);
            Assert.AreEqual(obj[4].ValueType, LSType.Null);
        }

        /// <summary>
        /// 测试对象序列化
        /// </summary>
        [TestMethod]
        public void Test03() {
            Student s = new Student() {
                Name = "小明",
                Sex='男',
                Age=23,
                School="中国大学",
            };
            s.SetNickName("明");
            LSObject obj = new LSObject();
            obj.Value = s;
            Assert.AreEqual(s.Name, obj["Name"].Value);
            Assert.AreEqual(s.Sex,obj["Sex"].Value);
            Assert.AreEqual(s.Age, obj["Age"].Value);
            Assert.AreEqual(s.School, obj["School"].Value);
            //无法访问私有对象
            Assert.AreNotEqual(s?.GetNickName(), obj["NickName"]?.Value);
        }

        /// <summary>
        /// 测试枚举序列化
        /// </summary>
        [TestMethod]
        public void Test04() {
            LSObject obj = new LSObject();
            obj.Value = ClassType.English;
            Assert.IsTrue((int)(obj.Value)==(int)(ClassType.English));
        }

        /// <summary>
        /// 测试枚举字段
        /// </summary>
        [TestMethod]
        public void Test05() {
            Teacher s = new Teacher();
            s.Type = ClassType.Math;
            LSObject obj = new LSObject();
            obj.Value = s;
            var i = (ClassType)obj["Type"].Value;
            var j = s.Type;
            Assert.IsTrue(i==j);
        }

        /// <summary>
        /// 测试对象字段
        /// </summary>
        [TestMethod]
        public void Test06() {
            Student s = new Student();
            Teacher t = new Teacher();
            t.Type = ClassType.English;
            t.Name = "王老师";
            s.Teacher = t;
            LSObject o = new LSObject();
            o.Value = s;
            var t2 = o["Teacher"].GetValue(typeof(List<Student>))as Teacher;
            Assert.AreEqual(t2.Name,t.Name);
        }
        
        /// <summary>
        /// 测试对象列表
        /// </summary>
        [TestMethod]
        public void Test07() {
            Student s = new Student();
            s.Name = "张三";
            Student s2 = new Student();
            s2.Name = "李四";
            List<Student> list = new List<Student>();
            list.Add(s);
            list.Add(s2);
            LSObject obj= new LSObject();
            obj.SetValue(list);
            var temp = obj.GetValue(typeof(List<Student>));
            Assert.IsNotNull(temp);
        }


        [TestMethod]
        public void Test08() {
            Student s = new Student();
            s.Name = "HelloWorld";
            LSObject obj = new LSObject();
            obj.SetValue(s);
            obj = obj.Clone()as LSObject;
            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void Test09() {
            Student bs = new Student();
            Student cs = new Student();
            byte[] b =  { 1, 3 };
            byte[] c = { 1, 3 };
            Color color = Color.FromArgb(1, 3, 4, 5);
            Color color2 = Color.FromArgb(1, 3, 4, 5);
            var list = new List<Color>();
            list.Add(color);
            Assert.IsTrue(color==color2);
        }

        [TestMethod]
        public void Test10() {
            var type = typeof(string);

            var obj = type.CreateInstanceByPoint("",3,2);
            Assert.IsNotNull(obj);
        }

        /// <summary>
        /// 转义字符的测试
        /// </summary>
        [TestMethod]
        public void Test11() {
            var reader = new LSBuilder();
            var obj = new LSObject();
            var child = new LSObject();
            child.Name = "test";
            child.Value = "HelloWorld'\n\t\b\r\f\\\u1111";
            obj.Add(child);
            var path = @"E:\ES\ExtractorSharp\bin\Debug\conf\test.json";
            reader.Write(obj, path);
            obj = reader.Read(path);
            Assert.AreEqual(obj["test"].Value, child.Value);
        }

        /// <summary>
        /// 转行测试
        /// </summary>
        [TestMethod]
        public void Test12() {
            var obj = new LSObject();
            obj.Add("test", "Hello");
            obj.Add("test2", "HelloWorld");
            var child = new LSObject();
            child.Add("test3", "Hello Java");
            obj.Add("student",child);

            var child1 = new LSObject();
            child1.Add("test3", "Hello Java");
            obj.Add("ja", child1);
            var reader = new LSBuilder();
            reader.Write(obj, @"E:\ES\ExtractorSharp\bin\Debug\conf\test.json");
            Assert.IsTrue(true);
        }


        /// <summary>
        /// 数据映射测试
        /// </summary>
        [TestMethod]
        public void Test13() {
            var path = @"D:\ES\ExtractorSharp\Resources\weapon.json";
            var reader = new LSBuilder();
            var root = reader.Read(path);
            var type = typeof(WeaponInfo[]);
            var list=root.GetValue(type);
            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void Test14() {
            object obj = new WeaponInfo();
            Assert.AreEqual(typeof(object), obj.GetType());
        }
    }
}
