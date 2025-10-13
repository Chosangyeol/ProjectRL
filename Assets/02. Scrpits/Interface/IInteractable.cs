using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    string interactName { get; }

    void OnFocus();
    void OnUnFocus();
    void OnInteract();
}
