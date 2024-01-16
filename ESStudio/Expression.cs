using Esprima.Ast;

namespace ESAnalyzer;

public static class Expression
{
	public static string GetString(this Esprima.Ast.Expression expression)
	{
		switch (expression.Type)
		{
			case Nodes.CallExpression:
				return (expression as CallExpression).ToString().TrimComment();
			default:
				return "temp none";
		}
	}
}