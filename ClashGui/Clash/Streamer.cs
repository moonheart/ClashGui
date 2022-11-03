﻿using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

namespace ClashGui.Clash;

public class Streamer : IAsyncEnumerable<string>
{
    private readonly string _uri;

    public Streamer(string url, string path)
    {
        if (url.StartsWith("http://"))
        {
            var wsUrl = url.Replace("http://", "ws://");
            _uri = $"{wsUrl}{path}";
        }
        else
        {
            _uri = $"ws://{url}{path}";
        }
    }

    public async IAsyncEnumerator<string> GetAsyncEnumerator(CancellationToken cancellationToken = new())
    {
        var clientWebSocket = new ClientWebSocket();
        await clientWebSocket.ConnectAsync(new Uri(_uri), cancellationToken);
        while (true)
        {
            var buffer = new ArraySegment<byte>(new byte[1024]);
            var result = await clientWebSocket.ReceiveAsync(buffer, cancellationToken);
            if (buffer.Array != null)
            {
                var s = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                foreach (var s1 in s.Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries))
                {
                    yield return s1;
                }
            }

            if (result.MessageType == WebSocketMessageType.Close)
            {
                break;
            }
        }
    }
}