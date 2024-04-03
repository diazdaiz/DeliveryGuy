using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SceneTransitioner : Node {
    ColorRect fadeScreen;
    float fadeInTime;
    float fadeOutTime;
    string scenePath;
    List<Node> nodesToBeFreed;
    Node root;
    enum FadeState { In, Out }
    FadeState fadeState;

    //untuk sementara, kalau diisi string kosong "" jadi quit
    public static void TransitionToScene(string scenePath, Node caller, float fadeInTime = 0.18f, float fadeOutTime = 0.18f) {
        PackedScene packagePackedScene = ResourceLoader.Load<PackedScene>("res://Scenes/SceneTransitioner.tscn");
        SceneTransitioner sceneTransitioner = (SceneTransitioner)packagePackedScene.Instantiate();
        sceneTransitioner.scenePath = scenePath;
        sceneTransitioner.fadeInTime = fadeInTime;
        sceneTransitioner.fadeOutTime = fadeOutTime;
        sceneTransitioner.fadeScreen = (ColorRect)sceneTransitioner.GetChild(0);
        while (caller.GetParent() != null) {
            caller = caller.GetParent();
        }
        sceneTransitioner.nodesToBeFreed = caller.GetChildren().ToList();
        sceneTransitioner.root = caller;
        caller.AddChild(sceneTransitioner);
    }

    public override void _Process(double delta) {
        if (fadeState == FadeState.In && fadeScreen.Color.A < 1) {
            if (fadeScreen.Color.A + 1f / fadeInTime * delta >= 1) {
                fadeScreen.Color = new Color(fadeScreen.Color.R, fadeScreen.Color.G, fadeScreen.Color.B, 1);
            }
            else {
                fadeScreen.Color = new Color(fadeScreen.Color.R, fadeScreen.Color.G, fadeScreen.Color.B, fadeScreen.Color.A + 1f / fadeInTime * (float)delta);
            }
            return;
        }
        else if (fadeState == FadeState.Out && fadeScreen.Color.A > 0) {
            if (fadeScreen.Color.A - 1f / fadeOutTime * delta <= 0) {
                fadeScreen.Color = new Color(fadeScreen.Color.R, fadeScreen.Color.G, fadeScreen.Color.B, 0);
            }
            else {
                fadeScreen.Color = new Color(fadeScreen.Color.R, fadeScreen.Color.G, fadeScreen.Color.B, fadeScreen.Color.A - 1f / fadeOutTime * (float)delta);
            }
            return;
        }
        if (fadeState == FadeState.In) {
            if (string.IsNullOrEmpty(scenePath)) {
                GetTree().Quit();
            }
            else {
                PackedScene packagePackedScene = ResourceLoader.Load<PackedScene>(scenePath);
                Node nextScene = (Node)packagePackedScene.Instantiate();
                foreach (Node node in nodesToBeFreed) {
                    node.QueueFree();
                }
                root.AddChild(nextScene);
                root.MoveChild(nextScene, 0);
            }
            fadeState = FadeState.Out;
        }
        else if (fadeState == FadeState.Out) {
            QueueFree();
        }

    }
}
