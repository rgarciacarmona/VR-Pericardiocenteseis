using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Injectable : MonoBehaviour
{
    [HideInInspector]
    public Syringe syringe = null;

}
