using Megaphone.Api.Models.Views;
using Megaphone.Standard.Commands;
using Megaphone.Standard.Services;
using System.Threading.Tasks;
using System;
using System.Diagnostics;

namespace Megaphone.Api.Commands
{
    internal class PersistResourceListViewCommand : ICommand<IPartionedStorageService<ResourceListView>>
    {
        const string CONTENT_KEY = "resources.json";

        readonly ResourceListView view;
        readonly string partitionKey;

        public PersistResourceListViewCommand(DateTime date, ResourceListView view)
        {
            partitionKey = $"{date.Year}/{date.Month}/{date.Day}";
            this.view = view;
        }

        public async Task ApplyAsync(IPartionedStorageService<ResourceListView> model)
        {
            await model.SetAsync(partitionKey, CONTENT_KEY, view);

            if (Debugger.IsAttached)
                    Console.WriteLine($"updated : \"{partitionKey}\"");
        }
    }
}
