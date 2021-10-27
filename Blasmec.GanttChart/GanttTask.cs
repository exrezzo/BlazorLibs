using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Blasmec.GanttChart
{
    public class GanttTask
    {
        /*
         * TODO: AMPLIARE LA STRUTTURA DATI IN MANIERA TALE DA INCLUDERE I LINK
         */
        private string _color;
        private string _progressColor;
        [JsonPropertyName("id")] public long Id { get; set; } = 1;
        [JsonPropertyName("text")]
        public string Text { get; set; }
        [JsonIgnore]
        public DateTime StartDate { get; set; }
        [JsonIgnore]
        public DateTime EndDate { get; set; }
        [JsonPropertyName("start_date")]
        public string StartDateString
        {
            get => StartDate.ToString("yyyy-MM-dd");
            set
            {
                Console.WriteLine(value);
                StartDate = DateTime.Parse(value);
            }
        }

        [JsonPropertyName("end_date")]
        public string EndDateString
        {
            get => EndDate.ToString("yyyy-MM-dd");
            set
            {
                Console.WriteLine(value);
                EndDate = DateTime.Parse(value);
            }
        }
        //[JsonPropertyName("duration")]
        //public int? Duration { get; set; }
        [JsonPropertyName("parent")]
        public int ParentId { get; set; } = 0;
        [JsonPropertyName("progress")]
        public double Progress { get; set; } = 0.0;

        [JsonPropertyName("color")]
        public string Color
        {
            get => _color;
            set
            {
                _color = value;
                var c = (Color) new ColorConverter().ConvertFrom(value);
                var level = 2;
                var darkenColor = System.Drawing.Color.FromArgb(c.A,
                    (int)(c.R / level), (int)(c.G / level), (int)(c.B / level));
                _progressColor = ColorTranslator.ToHtml(darkenColor);
            }
        }

        [JsonPropertyName("progressColor")]
        public string ProgressColor
        {
            get => _progressColor;
            private set => _progressColor = value;
        }
    }


}
