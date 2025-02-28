
using Godot;

public partial class OptionsMenu: Menu{
	public override void _Ready()
	{
		base._Ready();
		GetNode<Button>("HBoxContainer/VBoxContainer/Back").ButtonDown += ()=>{
			menuSystem.PopMenu();
		};
	}
}
