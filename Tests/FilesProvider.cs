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

        public static string ProvideCompoundConfig() =>
            "section.csv:\n" +
            "sampleInt;sampleString;sampleFloat\n" +
            "45;\"value\";2.45\n" +
            "34;\"value2\";2.75\n" +
            "section.anotherCsv:\n" +
            "sampleInt;sampleString;sampleFloat\n" +
            "66;\"value1\";1.45\n" +
            "67;\"value2\";6.42\n" +
            "32;\"value3\";3.75\n" +
            "section.json:\n" + ProvideJson();
    }
}