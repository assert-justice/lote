using System;
using Godot;

public partial class CopBike : Entity
{
	[Export] private PackedScene BulletScene;
	[Export] private float FireTime = 0.3f;
	[Export] private float BulletSpeed = 500;
	EntPool bulletPool;
	Clock fireClock;
	GunArm gunArm;
	Player player;
	Node2D pivot;
	Vector2 pivotPos;
	Clock damageClock;
	public override void _Ready()
	{
		base._Ready();
		gunArm = GetNode<GunArm>("Pivot/GunArm");
		pivot = GetNode<Node2D>("Pivot");
		pivotPos = pivot.Position;
		bulletPool = AddPool(GetParent(), ()=>{return BulletScene.Instantiate<Bullet>();});
		fireClock = AddClock(FireTime);
		fireClock.Timeout = ()=>{
			var b = bulletPool.GetNew();
			b.GlobalPosition = gunArm.GlobalPosition;
			b.Velocity = Vector2.FromAngle(gunArm.Rotation) * BulletSpeed;
			fireClock.Reset();
		};
		damageClock = AddClock(1);
		damageClock.Timeout = ()=>{
			pivot.Position = pivotPos;
			pivot.Rotation = 0;
		};
		player = GetTree().GetNodesInGroup("Player")[0] as Player;
	}

	public override void _PhysicsProcess(double delta)
	{
		gunArm.Rotation = (player.GetHitboxPosition() - gunArm.GlobalPosition).Angle();
		base._PhysicsProcess(delta);
	}
	public override void Damage(float damage)
	{
		base.Damage(damage);
		// Randomize pivot angle/offset
		Vector2 posOffset = Vector2.FromAngle(GD.Randf() * 2 * (float)Math.PI);
		float angle = (GD.Randf() * 2 - 1) * (float)Math.PI/2;
		pivot.Position = pivotPos + posOffset;
		pivot.Rotation = angle;
	}
}
