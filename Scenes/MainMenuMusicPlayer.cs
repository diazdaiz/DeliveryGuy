using Godot;
using System;

public partial class MainMenuMusicPlayer : AudioStreamPlayer2D {
    public override void _Ready() {
        Stream = GD.Load<AudioStream>("res://Audios/Modern7.mp3");
    }

    public override void _Process(double delta) {
        if (Input.IsActionJustPressed("Up") || Input.IsActionJustPressed("Down")) {
            Play();
        }
    }
}
