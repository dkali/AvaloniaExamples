using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Controls.Shapes;
using SimpleToDoList.Models;
using Path = Avalonia.Controls.Shapes.Path;

namespace SimpleToDoList.Services;

public class ToDoListFileService
{
    private static string _jsonFileName = System.IO.Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "Avalonia.SimpleToDoList", "MyToDoList.txt");

    public static async Task SaveToFileAsync(IEnumerable<ToDoItem> itemsToSave)
    {
        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(_jsonFileName)!);

        using (var fs = File.Create(_jsonFileName))
        {
            await JsonSerializer.SerializeAsync(fs, itemsToSave);
        }
    }

    public static async Task<IEnumerable<ToDoItem>> LoadFromFileAsync()
    {
        try
        {
            using (var fs = File.OpenRead(_jsonFileName))
            {
                return await JsonSerializer.DeserializeAsync<IEnumerable<ToDoItem>>(fs);
            }
        }
        catch (Exception e) when (e is FileNotFoundException || e is DirectoryNotFoundException)
        {
            return null;
        }
    }
}