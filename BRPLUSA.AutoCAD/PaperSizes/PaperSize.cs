using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Internal;

namespace BRPLUSA.AutoCAD.Wrappers
{
    public abstract class PaperSize
    {
        public abstract double X { get; }
        public abstract double Y { get; }
    }
}
