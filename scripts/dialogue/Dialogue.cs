using System.Collections.Generic;
using Godot;
using GodotInk;

public partial class Dialogue : Control
{
	[Export] private InkStory story;
	TextureRect image;
	Label text;
	VBoxContainer optionContainer;
	public override void _Ready()
	{
		image = GetNode<TextureRect>("HBox/VBox/ImageContainer/Image");
		text = GetNode<Label>("HBox/VBox/Text");
		optionContainer = GetNode<VBoxContainer>("HBox/VBox/OptionContainer");
		story.BindExternalFunction("print", (string s)=>{GD.Print(s);});
		Continue();
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
		// var first = optionContainer.GetChild<Button>(0);
		// first.FocusMode = FocusModeEnum.All;
		// first.GrabFocus();
		// first.CallDeferred("grab_focus");
	}
}
