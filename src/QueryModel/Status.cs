using LinqToDB.Mapping;

namespace QueryModel;

public enum Status
{
    [MapValue(Value = "Pending")]
    Pending,
    [MapValue(Value = "Approved")]
    Approved,
    [MapValue(Value = "Rejected")]
    Rejected
}
