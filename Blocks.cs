using Godot;
using Godot.Collections;

public partial class Agent: GodotObject
{
	public Rid Rid;
	public Color Color;

	public Agent(Rid rid, Color color)
	{
		Rid = rid;
		Color = color;
	}
}
public partial class Goal: GodotObject
{
	public Vector2 Position;
	public Color Color;
}
public partial class Obstacle: GodotObject
{
	public Vector2[] Obstcales;
	public Rid Rid;
}

public partial class Blocks : Node2D
{
	private Array<Vector2> goals = new Array<Vector2>();
	private Array<Goal> draw_goals = new Array<Goal>();
	private Array<Agent> agents = new Array<Agent>();
	private Array<Obstacle> obstacles = new Array<Obstacle>();
	private NavigationSystem nav_system;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		nav_system = GetNode<NavigationSystem>("/root/NavigationSystem");
		//nav_system.SetAgentDefaults(15f, 10, 5f, 5f, 2f, 10, Vector2.Zero);
		nav_system.SetAgentDefaults(15f, 10, 10f, 5f, 2f, 20, Vector2.Zero);

		for (int i = 0; i < 5; ++i) {
			for (int j = 0; j < 5; ++j) {
				agents.Add(new Agent(nav_system.AgentCreate(new Vector2(255.0f + i * 10.0f,  165.0f + j * 10.0f)), Colors.Red));
				goals.Add(new Vector2(125, 125));

				agents.Add(new Agent(nav_system.AgentCreate(new Vector2(145.0f - i * 10.0f,  165.0f + j * 10.0f)), Colors.Green));
				goals.Add(new Vector2(275.0f, 125.0f));

				agents.Add(new Agent(nav_system.AgentCreate(new Vector2(255.0f + i * 10.0f, 55.0f - j * 10.0f)), Colors.Blue));
				goals.Add(new Vector2(125.0f, 200.0f));

				agents.Add(new Agent(nav_system.AgentCreate(new Vector2(145.0f - i * 10.0f, 55.0f - j * 10.0f)), Colors.Yellow));
				goals.Add(new Vector2(275.0f, 200.0f));
			}
		}
		draw_goals.Add(new Goal { Position = new Vector2(125.0f, 125.0f), Color = Colors.Red });
		draw_goals.Add(new Goal { Position = new Vector2(275.0f, 125.0f), Color = Colors.Green });
		draw_goals.Add(new Goal { Position = new Vector2(125.0f, 200.0f), Color = Colors.Blue });
		draw_goals.Add(new Goal { Position = new Vector2(275.0f, 200.0f), Color = Colors.Yellow });

		Vector2[] obstacle1 = new Vector2[4] {new Vector2(190, 100), new Vector2(200, 100), new Vector2(200, 110), new Vector2(190, 110)};
		Rid rid = nav_system.ObstacleCreate(obstacle1);
		obstacles.Add(new Obstacle() { Obstcales = obstacle1, Rid = rid });
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 velocity;
		Vector2 pos;
		foreach (Agent agent in agents) {
			velocity = nav_system.AgentGetVelocity(agent.Rid);
			pos = nav_system.AgentGetPosition(agent.Rid);
			pos = pos + velocity * (float)delta;
			nav_system.AgentSetPosition(agent.Rid, pos);
		}
		SetPreferredVelocities();
		QueueRedraw();
	}
    public override void _Draw()
    {
		foreach (Obstacle obstacle in obstacles) {
			DrawColoredPolygon(obstacle.Obstcales, Colors.Black);
		}
		foreach (Agent agent in agents) {
			Vector2 pos = nav_system.AgentGetPosition(agent.Rid);
			DrawCircle(pos, nav_system.Radius, agent.Color);
		}
		foreach (Goal goal in draw_goals) {
			DrawRect(new Rect2(goal.Position - new Vector2(10, 10), new Vector2(20, 20)), goal.Color, false);
		}
    }
	private void SetPreferredVelocities()
	{
		for (int i = 0; i < agents.Count; ++i) {
			Rid rid = agents[i].Rid;
			Vector2 goalVector = goals[i] - nav_system.AgentGetPosition(rid);
			if (goalVector.Length() > 1.0f) {
				goalVector = goalVector.Normalized() * nav_system.MaxSpeed;
			}
			//nav_system.AgentSetVelocity(rid, goalVector * nav_system.MaxSpeed);
			//Perturb a little to avoid deadlocks due to perfect symmetry
			float angle = GD.Randf() * 2.0f * Mathf.Pi/int.MaxValue;
			float dist = GD.Randf() * 0.0001f / int.MaxValue;

			Vector2 velocity = goalVector + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * dist;
			nav_system.AgentSetVelocity(rid, velocity);
		}
	}
}
