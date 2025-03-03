using System.Collections.Generic;
using Godot;
using GodotInk;

public partial class Dialogue : Menu
{
	[Export] private InkStory story;
	TextureRect image;
	Label text;
	VBoxContainer optionContainer;
	bool firstWake = true;
	public override void _Ready()
	{
		base._Ready();
		image = GetNode<TextureRect>("HBox/VBox/ImageContainer/Image");
		text = GetNode<Label>("HBox/VBox/PanelContainer/Text");
		optionContainer = GetNode<VBoxContainer>("HBox/VBox/HBox/OptionContainer");
		story.BindExternalFunction("print", (string s)=>{GD.Print(s);});
		story.BindExternalFunction("set_menu", (string s)=>{menuSystem.PushMenu(s);});
		story.BindExternalFunction("launch", ()=>{menuSystem.Launch();});
		story.BindExternalFunction("quit", ()=>{GetTree().Quit();});
		// story.ChoosePathString()
		// story.StoreVariable();
		// story.FetchVariable();
		// Continue();
	}
	public override void OnWake()
	{
		base.OnWake();
		if(firstWake){
			firstWake = false;
			CallDeferred("Continue");
		}
	}
	public void SetWon(bool won){
		if (won) story.ChoosePathString("win");
		else story.ChoosePathString("lose");
		CallDeferred("Continue");
	}
	void Clear(){
		foreach (var c in optionContainer.GetChildren())
		{
			c.QueueFree();
		}
	}
	void Continue(){
		Clear();
		text.Text = story.Continue();
		List<Button> newButtons = new();
		if(story.CanContinue){
			var b = new Button{
				Text = "...",
			};
			b.Pressed += ()=>{Continue();};
			newButtons.Add(b);
			// optionContainer.AddChild(b);
		}
		else{
			foreach (var option in story.CurrentChoices)
			{
				var b = new Button{
					Text = option.Text,
				};
				b.Pressed += ()=>{
					story.ChooseChoiceIndex(option.Index);
					Continue();
				};
				newButtons.Add(b);
				// optionContainer.AddChild(b);
			}
		}
		foreach (var b in newButtons)
		{
			optionContainer.AddChild(b);
		}
		if(newButtons.Count > 0) newButtons[0].GrabFocus();
	}
}
