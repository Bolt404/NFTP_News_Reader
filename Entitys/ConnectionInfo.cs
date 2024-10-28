namespace NNTP_NewsReader.Entitys;

public class ConnectionInfo
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"Host: {Host}, Port: {Port}, Username: {Username}, Password: {Password}";
    }
}