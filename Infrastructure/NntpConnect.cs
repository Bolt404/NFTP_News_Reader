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
using System.Windows;

namespace NNTP_NewsReader.Infrastructure;

public class NntpConnect
{
    private static readonly object Lock = new();
    private static NntpConnect instance = null;
    private TcpClient tcpClient;
    private NetworkStream networkStream;
    private StreamReader reader;
    public bool isConnected = false;
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


    public async Task<ENntpResponseCodes> Connect(string server, int port)
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
            
            isConnected = true;
            
            Console.WriteLine($"Connecting to {server}:{port} | {isConnected}");
            var responseCode = HandleResponse(ParseResponseCodeStringToEnum(await reader.ReadLineAsync()));

            return await responseCode;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    /// <summary>
    /// Logs into a NNTP using username and Password.
    /// Doing both operations at once as it'll hardly ever make sense just to do user without password.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<ENntpResponseCodes> Login(string username, string password)
    {
        Console.WriteLine("Login: " + username);
        //Checker om vi har en connection og om vi er logged ind.
        if (!isConnected || isLoggedIn)
        {
            throw new InvalidOperationException($"{isConnected} : No active connection to login \n {isLoggedIn} : Already logged in.");
        } 
        
        ENntpResponseCodes userResponse = await SendMessage($"AUTHINFO USER {username}");
        await HandleResponse(userResponse);

        if (userResponse == ENntpResponseCodes.SendPassword)
        {
            isLoggedIn = true;
            userResponse = await SendMessage($"AUTHINFO PASS {password}");
        }
        
        return await HandleResponse(userResponse);
    }

    private async Task<ENntpResponseCodes> SendMessage(string messagetoSend)
    {
        if (streamWriter == null)
        {
            streamWriter = new StreamWriter(networkStream, Encoding.ASCII)
            {
                AutoFlush = true
            };  
        } 
        //messagetoSend = $"AUTHINFO USER {username}";
        Console.WriteLine($"Message Being sent: {messagetoSend}");
        
        await streamWriter.WriteLineAsync(messagetoSend);
        var responseCode = ParseResponseCodeStringToEnum(await reader.ReadLineAsync());
        
        return responseCode;
    }

    private ENntpResponseCodes ParseResponseCodeStringToEnum(string responseString)
    {
        ENntpResponseCodes responseCode = (ENntpResponseCodes)int.Parse(responseString?.Substring(0, 3) ?? throw new InvalidOperationException("No response"));
        return responseCode;
    }

    public async Task<ENntpResponseCodes> HandleResponse(ENntpResponseCodes code)
    {
        switch (code)
        {
            case ENntpResponseCodes.ConnectionReady:
                Console.WriteLine($"{(int)code} : Connection successful.");
                break;
            case ENntpResponseCodes.AuthSuccess:
                Console.WriteLine($"{(int)code} : Authentication Successful.");
                break;
            case ENntpResponseCodes.SendPassword:
                Console.WriteLine($"{(int)code} : Server Requires Password.");
                break;
            case ENntpResponseCodes.CommandNotRecognised:
                Console.WriteLine($"{(int)code} : Command Not Recognised.");
                //Disconnect as we assume we need to start over due to the way the program is setup.
                Disconnect();
                throw new InvalidOperationException("A Command was Not Recognised");
            case ENntpResponseCodes.SyntaxError:
                Console.WriteLine($"{(int)code} : Syntax Error.");
                break;
            case ENntpResponseCodes.AccessRestricted:
                Console.WriteLine($"{(int)code} : Access Restricted.");
                break;
            case ENntpResponseCodes.ConnectionClosed:
                Console.WriteLine($"{(int)code} : Connection Closed - GoodBye!");
                break;
            default:
                Console.WriteLine($"{(int)code} : Unhandled response code");
                break;
        }
        return code;
    }

    public async Task Disconnect()
    {
        if (!isConnected)
        {
            throw new InvalidOperationException("No active connection to disconnect.");
        }
        
        isConnected = false;
        
        ENntpResponseCodes responseCode = await SendMessage("QUIT");
        await HandleResponse(responseCode);
        
        reader?.Close();
        networkStream?.Close();
        tcpClient?.Close();
    }
}
