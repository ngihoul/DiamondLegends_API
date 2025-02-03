using DiamondLegends.API.Hubs;
using DiamondLegends.BLL.Services.Interfaces;
using DiamondLegends.Domain.Models;
using Microsoft.AspNetCore.SignalR;

namespace DiamondLegends.API.Services
{
    public class PlayByPlayNotifier : IPlayByPlayNotifier
    {
        private readonly IHubContext<PlayByPlayHub> _hubContext;

        public PlayByPlayNotifier(IHubContext<PlayByPlayHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendEvent(GameEvent gameEvent)
        {
            await _hubContext.Clients.All.SendAsync("SendEvent", gameEvent);
        }
    }
}
