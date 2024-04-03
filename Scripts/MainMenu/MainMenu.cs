using Godot;
using System.Collections.Generic;
using System.Linq.Expressions;

public partial class MainMenu : Node {
    float fadeAlpha;
    bool isTransitioning;
    OptionsManager optionsManager;
    Label HiScoreLabel;

    public override void _Ready() {
        isTransitioning = false;
        optionsManager = GetNode<OptionsManager>("Options");
        HiScoreLabel = GetNode<Label>("../HiScoreLabel");
        HiScoreLabel.Text = "Hi score: \n" + DataManager.Load();
    }

    public override void _Process(double delta) {
        if (isTransitioning) {
            return;
        }
        if (Input.IsActionJustPressed("Select")) {
            string path = "";
            if (optionsManager.CurrentHoveredIndex == 0) {
                path = "res://Scenes/Gameplay.tscn";
            }
            else if (optionsManager.CurrentHoveredIndex == 1) {
                path = "res://Scenes/Credit.tscn";
            }
            isTransitioning = true;
            SceneTransitioner.TransitionToScene(path, this);
            return;
        };
    }
}
