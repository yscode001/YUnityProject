using System.Linq;
using System.Reflection;

#if !UNITY_EDITOR
using System.IO;
#endif

public static class HotUpdateAssembly
{
    private static bool IsInited = false;

    public static Assembly CfgForLogicAndEditor { get; private set; } = null;
    public static Assembly ProjectUtil { get; private set; } = null;
    public static Assembly Login { get; private set; } = null;
    public static Assembly Lobby { get; private set; } = null;

    public static void Init()
    {
        if (IsInited)
        {
            return;
        }
        IsInited = true;
#if UNITY_EDITOR
        // Editor下，已经被自动加载，不需要手动加载，重复加载反而会出问题。直接查找获得HotUpdate程序集。
        CfgForLogicAndEditor = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == ProjectPaths.Hybrid.CfgForLogicAndEditor);
        ProjectUtil = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == ProjectPaths.Hybrid.ProjectUtil);
        Login = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == ProjectPaths.Hybrid.Login);
        Lobby = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == ProjectPaths.Hybrid.Lobby);
#else
        CfgForLogicAndEditor = Assembly.Load(File.ReadAllBytes(ProjectPaths.Hybrid.CfgForLogicAndEditorFullPath));
        ProjectUtil = Assembly.Load(File.ReadAllBytes(ProjectPaths.Hybrid.ProjectUtilFullPath));
        Login = Assembly.Load(File.ReadAllBytes(ProjectPaths.Hybrid.LoginFullPath));
        Lobby = Assembly.Load(File.ReadAllBytes(ProjectPaths.Hybrid.LobbyFullPath));
#endif
    }
}