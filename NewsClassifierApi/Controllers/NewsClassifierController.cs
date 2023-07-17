using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsClassifierModel;
using System.Xml.Linq;

namespace NewsClassifierApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsClassifierController : ControllerBase
    {
        private readonly ConsumeModel _newsClassifier;

        public NewsClassifierController(ConsumeModel newsClassifier)
        {
            _newsClassifier = newsClassifier;
        }

        [HttpPost]
        public JsonResult Post([FromBody] string input)
        {
            var classification = _newsClassifier.Predict(input);
            return new JsonResult(classification);
        }

    }
   
}
