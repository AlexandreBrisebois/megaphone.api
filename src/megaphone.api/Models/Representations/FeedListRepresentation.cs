using Megaphone.API.Models.Representations.Links;
using Megaphone.Standard.Representations;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Megaphone.Api.Models.Representations
{
    public class FeedListRepresentation : Representation
    {
        public FeedListRepresentation()
        {
            base.AddLink(Relations.Self, "api/feeds");
        }

        [JsonPropertyName("feeds")]
        public List<FeedRepresentation> Feeds { get; set; } = new List<FeedRepresentation>();

        [JsonPropertyName("updated")]
        public DateTimeOffset Updated { get; internal set; }
    }
}