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
	readonly List<float> rails = new();
	EntPool copBikePool;
	EntPool playerPool;
	int lives = 3;
	Label hudLabel;
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
		copBikePool = new(this, ()=>{return CopBikeScene.Instantiate<CopBike>();});
		playerPool = new(this, ()=>{return PlayerScene.Instantiate<Player>();});
		hudLabel = GetNode<Label>("Hud/Label");
		Start();
	}
	public override void _Draw()
	{
		base._Draw();
		foreach (var rail in rails)
		{
			DrawRect(new Rect2(0, rail, 1920, 5), Colors.DarkGray);
		}
	}
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		foreach (var p in playerPool.GetAlive())
		{
			if(p is Player player){
				// Update hud
				hudLabel.Text = $"Health: {player.Health}x{lives}\nAmmo: {player.GetAmmoText()}";
			}
		}
		if(playerPool.pool.CountAlive() == 0){
			if(lives > 0){
				lives--;
				SpawnPlayer(100, 500);
			}
			else{
				// End game
			}
		}
	}
	public void Start(){
		SpawnPlayer(100, 500);
		// SpawnEnt(800, 500, copBikePool);
		lives = 3;
	}
	void SpawnPlayer(float x, float y){
		var player = playerPool.GetNew() as Player;
		player.Position = new Vector2(x, y);
		player.SetRails(rails);
	}
	static Entity SpawnEnt(float x, float y, EntPool pool){
		var ent = pool.GetNew();
		ent.Position = new Vector2(x, y);
		return ent;
	}
}
