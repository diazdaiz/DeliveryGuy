using Godot;
using System;

public partial class CharacterController : Node2D {
    Movement movement;

    public override void _Ready() {
        movement = GetNode<Movement>("../Movement");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
        movement.SetMovementAction(Input.IsActionPressed("Left"), Input.IsActionPressed("Right"));
    }
}
