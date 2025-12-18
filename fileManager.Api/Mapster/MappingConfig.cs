namespace fileManager.Api.Mapster
{
    public static class MappingConfig
    {
        public static void ConfigureMappings()
        {
           PersonalDataMap.Configure();
           PersonalDocsMap.Configure();
        }
    }
}
