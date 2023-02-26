using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompleteData.Server.Hubs
{
    public class DataHub : Hub
    {
        public async Task SyncRecord(string Table, string Action, string Id)
        {
            await Clients.Others.SendAsync("ReceiveSyncRecord",
                  Table, Action, Id);
        }
    }
}
