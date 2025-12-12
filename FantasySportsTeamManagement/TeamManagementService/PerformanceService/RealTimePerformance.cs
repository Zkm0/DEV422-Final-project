using Microsoft.AspNetCore.SignalR;

namespace PerformanceService
{
    public class RealTimePerformance : Hub
    {
        public async Task SendResults(string team1, string team2, int score1, int score2)
        {
            await Clients.All.SendAsync("ReceiveScore", team1, team2, score1, score2);
        }
    }
}
