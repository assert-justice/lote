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
	MenuSystem menuSystem;
	public override void _Ready()
	{
		base._Ready();
		var temp = GetTree().GetNodesInGroup("MenuSystem");
		if(temp.Count > 0) menuSystem = temp[0] as MenuSystem;
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
				menuSystem.Lose();
				QueueFree();
			}
		}
		if(copBikePool.pool.CountAlive() == 0){
			menuSystem.Win();
			QueueFree();
		}
	}
	public void Start(){
		SpawnPlayer(100, 500);
		SpawnEnt(2500, 500, copBikePool);
		SpawnEnt(3000, 800, copBikePool);
		SpawnEnt(3500, 300, copBikePool);
		SpawnEnt(4000, 100, copBikePool);
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
