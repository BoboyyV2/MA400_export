using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MA400_export
{
    /**
     * <summary>
     * The <c>Scale</c> Class is used to store the orientation and scale of the current part 
     * </summary>
     */
    public class Scale
    {
        public double Xscale { get; set; }
        public double Yscale { get; set; }

        public Scale()
        {
            Xscale = 1;
            Yscale = 1;
        }

        public Scale(double Xscale, double Yscale)
        {
            this.Xscale = Xscale;
            this.Yscale = Yscale;
        }
    }

}
