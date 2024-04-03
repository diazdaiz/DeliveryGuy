using Godot;
using System;

public partial class RecipientInteraction : Node2D, IInteractable {
    PackageInformation packageInformation;
    Sprite2D thinkBox;

    public void OnInteracted(object interactor, Interaction interaction) {
        //check package if given information
        if (interaction.GetType() == typeof(GivePackageInformation)) {
            if ((interaction as GivePackageInformation).PackageInformation == packageInformation) {
                GD.Print("package information is corrent");
                GD.Print("ask to take package from delivery");
                (interactor as Node2D).GetNode<PlayerInteraction>("Interaction").OnInteracted(GetNode<Node2D>(".."), new AskForPackage(packageInformation));
            }
            else {
                GD.Print("not want to receive package, not recipient package");
            }
        }
        else if (interaction.GetType() == typeof(GivePackage)) {
            GameplayManager.Instance.ReportPackageArrival(packageInformation.Name);
            packageInformation = null;
            (interaction as GivePackage).Package.QueueFree();

        }
        //(in robot) recipient.interact(this, packageInformation)
        //check package
        //if interaction == packageInformation && packageInformation.name == currentDesiredPackageName
        //get package (robot.interact(this, GiveMeThePackageCommand))
        //(in robot) if interaction == give him/her the package command
        //(in robot) recipient.interact(this, givePackage)
        //if interaction == package && package.name == currentDesiredPackageName
        //get package
    }

    public override void _Ready() {
        thinkBox = GetNode<Sprite2D>("ThinkBox");
    }

    public override void _Process(double delta) {
        if (GameplayManager.Instance.GameplayState == GameplayState.Run) {
            if (!thinkBox.Visible) {
                thinkBox.Visible = true;
            }
            if (packageInformation == null) {
                packageInformation = GameplayManager.Instance.OrderANewPackage(Name);
                for (int i = 0; i < thinkBox.GetChildCount(); i++) {
                    thinkBox.GetChild<Sprite2D>(i).QueueFree();
                }
                packageInformation.Sticker.Scale *= 12;
                packageInformation.Sticker.ZIndex = 17;
                thinkBox.AddChild(packageInformation.Sticker);
            }
        }
        else if (GameplayManager.Instance.GameplayState == GameplayState.Prepare) {
            if (thinkBox.Visible) {
                thinkBox.Visible = false;
            }
            if (packageInformation != null) {
                GameplayManager.Instance.CancelOrder(packageInformation.Name);
                packageInformation = null;
            }
        }
    }
}
