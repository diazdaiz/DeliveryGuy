using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class GameplayManager : Node2D {
    public static GameplayManager Instance;
    List<string> availablePackagesName;
    List<string> currentOrderedPackagesName;
    Node2D packageContainer;
    public GameplayState GameplayState { get; private set; }
    Node2D world;
    public float Timer { get; private set; }
    public int HiScore { get; private set; }
    public int Score { get; private set; }
    public bool celebrateNewHighScore;
    ColorRect prepareUI;
    ColorRect pauseUI;
    OptionsManager prepareOptions;
    OptionsManager pauseOptions;
    List<CpuParticles2D> confetti;
    AudioStreamPlayer2D musicPlayer;
    AudioStreamOggVorbis PrepareMusic;
    AudioStreamOggVorbis RunMusic;

    // fungsi untuk dipanggil oleh recepient
    // paketnya random aja instead of recepient yang nentuin
    // public static PackageInformation OrderANewPackage(string recipientName, string package name) {
    public PackageInformation OrderANewPackage(string recipientName) {
        int index = new Random().Next(availablePackagesName.Count);
        PackageInformation packageInformation = Package.GeneratePackage(recipientName, availablePackagesName[index], packageContainer).PackageInformation;
        currentOrderedPackagesName.Add(availablePackagesName[index]);
        availablePackagesName.Remove(availablePackagesName[index]);
        return packageInformation;
    }

    public void ReportPackageArrival(string packageName) {
        currentOrderedPackagesName.Remove(packageName);
        availablePackagesName.Add(packageName);
        Score += 100;
        if (Score > HiScore) {
            HiScore = Score;
            celebrateNewHighScore = true;
            DataManager.Save(HiScore);
        }
    }

    public void CancelOrder(string packageName) {
        currentOrderedPackagesName.Remove(packageName);
        availablePackagesName.Add(packageName);
    }

    void Start() {
        musicPlayer.Stream = RunMusic;
        celebrateNewHighScore = false;
        Timer = 60;
        Score = 0;
        GameplayState = GameplayState.Run;
        prepareUI.Visible = false;
        pauseUI.Visible = false;
    }

    void Pause() {
        GameplayState = GameplayState.Pause;
        prepareUI.Visible = false;
        pauseUI.Visible = true;
    }

    void Continue() {
        GameplayState = GameplayState.Run;
        prepareUI.Visible = false;
        pauseUI.Visible = false;
    }

    void Finish() {
        musicPlayer.Stream = PrepareMusic;
        GameplayState = GameplayState.Prepare;
        prepareUI.Visible = true;
        pauseUI.Visible = false;
    }

    public override void _Ready() {
        HiScore = DataManager.Load();
        world = GetNode<Node2D>("World");
        musicPlayer = GetNode<AudioStreamPlayer2D>("MusicPlayer");
        PrepareMusic = ResourceLoader.Load<AudioStreamOggVorbis>("res://Audios/Retro Polka.ogg");
        RunMusic = ResourceLoader.Load<AudioStreamOggVorbis>("res://Audios/Retro Comedy.ogg");
        musicPlayer.Stream = PrepareMusic;
        prepareUI = GetNode<ColorRect>("UI/PreparingGameplay");
        pauseUI = GetNode<ColorRect>("UI/PausingGameplay");
        prepareOptions = GetNode<OptionsManager>("UI/PreparingGameplay/Options");
        pauseOptions = GetNode<OptionsManager>("UI/PausingGameplay/ColorRect/Options");
        world.SetProcess(false);
        world.SetPhysicsProcess(false);
        GameplayState = GameplayState.Prepare;
        packageContainer = GetNode<Node2D>("World/Truck/PackagesContainer");
        availablePackagesName = DirAccess.GetFilesAt("res://Scenes/Packages").ToList();
        currentOrderedPackagesName = new();
        for (int i = 0; i < availablePackagesName.Count; i++) {
            availablePackagesName[i] = availablePackagesName[i].Split('.')[0];
        }
        foreach (string name in availablePackagesName) {
            GD.Print(name);
        }
        Node2D confettiContainer = GetNode<Node2D>("Confetti");
        confetti = new();
        for (int i = 0; i < confettiContainer.GetChildCount(); i++) {
            confetti.Add(confettiContainer.GetChild<CpuParticles2D>(i));
        }
        Instance = this;
    }

    public override void _Process(double delta) {
        if (Input.IsActionJustPressed("Reset")) {
            HiScore = Score;
            DataManager.Save(HiScore);
        }
        if (GameplayState == GameplayState.Prepare) {
            if (!musicPlayer.Playing) {
                musicPlayer.Play();
            }
            if (!confetti[0].Emitting) {
                foreach (CpuParticles2D confettiParticle in confetti) {
                    confettiParticle.Emitting = celebrateNewHighScore;
                }
            }
            if (Input.IsActionJustPressed("Select")) {
                if (prepareOptions.CurrentHoveredIndex == 0) {
                    Start();
                }
                else if (prepareOptions.CurrentHoveredIndex == 1) {
                    SceneTransitioner.TransitionToScene("res://Scenes/MainMenu.tscn", this);
                }
            }
        }
        else if (GameplayState == GameplayState.Run) {
            if (!musicPlayer.Playing) {
                musicPlayer.Play();
            }
            if (Input.IsActionJustPressed("Escape")) {
                Pause();
            }
        }
        else if (GameplayState == GameplayState.Pause) {
            if (musicPlayer.Playing) {
                musicPlayer.Stop();
            }
            if (Input.IsActionJustPressed("Escape")) {
                Continue();
            }
            if (Input.IsActionJustPressed("Select")) {
                if (pauseOptions.CurrentHoveredIndex == 0) {
                    Continue();
                }
                else if (pauseOptions.CurrentHoveredIndex == 1) {
                    SceneTransitioner.TransitionToScene("res://Scenes/MainMenu.tscn", this);
                }
            }
        }
    }

    public override void _PhysicsProcess(double delta) {
        if (GameplayState == GameplayState.Run) {
            Timer -= (float)delta;
            if (Timer < 0) {
                Timer = 0;
                Finish();
            }
        }
    }
}

public enum GameplayState { Prepare, Run, Pause }