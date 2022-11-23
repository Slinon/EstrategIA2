using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{

    // @IGM -------------------------------------
    // Metodo para interactuar con los elementos.
    // ------------------------------------------
    void Interact(Action onInteractionComplete, Unit unit);

}
