using Godot;
using System;

public partial class Bullet : Entity
{
	// public Pool<Bullet> Pool;
	// public Vector2 Velocity;
	public override void _Draw()
	{
		base._Draw();
		DrawCircle(Vector2.Zero, 10, Colors.Orange);
	}
	public override void _Ready()
	{
		base._Ready();
	}

	public override void _Process(double delta)
	{
		float dt = (float)delta;
		Position += Velocity * dt;
		if(GlobalPosition.X < 0 || GlobalPosition.X > 1920 || GlobalPosition.Y < 0 || GlobalPosition.Y > 1080){
			Die();
		}
	}

	public override void Die(){
		base.Die();
	}
}
