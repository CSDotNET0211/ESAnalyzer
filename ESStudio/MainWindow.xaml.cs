using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Shell;
using AvalonDock.Converters;
using AvalonDock.Layout;
using Esprima;
using Esprima.Ast;
using Esprima.Utils;
using HL.Interfaces;
using HL.Manager;
using SoftCircuits.JavaScriptFormatter;

namespace ESAnalyzer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private ObservableCollection<JSItem> _jsAssemblyNodes = new ObservableCollection<JSItem>();

		private string tempCode =
			"function PopulateBag() {\n\t\t\t\tlet s = [];\n\t\t\t\tswitch (t.setoptions.bagtype) {\n\t\t\t\t\tcase \"total mayhem\":\n\t\t\t\t\t\tfor (let a = 0; a < 7; a++) s.push(e.cm.constants.minotypes[Math.floor(t.rng.nextFloat() * e.cm.constants.minotypes.length)]);\n\t\t\t\t\t\tbreak;\n\t\t\t\t\tcase \"classic\":\n\t\t\t\t\t\tfor (let a = 0; a < 7; a++) {\n\t\t\t\t\t\t\tlet a = Math.floor(t.rng.nextFloat() * (e.cm.constants.minotypes.length + 1));\n\t\t\t\t\t\t\t(a === t.lastGenerated || a >= e.cm.constants.minotypes.length) && (a = Math.floor(t.rng.nextFloat() * e.cm.constants.minotypes.length)), t.lastGenerated = a, s.push(e.cm.constants.minotypes[a])\n\t\t\t\t\t\t}\n\t\t\t\t\t\tbreak;\n\t\t\t\t\tcase \"pairs\": {\n\t\t\t\t\t\tconst a = [...e.cm.constants.minotypes];\n\t\t\t\t\t\tt.rng.shuffleArray(a), s = [a[0], a[0], a[0], a[1], a[1], a[1]], t.rng.shuffleArray(s);\n\t\t\t\t\t\tbreak\n\t\t\t\t\t}\n\t\t\t\t\tcase \"14-bag\":\n\t\t\t\t\t\ts = [...e.cm.constants.minotypes, ...e.cm.constants.minotypes], t.rng.shuffleArray(s);\n\t\t\t\t\t\tbreak;\n\t\t\t\t\tcase \"7-bag+oo\": {\n\t\t\t\t\t\tconst a = [...e.cm.constants.minotypes],\n\t\t\t\t\t\t\tn = [...e.cm.constants.minotypes.map((e => \"o\" === e ? \"oo\" : e))];\n\t\t\t\t\t\tt.rng.shuffleArray(a), t.rng.shuffleArray(n), s = [...a, ...n];\n\t\t\t\t\t\tbreak\n\t\t\t\t\t}\n\t\t\t\t\tdefault:\n\t\t\t\t\t\ts = [...e.cm.constants.minotypes], t.rng.shuffleArray(s)\n\t\t\t\t}\n\t\t\t\tt.bag.push(...s)\n\t\t\t}\n\t\t\treturn {\n\t\t\t\tPopulateBag: PopulateBag,\n\t\t\t\tPullFromBag: function () {\n\t\t\t\t\tfor (; t.bag.length < 14;) PopulateBag();\n\t\t\t\t\treturn t.bag.shift()\n\t\t\t\t}\n\t\t\t}";

		public MainWindow()
		{
			InitializeComponent();
			uxDockingManager.Theme = new AvalonDock.Themes.Vs2013DarkTheme();
			editor.ShowLineNumbers = true;
			editor.Options.ShowColumnRuler = true;
			ThemedHighlightingManager.Instance.SetCurrentTheme("VS2019_Dark");
			editor.SyntaxHighlighting = ThemedHighlightingManager.Instance.GetDefinitionByExtension(".js");

			var parser = new JavaScriptParser();
			var path = @"C:\Users\CSDotNET\Documents\override\tetr.io\js\tetrio.js%3Fhv=6edca436.uANQHF9wY.u3Dewr15Hn";
			var program = parser.ParseScript(File.ReadAllText(path));
			var option = new JsonDocumentOptions();
			option.MaxDepth = 300;

			RefreshAssemblyExplorer(program, _jsAssemblyNodes);
		}


		public void RefreshAssemblyExplorer(JSItem parent, Node parentNode)
		{
			parent.Children.Clear();

			if (parentNode.ChildNodes.Count() == 1)
			{
				switch (parentNode.ChildNodes.ToArray()[0].Type)
				{
					case Nodes.BlockStatement:
					case Nodes.ArrowFunctionExpression:
						RefreshAssemblyExplorer(parent, parentNode.ChildNodes.ToArray()[0]);
						return;
				}
			}

			foreach (var child in parentNode.ChildNodes)
			{
				//子供が1つかつ所定の

				parent.Children.Add(new JSItem(AST.GetASTName(child), AST.GetASTType(child), child));
				//	_jsAssemblyNodes.
				//	_jsAssemblyNodes.Add(parent);
			}

			parent.Change();

			//CTreeView.ItemsSource = null;
			//	CTreeView.ItemsSource = _jsAssemblyNodes;
		}

		private void RefreshAssemblyExplorer(Node node, ObservableCollection<JSItem> nodeData)
		{
			foreach (var child in node.ChildNodes)
			{
				nodeData.Add(new JSItem(AST.GetASTName(child), AST.GetASTType(child), child));
			}

			//	RefreshAssemblyExplorer(child, nodeData);
			CTreeView.ItemsSource = _jsAssemblyNodes;
		}

		public void SetCodeIntoEditor(string code)
		{
			JavaScriptFormatter formatter = new JavaScriptFormatter();
			code = formatter.Format(code);
			editor.Text = code;
		}

		private void ClickA(object sender, RoutedEventArgs e)
		{
			throw new System.NotImplementedException();
		}


		private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
		}


		private void ExecuteClickCommand(object parameter)
		{
			Console.WriteLine("excute");
		}

		private bool CanExecuteClickCommand(object parameter)
		{
			// Return true or false based on your conditions
			return true;
		}

		private void MenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			var layout = new LayoutAnchorable();
			layout.Title = "Extract strings";
			layout.ContentId = "ID_0001";
			//layout.WriteXml();

			layout.CanFloat = true;
			//layout.can
			var window = uxDockingManager.CreateFloatingWindow(layout, true);
			window.Show(); 
			
			
			var menuItem = (MenuItem)sender;
			Console.WriteLine(menuItem.Tag.ToString());
			var contextMenu = (ContextMenu)menuItem.Parent;
			var textBlock = (TextBlock)contextMenu.PlacementTarget;
			var clickedItem = (JSItem)textBlock.DataContext;
			//contextMenu.Focus();
			//textBlock.Focus();
			Console.WriteLine(clickedItem.Name);

			switch (menuItem.Tag.ToString())
			{
				case "ExtractStrings":
					var result = ExtractStrings(clickedItem.Node.ToJavaScriptString());
					foreach (var str in result)
					{
						Console.WriteLine(str);
					}

					break;
				case "RefreshChildren":
					RefreshAssemblyExplorer(clickedItem, clickedItem.Node);
					break;
			}
		}

		static string[] ExtractStrings(string input)
		{
			var regex = new Regex(@"'([^'\\]*(?:\\.[^'\\]*)*)'|""([^""\\]*(?:\\.[^""\\]*)*)""");
			var matches = regex.Matches(input);
			var result = new HashSet<string>();
			foreach (Match match in matches)
			{
				result.Add(match.Value);
			}

			return result.ToArray();
		}
	}

	public class JSItem : INotifyPropertyChanged
	{
		public JSItem(string name, string type, Node node)
		{
			Name = name;
			string basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
			Uri baseUri = new Uri(basePath);
			Uri imageUri = new Uri(baseUri, $"./net7.0-windows/Res/{type}.png");
			Icon = new BitmapImage(imageUri);
			Node = node;
			Children = new ObservableCollection<JSItem>();

			ClickCommand = new RelayCommand(ExecuteClickCommand);
			MenuCommand = new RelayCommand(ExecuteMenuCommand);
		}

		private void ExecuteClickCommand()
		{
			(Application.Current.MainWindow as MainWindow).SetCodeIntoEditor(Node.ToJavaScriptString());
		}

		private void ExecuteMenuCommand()
		{
			(Application.Current.MainWindow as MainWindow).RefreshAssemblyExplorer(this, Node);

			//	PropertyChanged(this, new PropertyChangedEventArgs("Children"));
		}

		public void Change()
		{
			OnPropertyChanged("Children");
			OnPropertyChanged("Node");
			OnPropertyChanged("Name");
		}


		public ICommand ClickCommand { get; set; }
		public ICommand MenuCommand { get; set; }

		public string Name { get; set; }
		public Node Node { get; set; }
		public BitmapImage Icon { get; set; }
		public ObservableCollection<JSItem> Children { get; set; } = new();


		public event PropertyChangedEventHandler? PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			if (PropertyChanged != null)
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
		{
			if (EqualityComparer<T>.Default.Equals(field, value)) return false;
			field = value;
			OnPropertyChanged(propertyName);
			return true;
		}
	}

	public class RelayCommand : ICommand
	{
		private Action _execute;

		public RelayCommand(Action execute)
		{
			_execute = execute;
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			_execute();
		}
	}
}