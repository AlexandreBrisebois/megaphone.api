using System;
using System.Text.Json.Serialization;

namespace Megaphone.Api.Models
{
    public class NewFeed
    {
        [JsonPropertyName("url")]
        public Uri Url { get; set; }
    }
}
