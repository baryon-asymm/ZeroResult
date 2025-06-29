using System.Text;
using CodeGen.Helpers;

namespace CodeGen.Generators;

public class ConversionsGenerator : BaseGenerator
{
    public override string TemplateName => "Conversions.template.cs";

    public override string Generate(dynamic model)
    {
        var template = TemplateManager.GetTemplate(TemplateName);

        var builder = new StringBuilder(template);

        if (model.IsStackResult)
        {
            builder.Replace("{{ASYNC_CONVERSIONS}}", string.Empty);
        }
        else
        {
            builder.Replace("{{ASYNC_CONVERSIONS}}", $"\n\n{GenerateAsyncConversions()}");
        }

        builder.Replace("{{AS_T_METHODS}}", $"\n\n{GenerateAsTMethod(model)}");

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

    private static string GenerateAsyncConversions() =>
        """
            /// <summary>
            /// Implicitly converts a <see cref="{{ResultType}}{T, TError}"/> to a completed <see cref="ValueTask"/> of <see cref="{{ResultType}}{T, TError}"/>.
            /// </summary>
            /// <param name="result">The result to convert</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator ValueTask<{{ResultType}}<T, TError>>({{ResultType}}<T, TError> result)
                => new(result);
            
            /// <summary>
            /// Implicitly converts a <see cref="{{ResultType}}{T, TError}"/> to a completed <see cref="Task"/> of <see cref="{{ResultType}}{T, TError}"/>.
            /// </summary>
            /// <param name="result">The result to convert</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator Task<{{ResultType}}<T, TError>>({{ResultType}}<T, TError> result)
                => Task.FromResult(result);
        """;

    private static string GenerateAsTMethod(dynamic model)
    {
        var targetType = model.IsStackResult ? "Result" : "StackResult";
        var sourceType = model.ResultType;

        return
        """
            /// <summary>
            /// Converts the current <see cref="{sourceType}{T, TError}"/> to <see cref="{targetType}{T, TError}"/>.
            /// </summary>
            /// <returns>
            /// A new <see cref="{targetType}{T, TError}"/> with the same success/failure state.
            /// </returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public {targetType}<T, TError> As{targetType}() 
                => IsSuccess 
                    ? new {targetType}<T, TError>(_value) 
                    : new {targetType}<T, TError>(_error);
        """
        .Replace("{sourceType}", sourceType)
        .Replace("{targetType}", targetType);
    }
}
