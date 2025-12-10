using Godot;
using Godot.NativeInterop;
using System;
using System.Reflection.Metadata;
using System.Threading.Tasks;

public partial class Rabbit : CharacterBody2D
{
	public const float Speed = 100.0f;
	public const float ExploisonRadius = 100.0f;
	public const int ExploisonDamage = 15;

	private bool chaseState = false;

	private bool isExploiding = false;

	private CollisionShape2D _collisionShape;
	
	[Export] public AnimatedSprite2D RabbitAnim;
	[Export] public Player _player;
	
	public override void _Ready()
	{
        AddToGroup("enemies");
        Area2D _detect = GetNode<Area2D>($"./Detector");
		_detect.Connect(Area2D.SignalName.BodyEntered, Callable.From<Node>(OnBodyEntered));

		Area2D _detect_boom = GetNode<Area2D>($"./Detector_BOOM");
		_detect_boom.Connect(Area2D.SignalName.BodyEntered, Callable.From<Node>(OnBoomEntered));

		//Area2D _detect_player = GetNode<Area2D>($"../Player");
		//_detect_player.Connect(Area2D.SignalName.BodyEntered, Callable.From<Node>(OnBoomEntered));
		
		_player = GetTree().Root.FindChild("Player", true, false) as Player;

        _collisionShape = GetNode<CollisionShape2D>($"./CollisionShape2D");
        RabbitAnim.Play("move");
	}
	public override void _PhysicsProcess(double delta)
	{
		if (isExploiding) return;

		CharacterBody2D _player = GetNode<CharacterBody2D>($"../Player");
		
		Vector2 velocity = Velocity;

		Vector2 direction;
		direction.X = Math.Sign(_player.Position.X - this.Position.X);
		direction.Y = Math.Sign(_player.Position.Y - this.Position.Y);
		if ((direction != Vector2.Zero) && (chaseState))
		{
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;
		}

		if ((direction.X< 0) || (Math.Abs(_player.Position.X - this.Position.X) < 10))
		{
			RabbitAnim.FlipH = true;
		}
		else
		{
			RabbitAnim.FlipH = false;
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	private void OnBodyEntered(Node body)
	{
		if (!(body is CharacterBody2D))
		{
			return;
		}

		if (body.Name == "Player")
		{
			chaseState = true;
		}
	}
	private void OnBoomEntered(Node body)
	{
		if (!(body is CharacterBody2D))
		{
			return;
		}

		if (body.Name == "Player")
        {
            _player.BunnyTakeDamage();
			isExploiding = true;
			Death();
		}
	}

	public async void Death()
    {
        RabbitAnim.Play("death");
        _collisionShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        await ToSignal(RabbitAnim, "animation_finished");
		QueueFree();
    }

    public void VarDeath(Vector2 coord, float radius)
    {
		if (coord.DistanceTo(GlobalPosition) < radius)
		{
			this.Death();
		}
    }


}
