using Godot;
using System;

public partial class RecipientModel : Node {
    AnimationPlayer animationPlayer;

    public override void _Ready() {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public override void _Process(double delta) {
        animationPlayer.Play();
    }
}
