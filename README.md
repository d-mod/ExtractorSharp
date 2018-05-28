ExtractorSharp 
==
[![Stars](https://img.shields.io/github/stars/Kritsu/ExtractorSharp.svg?style=flat-square)](https://github.com/Kritsu/ExtractorSharp/stargazers)
[![Fork](https://img.shields.io/github/forks/Kritsu/ExtractorSharp.svg?style=flat-square)](https://github.com/Kritsu/ExtractorSharp/network/members)
[![Release](https://img.shields.io/github/release/Kritsu/ExtractorSharp.svg?style=flat-square)](https://github.com/Kritsu/ExtractorSharp/releases/latest)
[![Downloads](https://img.shields.io/github/downloads/Kritsu/ExtractorSharp/total.svg?style=flat-square)](https://github.com/Kritsu/ExtractorSharp/releases/latest)
[![License](https://img.shields.io/github/license/Kritsu/ExtractorSharp.svg?style=flat-square)](https://github.com/Kritsu/ExtractorSharp/blob/master/LICENSE)


关于
---
   ExtractorSharp是一个用于编辑IMG,NPK等类型文件的软件
   
   Extractor是沿袭之前的软件[DNF Extractor](http://bbs.exrpg.com/thread-106989-1-1.html)
   
   Sharp表示C#开发,并且有锋利/增强的双关之意
   
   不过我最近给它的定位是Editor,而不是Extractor

文件支持
---
   + SPK (Readable)
   + OGG (Readable)
   + DDS (Readable)
        + DXT1
        + DXT3
        + DXT5
   + NPK (Readable/Writeable)
   + GIF (Readable/Writeable)
   + IMG (Readable/Writeable)
        + Ver1 
        + Ver2
        + Ver4
        + Ver5
        + Ver6
   
   ES可以以文件转换插件的形式,将其他类型的文件转换为IMG结构,例如GIF和SPK的支持

   或者是以版本Handler插件的形式,增加对无法转换为IMG结构的文件支持,例如OGG
        
第三方依赖(Dependencies)
---
   + 音频处理
        + [Bass](https://github.com/Kritsu/ExtractorSharp/blob/master/Licenses/bass-license.txt)
   + 压缩
        + [Zlib](https://github.com/Kritsu/ExtractorSharp/blob/master/Licenses/zlib-license.txt)
        + [SharpZipLib](https://github.com/Kritsu/ExtractorSharp/blob/master/Licenses/SharpZipLib-license.txt)
       
开发环境
---
   + Windows 10
   + .NET Framework 4.6.1
     
运行环境
---
   + Windows 7,8,10
   + .NET Framework 4.6 以上

**在Windows7环境中,你需要自行安装[.NET Framework 4.6](https://www.microsoft.com/zh-CN/download/confirmation.aspx?id=48130)**

**事实上.NET4.5即可支持运行,但是由于微软糟糕的兼容,有一部分的公共库会出现异常的情况,所以请务必使用.NET4.6以上**
   
语言(Language)
---
   + 中文(简体) (default)
   + English 
 
 **ExtractorSharp自带了一个英文的语言文件。请打开设置(Setting)-语言(Language)切换**
 
 (请不要高估作者的英文水平
 
文档(Documents)
 ---
   [ExtractorSharp Docs](https://kritsu.github.io/docs/)

插件(Plugins)
---
   ES支持自定义插件。并且内置了由ES作者开发的插件
      
   你可以自行开发或者安装/卸载插件
   
   (当然API什么的没空整理的

许可(License)
---
[MIT LICENSE](https://github.com/Kritsu/ExtractorSharp/blob/master/LICENSE)


