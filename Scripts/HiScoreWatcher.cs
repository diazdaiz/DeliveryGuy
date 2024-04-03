using Godot;
using System;

public partial class HiScoreWatcher : Label {
    public override void _Process(double delta) {
        Text = "Hi-Score: " + GameplayManager.Instance.HiScore.ToString();
    }
}
