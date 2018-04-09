using backend.Models.Documents;

namespace backend.Interfaces.Database
{
    public interface IDocumentSerializer
    {
        UserObject Deserialize(DatabaseObject src);

        DatabaseObject Serialize(UserObject src);
    }
}
