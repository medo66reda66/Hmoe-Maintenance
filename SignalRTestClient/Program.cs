using Hmoe_Maintenance.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;

namespace SignalRTestClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiZ29kZWJvczY0NkBheWFibGUuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZW1haWxhZGRyZXNzIjoiZ29kZWJvczY0NkBheWFibGUuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiJkODcxNDU5Ni1hMzllLTRlZDQtODZhOC02MDk2ZTdiMGZjZjQiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJDb21wYW55T3duZXIiLCJqdGkiOiIwZDY4NTA5Yi0xNWYzLTQ0ZGQtOTE3ZC02MjM1NmRmNjFjOTIiLCJleHAiOjE3ODc3NDQ1MDMsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcwNjYiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo3MDY2In0.okKNIDe2ijbz5RzUMzTA0SPHSE3K3BnQuVduzhF7J2g";

            var connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7066/notificationHub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(token)!;
                })
                .Build();

            connection.On<Notification>("ReceiveNotification", notification =>
            {
                Console.WriteLine($"Title: {notification.Title}");
                Console.WriteLine($"Message: {notification.Message}");
                Console.WriteLine($"Type: {notification.Type}");
                Console.WriteLine($"IsRead: {notification.IsRead}");
                Console.WriteLine($"CreatedAt: {notification.CreatedAt}");
            });


            await connection.StartAsync();

            Console.WriteLine("Connected to SignalR!");
            await Task.Delay(Timeout.Infinite);
        }
    }
}
