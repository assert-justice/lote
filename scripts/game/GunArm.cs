using Godot;
using System;

public partial class GunArm : Node2D
{
    public override void _Draw()
    {
        base._Draw();
		DrawRect(new Rect2(0,0,100,20), Colors.Gray);
    }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
