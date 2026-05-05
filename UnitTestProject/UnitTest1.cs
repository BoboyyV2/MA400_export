using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MA400_export;
using System.IO;
using ACadSharp;
using ACadSharp.IO;
using ACadSharp.Entities;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using DXFImporter;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest
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

    // -------------------------------------------------------------------------
    // Tests de la classe Scale
    // -------------------------------------------------------------------------

    [TestClass]
    public class ScaleTests
    {
        [TestMethod]
        public void DefaultConstructor_SetsScaleToOne()
        {
            Scale s = new Scale();

            Assert.AreEqual(1.0, s.Xscale);
            Assert.AreEqual(1.0, s.Yscale);
        }

        [TestMethod]
        public void ParameterizedConstructor_StoresValues()
        {
            Scale s = new Scale(true, false);

            Assert.AreEqual(1, s.Xscale);
            Assert.AreEqual(-1, s.Yscale);
        }


    }

    // -------------------------------------------------------------------------
    // Tests de la classe Stud
    // -------------------------------------------------------------------------

    [TestClass]
    public class StudTests
    {
        [TestMethod]
        public void DefaultConstructor_IdIsMinusOne()
        {
            Stud stud = new Stud();

            Assert.AreEqual(-1, stud.id);
        }

        [TestMethod]
        public void DefaultConstructor_CircleIsNotNull()
        {
            Stud stud = new Stud();

            Assert.IsNotNull(stud.circle);
        }

        [TestMethod]
        public void ParameterizedConstructor_StoresIdAndCircle()
        {
            Circle circle = new Circle();
            circle.Center = new CSMath.XYZ(10.0, 20.0, 0);
            circle.Radius = Constants.StudRadius3;

            Stud stud = new Stud(circle, 5);

            Assert.AreEqual(5, stud.id);
            Assert.AreSame(circle, stud.circle);
        }

        [TestMethod]
        public void ToString_ContainsId()
        {
            Circle circle = new Circle();
            circle.Center = new CSMath.XYZ(10.5, 20.0, 0);
            circle.Radius = Constants.StudRadius3;

            Stud stud = new Stud(circle, 3);

            StringAssert.Contains(stud.ToString(), "G3");
        }

        [TestMethod]
        public void ToString_ContainsCoordinates()
        {
            Circle circle = new Circle();
            circle.Center = new CSMath.XYZ(10.5, 20.0, 0);
            circle.Radius = Constants.StudRadius3;

            Stud stud = new Stud(circle, 3);
            string result = stud.ToString();

            StringAssert.Contains(result, "10,5");
            StringAssert.Contains(result, "20,0");
        }

        [TestMethod]
        public void ToString_ContainsDiameter_ForRadius3()
        {
            Circle circle = new Circle();
            circle.Center = new CSMath.XYZ(0, 0, 0);
            circle.Radius = Constants.StudRadius3; // 1.5 => diamètre = 3.0

            Stud stud = new Stud(circle, 1);

            StringAssert.Contains(stud.ToString(), "3");
        }

        [TestMethod]
        public void ToString_ContainsDiameter_ForRadius4()
        {
            Circle circle = new Circle();
            circle.Center = new CSMath.XYZ(0, 0, 0);
            circle.Radius = Constants.StudRadius4; // 2.0 => diamètre = 4.0

            Stud stud = new Stud(circle, 2);

            StringAssert.Contains(stud.ToString(), "4");
        }
    }

    // -------------------------------------------------------------------------
    // Tests de la classe FileSystem
    // -------------------------------------------------------------------------

    [TestClass]
    public class FileSystemTests
    {
        [TestMethod]
        public void Constructor_IsClosedAndEmpty()
        {
            FileSystem fs = new FileSystem();

            Assert.IsFalse(fs.open);
            Assert.IsNotNull(fs.Studs);
            Assert.AreEqual(0, fs.Studs.Count);
        }

        [TestMethod]
        public void Reset_ClearsStudsAndClosesState()
        {
            FileSystem fs = new FileSystem();

            fs.reset();

            Assert.IsFalse(fs.open);
            Assert.AreEqual(0, fs.Studs.Count);
        }



        [TestMethod]
        public void OpenDxfFile_ReturnsFalse_WhenPathDoesNotExist()
        {
            FileSystem fs = new FileSystem();

            bool result = fs.OpenDxfFile("fichier_inexistant_12345.dxf");

            Assert.IsFalse(result);
            Assert.IsFalse(fs.open);
        }



        // -------------------------------------------------------------------------
        // Tests de ApplyTransform
        // -------------------------------------------------------------------------

        [TestMethod]
        public void ApplyTransform_PositiveScales_ComputesCorrectPosition()
        {
            FileSystem fs = new FileSystem();
            Circle stud = new Circle();
            stud.Center = new CSMath.XYZ(710.35, 73.70, 0);
            stud.Radius = Constants.StudRadius3;

            PointF offset = new PointF(708.35f, 71.70f);
            RectangleF dim = new RectangleF(0, 0, 210, 110);
            Scale scale = new Scale();

            Circle result = fs.ApplyTransform(stud, offset, dim, scale);

            // X = (710.35 - 708.35) * 1 = 2.0
            // Y = (73.70  - 71.70)  * 1 = 2.0
            Assert.AreEqual(2.0f, (float)result.Center.X, 0.001f);
            Assert.AreEqual(2.0f, (float)result.Center.Y, 0.001f);
        }



        [TestMethod]
        public void ApplyTransform_DoesNotModifyOriginalCircle()
        {
            FileSystem fs = new FileSystem();
            Circle stud = new Circle();
            stud.Center = new CSMath.XYZ(710.35, 73.70, 0);
            stud.Radius = Constants.StudRadius3;

            PointF offset = new PointF(708.35f, 71.70f);
            RectangleF dim = new RectangleF(0, 0, 210, 110);
            Scale scale = new Scale(true, false);

            fs.ApplyTransform(stud, offset, dim, scale);

            Assert.AreEqual(710.35, stud.Center.X, 0.001);
            Assert.AreEqual(73.70, stud.Center.Y, 0.001);
        }

        [TestMethod]
        public void ApplyTransform_PreservesRadius()
        {
            FileSystem fs = new FileSystem();
            Circle stud = new Circle();
            stud.Center = new CSMath.XYZ(710.35, 73.70, 0);
            stud.Radius = Constants.StudRadius4;

            PointF offset = new PointF(708.35f, 71.70f);
            RectangleF dim = new RectangleF(0, 0, 210, 110);
            Scale scale = new Scale(true, false);

            Circle result = fs.ApplyTransform(stud, offset, dim, scale);

            Assert.AreEqual(Constants.StudRadius4, result.Radius, 0.001);
        }

    }

    // -------------------------------------------------------------------------
    // Tests de Shape methode
    // -------------------------------------------------------------------------

    [TestClass]
    public class ShapeTests
    {
        [TestMethod]
        public void GetOffsetedPositionEquivalent()
        {
            circle c = new circle(new System.Drawing.Point(250, -100), 1.5, System.Drawing.Color.White, System.Drawing.Color.White, 2);

            PointF pf = DXFImporter.Shape.getOffsetedPosition(c.AccessCenterPoint);
            float x = DXFImporter.Shape.getOffsetedPositionX(c.AccessCenterPoint.X);
            float y = DXFImporter.Shape.getOffsetedPositionY(c.AccessCenterPoint.Y);

            Assert.AreEqual(pf.X, x);
            Assert.AreEqual(pf.Y, y);

        }

    }

}