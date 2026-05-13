using System.IO;
using UnityEngine;

public static partial class Paths
{
    public static class AB
    {
        public const string SourceDir = "Assets/Editor/ABRes/";
        public static readonly string BundleDir = Path.Combine("file://", Application.persistentDataPath, "ABRes");
    }
}
public static partial class Paths
{
    public static class Hybrid
    {
        public static readonly string CLRDir = Path.Combine(Application.persistentDataPath, "HrbridCLR");

        public const string Login = "Login";
        public static readonly string LoginFullPath = Path.Combine(CLRDir, $"{Login}.dll.bytes");

        public const string Lobby = "Lobby";
        public static readonly string LobbyFullPath = Path.Combine(CLRDir, $"{Lobby}.dll.bytes");
    }
}