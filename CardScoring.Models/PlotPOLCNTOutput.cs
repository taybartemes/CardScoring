using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardScoring.Models
{
    public interface IPlotPOLCNTOutput
    {
        int EUID { get; set; }
        int Plot { get; set; }
        int POLCNT { get; set; }
    }
    public class PlotPOLCNTOutput : IPlotPOLCNTOutput
    {
        public int EUID { get; set; }

        public int Plot { get; set; }

        public int POLCNT { get; set; }
    }
}
