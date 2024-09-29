using System;
using System.Collections.Generic;
using System.Text;

namespace NewsClassifierModel
{
    public class PredictResponse
    {
        public string PredictionCategory { get; set; }
        public string BusinessPercent { get; set; }
        public string TechnologyPercent { get; set; }
        public string EntertainmentPercent { get; set; }
        public string HealthPercent { get; set; }
        
    }
}
