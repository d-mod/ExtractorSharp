using System;
using System.Linq;
using ExtractorSharp.Script.Mes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtractorSharp.UnitTest.Command {
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestInvoke()
        {
            var commandParser = new MesParser();
            Assert.AreEqual(commandParser.InvokeToken("|asNull;"), null);
            Assert.AreEqual(commandParser.InvokeToken("12342134sdgwseg|asNull;"), null);

            Assert.AreEqual(commandParser.InvokeToken("true|toBool;"), true);
            Assert.AreEqual(commandParser.InvokeToken("123|toBool;"), true);
            Assert.AreEqual(commandParser.InvokeToken("false|toBool;"), false);

            Assert.AreEqual(commandParser.InvokeToken("1;"), "1");
            Assert.AreEqual(commandParser.InvokeToken("1|toInt;"), 1);
            Assert.IsTrue(((commandParser.InvokeToken("1|toArray;") as string[]) ?? new string[]{}).SequenceEqual(new[] { "1" }));
            Assert.IsTrue(((commandParser.InvokeToken("1|toArray|toInt;") as int[]) ?? new int[]{}).SequenceEqual(new[] { 1 }));

            Assert.IsTrue(((commandParser.InvokeToken("1,2,3|toArray;") as string[]) ?? new string[] { }).SequenceEqual(new[] { "1", "2", "3" }));
            Assert.IsTrue(((commandParser.InvokeToken("1,2,3|toArray|toInt;") as int[]) ?? new int[]{}).SequenceEqual(new[] { 1, 2, 3 }));
            commandParser.InvokeToken("1,2,3|toArray|toInt|asVar|a;");
            Assert.IsTrue((commandParser.InvokeToken("|useVar|a;") as int[] ?? new int[]{}).SequenceEqual(new[] { 1, 2, 3 }));

            commandParser.InvokeToken("1,2,3|toArray|asVar|a;");
            Assert.IsTrue((commandParser.InvokeToken("|useVar|a|toInt;") as int[] ?? new int[]{}).SequenceEqual(new[] { 1, 2, 3 }));

            Assert.AreEqual(commandParser.InvokeToken("1|toInt|addOne|addOne|addOne|asVar|b;"), 4);
            Assert.AreEqual(commandParser.InvokeToken("|useVar|b|addOne;"), 5);

            commandParser.InvokeToken(
                @"|useVar|a|toInt|forEach|i|{
                            |useVar|i|addOne|addOne|addOne|asVar|b;
                       }"
            );

            Assert.AreEqual(commandParser.InvokeToken("|useVar|b;"), 6);
            Assert.AreEqual(commandParser.InvokeToken("|useVar|b.ToString();"), "6");
            Assert.AreEqual(commandParser.InvokeToken("|useVar|a.Length;"), 3);

            // Assert.AreEqual(commandParser.InvokeToken("|useVar|b.CompareTo(3);"), 1); // 目前不支持传参

            Assert.AreEqual(commandParser.InvokeToken("|useVar|b.ToString()|Concat|1;"), "61");
            Assert.AreEqual(commandParser.InvokeToken("1|Concat|useVar|b;"), "16");

            Assert.AreEqual(commandParser.InvokeToken("|useVar|b.ToString()|Concat|aaaa;"), "6aaaa");
            Assert.IsTrue((
                    commandParser.InvokeToken("|useVar|a|toInt|Concat|4|toInt|addOne|addOne;") as int[] ?? new int[]{}
                ).SequenceEqual(
                    new[] { 1, 2, 3, 6 }
                ));
            Assert.IsFalse((
                commandParser.InvokeToken("4|toInt|addOne|addOne|Concat|useVar|a|toInt;") as int[] ?? new int[]{}
            ).SequenceEqual(
                new[] { 6, 1, 2, 3 }
            )); // concat 左侧的值不应该因右侧的值改变类型.

            // 环境变量部分
            Assert.AreEqual(null, commandParser.InvokeToken("|useEnvVar|testEnv;"));
            Assert.AreEqual("qaerfqaw", commandParser.InvokeToken("qaerfqaw|asEnvVar|testEnv;"));
            Assert.AreEqual("qaerfqaw", commandParser.InvokeToken("|useEnvVar|testEnv;"));
            Assert.AreEqual("qaerfqaw", Environment.GetEnvironmentVariable("testEnv"));
        }

        [TestMethod]
        public void TestParseBlock()
        {
            var commandParser = new MesParser();

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
        }
    }
}