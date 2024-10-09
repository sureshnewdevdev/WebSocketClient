using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class WebSocketTestController : ControllerBase
{
    [HttpGet("connect")]
    public async Task<IActionResult> ConnectToWebSocket(string msg)
    {
        using var client = new ClientWebSocket();
        // Connecting to the WebSocket server at http://localhost:5165/ws
        await client.ConnectAsync(new Uri("ws://localhost:5165/ws"), CancellationToken.None);

        // Message to send to the server
        var message = msg; 
        var bytes = Encoding.UTF8.GetBytes(message);

        // Send message to WebSocket server
        await client.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);

        // Receive message from the WebSocket server
        var buffer = new byte[1024];
        var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        var serverResponse = Encoding.UTF8.GetString(buffer, 0, result.Count);

        // Return the server's response as a JSON result
        return Ok(new { serverResponse });
    }
}
