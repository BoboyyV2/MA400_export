using ACadSharp.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MA400_export
{
    
    public class Stud
    {


        public Circle circle { get; set; }

        public Stud()
        {
            circle = new Circle();
        }

        public Stud(Circle stud)
        {
            circle = stud;
        }

        


        public override string ToString()
        {
            return string.Format("G : X = {0}; Y = {1}; D = {2}",
                                  circle.Center.X.ToString("0.0"), circle.Center.Y.ToString("0.0"), circle.Radius*2);

        }
    }
}
