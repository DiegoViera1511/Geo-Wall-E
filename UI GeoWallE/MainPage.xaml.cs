using Interpreter;
using System.Text.RegularExpressions;
namespace UI_GeoWallE;

public partial class MainPage : ContentPage
{
	GSharpInterpreter GeoWallEInterpreter = new GSharpInterpreter();
	public int CountLines;
	public MainPage()
	{
		InitializeComponent();
		CountLines = 10;

	}

    void Code_TextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
		string newLines = e.NewTextValue;
		MatchCollection numberOfLines = Regex.Matches(newLines, @"\n");
		int count = numberOfLines.Count;
		if (count == 0)
		{
			CountLines = 10;
			Lines.Text = "1\n2\n3\n4\n5\n6\n7\n8\n9\n10\n";
            return;
		}
		if(count + 5 > CountLines)
		{
			for (int i = CountLines + 1; i < count + 5; i++)
			{
				Lines.Text += $"{i}\n";
			}
			CountLines += 5;
		}
	}

}


