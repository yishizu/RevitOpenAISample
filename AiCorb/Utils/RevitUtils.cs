using Autodesk.Revit.DB;

namespace AiCorb.Utils
{
    public class RevitUtils
    {
        public XYZ ConvertFeetToMeters(XYZ pointInFeet)
        {
            const double feetToMeters = 0.3048; // 1フィートは0.3048メートル
            return new XYZ(pointInFeet.X * feetToMeters, pointInFeet.Y * feetToMeters, pointInFeet.Z * feetToMeters);
        }
        public static XYZ ConvertMetersToFeet(XYZ pointInMeters)
        {
            const double metersToFeet = 3.28084; // 1メートルは約3.28084フィート
            return new XYZ(pointInMeters.X * metersToFeet, pointInMeters.Y * metersToFeet, pointInMeters.Z * metersToFeet);
        }
    }
}