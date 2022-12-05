namespace Shared;

public static class ReadHelper
{
    public static IEnumerable<string> ReadAllLines(this StreamReader reader)
    {
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            yield return line;
        }
    }
}