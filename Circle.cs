using Godot;
using Godot.Collections;

public partial class Circle : Node2D
{
	private NavigationSystem nav_system;
	private Array<Agent> agents = new Array<Agent>();
	private Array<Vector2> goals = new Array<Vector2>();

	public override void _Ready()
	{
		nav_system = GetNode<NavigationSystem>("/root/NavigationSystem");
		nav_system.SetAgentDefaults(15f, 10, 10f, 10f, 1.5f, 10, Vector2.Zero);

		for (int i = 0; i < 250; ++i) {
			Vector2 pos = new Vector2(Mathf.Cos(i*2f*Mathf.Pi/250f) * 100 + 192, Mathf.Sin(i*2f*Mathf.Pi/250f) * 100 + 108);
			Color color = new Color(GD.Randf(), GD.Randf(), GD.Randf());
			agents.Add(new Agent(nav_system.AgentCreate(pos), color));
			goals.Add(-pos);
		}
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
		foreach (Agent agent in agents) {
			Vector2 pos = nav_system.AgentGetPosition(agent.Rid);
			DrawCircle(pos, nav_system.Radius, agent.Color);
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
			nav_system.AgentSetVelocity(rid, goalVector);
		}
	}
}
