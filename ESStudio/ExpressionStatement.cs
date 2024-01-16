using System;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using Esprima.Ast;

namespace ESAnalyzer;

public static class ExpressionStatement
{
	private static string GetStringName(this Esprima.Ast.ExpressionStatement state)
	{
		return (state.Expression as Node).GetStringName();
	}


	private static string GetStringName(this VariableDeclaration state)
	{
		string result = string.Empty;
		for (var index = 0; index < state.Declarations.Count; index++)
		{
			result += state.Declarations[index].Id.ToString();
			if (index != state.Declarations.Count - 1)
				result += ",";
		}

		return result;
	}

	private static string GetStringName(this AssignmentExpression state)
	{
		return "expression";
	}

	private static string GetStringName(this UnaryExpression state)
	{
		return "expression";
	}

	private static string GetStringName(this FunctionDeclaration state)
	{
		return state.Id.ToString();
	}

	private static string GetStringName(this MemberExpression state)
	{
		return state.ToString();
	}

	public static string GetStringName(this Node state)
	{
		switch (state.Type)
		{
			case Nodes.ExpressionStatement:
				return ((state as Esprima.Ast.ExpressionStatement).Expression as Node).GetStringName();
			case Nodes.VariableDeclaration:
				return (state as Esprima.Ast.VariableDeclaration).GetStringName();
			case Nodes.FunctionDeclaration:
				return (state as Esprima.Ast.FunctionDeclaration).GetStringName();

			case Nodes.CallExpression:
				return (state as CallExpression).Callee.GetStringName();

			case Nodes.AssignmentExpression:
				return (state as AssignmentExpression).Left.ToString();

			case Nodes.UnaryExpression:
				return (state as UnaryExpression).Argument.GetStringName();

			case Nodes.FunctionExpression:
				if ((state as FunctionExpression).Id == null)
					return "Anonymous function";
				return (state as FunctionExpression).Id.Name;

			case Nodes.MemberExpression:
				return (state as MemberExpression).ToString();

			case Nodes.ArrowFunctionExpression:
				return "Arrow function";

			case Nodes.ClassDeclaration:
				return (state as ClassDeclaration).Id.Name;

			case Nodes.BlockStatement:
				return "BlockStatement";

			case Nodes.SequenceExpression:
				return "SequenceExpression";

			case Nodes.VariableDeclarator:
				return (state as VariableDeclarator).Id.GetStringName();

			case Nodes.Identifier:
				return (state as Identifier).Name;
			case Nodes.ObjectExpression:
				return "Object";
			
			default:
				return "aa";
				break;
		}
	}
}