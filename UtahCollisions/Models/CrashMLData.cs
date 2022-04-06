using Microsoft.ML.OnnxRuntime.Tensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UtahCollisions.Models
{
    public class CrashMLData
    {
        //Insert features user imputs to get prediction here//
        /////////////////////////////////////////////////////

        public float MilePoint { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public float Day { get; set; }
        public float Month { get; set; }
        public float Hour { get; set; }
        
        public Tensor<float> AsTensor()
        {
            float[] data = new float[]
            {
                MilePoint, Latitude, Longitude, Day, Month, Hour
            };
            int[] dimensions = new int[] { 1, 6 };
            return new DenseTensor<float>(data, dimensions);
        }
    }
}
