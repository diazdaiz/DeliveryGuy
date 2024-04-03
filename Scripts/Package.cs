using Godot;
using System;

public partial class Package : Node2D {
    public PackageInformation PackageInformation { get; private set; }

    public static Package GeneratePackage(string recipientName, string packageName, Node2D packageContainer) {
        PackedScene packagePackedScene = ResourceLoader.Load<PackedScene>("res://Scenes/Packages/" + packageName + ".tscn");
        Package package = (Package)packagePackedScene.Instantiate();
        packageContainer.AddChild(package);
        package.PackageInformation = new PackageInformation(recipientName, packageName, (Sprite2D)package.GetNode<Sprite2D>("Sticker").Duplicate());
        //package.Owner = PackageContainer;
        return package;
    }


}
