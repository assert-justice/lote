using System;
using Godot;

public partial class CopBike : Entity
{
	[Export] private PackedScene BulletScene;
	[Export] private float FireTime = 0.3f;
	[Export] private float BulletSpeed = 500;
	[Export] private float Speed = 100;
	EntPool bulletPool;
	Clock fireClock;
	GunArm gunArm;
	Player player;
	Node2D pivot;
	Vector2 pivotPos;
	Clock damageClock;
	Area2D area2D;
	public override void _Ready()
	{
		base._Ready();
		Team = 1;
		gunArm = GetNode<GunArm>("Pivot/GunArm");
		pivot = GetNode<Node2D>("Pivot");
		pivotPos = pivot.Position;
		area2D = GetNode<Area2D>("Pivot/Area2D");
		area2D.AreaEntered += a => {
			if(a.GetParent() is Bullet b && b.Team != Team){
				CallDeferred("Damage", b.DamageVal);
			}
		};
		bulletPool = AddPool(GetParent(), ()=>{return BulletScene.Instantiate<Bullet>();});
		fireClock = AddClock(FireTime);
		fireClock.Timeout = ()=>{
			var b = bulletPool.GetNew();
			b.Team = Team;
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
    public override void Init()
    {
        base.Init();
		Velocity = Vector2.Left * Speed;
    }

    public override void _PhysicsProcess(double delta)
	{
		gunArm.Rotation = (player.GetHitboxPosition() - gunArm.GlobalPosition).Angle();
		base._PhysicsProcess(delta);
		if(Position.X < 200){
			Velocity = Vector2.Right * Speed;
		}
		if(Position.X > 1700){
			Velocity = Vector2.Left * Speed;
		}
	}
	public override void Damage(float damage)
	{
		base.Damage(damage);
		// Randomize pivot angle/offset
		Vector2 posOffset = Vector2.FromAngle(GD.Randf() * 2 * (float)Math.PI);
		float angle = (GD.Randf() * 2 - 1) * (float)Math.PI/8;
		pivot.Position = pivotPos + posOffset;
		pivot.Rotation = angle;
		damageClock.Reset();
	}
}
