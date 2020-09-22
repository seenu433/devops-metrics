using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace devops_metrics.Controllers
{
    [ApiController]
    [Route("apis/custom.metrics.k8s.io/v1beta1")]
    [Route("apis/custom.metrics.k8s.io/v1beta1/namespaces/default/services/devops-proxy/count")]
    [Route("[controller]")]
    public class MetricsController : ControllerBase
    {
        private readonly ILogger<MetricsController> _logger;

        public MetricsController(ILogger<MetricsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Produces("application/json")]
        public Rootobject Get()
        {
            string pendingJobs;

            WebRequest request = WebRequest.Create("https://dev.azure.com/{org}/{project}/_apis/build/builds?api-version=6.1-preview.6&statusFilter=notStarted");
            request.Headers.Add("Authorization", "Basic <<base64 encoded vakue of username:PAT>>");

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader responseReader = new StreamReader(responseStream))
                    {
                        string json = responseReader.ReadToEnd();
                        pendingJobs = JObject.Parse(json)["count"].ToString();
                    }
                }
            }

            Rootobject obj = new Rootobject();
            obj.kind = "MetricValueList";
            obj.apiVersion = "custom.metrics.k8s.io/v1beta1";
            obj.metadata = new Metadata() { selfLink = "/apis/custom.metrics.k8s.io/v1beta1/" };
            obj.items = new[] { new Item() { metricName = "count", timestamp = DateTime.Now, value = pendingJobs, describedObject = new Describedobject() { apiVersion = "/v1beta1", kind = "Service", _namespace = "default", name = "devops-proxy" } } };

            return obj;
        }
    }
}
