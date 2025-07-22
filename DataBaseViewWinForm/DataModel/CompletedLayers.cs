using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DataBaseViewWinForm.DataModel
{
    public class CompletedLayers
    {
       public Int64 ID { get; set;  }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string PartNumber { get; set; }
        public Int64 TaskID { get; set; }
        public string ExpUseTable { get; set; }
        public string ExpMode { get; set; }
        public string ExpSide { get; set; }
        public string LayerName { get; set; }
        public Int64 PrintTotle { get; set; }
        public string SetupFilm { get; set; }
        public double SnsEnergy { get; set; }
        public double ScanSpeed { get; set; }
        public double BoardThickness { get; set; }
        public double BoardWidth { get; set; }
        public double BorardHeight { get; set; }
        public double ImageWidth { get; set; }
        public double ImageHeight { get; set; }
        public double ScaleFactorX { get; set; }
        public double ScaleFactorY { get; set; }
        public string AutoScaleFactorX { get; set; }
        public string AutoScaleFactorY { get; set; }
        public Int64 MakeupCount { get; set; }
        public string UserName { get; set; }
    }
}
