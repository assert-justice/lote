using Godot;
using System.Collections.Generic;

public partial class MenuSystem : Control
{
	[Export] PackedScene GameScene;
	Stack<string> menuStack = new();
	Node2D gameHolder;
	public override void _Ready()
	{
		gameHolder = GetNode<Node2D>("GameHolder");
		menuStack.Push(GetChild(1).Name);
		SetMenu();
	}

	public override void _Process(double delta)
	{
	}
	void HideMenus(){
		foreach (var child in GetChildren())
		{
			if (child is Control c){
				c.Visible = false;
			}
		}
	}
	void SetMenu(){
		HideMenus();
		GetNode<Control>(menuStack.Peek()).Visible = true;
	}
	public void PushMenu(string menuName){
		menuStack.Push(menuName);
		SetMenu();
	}
	public string PopMenu(){
		var name = menuStack.Peek();
		if(menuStack.Count > 1) menuStack.Pop();
		SetMenu();
		return name;
	}
	public void Launch(){
		HideMenus();
		gameHolder.AddChild(GameScene.Instantiate());
	}
}
