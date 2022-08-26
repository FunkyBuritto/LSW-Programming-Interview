using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Interactable : MonoBehaviour
{
    public virtual void Interact() => throw new NotImplementedException("this interactable does not have a interact void");
}
