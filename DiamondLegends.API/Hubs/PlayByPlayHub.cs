using Microsoft.AspNetCore.SignalR;

namespace DiamondLegends.API.Hubs
{
    public class PlayByPlayHub : Hub
    {
        public async Task SendEvent(string message)
        {
            await Clients.All.SendAsync("SendEvent", message);
        }
    }
}
