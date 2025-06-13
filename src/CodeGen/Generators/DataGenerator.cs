using System.Text;
using CodeGen.Helpers;

namespace CodeGen.Generators;

public class DataGenerator : ICodeGenerator
{
    public const string TemplateName = "Data.template.cs";

    public string Generate(dynamic model)
    {
        var template = TemplateManager.GetTemplate(TemplateName);

        var builder = new StringBuilder(template)
            .Replace("{{Modifier}}", model.Modifier)
            .Replace("{{ResultType}}", model.ResultType)
            .Replace("{{GENERATOR_VERSION}}", model.Version)
            .Replace("{{GENERATOR_VERSION}}", model.Version)
            .Replace("{{GENERATED_AT}}", model.GeneratedAt)
            .Replace("{{TEMPLATE_NAME}}", TemplateName)
            .Replace("{{RESULT_TYPE}}", model.ResultType)
            .Replace("{{TARGET_FRAMEWORK}}", model.TargetFramework);

        return builder.ToString();
    }
}
