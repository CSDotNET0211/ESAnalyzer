namespace ESAnalyzer;

public static class Extension
{
	public static string TrimComment(this string str)
	{
		return str.Substring(str.IndexOf("*/")+1);
	}
}