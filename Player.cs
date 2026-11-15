using Godot;

public partial class Player : CharacterBody3D
{
	[Export]
	public AnimationTree anim_tree {get; set;} 
	
	//anim_tree.Active = true;
	// How fast the player moves in meters per second.
	[Export]
	public int Speed { get; set; } = 14;
	// The downward acceleration when in the air, in meters per second squared.
	[Export]
	public int JumpImpulse { get; set; } = 40;
	public int FallAcceleration { get; set; } = 75;

	private Vector3 _targetVelocity = Vector3.Zero;

	public override void _PhysicsProcess(double delta)
	{
		var direction = Vector3.Zero;

		if (Input.IsActionPressed("move_right"))
		{
			direction.Z -= 1.0f;
			anim_tree.Set("parameters/Transition/transition_request", "run");
			 
		}
		
		if(Input.IsAnythingPressed() == false){
			anim_tree.Set("parameters/Transition/transition_request", "idle");
		}
		
		if (Input.IsActionPressed("move_left"))
		{
			direction.Z += 1.0f;
			anim_tree.Set("parameters/Transition/transition_request", "run");
		}
		
		if (IsOnFloor() && Input.IsActionJustPressed("jump"))
		{
	   		_targetVelocity.Y = JumpImpulse;
		}


		if (direction != Vector3.Zero)
		{
			direction = direction.Normalized();
			// Setting the basis property will affect the rotation of the node.
			GetNode<Node3D>("Pivot").Basis = Basis.LookingAt(direction);
		}

		// Ground velocity
		_targetVelocity.X = direction.X * Speed;
		_targetVelocity.Z = direction.Z * Speed;

		// Vertical velocity
		if (!IsOnFloor()) // If in the air, fall towards the floor. Literally gravity
		{
			_targetVelocity.Y -= FallAcceleration * (float)delta;
		}

		// Moving the character
		Velocity = _targetVelocity;
		MoveAndSlide();
	}
}
