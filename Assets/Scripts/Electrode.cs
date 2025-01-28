using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Electrode : MonoBehaviour
{
    // Current placeholder
    private E_Placeholder current_placeholder = null;

    // List of placeholders that the electrode is touching
    // public List<E_Placeholder> contact_placeholders = new List<E_Placeholder>();

    //public GameObject text = null;

    // We need a renderer for each signal and the gameobject of its place.
    private Renderer EKGRenderer;
    public EKG_VideoPlayer EKG_videoPlayer;
    [SerializeField]
    private GameObject EKGsignal;

    // We need a list of materials we can change into
    [SerializeField]
    private Material[] materials;

    // Access to global variable connected electrodes through Electrodes
    public Electrodes electrodes;

    public Hand left_hand;
    public Hand right_hand;
    private SteamVR_Behaviour_Pose pose_right = null;
    private SteamVR_Behaviour_Pose pose_left = null;

    private void Awake()
    {
        EKGRenderer = EKGsignal.GetComponent<Renderer>();
        EKGRenderer.enabled = true;
        EKGRenderer.sharedMaterial = materials[0];

        pose_right = right_hand.GetComponent<SteamVR_Behaviour_Pose>();
        pose_left = left_hand.GetComponent<SteamVR_Behaviour_Pose>();
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other_collider)
    {
        // First, check the tag of whatever triggered
        if (!other_collider.gameObject.CompareTag("Electrode"))
            return;

        // If the tag is "Electrode", we will indicate that this is the current placeholder
        current_placeholder = other_collider.gameObject.GetComponent<E_Placeholder>();

        //Place_Electrode();
        
    }

    private void OnTriggerStay(Collider other_collider)
    {
        // We check the tag of whatever triggered and whether the trigger is released on either of the controllers
        if (other_collider.gameObject.CompareTag("Electrode") &&
            (right_hand.grab_action.GetLastStateUp(pose_right.inputSource) || left_hand.grab_action.GetLastStateUp(pose_left.inputSource)))
        {
            print("Place_Electrode stay " + pose_right.inputSource);

            Place_Electrode();
        }
    }

    private void OnTriggerExit(Collider other_collider)
    {
        // We check the tag of whatever triggered
        if (!other_collider.gameObject.CompareTag("Electrode"))
            return;

        // If the tag was "Electrode", we will no longer have a current placeholder
        current_placeholder = null;

    }

    public void Place_Electrode()
    {
        // Null check
        if (!current_placeholder)
            return;

        // Place electrode
        transform.position = current_placeholder.transform.position;
        transform.rotation = current_placeholder.transform.rotation;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;

        // Increase the number of connected electrodes
        electrodes.connectedElectrodes = electrodes.connectedElectrodes + 1;

        // If all the electrodes are connected...
        if(electrodes.connectedElectrodes == 6)
        {
            // ... change material to videoMaterial
            EKGRenderer.sharedMaterial = materials[1];
            EKG_videoPlayer.SetClip(1);
        }
        
    }
}
