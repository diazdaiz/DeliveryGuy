using Godot;
using System;

public partial class TimeWatcher : Label {
    public override void _Process(double delta) {
        Text = "Time Remaining: " + Math.Ceiling(GameplayManager.Instance.Timer).ToString();
    }
}
