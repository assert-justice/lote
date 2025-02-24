using Godot;
using System.Collections.Generic;

public partial class Game : Node2D
{
	[ExportGroup("Vehicles")]
	[Export] PackedScene PlayerScene;
	[Export] PackedScene CopBikeScene;
	[Export] PackedScene CopCarScene;
	[Export] PackedScene GangsterWheelScene;
	[ExportGroup("PowerUps")]
	[Export] PackedScene ETankScene;
	Player player;
	List<float> rails = new();
	Pool<Entity> copBikePool;
	public override void _Ready()
	{
		base._Ready();
		int screenHeight = 1080;
		int numRails = 5;
		int stride = screenHeight/(numRails + 1);
		int y = stride;
		for (int i = 0; i < numRails; i++)
		{
			rails.Add(y);
			y += stride;
		}
		copBikePool = new(() =>
		{
			var b = CopBikeScene.Instantiate<CopBike>();
			return b;
		})
		{
			InitFn = b =>
			{
				AddChild(b);
			},
			FreeFn = b =>
			{
				RemoveChild(b);
			}
		};
		SpawnPlayer(100, 500);
		SpawnEnt(800, 500, copBikePool);
	}
	public override void _Draw()
	{
		base._Draw();
		foreach (var rail in rails)
		{
			DrawRect(new Rect2(0, rail, 1920, 5), Colors.DarkGray);
		}
	}
	void SpawnPlayer(float x, float y){
		player = PlayerScene.Instantiate<Player>();
		AddChild(player);
		player.Position = new Vector2(x, y);
		player.SetRails(rails);
	}
	Entity SpawnEnt(float x, float y, Pool<Entity> pool){
		var ent = pool.GetNew();
		ent.Position = new Vector2(x, y);
		return ent;
	}
}
