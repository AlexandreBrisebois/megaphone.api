using Dapr.Client;
using System;
using System.Diagnostics;
using Megaphone.Api.Events;
using Megaphone.Api.Models;
using Megaphone.Api.Models.Representations;
using Megaphone.Api.Models.Views;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace megaphone.api.Controllers
{
    [ApiController]
    [Route("api/feeds")]
    public class FeedController : ControllerBase
    {
        private readonly DaprClient daprClient;

        public FeedController([FromServices] DaprClient daprClient)
        {
            this.daprClient = daprClient;
        }

        [Route("")]
        [HttpPost]
        public async Task<IActionResult> PostAsync(NewFeed newFeed)
        {
            var e = CommandFactory.MakeAddFeedEvent(newFeed.Url);

            await daprClient.InvokeBindingAsync("feed-requests", "create", e);

            return Accepted();
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var e = CommandFactory.MakeDeleteFeedEvent(id);

            await daprClient.InvokeBindingAsync("feed-requests", "create", e);

            return Accepted();
        }

        [Route("")]
        [HttpPut]
        public async Task<IActionResult> PutAsync(FeedListView view)
        {
            await daprClient.SaveStateAsync("api-state", "list", view);

            if (Debugger.IsAttached)
                    Console.WriteLine("updated : \"feed list\"");

            return Ok();
        }

        [Route("")]
        [HttpGet]
        public async Task<JsonResult> GetAsync()
        {
            var view = await daprClient.GetStateAsync<FeedListView>("api-state", "list");
            if (view == null)
                view = new FeedListView() { Feeds = new List<FeedView>() };

            var r = RepresentationFactory.MakeFeedListRepresentation(view);

            return new JsonResult(r);
        }
    }
}