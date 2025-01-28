using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Oxygen_mask : MonoBehaviour
{
    // Current placeholder
    private OM_Placeholder current_placeholder = null;

    // Spring object
    [SerializeField]
    private GameObject spring;

    public SAT_VideoPlayer SAT_videoPlayer;

    public Hand left_hand;
    public Hand right_hand;
    private SteamVR_Behaviour_Pose pose_right = null;
    private SteamVR_Behaviour_Pose pose_left = null;

    void Awake()
    {
        pose_right = right_hand.GetComponent<SteamVR_Behaviour_Pose>();
        pose_left = left_hand.GetComponent<SteamVR_Behaviour_Pose>();
    }

    private void OnTriggerEnter(Collider other_collider)
    {
        // First, check the tag of whatever triggered
        if (!other_collider.gameObject.CompareTag("Oxygen_mask"))
            return;

        // If the tag was "Oxygen_mask", we will indicate that this is the current placeholder
        current_placeholder = other_collider.gameObject.GetComponent<OM_Placeholder>();

        // Place_Mask();
    }

    private void OnTriggerStay(Collider other_collider)
    {
        // We check the tag of whatever triggered and whether the trigger is released on either of the controllers
        if (other_collider.gameObject.CompareTag("Oxygen_mask") &&
            (right_hand.grab_action.GetLastStateUp(pose_right.inputSource) || left_hand.grab_action.GetLastStateUp(pose_left.inputSource)))
        {
            Place_Mask();
        }
    }

    private void OnTriggerExit(Collider other_collider)
    {
        // We check the tag of whatever triggered
        if (!other_collider.gameObject.CompareTag("Oxygen_mask"))
            return;

        // If the tag was "Oxygen_mask", we will no longer have a current placeholder
        current_placeholder = null;
    }

    public void Place_Mask()
    {
        // Null check
        if (!current_placeholder)
            return;

        // Remove spring
        spring.SetActive(false);

        // Place mask
        transform.position = current_placeholder.transform.position;
        transform.rotation = current_placeholder.transform.rotation;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;

        // Change the saturation only
        SAT_videoPlayer.SetClip(2);
    }
}
