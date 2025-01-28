using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Syringe : MonoBehaviour
{
    // We will use a variable to store the current object that we are touching that is injectable
    private Injectable current_injectable = null;

    [SerializeField]
    private GameObject body;
    [SerializeField]
    private GameObject injection_site;

    private ConfigurableJoint s_body_joint = null;

    public EKG_VideoPlayer EKG_videoPlayer;
    public TA_VideoPlayer TA_videoPlayer;
    public SAT_VideoPlayer SAT_videoPlayer;

    public Hand left_hand;
    public Hand right_hand;
    private SteamVR_Behaviour_Pose pose_right = null;
    private SteamVR_Behaviour_Pose pose_left = null;

    private void Awake()
    {
        pose_right = right_hand.GetComponent<SteamVR_Behaviour_Pose>();
        pose_left = left_hand.GetComponent<SteamVR_Behaviour_Pose>();

        s_body_joint = body.gameObject.GetComponent<ConfigurableJoint>();
    }

    private void Update()
    {
        // Inject();
    }

    private void OnTriggerEnter(Collider other_collider)
    {
        // We check the tag of whatever triggered
        if (!other_collider.gameObject.CompareTag("Injectable"))
            return;

        // If the object is an injectable object, we will add it to the list of things that can be injected
        current_injectable = other_collider.gameObject.GetComponent<Injectable>();
        
        Place_Needle();
    }

    private void OnTriggerExit(Collider other_collider)
    {
        // We check the tag of whatever triggered
        if (!other_collider.gameObject.CompareTag("Injectable"))
            return;

        // If the object is an injectable, we will no longer have a current_injectable
        current_injectable = null;
    }

    public void Place_Needle()
    {
        // Null check (see if we actually have something to inject)
        if (!current_injectable)
            return;

        //print("INJECT!");

        //syringe.tag = "Untagged";
        //syringe.gameObject.GetComponent<Rigidbody>().useGravity = false;

        this.transform.tag = "Holdable";

        // Change the screens
        EKG_videoPlayer.SetClip(2);
        TA_videoPlayer.SetClip(2);
        SAT_videoPlayer.SetClip(2);
    }
}
