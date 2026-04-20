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
        public int id { get; set; }

        public Stud()
        {
            circle = new Circle();
            id = -1;
        }

        public Stud(Circle stud, int id)
        {
            circle = stud;
            this.id = id;
        }

        


        public override string ToString()
        {
            return string.Format("G{0} : X = {1}; Y = {2}; D = {3}",
              id, circle.Center.X, circle.Center.Y, circle.Radius);

        }
    }
}
