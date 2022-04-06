using Microsoft.AspNetCore.Mvc;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtahCollisions.Models;

namespace UtahCollisions.Controllers
{
    public class InferenceController : Controller
    {
        private InferenceSession _session;

        public InferenceController(InferenceSession session)
        {
            _session = session;
        }

        [HttpGet]
        public IActionResult EnterData()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Score(CrashMLData data)
        {
            var result = _session.Run(new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("float_input", data.AsTensor())
            });

            Tensor<float> score = result.First().AsTensor<float>();

            var prediction = new Prediction { PredictedValue = score.First() };
            result.Dispose();
            return View("Score", prediction);
        }

    }
}
