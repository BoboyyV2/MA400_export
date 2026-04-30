using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MA400_export
{
    /**
     * <summary>
     * The <c>Scale</c> Class is used to store the orientation and scale of the current part.
     * </summary>
     */
    public class Scale
    {
        public double Xscale { get; private set; }
        public double Yscale { get; private set; }

        public Scale()
        {
            Xscale = 1;
            Yscale = 1;
        }

        public Scale(bool XscalePositive, bool YscalePositive)
        {
            this.Xscale = XscalePositive ? 1 : -1;
            this.Yscale = YscalePositive ? 1 : -1;
        }
    }

}
