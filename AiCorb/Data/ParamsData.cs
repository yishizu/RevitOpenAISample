namespace AiCorb.Data
{
    using Newtonsoft.Json;

    public class ParamsData
    {
        [JsonProperty("params")]
        public Params Params { get; set; }
    }

    public class Params
    {
        [JsonProperty("window_aspect_ratio")]
        public double WindowAspectRatio { get; set; }

        [JsonProperty("frame_thickness_ratio")]
        public double FrameThicknessRatio { get; set; }

        [JsonProperty("panel_aspect_ratio")]
        public double PanelAspectRatio { get; set; }

        [JsonProperty("window_depth")]
        public double WindowDepth { get; set; }
    }
}