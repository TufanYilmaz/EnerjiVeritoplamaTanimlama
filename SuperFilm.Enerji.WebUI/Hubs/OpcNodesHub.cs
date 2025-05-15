

using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SuperFilm.Enerji.Entites;
using TanvirArjel.EFCore.GenericRepository;

namespace SuperFilm.Enerji.WebUI.Hubs
{
    public class OpcNodesHub : Hub
    {
        //public override async Task OnDisconnectedAsync(Exception? exception)
        //{
        //    await Clients.All.SendAsync("ReceiveMessage", "System", $"!!! {Context.ConnectionId} disconnected.");

        //}
        //public override async Task OnConnectedAsync()
        //{
        //    await Clients.All.SendAsync("ReceiveMessage", "System", $"{Context.ConnectionId} connected.");
        //}
        private readonly IQueryRepository<EnerjiDbContext> _queryRepository;

        public OpcNodesHub(IQueryRepository<EnerjiDbContext> queryRepository)
        {
            _queryRepository = queryRepository;
        }

        public async Task SendOpcNodesCount()
        {
            var numOfOpcNodes = await _queryRepository.GetQueryable<OpcNodes>().CountAsync();
            await Clients.All.SendAsync("ReceiveMessage", numOfOpcNodes);
        }
    }
}
