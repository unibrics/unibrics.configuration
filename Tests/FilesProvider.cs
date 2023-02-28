namespace Unibrics.Configuration.Tests
{
    public static class FilesProvider
    {
        public static string ProvideJson() => 
            "{" +
            " \"intValue\" : 12," +
            " \"stringValue\" : \"test\"" +
            "}";

        public static string ProvideCsv() =>
            "sampleInt;sampleString;sampleFloat\n" +
            "45;\"value\";2.45\n" +
            "34;\"value2\";2.75";
        
        public static string ProvideCsvWithMetadata() =>
            "metadata:\n" +
            "ab_test_name=test_test\n" +
            "apply=EveryTimeNoCache\n" +
            "values:\n" +
            "sampleInt;sampleString;sampleFloat\n" +
            "45;\"value\";2.45\n" +
            "34;\"value2\";2.75";
    }
}