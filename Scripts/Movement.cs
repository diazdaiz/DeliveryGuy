using Godot;
using System;
using System.Collections.Generic;

public partial class Movement : Node2D {
    Node2D Robot;
    enum TryMoving { Right, Left, None }
    List<TryMoving> tryMovingActions;
    public Direction Direction { get; private set; }
    public Vector2 MaxSpeed { get; set; }
    public Vector2 Acceleration { get; set; }
    public Vector2 Velocity { get; private set; }
    bool hitLeftBorder;
    bool hitRightBorder;

    public void SetMovementAction(bool left, bool right) {
        if (left) {
            if (!tryMovingActions.Contains(TryMoving.Left)) {
                tryMovingActions.Add(TryMoving.Left);
            }
        }
        else {
            if (tryMovingActions.Contains(TryMoving.Left)) {
                tryMovingActions.Remove(TryMoving.Left);
            }
        }
        if (right) {
            if (!tryMovingActions.Contains(TryMoving.Right)) {
                tryMovingActions.Add(TryMoving.Right);
            }
        }
        else {
            if (tryMovingActions.Contains(TryMoving.Right)) {
                tryMovingActions.Remove(TryMoving.Right);
            }
        }
    }

    public override void _Ready() {
        Robot = GetNode<Node2D>("..");
        tryMovingActions = new() { TryMoving.None };
        MaxSpeed = new Vector2(55, 0);
        Acceleration = new Vector2(130, 0);
        hitLeftBorder = false;
        hitRightBorder = false;
    }

    public override void _PhysicsProcess(double delta) {
        if (tryMovingActions[^1] == TryMoving.Right) {
            if (Velocity.X != MaxSpeed.X) {
                if (Velocity.X + Acceleration.X * (float)delta >= MaxSpeed.X) {
                    Velocity = new Vector2(MaxSpeed.X, Velocity.Y);
                }
                else {
                    Velocity += new Vector2(Acceleration.X, 0) * (float)delta;
                }
                //sekedar ngasih extra turn rate
                if (Velocity.X < 0) {
                    Velocity += new Vector2(Acceleration.X / 2, 0) * (float)delta;
                }
            }
        }
        else if (tryMovingActions[^1] == TryMoving.Left) {
            if (Velocity.X != -MaxSpeed.X) {
                if (Velocity.X - Acceleration.X * (float)delta <= -MaxSpeed.X) {
                    Velocity = new Vector2(-MaxSpeed.X, Velocity.Y);
                }
                else {
                    Velocity += new Vector2(-Acceleration.X, 0) * (float)delta;
                }
                //sekedar ngasih extra turn rate
                if (Velocity.X > 0) {
                    Velocity += new Vector2(-Acceleration.X / 2, 0) * (float)delta;
                }
            }
        }
        else {
            if (Velocity.X != 0) {
                if (Math.Abs(Velocity.X) - Acceleration.X * (float)delta <= 0) {
                    Velocity = new Vector2(0, Velocity.Y);
                }
                else {
                    Velocity += new Vector2(-Acceleration.X * Velocity.X / Math.Abs(Velocity.X), 0) * (float)delta;
                }
            }
        }
        Robot.GlobalPosition += Velocity * (float)delta;
        if (Velocity.X > 0) {
            Direction = Direction.Right;
        }
        if (Velocity.X < 0) {
            Direction = Direction.Left;
        }
    }

    private void OnBodyAreaEntered(Area2D body) {
        GD.Print(body.Name);
    }
}

public enum Direction { Left, Right }