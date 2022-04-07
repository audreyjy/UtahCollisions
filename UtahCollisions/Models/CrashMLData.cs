using Microsoft.ML.OnnxRuntime.Tensors;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UtahCollisions.Models
{
    public class CrashMLData
    {
        //Insert features user imputs to get prediction here//
        /////////////////////////////////////////////////////
        [Required(ErrorMessage = "Please enter a valid Mile Point")]
        public float MilePoint { get; set; }
        [Required(ErrorMessage = "Please enter a valid Latitude Number")]
        public float Latitude { get; set; }
        [Required(ErrorMessage = "Please enter a valid Longitude Number")]
        public float Longitude { get; set; }
        [Required(ErrorMessage = "Please enter a valid Day of the Month")]
        public float Day { get; set; }
        [Required(ErrorMessage = "Please enter a valid Month")]
        public float Month { get; set; }
        [Required(ErrorMessage = "Please enter a valid Hour of the Day")]
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
