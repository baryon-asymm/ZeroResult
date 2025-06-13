using System;
using System.IO;
using System.Linq;
using System.Reflection;
using CodeGen.Generators;

try
{
    var repoRoot = FindRepositoryRoot();
    var outputRoot = Path.Combine(repoRoot.FullName, "src/ZeroResult/Core/Models");
    var version = GetVersion();

    Console.WriteLine($"Repository root: {repoRoot}");
    Console.WriteLine($"Output directory: {outputRoot}");

    GenerateStackResult(outputRoot, version);
    GenerateHeapResult(outputRoot, version);
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Generation failed: {ex}");
}

static DirectoryInfo FindRepositoryRoot()
{
    var executableParentDir = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!);
    Console.WriteLine($"Executable dir: {executableParentDir}");

    for (var dir = executableParentDir; dir != null; dir = dir.Parent)
    {
        if (dir.GetFiles("ZeroResult.sln").Any())
        {
            Console.WriteLine($"Found repo root: {dir}");
            return dir;
        }

        Console.WriteLine($"Not repo root: {dir}");
    }

    throw new Exception($"Unable to find repository root in directory hierarchy: {executableParentDir}");
}

static void GenerateStackResult(string outputRoot, string version)
{
    var model = new
    {
        Modifier = "readonly ref",
        ResultType = "StackResult",
        IsStackResult = true,
        Version = version,
        GeneratedAt = DateTime.UtcNow.ToString("O"),
        TargetFramework = "net8.0+"
    };

    Generate<OperationsGenerator>("Operations", outputRoot, model);
    Generate<AccessorsGenerator>("Accessors", outputRoot, model);
    Generate<DataGenerator>("Data", outputRoot, model);
    Generate<ResultCreateGenerator>("StackResult.Create", outputRoot, model);
}

static void GenerateHeapResult(string outputRoot, string version)
{
    var model = new {
        Modifier = "readonly",
        ResultType = "HeapResult",
        IsStackResult = false,
        Version = version,
        GeneratedAt = DateTime.UtcNow.ToString("O"),
        TargetFramework = "net8.0+"
    };

    Generate<OperationsGenerator>("ResultOperations", outputRoot, model);
    Generate<AccessorsGenerator>("Accessors", outputRoot, model);
    Generate<DataGenerator>("Data", outputRoot, model);
    Generate<ResultCreateGenerator>("HeapResult.Create", outputRoot, model);
}

static void Generate<T>(string component, string outputRoot, dynamic model) 
    where T : ICodeGenerator, new()
{
    try
    {
        var generator = new T();
        var outputPath = Path.Combine(outputRoot, model.ResultType, $"{component}.g.cs");
        Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);

        Console.WriteLine($"Generating {model.ResultType}.{component}...");
        var generatedCode = generator.Generate(model);

        File.WriteAllText(outputPath, generatedCode);
        Console.WriteLine($"Successfully generated: {outputPath}");
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Error generating {model.ResultType}.{component}: {ex}");
        throw;
    }
}

static string GetVersion()
{
    var assembly = Assembly.GetEntryAssembly()!;
    return assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion 
        ?? assembly.GetName().Version?.ToString() 
        ?? "1.0.0";
}
