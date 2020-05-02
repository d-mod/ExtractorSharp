using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Handle;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtractorSharp.UnitTest.Command
{
    [TestClass]
    public class UnitTest1
    {
        static UnitTest1()
        {
            Handler.Regisity(ImgVersion.Other, typeof(OtherHandler));
            Handler.Regisity(ImgVersion.Ver1, typeof(FirstHandler));
            Handler.Regisity(ImgVersion.Ver2, typeof(SecondHandler));
            Handler.Regisity(ImgVersion.Ver4, typeof(FourthHandler));
            Handler.Regisity(ImgVersion.Ver5, typeof(FifthHandler));
            Handler.Regisity(ImgVersion.Ver6, typeof(SixthHandler));
        }


        /// <summary>
        ///     测试IMG保存
        /// </summary>
        [TestMethod]
        public void TestSaveImg()
        {
            var list = NpkCoder.Load(@"D:\地下城与勇士\ImagePacks2\sprite_character_swordman_equipment_avatar_coat.NPK");
            Assert.IsTrue(list.Count > 0);
            var img = list[0];
            Assert.IsNotNull(img);
            var target = @"d:\test\v6\01.img";
            img.Save(target);
            Assert.IsTrue(File.Exists(target));
            list = NpkCoder.Load(target);
            Assert.IsTrue(list.Count > 0);
            img = list[0];
            Assert.IsNotNull(img);
        }


        [TestMethod]
        public void TestSaveImage()
        {
            var list = NpkCoder.Load(@"D:\test\v6\01.img");
            Assert.IsTrue(list.Count > 0);
            var img = list[0];
            Assert.IsNotNull(img);
            var dir = @"D:\test\v6\image\";
            if (Directory.Exists(dir))
            {
                Directory.Delete(dir, true);
                Directory.CreateDirectory(dir);
            }
            img.List.ForEach(e => e.Picture.Save(dir + e.Index + ".png"));
            Assert.IsTrue(Directory.GetFiles(dir, "*.png").Length == img.Count);
        }


        [TestMethod]
        public void TestInvoke()
        {
            var commandParser = new CommandParser();
            Assert.AreEqual(commandParser.InvokeToken("|asNull;"), null);
            Assert.AreEqual(commandParser.InvokeToken("12342134sdgwseg|asNull;"), null);

            Assert.AreEqual(commandParser.InvokeToken("true|toBool;"), true);
            Assert.AreEqual(commandParser.InvokeToken("123|toBool;"), true);
            Assert.AreEqual(commandParser.InvokeToken("false|toBool;"), false);

            Assert.AreEqual(commandParser.InvokeToken("1;"), "1");
            Assert.AreEqual(commandParser.InvokeToken("1|toInt;"), 1);
            Assert.IsTrue(((commandParser.InvokeToken("1|toArray;") as string[]) ?? Array.Empty<string>()).SequenceEqual(new[] { "1" }));
            Assert.IsTrue(((commandParser.InvokeToken("1|toArray|toInt;") as int[]) ?? Array.Empty<int>()).SequenceEqual(new[] { 1 }));

            Assert.IsTrue(((commandParser.InvokeToken("1,2,3|toArray;") as string[]) ?? Array.Empty<string>()).SequenceEqual(new[] { "1", "2", "3" }));
            Assert.IsTrue(((commandParser.InvokeToken("1,2,3|toArray|toInt;") as int[]) ?? Array.Empty<int>()).SequenceEqual(new[] { 1, 2, 3 }));
            commandParser.InvokeToken("1,2,3|toArray|toInt|asVar|a;");
            Assert.IsTrue((commandParser.InvokeToken("|useVar|a;") as int[] ?? Array.Empty<int>()).SequenceEqual(new[] { 1, 2, 3 }));

            commandParser.InvokeToken("1,2,3|toArray|asVar|a;");
            Assert.IsTrue((commandParser.InvokeToken("|useVar|a|toInt;") as int[] ?? Array.Empty<int>()).SequenceEqual(new[] { 1, 2, 3 }));

            Assert.AreEqual(commandParser.InvokeToken("1|toInt|addOne|addOne|addOne|asVar|b;"), 4);
            Assert.AreEqual(commandParser.InvokeToken("|useVar|b|addOne;"), 5);

            commandParser.InvokeToken(
                @"|useVar|a|toInt|forEach|i|{
                            |useVar|i|addOne|addOne|addOne|asVar|b;
                       }"
            );

            Assert.AreEqual(commandParser.InvokeToken("|useVar|b;"), 6);
            // commandParser.ParseInvoke("|useVar|a|message");


        }

        [TestMethod]
        public void TestParseBlock()
        {
            var commandParser = new CommandParser();

            var ast = commandParser.GetAST(commandParser.ParseBlock("1|toInt;\t"));
            Assert.AreEqual("1\ntoInt\n;\n", ast);

            ast = commandParser.GetAST(commandParser.ParseBlock(@"
                1|toInt|toList;
                1,2,3|toList;
            "
            ));
            Assert.AreEqual("1\ntoInt\ntoList\n;\n1,2,3\ntoList\n;\n", ast);

            ast = commandParser.GetAST(commandParser.ParseBlock(@"
                1|toList|forEach|i {
                    2|toInt;
                    message|i;
                }
                
            "
            ));
            Assert.AreEqual(
                string.Join(
                "\n", 
                "1", "toList", "forEach", "i", "    2", "    toInt", "    ;",
                "    message", "    i", "    ;", "", ""
                ), 
                ast
            );

            var tokens = commandParser.ParseBlock(@"
                1|toList|forEach|i| {
                    2|toInt;
                    @{
                        调用外部函数;
                        参数1;
                        参数2;
                    }
                    1,2,3|toList|forEach|j|{
                        1,2|toList|forEach|k|{
                            k|message;
                        }
                    }
                    3|toInt;
                }
                4|toInt;
                
            "
            );
            ast = commandParser.GetAST(tokens);
            Assert.AreEqual(@"1
toList
forEach
i
    2
    toInt
    ;
    @
        调用外部函数
        ;
        参数1
        ;
        参数2
        ;

    1,2,3
    toList
    forEach
    j
        1,2
        toList
        forEach
        k
            k
            message
            ;


    3
    toInt
    ;

4
toInt
;
".Replace("\r", ""), ast);
            Console.WriteLine(ast);
        }
    }
}