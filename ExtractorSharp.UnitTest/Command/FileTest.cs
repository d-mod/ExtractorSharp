using ExtractorSharp.Composition;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ExtractorSharp.UnitTest.Command {
    [TestClass]
    public class FileTest :InjectService{




        public FileTest() {
            var catalog = new AssemblyCatalog(Assembly.GetAssembly(typeof(Store)));
            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
            /*            Store.Set("/data/files", new List<Album>())
                            .Set("/pathbox/text", "D:/test.npk")
                            .Set("/data/is-save", false)
                            .Set("/data/recents", new List<string>());
                        ;
             */
        }


        [TestMethod]
        public List<Album> TestAddFile() {
            Store.Set("/data/files", new List<Album>());
            Controller.Do("AddFile", false, "D:/test.npk");
            Store.Get("/data/files", out List<Album> files);
            Assert.IsNotNull(files);
            Assert.IsTrue(files.Count > 0);
            return files;
        }

        [TestMethod]
        public void TestDeleteFile() {
            var files = TestAddFile();
            var file = files[0];
            Controller.Do("DeleteFile", new int[] { 0 });
            Store.Get("/data/files", out files);
            Assert.IsFalse(files.Contains(file));
        }

        [TestMethod]
        public void TestNewFile() {
            Store.Set("/data/files", new List<Album>());
            Controller.Do("NewFile", null, "test.img");
             Store.Get("/data/files",out List<Album> files);
            Assert.IsTrue(files.Count > 0);
            var file = files[0];
            Assert.IsTrue(file.Name == "test.img");
        }

        [TestMethod]
        public void TestHideFile() {

        }


        [TestMethod]
        public void TestDataChange() {
            Store.Set("/data/recents", new List<string>())
                .Use<List<string>>("/data/recents", value => {
                    return value;
                }).
                Use<List<string>>("/data/recents", list => {
                    list.Add("Hello");
                    return list;
                });
        }


    }
}
