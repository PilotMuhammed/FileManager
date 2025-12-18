using fileManager.Api.Dtos.PersonalDocuments;
using fileManager.Api.Entities;
using Mapster;

namespace fileManager.Api.Mapster
{
    public class PersonalDataMap
    {
        public static void Configure()
        {
            TypeAdapterConfig<PersonalDocument, PersonalDocsDTO>.NewConfig();
        }
    }
}
