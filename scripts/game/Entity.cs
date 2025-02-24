using System;
using System.Collections.Generic;
using Godot;

public partial class Entity : Node2D{
	public Vector2 Velocity = new();
	public float Health = 100;
	[Export] public float MaxHealth = 100;
	public Pool<Entity> Pool;
	List<Clock> clocks = new();
	List<EntPool> pools = new();
	public override void _Ready()
	{
		base._Ready();
	}
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		float dt = (float)delta;
		Position += Velocity * dt;
		foreach (var c in clocks)
		{
			c.Update(dt);
		}
	}
	public virtual void Init(){
		Health = MaxHealth;
		Velocity = Vector2.Zero;
		foreach (var c in clocks)
		{
			c.Reset();
		}
	}

	public virtual void Damage(float damage){
		Health -= damage;
	}
	public virtual void Die(){
		Pool.Free(this);
	}
	public Clock AddClock(float fullDuration, float duration = -1){
		Clock c = new(fullDuration, duration);
		clocks.Add(c);
		return c;
	}
	public EntPool AddPool(Node parent, Func<Entity> fn){
		var pool = new EntPool(parent, fn);
		pools.Add(pool);
		return pool;
	}
}
