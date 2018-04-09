using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models.Documents
{
    public interface IDocumentObject
    {
        string Id { get; set; }
        string Etag { get; set; }
    }
}
