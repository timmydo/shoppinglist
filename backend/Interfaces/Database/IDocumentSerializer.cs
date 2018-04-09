using backend.Models.Documents;

namespace backend.Interfaces.Database
{
    public interface IDocumentSerializer
    {
        T Deserialize<T>(DatabaseObject src) where T : IDocumentObject;

        DatabaseObject Serialize<T>(T src) where T : IDocumentObject;
    }
}
