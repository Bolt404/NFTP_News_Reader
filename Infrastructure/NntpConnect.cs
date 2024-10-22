using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using NNTP_NewsReader.InterfaceAdapter;

using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace NNTP_NewsReader.Infrastructure;

public class NntpConnect
{
    private static readonly object Lock = new();
    private static NntpConnect instance = null;
    private TcpClient tcpClient;
    private NetworkStream networkStream;
    private StreamReader reader;
    private bool isConnected;
    private bool isLoggedIn = false;
    private StreamWriter streamWriter;

    /// <summary>
    /// NntpConnect creates and holds the connection to the NNTP
    /// It's a Singleton to ensure we don't get multiple connections.
    /// It's thread-safe to avoid multiple threads accessing it at the same time.
    /// </summary>
    public static NntpConnect Instance
    {
        get
        {
            lock (Lock)
            {
                if (instance == null)
                {
                    instance = new NntpConnect();
                }

                return instance;
            }
        }
    }


    public async Task<string> Connect(string server, int port)
    {
        if (isConnected)
        {
            throw new InvalidOperationException("Already connected.");
        }

        try
        {
            tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(server, port);
            networkStream = tcpClient.GetStream();
            reader = new StreamReader(networkStream, Encoding.ASCII);
        
            string responseCode = (await reader.ReadLineAsync())?.Substring(0, 3) ?? throw new InvalidOperationException();
            Console.WriteLine("Response Code Connection : " + responseCode);

            isConnected = true;

            return responseCode;
        }
        catch (Exception e)
        {
            
            string responseCode = (await reader.ReadLineAsync())?.Substring(0, 3) ?? throw new InvalidOperationException();

            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<string> Login(string username, string password)
    {
        Console.WriteLine("Login: " + username);
        if (!isConnected || isLoggedIn)
        {
            throw new InvalidOperationException("No active connection to login.");
        } 
        
        streamWriter = new StreamWriter(networkStream, Encoding.ASCII)
        {
            AutoFlush = true
        };
        string messagetoSend = $"AUTHINFO USER {username}";
        Console.WriteLine($"DEBUG MESSAGE BEING SEND: {messagetoSend}");
        
        await streamWriter.WriteLineAsync(messagetoSend);
        string responseCode = (await reader.ReadLineAsync())?.Substring(0, 3) ?? throw new InvalidOperationException();
        Console.WriteLine("Response Code Login : " + responseCode);

        
        return responseCode;
    }

    public void Disconnect()
    {
        if (!isConnected)
        {
            throw new InvalidOperationException("No active connection to disconnect.");
        }

        reader?.Close();
        networkStream?.Close();
        tcpClient?.Close();
        isConnected = false;
    }
}
