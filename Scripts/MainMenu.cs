using Godot;
using System.Collections.Generic;

public partial class MainMenu : ColorRect {
    int currentHoveredIndex;
    Color defaultTargetColor;
    Color hoveredTargetColor;
    List<NinePatchRect> options;
    ColorRect fadeScreen;
    float fadeAlpha;
    bool isTransitioning;

    public override void _Ready() {
        isTransitioning = false;
        defaultTargetColor = new Color(1f, 1f, 1f, 1f);
        hoveredTargetColor = new Color(1f, 252f / 255f, 171f / 255f, 1f);
        currentHoveredIndex = 0;
        options = new() {
            GetNode<NinePatchRect>("VBoxContainer/Option1"),
            GetNode<NinePatchRect>("VBoxContainer/Option2"),
            GetNode<NinePatchRect>("VBoxContainer/Option3"),
        };
        fadeScreen = GetNode<ColorRect>("../FadeScreen");
    }

    public override void _Process(double delta) {
        if (isTransitioning) {
            TransitionProcess((float)delta);
            return;
        };
        InputProcess();
    }

    void TransitionProcess(float delta) {
        if (fadeScreen.Color.A < 1) {
            if (fadeScreen.Color.A + 1 * delta >= 1) {
                fadeScreen.Color = new Color(fadeScreen.Color.R, fadeScreen.Color.G, fadeScreen.Color.B, 1);
            }
            else {
                fadeScreen.Color = new Color(fadeScreen.Color.R, fadeScreen.Color.G, fadeScreen.Color.B, fadeScreen.Color.A + 1f * delta);
            }
            return;
        }
        if (currentHoveredIndex == 0) {
            GetTree().ChangeSceneToFile("res://Scenes/Gameplay.tscn");
        }
        else if (currentHoveredIndex == 1) {
            GetTree().ChangeSceneToFile("res://Scenes/About.tscn");
        }
        else if (currentHoveredIndex == 2) {
            GetTree().Quit();
        }
    }

    void InputProcess() {
        if (Input.IsActionJustPressed("Down")) {
            currentHoveredIndex += 1;
        }
        else if (Input.IsActionJustPressed("Up")) {
            currentHoveredIndex -= 1;
        }
        if (currentHoveredIndex == -1) {
            currentHoveredIndex = options.Count - 1;
        }
        currentHoveredIndex %= options.Count;
        for (int i = 0; i < options.Count; i++) {
            if (i == currentHoveredIndex) {
                options[i].Modulate = hoveredTargetColor;
            }
            else {
                options[i].Modulate = defaultTargetColor;
            }
        }

        if (Input.IsActionJustPressed("Select")) {
            isTransitioning = true;
        }
    }

    public void LoadGameplay(AnimationMixer.AnimationFinishedEventHandler handler) {
        GetTree().ChangeSceneToFile("res://Scenes/Gameplay.tscn");
    }
}
