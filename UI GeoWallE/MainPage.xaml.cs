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
		CountLines = 0;
		for(int i = 1; i <= 150; i++)
		{
			CountLines += 1;
			Lines.Text += $"{i}\n";
		}

	}

    void Code_TextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
		string newLines = e.NewTextValue;
		MatchCollection numberOfLines = Regex.Matches(newLines, @"\n");
		int count = numberOfLines.Count;
		if (count + 20 > CountLines)
		{
			for (int i = CountLines + 1; i < count + 30; i++)
			{
				Lines.Text += $"{i}\n";
			}
			CountLines += 30;
		}
	}

    void ButtonRun_Clicked(System.Object sender, System.EventArgs e)
    {
		Errors.Text = "";
		try
		{
			GeoWallEInterpreter.RunInterpreter(Code.Text);

			Graphics.DrawPrints.Prints = GSharpInterpreter.GraficsViewPrints;

			DrawZone.Invalidate();
		}
		catch(InterpreterErrors errors)
		{
			Errors.Text = errors.PrintError();
		}
		catch(Exception er)
		{
			Errors.Text = er.Message;
		}
    }

    void ButtonSave_Clicked(System.Object sender, System.EventArgs e)
    {
		//Pavlo
    }

    void ButtonClear_Clicked(System.Object sender, System.EventArgs e)
    {
		Code.Text = "";
    }

    void ButtonImport_Clicked(System.Object sender, System.EventArgs e)
    {
		//Pavlo
    }
}


