namespace Data_Access_Layer.Common;

public class QueryParameter
{
    public QueryParameter(string name, object? value)
    {
        Name = name;
        Value = value;
    }

    public string Name { get; }
    public object? Value { get; }
}
