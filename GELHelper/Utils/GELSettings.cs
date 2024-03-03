using System;
using System.IO;

namespace GELHelper.Utils
{
    public static class GELHelperSettings
    {
        public static  string  SAVE_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GELHelper", "FacadeData");
        public static string DtypeUrl = "https://modeler.GELHelper.com/dev/dtype";
        public static string ParamUrl = "https://modeler.GELHelper.com/dev/param";
        public static string ApiKey = "F2mChnT4wr8MwjnVu0w2620zxEkCZVcX3CLxN5Gd"; 
        public static string ImagePath = @"C:\Users\ykish\OneDrive\デスクトップ\test.jpg";
        public static int Panel_Height = 4000;
    }
}