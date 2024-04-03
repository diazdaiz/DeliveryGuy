using Godot;
using System;

public partial class AskForPackage : Interaction {
    public PackageInformation PackageInformation { get; private set; }

    public AskForPackage(PackageInformation packageInformation) {
        PackageInformation = packageInformation;
    }

    public AskForPackage() {
        PackageInformation = null;
    }
}
