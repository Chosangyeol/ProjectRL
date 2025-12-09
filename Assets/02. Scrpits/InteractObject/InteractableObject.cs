using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : PoolableMono
{
    public GameObject interactCanvas;
    protected float interactRange;
    public int price = 25;

    protected virtual void Start()
    {
        interactCanvas = GameObject.FindGameObjectWithTag("InteractCanvas");
        Debug.Log(interactCanvas.name);
        interactRange = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().interactRange;
    }
}
