using Megaphone.Standard.Events;
using System;

namespace Megaphone.Api.Events
{
    internal class CommandFactory
    {
        internal static Event MakeAddFeedEvent(Uri uri) => EventBuilder.NewEvent(Events.Feed.Add).WithMetadata("url", uri.ToString()).Make();
        internal static Event MakeDeleteFeedEvent(string id) => EventBuilder.NewEvent(Events.Feed.Delete).WithMetadata("id", id).Make();
    }
}
