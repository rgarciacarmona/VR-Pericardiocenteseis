using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PressureCuff : MonoBehaviour
{
    // Current placeholder
    private PC_Placeholder current_placeholder = null;

    // List of placeholders that the electrode is touching
    // public List<E_Placeholder> contact_placeholders = new List<E_Placeholder>();

    [SerializeField]
    private GameObject placed_cuff = null;

    // We need a renderer for each signal and the gameobject of its place.
    private Renderer TARenderer;
    public TA_VideoPlayer TA_videoPlayer;
    [SerializeField]
    private GameObject TAsignal;

    // We need a list of materials we can change into
    [SerializeField]
    private Material[] materials;

    public Hand left_hand;
    public Hand right_hand;
    private SteamVR_Behaviour_Pose pose_right = null;
    private SteamVR_Behaviour_Pose pose_left = null;

    private void Awake()
    {
        TARenderer = TAsignal.GetComponent<Renderer>();
        TARenderer.enabled = true;
        TARenderer.sharedMaterial = materials[0];

        pose_right = right_hand.GetComponent<SteamVR_Behaviour_Pose>();
        pose_left = left_hand.GetComponent<SteamVR_Behaviour_Pose>();
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other_collider)
    {
        // First, check the tag of whatever triggered
        if (!other_collider.gameObject.CompareTag("PressureCuff"))
            return;

        // If the tag is "PressureCuff", we will indicate that this is the current placeholder
        current_placeholder = other_collider.gameObject.GetComponent<PC_Placeholder>();

        // Place_Cuff();
    }

    private void OnTriggerStay(Collider other_collider)
    {
        // We check the tag of whatever triggered and whether the trigger is released on either of the controllers
        if (other_collider.gameObject.CompareTag("PressureCuff") &&
            (right_hand.grab_action.GetLastStateUp(pose_right.inputSource) || left_hand.grab_action.GetLastStateUp(pose_left.inputSource)))
        {
            Place_Cuff();
        }
    }

    private void OnTriggerExit(Collider other_collider)
    {
        // We check the tag of whatever triggered
        if (!other_collider.gameObject.CompareTag("PressureCuff"))
            return;

        // If the tag was "PressureCuff", we will no longer have a current placeholder
        current_placeholder = null;
    }

    public void Place_Cuff()
    {
        // Null check
        if (!current_placeholder)
            return;

        // Change material to videoMaterial
        TARenderer.sharedMaterial = materials[1];
        TA_videoPlayer.SetClip(1);

        // Place cuff
        placed_cuff.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
