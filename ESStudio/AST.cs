using System;
using System.Collections.Generic;
using System.Text.Json;
using Esprima.Ast;

namespace ESAnalyzer;

public class AST
{
	public static string GetASTName(Node node)
	{
		var str = node.GetStringName();
		//var str = node.ToString().TrimComment();
		return str;
	}

	public static string GetASTType(Node node)
	{
		switch (node.Type)
		{
			case Nodes.ExpressionStatement:
				return GetASTType((node as Esprima.Ast.ExpressionStatement).Expression as Node);
			case Nodes.FunctionDeclaration:
			case Nodes.ArrowFunctionExpression:
				return "function";

			case Nodes.CallExpression:
				return "call";
				return GetASTType((node as Esprima.Ast.CallExpression).Callee as Node);

			case Nodes.AssignmentExpression:
				return GetASTType((node as Esprima.Ast.AssignmentExpression).Left as Node);

			case Nodes.ClassDeclaration:
				return "class";

			case Nodes.MemberExpression:
				if ((node as Esprima.Ast.MemberExpression).Object != null)
					return "variable";
				//	else
				return "none";

			case Nodes.VariableDeclarator:
				return GetASTType((node as Esprima.Ast.VariableDeclarator).Id as Node);

			case Nodes.Identifier:
			case Nodes.VariableDeclaration:
				return "variable";

			default: return "none";
		}
	}

	public static List<Node> GetChildren(Node target, bool detail)
	{
		return null;
	}
}