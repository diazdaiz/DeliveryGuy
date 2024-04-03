using Godot;
using System;

public partial class MainMenuMusicPlayer : AudioStreamPlayer2D {
    public override void _Ready() {
        Stream = GD.Load<AudioStream>("res://Audios/Retro Beat.ogg");

    }

    public override void _Process(double delta) {
        if (!Playing) {
            Play();
        }
    }
}
