
using Godot;

public partial class MainMenu: Menu{
	public override void _Ready()
	{
		base._Ready();
		GetNode<Button>("HBoxContainer/VBoxContainer/Launch").ButtonDown += ()=>{
			menuSystem.PushMenu("Dialogue");
		};
		GetNode<Button>("HBoxContainer/VBoxContainer/Options").ButtonDown += ()=>{
			menuSystem.PushMenu("Options");
		};
		GetNode<Button>("HBoxContainer/VBoxContainer/Quit").ButtonDown += ()=>{
			GetTree().Quit();
		};
	}
}
