using System.IO;
using UnityEngine;

public static class Tools
{
    public static T ImportJson<T>(string path)
    {
        var jsonString =  ReadFromFIle(path);
        return JsonUtility.FromJson<T>(jsonString);
    }
    
    public static void ExportJson(object jsonObject, string path)
    {
        var jsonString = JsonUtility.ToJson(jsonObject);
        WriteToFile(path, jsonString);
    }
    
    private static string ReadFromFIle(string fileName)
    {
        var path = GetFilePath(fileName);
        if (File.Exists(path))
        {
            using var reader = new StreamReader(path);
            return reader.ReadToEnd();
        }

        Debug.LogWarning("File not found");
        return string.Empty;
    }
    
    private static void  WriteToFile(string fileName, string json)
    {
        var path = GetFilePath(fileName);
        var fileStream = new FileStream(path, FileMode.Create);

        using var writer = new StreamWriter(fileStream);
        writer.Write(json);
    }
    
    private static string GetFilePath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }
}