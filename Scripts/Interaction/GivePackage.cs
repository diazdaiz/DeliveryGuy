using Godot;
using System;

public partial class GivePackage : Interaction {
    public Package Package { get; private set; }

    public GivePackage(Package package) {
        Package = package;
    }
}
