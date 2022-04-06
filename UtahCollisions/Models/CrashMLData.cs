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
        //MAKE SURE THEY ARE ALL FLOATS//
        /////////////////////////////////////////////////////

        //public float MedianIncome { get; set; }
        //public float MedianHouseAge { get; set; }
        //public float AverageNumberOfRooms { get; set; }
        //public float AverageNumberOfBedrooms { get; set; }
        //public float Population { get; set; }
        //public float AverageOccupancy { get; set; }
        //public float Latitude { get; set; }
        //public float Longitude { get; set; }

        public Tensor<float> AsTensor()
        {
            float[] data = new float[]
            {
            //MedianIncome, MedianHouseAge, AverageNumberOfRooms, AverageNumberOfBedrooms,
            //Population, AverageOccupancy, Latitude, Longitude
            };
            int[] dimensions = new int[] { 1, 8 }; //I think the 8 represents all the features
            return new DenseTensor<float>(data, dimensions);
        }
    }
}
