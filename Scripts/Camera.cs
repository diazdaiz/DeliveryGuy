using Godot;
using System;

public partial class Camera : Camera2D {
    Node2D target;
    public override void _Ready() {
        target = GetNode<Node2D>("../World/DeliveryGuy");
    }

    public override void _Process(double delta) {
        GlobalPosition = new Vector2(target.GlobalPosition.X, GlobalPosition.Y);
        //1214
        //1597
        if (GlobalPosition.X < 1214) {
            GlobalPosition = new Vector2(1214, GlobalPosition.Y);
        }
        if (GlobalPosition.X > 1597) {
            GlobalPosition = new Vector2(1597, GlobalPosition.Y);
        }
    }
}
