using Godot;
using System;

public partial class TruckInteraction : Node2D, IInteractable {
    Node2D packagesContainer;

    public void OnInteracted(object interactor, Interaction interaction) {
        if (interaction.GetType() == typeof(AskForPackage)) {
            Node2D deliveryGuy = (Node2D)interactor;
            if (deliveryGuy.Name != "DeliveryGuy") {
                return;
            }
            PlayerInteraction deliveryGuyInteraction = deliveryGuy.GetNode<PlayerInteraction>("Interaction");
            GD.Print("has package:");
            GD.Print(deliveryGuyInteraction.HasPackage);
            if (deliveryGuyInteraction.HasPackage) {
                GD.Print("already have package");
                return;
            }
            Package package = (Package)packagesContainer.GetChild(new Random().Next(packagesContainer.GetChildCount()));
            (deliveryGuyInteraction as IInteractable).OnInteracted(GetNode<Node2D>(".."), new GivePackage(package));
        }
    }

    public override void _Ready() {
        packagesContainer = GetNode<Node2D>("../PackagesContainer");
    }

    public override void _Process(double delta) {
        if (GameplayManager.Instance.GameplayState == GameplayState.Prepare) {
            if (packagesContainer.GetChildCount() > 0) {
                for (int i = 0; i < packagesContainer.GetChildCount(); i++) {
                    packagesContainer.GetChild(i).QueueFree();
                }
            }
        }
    }
}
