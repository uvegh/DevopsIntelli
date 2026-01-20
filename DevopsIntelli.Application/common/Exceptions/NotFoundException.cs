

namespace DevopsIntelli.Application.common.Exceptions;

public class NotFoundException:Exception
{
    public string EntityName;
    public Object Key;
    public NotFoundException(string entityName, object key) : base($"{entityName} with {key} not found")
    {
        EntityName = entityName;
        Key = key;

    }
}
