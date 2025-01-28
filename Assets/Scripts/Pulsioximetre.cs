using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Pulsioximetre : MonoBehaviour
{
    // Current placeholder
    private P_Placeholder current_placeholder = null;

    // We need a renderer for each signal and the gameobject of its place.
    private Renderer SATRenderer;
    public SAT_VideoPlayer SAT_videoPlayer;
    [SerializeField]
    private GameObject SATsignal;

    // We need a list of materials we can change into
    [SerializeField]
    private Material[] materials;

    public Hand left_hand;
    public Hand right_hand;
    private SteamVR_Behaviour_Pose pose_right = null;
    private SteamVR_Behaviour_Pose pose_left = null;

    private void Awake()
    {
        SATRenderer = SATsignal.GetComponent<Renderer>();
        SATRenderer.enabled = true;
        SATRenderer.sharedMaterial = materials[0];

        pose_right = right_hand.GetComponent<SteamVR_Behaviour_Pose>();
        pose_left = left_hand.GetComponent<SteamVR_Behaviour_Pose>();
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other_collider)
    {
        // First, check the tag of whatever triggered
        if (!other_collider.gameObject.CompareTag("Pulsiox"))
            return;

        // If the tag is "Pulsiox", we will indicate that this is the current placeholder
        current_placeholder = other_collider.gameObject.GetComponent<P_Placeholder>();

        // Place_Pulsi();
    }

    private void OnTriggerStay(Collider other_collider)
    {
        // We check the tag of whatever triggered and whether the trigger is released on either of the controllers
        if (other_collider.gameObject.CompareTag("Pulsiox") &&
            (right_hand.grab_action.GetLastStateUp(pose_right.inputSource) || left_hand.grab_action.GetLastStateUp(pose_left.inputSource)))
        {
            Place_Pulsi();
        }
    }

    private void OnTriggerExit(Collider other_collider)
    {
        // We check the tag of whatever triggered
        if (!other_collider.gameObject.CompareTag("Pulsiox"))
            return;

        // If the tag was "Pulsiox", we will no longer have a current placeholder
        current_placeholder = null;
    }

    public void Place_Pulsi()
    {
        // Null check
        if (!current_placeholder)
            return;

        // Place pulsioximetre
        transform.position = current_placeholder.transform.position;
        transform.rotation = current_placeholder.transform.rotation;
        //transform.rotation = Quaternion.FromToRotation(Vector3.up, transform.forward);
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;

        // Change material to videoMaterial
        SATRenderer.sharedMaterial = materials[1];
        SAT_videoPlayer.SetClip(1);
    }
}
