using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseViewWinForm.DataModel
{
    public class MergedPartNumberInfo
    {
        public string UserName { get; set; }//用户名
        public string PartNumber { get; set; }//料号
        public Int64 ExposureACount { get; set; }    // 曝光A面数量
        public Int64 ExposureBCount { get; set; }    // 曝光B面数量
        public Int64 MakeupACount { get; set; }      // 拼板A面数量
        public Int64 MakeupBCount { get; set; }      // 拼板B面数量
        public string LayerName { get; set; } //层名称
        public string SetupFilm { get; set; }//干膜名称
        public double BoardThickness { get; set; }//板厚
        public double BoardWidth { get; set; }//板宽
        public double BoardHeight { get; set; }//板高
        public double ImageWidth { get; set; } //图宽
        public double ImageHeight { get; set; } //图高

        public double ScaleFactorX { get; set; }//x涨缩
        public double ScaleFactorY { get; set; }//y涨缩
        public double ScanSpeed { get; set; }//扫描速度

        public double SnsEnergy { get; set; }//扫描能量
        public string StartTime { get; set; }      // 开始时间    

    }
}
