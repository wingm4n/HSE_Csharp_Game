using Godot;

public partial class Player : CharacterBody2D
{
    [Export] public float Speed { get; set; } = 300.0f;
    [Export] public PackedScene BulletScene { get; set; }
    [Export] public int MaxHealth { get; set; } = 3;

    [Signal] public delegate void HealthChangedEventHandler(int health);
    [Signal] public delegate void PlayerDiedEventHandler();

    private int _health;
    private float _shootCooldown = 0.0f;
    private const float ShootDelay = 0.2f;

    public override void _Ready()
    {
        _health = MaxHealth;
        //EmitSignal(SignalName.HealthChanged, _health);
    }

    public override void _Process(double delta)
    {
        // Обработка стрельбы
        _shootCooldown -= (float)delta;

        /*if (Input.IsActionPressed("shoot") && _shootCooldown <= 0)
        {
            //Shoot();
            _shootCooldown = ShootDelay;
        }*/
    }

    public override void _PhysicsProcess(double delta)
    {
        // Обработка движения
        Vector2 direction = Input.GetVector("move_left", "move_right", "ui_up", "ui_down");
        Velocity = direction * Speed;

        MoveAndSlide();

        // Ограничиваем движение в пределах экрана
        Vector2 viewportSize = GetViewportRect().Size;
        Vector2 position = Position;

        position.X = Mathf.Clamp(position.X, 0, viewportSize.X);
        position.Y = Mathf.Clamp(position.Y, 0, viewportSize.Y);

        Position = position;
    }

    /*private void Shoot()
    {
        if (BulletScene != null)
        {
            var bullet = BulletScene.Instantiate<Bullet>();
            GetParent().AddChild(bullet);
            bullet.Position = Position + new Vector2(0, -30); // Позиция перед кораблем
            bullet.Launch(new Vector2(0, -1)); // Направление вверх
        }
    }*/

    public void TakeDamage()
    {
        _health--;
        EmitSignal(SignalName.HealthChanged, _health);

        if (_health <= 0)
        {
            Die();
        }
        else
        {
            // Визуальный эффект получения урона
            var tween = CreateTween();
            tween.TweenProperty(this, "modulate", Colors.Red, 0.1f);
            tween.TweenProperty(this, "modulate", Colors.White, 0.1f);
        }
    }

    private void Die()
    {
        //EmitSignal(SignalName.PlayerDied);
        QueueFree();
    }
}

