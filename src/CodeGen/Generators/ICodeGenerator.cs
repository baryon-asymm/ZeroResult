namespace CodeGen.Generators;

public interface ICodeGenerator
{
    string Generate(dynamic model);
}
