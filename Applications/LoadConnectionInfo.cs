using System.IO;
using System.Text.Json;
using NNTP_NewsReader.Entitys;

namespace NNTP_NewsReader.Application;

public class LoadConnectionInfo
{
    private readonly string filePath;

    public LoadConnectionInfo(string filePath)
    {
        this.filePath = filePath;
    }

    public ConnectionInfo LoadInfo()
    {
        if (File.Exists(filePath))
        {
            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<ConnectionInfo>(jsonString);
        }
        return null;
    }

    public void SaveUser(ConnectionInfo ConnectionInfo)
    {
        string jsonString = JsonSerializer.Serialize(ConnectionInfo, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, jsonString);
    }
}