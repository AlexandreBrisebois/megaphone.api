using Megaphone.Api.Models.Views;
using System.Linq;

namespace Megaphone.Api.Models.Representations
{
    public partial class RepresentationFactory
    {
        public static ResourceListRepresentation MakeResourceListRepresentation(ResourceListView view)
        {
            ResourceListRepresentation rlv = new ResourceListRepresentation(view.Date)
            {
                Updated = view.Updated
            };

            rlv.Resources.AddRange(view.Resources.Select(r => MakeResourceRepresentation(r)));
            return rlv;
        }

        public static ResourceRepresentation MakeResourceRepresentation(ResourceView view)
        {
            var r = new ResourceRepresentation
            {
                Display = view.Display,
                Url = view.Url
            };
            return r;
        }

        public static FeedListRepresentation MakeFeedListRepresentation(FeedListView view)
        {
            var flv = new FeedListRepresentation
            {
                Updated = view.Updated
            };

            flv.Feeds.AddRange(view.Feeds.Select(f => MakeFeedRepresentation(f)));
            return flv;
        }

        public static FeedRepresentation MakeFeedRepresentation(FeedView view)
        {
            var fv = new FeedRepresentation(view.Id)
            {
                Display = view.Display,
                Url = view.Url
            };
            return fv;
        }
    }
}