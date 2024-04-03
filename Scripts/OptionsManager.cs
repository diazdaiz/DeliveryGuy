using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class OptionsManager : Node {
    public int CurrentHoveredIndex { get; private set; }
    Color defaultTargetColor;
    Color hoveredTargetColor;
    List<Control> options;

    public override void _Ready() {
        defaultTargetColor = new Color(1f, 1f, 1f, 1f);
        hoveredTargetColor = new Color(1f, 252f / 255f, 171f / 255f, 1f);
        CurrentHoveredIndex = 0;
        List<Node> children = GetChildren().ToList();
        options = new();
        foreach (Node child in children) {
            options.Add((Control)child);
        }
    }

    public override void _Process(double delta) {
        if (Input.IsActionJustPressed("Down") || Input.IsActionJustPressed("Right")) {
            CurrentHoveredIndex += 1;
        }
        else if (Input.IsActionJustPressed("Up") || Input.IsActionJustPressed("Left")) {
            CurrentHoveredIndex -= 1;
        }
        if (CurrentHoveredIndex == -1) {
            CurrentHoveredIndex = options.Count - 1;
        }
        CurrentHoveredIndex %= options.Count;
        for (int i = 0; i < options.Count; i++) {
            if (i == CurrentHoveredIndex) {
                options[i].Modulate = hoveredTargetColor;
            }
            else {
                options[i].Modulate = defaultTargetColor;
            }
        }
    }
}
