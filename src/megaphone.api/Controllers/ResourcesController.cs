using Dapr.Client;
using Megaphone.Api.Commands;
using Megaphone.Api.Models.Representations;
using Megaphone.Api.Models.Views;
using Megaphone.Api.Queries;
using Megaphone.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Megaphone.Api.Controllers
{
    [ApiController]
    [Route("api/resources")]
    public class ResourcesController : ControllerBase
    {
        private readonly DaprClient daprClient;
        private readonly ResourceStorageService storageService;

        public ResourcesController([FromServices] DaprClient daprClient)
        {
            this.daprClient = daprClient;
            storageService = new ResourceStorageService(daprClient);
        }

        [Route("")]
        [HttpPut]
        public async Task<IActionResult> PutAsync(ResourceListView view)
        {
            var c = new PersistResourceListViewCommand(view.Date, view);
            await c.ApplyAsync(storageService);

            return Ok();
        }

        [Route("{year}/{month}/{day}")]
        [HttpGet]
        public async Task<JsonResult> GetResourcesByDateAsync(int year, int month, int day)
        {
            var q = new GetResourceListQuery(new DateTime(year, month, day));
            var view = await q.ExecuteAsync(storageService);

            var r = RepresentationFactory.MakeResourceListRepresentation(view);

            return new JsonResult(r);
        }

        [Route("{year}/{month}")]
        [HttpGet]
        public async Task<JsonResult> GetResourcesByMonthAsync(int year, int month)
        {
            DateTime date = new DateTime(year, month, 1);

            var dates = Enumerable.Range(0, 31).Select(i => date.Add(TimeSpan.FromDays(i))).Where(d => d.Month == date.Month && d <= DateTimeOffset.UtcNow).ToList();
            var r = await Task.WhenAll(dates.Select(async d =>
            {
                var q = new GetResourceListQuery(d);
                var view = await q.ExecuteAsync(storageService);

                return view;
            }));

            return new JsonResult(r.Where(v => v.Resources.Any()).Select(v => RepresentationFactory.MakeResourceListRepresentation(v)));
        }

        [Route("")]
        [HttpGet]
        public async Task<JsonResult> GetTodaysResourcesAsync()
        {
            var q = new GetResourceListQuery(DateTime.UtcNow);
            var view = await q.ExecuteAsync(storageService);

            var r = RepresentationFactory.MakeResourceListRepresentation(view);

            return new JsonResult(r);
        }
    }
}
