using Godot;
using System;

public partial class Credit : CanvasLayer {
    public override void _Process(double delta) {
        if (Input.IsActionJustPressed("Escape") || Input.IsActionJustPressed("Select") || Input.IsActionJustPressed("Cancel")) {
            SceneTransitioner.TransitionToScene("res://Scenes/MainMenu.tscn", this);
        }
    }
}
