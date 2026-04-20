using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MA400_export;
using System.IO;
using ACadSharp;
using ACadSharp.IO;
using ACadSharp.Entities;
using ACadSharp.Objects;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestOpenbasic()
        {
            string path = "../testfiles/sample.dxf";
            string pathlong = @"C:\Users\MAGASIN\source\repos\MA400_export\UnitTestProject\bin\testfiles\sample.dxf";
            FileSystem fs = new FileSystem();
            if (File.Exists(path))
            {
                fs.OpenDxfFile(path);
                fs.OpenDxfFile(pathlong);

            }

        }

        [TestMethod]
        public void TestOpen2013()
        {
            string path = "../testfiles/test.dxf";
            FileSystem fs = new FileSystem();
            if (File.Exists(path))
            {
                fs.OpenDxfFile(path);
            }

        }

        



    }
}
