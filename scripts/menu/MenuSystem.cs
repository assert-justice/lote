using Godot;
using System.Collections.Generic;

public partial class MenuSystem : Control
{
	[Export] PackedScene GameScene;
	Stack<string> menuStack = new();
	Node2D gameHolder;
	AudioStreamPlayer audioStreamPlayer;
	Dialogue dialogue;
	public override void _Ready()
	{
		gameHolder = GetNode<Node2D>("GameHolder");
		dialogue = GetNode<Dialogue>("Dialogue");
		audioStreamPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
		Pause(true);
		menuStack.Push(GetChild(1).Name);
		SetMenu();
	}

	public override void _Process(double delta)
	{
		// if(!audioStreamPlayer.Playing)audioStreamPlayer.Play();
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
		Pause(false);
		if(gameHolder.GetChildCount() > 0) gameHolder.GetChild(0).QueueFree();
		gameHolder.AddChild(GameScene.Instantiate());
		// gameHolder.CallDeferred("AddChild", GameScene.Instantiate());
	}
	public bool IsPaused(){
		return GetTree().Paused;
	}
	public void Pause(bool shouldPause){
		if(IsPaused() == shouldPause) return;
		GetTree().Paused = !IsPaused();
		if(IsPaused()){
			PushMenu("Pause");
		}
		else{
			HideMenus();
		}
	}
	public void Win(){
		Pause(true);
		PushMenu("Dialogue");
		dialogue.SetWon(true);
	}
	public void Lose(){
		Pause(true);
		PushMenu("Dialogue");
		dialogue.SetWon(false);
	}
}
