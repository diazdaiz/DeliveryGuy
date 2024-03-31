using Godot;
using System;

public partial class Model : Node2D {
    Movement movement;
    float targetScaleX;
    Direction scaleDirection;
    float scaleAcceleration;
    Sprite2D body;
    AnimationPlayer animationPlayer;

    public override void _Ready() {
        movement = GetNode<Movement>("../Movement");
        body = GetNode<Sprite2D>("Body");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        scaleDirection = Direction.Right;
        scaleAcceleration = 1f;
        targetScaleX = 0.1f;
    }

    public override void _Process(double delta) {
        body.RotationDegrees = Math.Abs(movement.Velocity.X) / movement.MaxSpeed.X * 8;
        int animationFrame = (int)(GlobalPosition.X / 4) % 2 + 1;
        animationPlayer.Play(animationFrame.ToString());
    }

    public override void _PhysicsProcess(double delta) {
        if (movement.Direction != scaleDirection) {
            if (movement.Direction == Direction.Right) {
                if (Scale.X + scaleAcceleration * (float)delta > targetScaleX) {
                    Scale = new Vector2(targetScaleX, Scale.Y);
                    scaleDirection = Direction.Right;
                }
                else {
                    Scale += new Vector2(scaleAcceleration, 0) * (float)delta;
                }
            }
            else {
                if (movement.Direction == Direction.Left) {
                    if (Scale.X - scaleAcceleration * (float)delta < -targetScaleX) {
                        Scale = new Vector2(-targetScaleX, Scale.Y);
                        scaleDirection = Direction.Left;
                    }
                    else {
                        Scale += new Vector2(-scaleAcceleration, 0) * (float)delta;
                    }
                }
            }
        }
    }
}
