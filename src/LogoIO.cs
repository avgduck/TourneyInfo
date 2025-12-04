using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace TourneyInfo;

internal static class LogoIO
{
    private static DirectoryInfo LogoDirectoryDefault;
    private static DirectoryInfo LogoDirectoryCustom;

    internal static Texture2D LogoDefault { get; private set; }
    internal static Texture2D LogoCustom { get; private set; }
    
    internal static void Init()
    {
        string defaultPath = Path.Combine(Path.GetDirectoryName(Plugin.Instance.Info.Location), "assets");
        if (!Directory.Exists(defaultPath)) Directory.CreateDirectory(defaultPath);
        LogoDirectoryDefault = new DirectoryInfo(defaultPath);
        
        DirectoryInfo moddingFolder = LLBML.Utils.ModdingFolder.GetModSubFolder(Plugin.Instance.Info);
        string customPath = moddingFolder.FullName;
        if (!Directory.Exists(customPath)) Directory.CreateDirectory(customPath);
        LogoDirectoryCustom = new DirectoryInfo(customPath);
        
        UpdateLogoDefault();
        UpdateLogoCustom();
    }
    
    private static void CopyStream(Stream input, Stream output)
    {
        byte[] buffer = new byte[8 * 1024];
        int len;
        while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
        {
            output.Write(buffer, 0, len);
        }
    }

    private static Texture2D LoadImageFile(FileInfo file)
    {
        Plugin.LogGlobal.LogInfo($"Loading image file: {file.FullName}");
        using FileStream fileStream = file.OpenRead();
        using MemoryStream memoryStream = new MemoryStream();
        {
            CopyStream(fileStream, memoryStream);
            Texture2D tex = new Texture2D(1, 1);
            tex.LoadImage(memoryStream.ToArray());
            return tex;
        }
    }

    private static void UpdateLogoDefault()
    {
        Plugin.LogGlobal.LogInfo($"Loading default logo file from path: {LogoDirectoryDefault.FullName}");
        IOrderedEnumerable<FileInfo> files = LogoDirectoryDefault.GetFiles().OrderBy(f => f.Name);
        if (!files.Any())
        {
            Plugin.LogGlobal.LogWarning("No default logo file exists");
            LogoDefault = null;
            return;
        }

        FileInfo file = files.First();
        Texture2D tex = LoadImageFile(file);
        LogoDefault = tex;
    }

    internal static void UpdateLogoCustom()
    {
        Plugin.LogGlobal.LogInfo($"Loading custom logo file from path: {LogoDirectoryCustom.FullName}");
        IOrderedEnumerable<FileInfo> files = LogoDirectoryCustom.GetFiles().OrderBy(f => f.Name);
        if (!files.Any())
        {
            Plugin.LogGlobal.LogWarning("No custom logo file exists");
            LogoCustom = null;
            return;
        }

        FileInfo file = files.First();
        Texture2D tex = LoadImageFile(file);
        LogoCustom = tex;
    }
}