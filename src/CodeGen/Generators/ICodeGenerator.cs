namespace CodeGen.Generators;

public interface ICodeGenerator
{
    public string Generate(dynamic model);
}
