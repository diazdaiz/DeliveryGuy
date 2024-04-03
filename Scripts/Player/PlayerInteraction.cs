using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerInteraction : Node2D, IInteractable {
    List<object> availableInteractableObject;
    Package package;
    Node2D packageStorage;
    public bool HasPackage => package != null;
    bool interacting;
    AudioStreamPlayer2D PickupAudio;
    AudioStreamPlayer2D DeliverAudio;

    private void OnInteractorAreaEntered(Area2D interactor) {
        Node2D interactorObject = interactor.GetNode<Node2D>("../..");
        if (interactorObject.Name == "DeliveryGuy") {
            return;
        }
        if (!availableInteractableObject.Contains(interactorObject)) {
            availableInteractableObject.Add(interactorObject);
        }
    }

    private void OnInteractorAreaExited(Area2D interactor) {
        Node2D interactorObject = interactor.GetNode<Node2D>("../..");
        if (interactorObject.Name == "DeliveryGuy") {
            return;
        }
        if (availableInteractableObject.Contains(interactorObject)) {
            availableInteractableObject.Remove(interactorObject);
        }
    }

    public void SetInteractAction(bool interact) {
        interacting = interact;
    }

    void Interact() {
        Node2D interactableObject = null;
        if (availableInteractableObject.Count > 0) {
            // NOTE - mungkin kalau ada banyak yang bisa di interact secara bersamaan, loop object untuk cari 
            // yang terdekat & interact object itu. Tapi ini special case aja ambil object index terakhir
            interactableObject = (Node2D)availableInteractableObject[^1];
        }
        if (interactableObject == null) {
            return;
        }
        IInteractable interactable = interactableObject.GetNode<IInteractable>("Interaction");
        if (interactable == null) {
            GD.Print("no interactable");
            return;
        }
        if (interactableObject.Name == "Truck") {
            GD.Print("ask for package");
            if (package == null) {
                interactable.OnInteracted(GetNode<Node2D>(".."), new AskForPackage());
            }
        }
        if (interactableObject.GetParent().Name == "Recipients") {
            if (package == null) {
                GD.Print("no package, can't give to recipient");
                return;
            }
            interactable.OnInteracted(GetNode<Node2D>(".."), new GivePackageInformation(package.PackageInformation));
        }
    }

    public void OnInteracted(object interactor, Interaction interaction) {
        if (interaction.GetType() == typeof(GivePackage)) {
            GD.Print("receive package");
            PickupAudio.Play();
            package = (interaction as GivePackage).Package;
            package.Reparent(packageStorage);
            package.GlobalPosition = packageStorage.GlobalPosition;
        }
        else if (interaction.GetType() == typeof(AskForPackage)) {
            GD.Print("give package to recepient");
            DeliverAudio.Play();
            (interactor as Node2D).GetNode<IInteractable>("Interaction").OnInteracted(GetNode<Node2D>(".."), new GivePackage(package));
            package = null;
        }
    }

    public override void _Ready() {
        PickupAudio = GetNode<AudioStreamPlayer2D>("PickupAudio");
        DeliverAudio = GetNode<AudioStreamPlayer2D>("DeliverAudio");
        availableInteractableObject = new();
        packageStorage = GetNode<Node2D>("PackageStorage");
    }

    public override void _Process(double delta) {
        if (GameplayManager.Instance.GameplayState == GameplayState.Prepare) {
            package = null;
            if (packageStorage.GetChildCount() > 0) {
                for (int i = 0; i < packageStorage.GetChildCount(); i++) {
                    packageStorage.GetChild(i).QueueFree();
                }
            }
        }
        if (interacting) {
            Interact();
            interacting = false;
        }
    }
}
