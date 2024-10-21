using System.IO;

namespace WPF_MVVM_TEMPLATE.Infrastructure;

public class FileService
{
    /// <summary>
    /// Method for validating if a directory exists
    /// </summary>
    /// <param name="directory"> the absolute path to the directory </param>
    /// <returns></returns>
    public bool IsDirectoryAvailable(string directory)
    {
        return Directory.Exists(directory);
    }
    
    
    /// <summary>
    /// Method for validating that a directory contains files. 
    /// </summary>
    /// <param name="directory"> the absolute path to the directory  </param>
    /// <returns></returns>
    /// <exception cref="DirectoryNotFoundException"> thrown if directory does not exist </exception>
    public bool DirectoryContainsFiles(string directory)
    {
        if (IsDirectoryAvailable(directory))
        {
            return Directory.GetFiles(directory).Any();
        }
        
        throw new DirectoryNotFoundException();
        
    }
    
    /// <summary>
    /// Method for fetching the path of all files in a directory.
    /// </summary>
    /// <param name="directory"> the absolute path to the directory </param>
    /// <returns></returns>
    /// <exception cref="DirectoryNotFoundException"> thrown if directory does not exist </exception>
    /// <exception cref="FileNotFoundException"> thrown if directory does not contain files </exception>
    public List<string> GetFiles(string directory)
    {
        
        if (!IsDirectoryAvailable(directory)) throw new DirectoryNotFoundException();
        if (!DirectoryContainsFiles(directory)) throw new FileNotFoundException();
        
        return Directory.GetFiles(directory).ToList();
        
    }
    
    
    /// <summary>
    /// Method for comparing the file extensions of a file
    /// </summary>
    /// <param name="filename"> filename with extensions</param>
    /// <param name="type"> expected extensions ex. "xml"</param>
    /// <returns></returns>
    public bool FileIsOfType(string filename, string type)
    {
        var splitFile = filename.Split(".");
        return splitFile.Length != 0 && splitFile[splitFile.Length - 1].Equals(type);
    }
}