using System;
using System.IO;

namespace AiCorb.Utils
{
    public static class AiCorbSettings
    {
        public static  string  SAVE_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AiCorb", "FacadeData");
    }
}