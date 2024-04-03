using Godot;
using System;

public class GivePackageInformation : Interaction {
    public PackageInformation PackageInformation { get; private set; }

    public GivePackageInformation(PackageInformation packageInformation) {
        PackageInformation = packageInformation;
    }
}
