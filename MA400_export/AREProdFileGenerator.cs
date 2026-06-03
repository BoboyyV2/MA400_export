using ACadSharp;
using ACadSharp.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MA400_export
{
    public class AREProdFileGenerator : ProdFileGenerator
    {
        public AREProdFileGenerator(BindingList<Stud> Studs, CadObjectCollection<Entity> Entities, RectangleF Dimension,
                                    PointF Offset, GeneratorData Data, Scale Scalefact, double Rotation)
                                    : base(Studs, Entities, Dimension, Offset, Data, Scalefact, Rotation)
        {
            this.Studs = Studs;
            this.Entities = Entities;
            this.Dimension = Dimension;
            this.Offset = Offset;
            this.Data = Data;
            this.Scalefact = Scalefact;
            this.Rotation = Rotation;

        }

        /**
        * <summary>Generate the files necessary for the machine to work and save them to the \daten and \cnc folder</summary>
        * <param name="fileID">The ProgramNumber of the files to write.</param>
        */
        public override void GenerateProductionFiles(string name)
        {
            //TODO: implement the file generation for ARE
        }
    }
}
