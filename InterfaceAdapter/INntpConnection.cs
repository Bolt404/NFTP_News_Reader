namespace NNTP_NewsReader.InterfaceAdapter;

public interface INntpConnection
{
    string Connect(string server, int port);
    string Disconnect();
}