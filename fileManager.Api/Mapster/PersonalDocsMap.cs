using fileManager.Api.Dtos.PersonalData;
using fileManager.Api.Entities;
using Mapster;

namespace fileManager.Api.Mapster
{
    public class PersonalDocsMap
    {
        public static void Configure()
        {
            TypeAdapterConfig<PersonalData, PersonalDataGet>.NewConfig();
        }
    }
}
