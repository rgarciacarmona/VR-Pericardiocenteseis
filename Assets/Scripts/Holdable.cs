using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Holdable : MonoBehaviour
{
    [HideInInspector]
    public Hand active_hand = null;
    //public bool is_holdable = true;
}
