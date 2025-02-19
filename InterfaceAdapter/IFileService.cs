﻿namespace NNTP_NewsReader.InterfaceAdapter;

public interface IFileService
{
    bool IsDirectoryAvailable(string directory);
    bool DirectoryContainsFiles(string directory); 
    List<string> GetFiles(string directory);
    bool FileIsOfType(string filename, string type);
}