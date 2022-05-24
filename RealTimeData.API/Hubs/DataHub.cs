using Microsoft.AspNetCore.SignalR;

namespace RealTimeData.API.Hubs
{
    public class DataHub : Hub
    {
        public async Task SendMessage(string data)
        {
            await Clients.All.SendAsync("ReceiveData", data);
        }
    }
}
