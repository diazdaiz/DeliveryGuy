using Godot;
using System;

public interface IInteractable {
    public void OnInteracted(object interactor, Interaction interaction);
}
