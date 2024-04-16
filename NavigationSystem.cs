using Godot;
using Godot.Collections;

public partial class NavigationSystem : Node
{
	private Rid nav_map;

	public float NeighborDistance;
	public int MaxNeighbors;
	public float TimeHorizon;
	public float TimeHorizonObst;
	public float Radius;
	public float MaxSpeed;
	public Vector2 Velocity;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		nav_map = NavigationServer2D.MapCreate();
		NavigationServer2D.MapSetActive(nav_map, true);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SetAgentDefaults(float neighborDist, int maxNeighbors,
					 float timeHorizon, float timeHorizonObst, float radius,
					 float maxSpeed, Vector2 velocity)
	{
		NeighborDistance = neighborDist;
		MaxNeighbors = maxNeighbors;
		TimeHorizon = timeHorizon;
		TimeHorizonObst = timeHorizonObst;
		Radius = radius;
		MaxSpeed = maxSpeed;
		Velocity = velocity;
	}

	public Rid AgentCreate(Vector2 position)
	{
		Rid rid = CreateAgent(NeighborDistance, MaxNeighbors, TimeHorizon, TimeHorizonObst, Radius, MaxSpeed, Velocity);
		NavigationServer2D.AgentSetPosition(rid, position);

		return rid;
	}

	public Rid CreateAgent(float neighborDist, int maxNeighbors,
					 float timeHorizon, float timeHorizonObst, float radius,
					 float maxSpeed, Vector2 velocity)
	{
		Rid rid = NavigationServer2D.AgentCreate();
		NavigationServer2D.AgentSetAvoidanceEnabled(rid, true);

		NavigationServer2D.AgentSetNeighborDistance(rid, neighborDist);
		NavigationServer2D.AgentSetMaxNeighbors(rid, maxNeighbors);
		NavigationServer2D.AgentSetTimeHorizonAgents(rid, timeHorizon);
		NavigationServer2D.AgentSetTimeHorizonObstacles(rid, timeHorizonObst);
		NavigationServer2D.AgentSetRadius(rid, radius);
		NavigationServer2D.AgentSetVelocity(rid, velocity);

		NavigationServer2D.AgentSetMap(rid, nav_map);
		NavigationServer2D.AgentSetMaxSpeed(rid, maxSpeed);

		return rid;
	}
	public Vector2 AgentGetVelocity(Rid rid)
	{
		return NavigationServer2D.AgentGetVelocity(rid);
	}
	public void AgentSetVelocity(Rid rid, Vector2 velocity)
	{
		NavigationServer2D.AgentSetVelocity(rid, velocity);
	}
	public Vector2 AgentGetPosition(Rid rid)
	{
		return NavigationServer2D.AgentGetPosition(rid);
	}
	public void AgentSetPosition(Rid rid, Vector2 position)
	{
		NavigationServer2D.AgentSetPosition(rid, position);
	}
	public float AgentGetMaxSpeed(Rid rid)
	{
		return NavigationServer2D.AgentGetMaxSpeed(rid);
	}
	public Rid ObstacleCreate(Vector2[] vertices)
	{
		Rid rid = NavigationServer2D.ObstacleCreate();
		NavigationServer2D.ObstacleSetVertices(rid, vertices);
		NavigationServer2D.ObstacleSetAvoidanceEnabled(rid, true);
		NavigationServer2D.ObstacleSetMap(rid, nav_map);

		return rid;
	}
}
