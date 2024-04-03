using Godot;
using System;

public class PackageInformation {
    public string Name { get; private set; }
    public string RecipientName { get; private set; }
    public Sprite2D Sticker { get; private set; }

    public PackageInformation(string recipientName, string name, Sprite2D sticker) {
        Name = name;
        RecipientName = recipientName;
        Sticker = sticker;
    }
}
