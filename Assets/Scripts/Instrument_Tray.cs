using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Instrument_Tray : MonoBehaviour
{
    // Current placeholder
    private T_Placeholder current_placeholder = null;

    public Hand left_hand;
    public Hand right_hand;
    private SteamVR_Behaviour_Pose pose_right = null;
    private SteamVR_Behaviour_Pose pose_left = null;

    void Awake()
    {
        pose_right = right_hand.GetComponent<SteamVR_Behaviour_Pose>();
        pose_left = left_hand.GetComponent<SteamVR_Behaviour_Pose>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other_collider)
    {
        // First, check the tag of whatever triggered
        if (!other_collider.gameObject.CompareTag("Tray"))
            return;

        // If the tag is "Tray", we will indicate that this is the current placeholder
        current_placeholder = other_collider.gameObject.GetComponent<T_Placeholder>();
        
    }

    private void OnTriggerStay(Collider other_collider)
    {
        // We check the tag of whatever triggered and whether the trigger is released on either of the controllers
        if (other_collider.gameObject.CompareTag("Tray") &&
            (right_hand.grab_action.GetLastStateUp(pose_right.inputSource) || left_hand.grab_action.GetLastStateUp(pose_left.inputSource)))
        {
            Place_Tray();
        }
    }

    private void OnTriggerExit(Collider other_collider)
    {
        // We check the tag of whatever triggered
        if (!other_collider.gameObject.CompareTag("Tray"))
            return;

        // If the tag was "Tray", we will no longer have a current placeholder
        current_placeholder = null;

    }

    public void Place_Tray()
    {
        // Null check
        if (!current_placeholder)
            return;

        // Place tray
        transform.position = current_placeholder.transform.position;
        transform.rotation = current_placeholder.transform.rotation;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;

        // Change tag to untagged
        this.tag = "Untagged";
    }
}
