using System;
using System.Text.Json.Serialization;

namespace devops_metrics
{
    public class Rootobject
    {
        public string kind { get; set; }
        public string apiVersion { get; set; }
        public Metadata metadata { get; set; }
        public Item[] items { get; set; }
    }

    public class Metadata
    {
        public string selfLink { get; set; }
    }

    public class Item
    {
        public Describedobject describedObject { get; set; }
        public string metricName { get; set; }
        public DateTime timestamp { get; set; }
        public string value { get; set; }
    }

    public class Describedobject
    {
        public string kind { get; set; }

        [JsonPropertyName("namespace")]
        public string _namespace { get; set; }
        public string name { get; set; }
        public string apiVersion { get; set; }
    }

}