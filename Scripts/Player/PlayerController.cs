using Godot;
using System;

public partial class PlayerController : Node2D {
    PlayerMovement movement;
    PlayerInteraction interaction;

    public override void _Ready() {
        movement = GetNode<PlayerMovement>("../Movement");
        interaction = GetNode<PlayerInteraction>("../Interaction");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
        movement.SetMovementAction(Input.IsActionPressed("Left"), Input.IsActionPressed("Right"));
        interaction.SetInteractAction(Input.IsActionJustPressed("Select"));
    }
}
