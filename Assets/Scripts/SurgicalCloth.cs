using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SurgicalCloth : MonoBehaviour
{
    public Hand left_hand;
    public Hand right_hand;
    private SteamVR_Behaviour_Pose pose_right = null;
    private SteamVR_Behaviour_Pose pose_left = null;

    [SerializeField]
    private GameObject extendedCloth;
    
    void Awake()
    {
        pose_right = right_hand.GetComponent<SteamVR_Behaviour_Pose>();
        pose_left = left_hand.GetComponent<SteamVR_Behaviour_Pose>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        // We check the tag of whatever triggered and whether the trigger is up
        if (collider.gameObject.CompareTag("Place_Sheet"))
        {
            print("Place_Sheet triggered");

            // Place_sheet();
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        // We check the tag of whatever triggered and whether the trigger is released on either of the controllers
        if (collider.gameObject.CompareTag("Place_Sheet") && 
            (right_hand.grab_action.GetLastStateUp(pose_right.inputSource) || left_hand.grab_action.GetLastStateUp(pose_left.inputSource)))
        {
            print("Place_Sheet stay " + pose_right.inputSource);

            Place_Sheet();
        }
    }

    private void Place_Sheet()
    {
        // We make the extended sheet sappear...
            extendedCloth.SetActive(true);
        // ... and the folded sheet disappear
        this.gameObject.SetActive(false);
    }
}
