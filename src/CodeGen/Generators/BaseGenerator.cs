using System.Text;
using CodeGen.Helpers;

namespace CodeGen.Generators;

public abstract class BaseGenerator : ICodeGenerator
{
    public abstract string TemplateName { get; }

    public virtual string Generate(dynamic model)
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

        if (model.IsStackResult)
        {
            builder.Replace("{{#if NET9_0_OR_GREATER}}", "\n" + "#if NET9_0_OR_GREATER" + "\n")
                   .Replace("{{#endif}}", "\n" + "#endif" + "\n");
        }
        else
        {
            builder.Replace("{{#if NET9_0_OR_GREATER}}", "")
                   .Replace("{{#endif}}", "");
        }

        return builder.ToString();
    }
}
