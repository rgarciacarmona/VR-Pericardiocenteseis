using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EVD_Straight : MonoBehaviour
{
    // We will use a variable to store the current object that we are touching that is injectable
    private Injectable current_injectable = null;

    private void OnTriggerEnter(Collider other_collider)
    {
        // We check the tag of whatever triggered
        if (!other_collider.gameObject.CompareTag("Injectable"))
            return;
        //print("triggered!");
        // If the object is an injectable object, we will add it to the list of things that can be injected
        current_injectable = other_collider.gameObject.GetComponent<Injectable>();

        Place_EVD_Straight();
    }

    private void OnTriggerExit(Collider other_collider)
    {
        // We check the tag of whatever triggered
        if (!other_collider.gameObject.CompareTag("Injectable"))
            return;

        // If the object is an injectable, we will no longer have a current_injectable
        current_injectable = null;
    }

    public void Place_EVD_Straight()
    {
        // Null check (see if we actually have something to inject)
        if (!current_injectable)
            return;

        print("EVD in position");

        this.transform.tag = "Holdable";
        
    }
}
