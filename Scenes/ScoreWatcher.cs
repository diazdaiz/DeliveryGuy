using Godot;
using System;

public partial class ScoreWatcher : Label {
    public override void _Process(double delta) {
        Text = "Score: " + GameplayManager.Instance.Score.ToString();
    }
}
