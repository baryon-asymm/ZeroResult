using System.Text;
using CodeGen.Helpers;

namespace CodeGen.Generators;

public class AsyncOperationsGenerator : BaseGenerator
{
    public override string TemplateName => "AsyncOperations.template.cs";

    private const string AdditionalFragmentName = "AsyncOperations.template.cs";
    private const string AdditionalFragmentPath = "AdditionalFragments/" + AdditionalFragmentName;

    public override string Generate(dynamic model)
    {
        var template = TemplateManager.GetTemplate(TemplateName);

        var builder = new StringBuilder(template);

        builder.Replace("{{" + AdditionalFragmentPath + "}}", GetAdditionalFragment());

        builder.Replace("{{Modifier}}", model.Modifier)
            .Replace("{{ResultType}}", model.ResultType)
            .Replace("{{GENERATOR_VERSION}}", model.Version)
            .Replace("{{GENERATOR_VERSION}}", model.Version)
            .Replace("{{GENERATED_AT}}", model.GeneratedAt)
            .Replace("{{TEMPLATE_NAME}}", TemplateName)
            .Replace("{{RESULT_TYPE}}", model.ResultType)
            .Replace("{{TARGET_FRAMEWORK}}", model.TargetFramework);
        
        return builder.ToString();
    }
    
    private static string GetAdditionalFragment()
    {
        var additionalFragment = TemplateManager.GetTemplate(AdditionalFragmentPath);

        return new StringBuilder(additionalFragment)
            .Replace("{{TaskType}}", "ValueTask")
            .AppendLine()
            .AppendLine()
            .Append(additionalFragment)
            .Replace("{{TaskType}}", "Task")
            .ToString();
    }
}
