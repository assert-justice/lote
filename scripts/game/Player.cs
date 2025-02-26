using Godot;
using System;
using System.Collections.Generic;

public partial class Player : Entity
{
	[Export] public float Speed = 300;
	[Export] public PackedScene BulletScene;
	[Export] public float Gravity = 10;
	[Export] public float JumpPower = 10;
	[Export] public float GripDistance = 10;
	[Export] float FireTime = 0.1f;
	Clock FireClock;
	float bulletSpeed = 1000;
	EntPool bulletPool;
	readonly PlayerInput playerInput = new();
	GunArm gunArm;
	List<float> rails = new();
	Area2D hitBox;
	float railOffset = 35;
	Clock InvulnClock;
	Clock ReloadClock;
	[Export] float InvulnTime = 1;
	[Export] float ReloadTime = 1;
	int magazineCapacity = 10;
	int magazine = 10;
	int bullets = 100;
	int bulletsCapacity = 100;
	public override void _Ready()
	{
		base._Ready();
		gunArm = GetNode<GunArm>("GunArm");
		hitBox = GetNode<Area2D>("Area2D");
		bulletPool = AddPool(GetParent(), () =>
			{
				return BulletScene.Instantiate<Bullet>();
			}
		);
		FireClock = AddClock(FireTime);
		InvulnClock = AddClock(InvulnTime);
		InvulnClock.Timeout = ()=>{Visible = true;};
		ReloadClock = AddClock(ReloadTime, 0);
		ReloadClock.Timeout = ()=>{
			int requested = magazineCapacity - magazine;
			if(requested > bullets) requested = bullets;
			bullets -= requested;
			magazine += requested;
		};
		var area2D = GetNode<Area2D>("Area2D");
		area2D.AreaEntered += a => {
			if(a.GetParent() is Bullet b && b.Team != Team){
				CallDeferred("Damage", b.DamageVal);
			}
		};
	}
	public override void Init()
	{
		base.Init();
		magazine = magazineCapacity;
		bullets = bulletsCapacity;
	}

	public override void _PhysicsProcess(double delta)
	{
		float dt = (float)delta;
		playerInput.Poll();
		Velocity.X = playerInput.GetMove() * Speed;
		Velocity.Y += Gravity;
		if(playerInput.IsJumping() && IsOnRail()) {
			Velocity.Y = -JumpPower / dt;
			Position = new Vector2(Position.X, Position.Y - GripDistance);
		}
		if (!playerInput.IsDropping()){
			SnapToRail();
		}
		if (playerInput.GetAim().Length() > 0) gunArm.Rotation = playerInput.GetAim().Angle();
		if (playerInput.IsReloading() && magazine < magazineCapacity && bullets > 0){
			ReloadClock.Reset();
		}
		bool canFire = FireClock.GetDuration() == 0 && ReloadClock.GetDuration() == 0 && magazine > 0;
		if (playerInput.IsFiring() && canFire){
			Fire();
		}
		if(InvulnClock.GetDuration() > 0) Visible = !Visible;
		base._PhysicsProcess(delta);
		if(Position.X < 0){
			Position = new Vector2(0, Position.Y);
		}
		if(Position.X > 1720){
			Position = new Vector2(1720, Position.Y);
		}
		if(Position.Y > 1080){
			Die();
			// TODO: add effects to show bike exploding
		}
	}
	void Fire(){
		FireClock.Reset();
		magazine--;
		var velocity = Vector2.FromAngle(gunArm.Rotation) * bulletSpeed;
		var bullet = bulletPool.GetNew();
		bullet.Velocity = velocity;
		bullet.GlobalPosition = gunArm.GlobalPosition;
	}
	void SnapToRail(){
		foreach (var rail in rails)
		{
			if(Math.Abs(Position.Y + railOffset - rail) < GripDistance){
				Position = new Vector2(Position.X, rail - railOffset);
				Velocity.Y = 0;
				break;
			}
		}
	}
	bool IsOnRail(){
		foreach (var rail in rails)
		{
			if(Math.Abs(Position.Y + railOffset - rail) < GripDistance){
				return true;
			}
		}
		return false;
	}
	public void SetRails(List<float> rails){
		this.rails = rails;
	}
	public Vector2 GetHitboxPosition(){
		return hitBox.GlobalPosition;
	}
	public override void Damage(float damage)
	{
		if(InvulnClock.GetDuration() > 0) return;
		InvulnClock.Reset();
		base.Damage(damage);
	}
	public string GetAmmoText(){
		return $"{magazine}/{bullets}";
	}
}
