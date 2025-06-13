using System.IO;
using System.Reflection;

namespace CodeGen.Helpers;

public static class TemplateManager
{
    private static readonly string TemplateRoot =
        Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!, "Templates");

    public static string GetTemplate(string name)
    {
        var path = Path.Combine(TemplateRoot, name);
        return File.ReadAllText(path);
    }
}
