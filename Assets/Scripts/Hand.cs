using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Hand : MonoBehaviour
{
    // We use a boolean to grab because we only want to know whether it's true or false
    public SteamVR_Action_Boolean grab_action = null;

    // We will use this to know whether we are using our left or our right hand
    private SteamVR_Behaviour_Pose pose = null;
    private FixedJoint joint = null;

    // HAPTIC FEEDBACK
    public SteamVR_Action_Vibration hapticAction;

    // We will use a variable to store the current object that we are touching that is pickupable
    private Pickupable current_pickupable = null;
    // And a list of pickupables that our hand is touching
    private List<Pickupable> contact_pickupables = new List<Pickupable>();

    // We will use a variable to store the current object that we are touching that is pickupable
    private Holdable current_holdable = null;
    // And a list of pickupables that our hand is touching
    //private List<Holdable> contact_holdables = new List<Holdable>();
    
        // [...]
    [SerializeField]
    private GameObject injection_site;
    public GameObject syringe_body;
    private ConfigurableJoint s_body_joint = null;
    FixedJoint site_joint = null;

    // We need a list of materials we can change into
    public Material[] materials;

    // ANIMATION
    private Animator animator;

    // CHANGE GLOVES
    private Renderer handRenderer;
    public GameObject hand;

    
    private void Awake()
    {
        pose = GetComponent<SteamVR_Behaviour_Pose>();
        joint = GetComponent<FixedJoint>();

        animator = GetComponentInChildren<Animator>();

        handRenderer = hand.GetComponent<Renderer>();

        site_joint = injection_site.GetComponent<FixedJoint>();
        s_body_joint = syringe_body.gameObject.GetComponent<ConfigurableJoint>();
    }

    private void Update()
    {
        // Here we will get the down and up state of the action grab_action. pose.inputSource will 
        // return whether it's the left or right controller.

        // Down
        if (grab_action.GetStateDown(pose.inputSource))
        {
            // Set animator bool isGrabbing to true --> Grabbing animation
            if (!animator.GetBool("IsGrabbing"))
                animator.SetBool("IsGrabbing", true);

            //print("IsGrabbing: " + animator.GetBool("IsGrabbing"));

            //print(pose.inputSource + " trigger down");
            Pickup();
            Hold();
        }

        // Up
        if (grab_action.GetStateUp(pose.inputSource))
        {
            // Set animator bool isGrabbing to false --> Idle animation
            if (animator.GetBool("IsGrabbing"))
                animator.SetBool("IsGrabbing", false);

            //print("IsGrabbing: " + animator.GetBool("IsGrabbing"));

            //print(pose.inputSource + " trigger up");
            Drop();
            StopHolding();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        // We check the tag of whatever triggered
        if (collider.gameObject.CompareTag("Pickupable"))
        {
            // If the object is a pickupable object, we will add it to the list of things that can be picked up
            contact_pickupables.Add(collider.gameObject.GetComponent<Pickupable>());
            print("In contact with pickupable " + collider.gameObject.name);
        }
        else if (collider.gameObject.CompareTag("GloveBox"))
        {
            handRenderer.sharedMaterial = materials[1];
        }
        else if (collider.gameObject.CompareTag("Holdable") && (joint.connectedBody == null))
        {
            //if (collider.GetComponent<Holdable>().is_holdable)
           // {
                print("In contact with holdable " + collider.gameObject.name);
                // If the object is a holdable object, we will add it to the list of things that can be held
                //contact_holdables.Add(collider.gameObject.GetComponent<Holdable>());
                current_holdable = collider.gameObject.GetComponent<Holdable>();
                this.gameObject.layer = 14;
                collider.gameObject.layer = 14;
            //}
            //else
            //{
            //    return;
            //}
        }
        else
        {
            return;
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        
    }

    private void OnTriggerExit(Collider collider)
    {
        // We check the tag of whatever triggered
        if (collider.gameObject.CompareTag("Pickupable"))
        {
            // If the object is a pickupable object, we will remove it from the list of things that can be picked up
            contact_pickupables.Remove(collider.gameObject.GetComponent<Pickupable>());
        }
        else if (collider.gameObject.CompareTag("Holdable"))
        {
            //contact_holdables.Remove(collider.gameObject.GetComponent<Holdable>());
            current_holdable = null;
            this.gameObject.layer = 0;
            collider.gameObject.layer = 9;
        }
    }

    public void Pickup()
    {
        // Get nearest pickupable
        current_pickupable = GetNearestPickupable();

        // Null check (see if we actually have something to pick up)
        if (!current_pickupable)
            return;
        print("Picking up " + current_pickupable.gameObject.name);
        // Check if it is already being held by the other hand
        if (current_pickupable.active_hand)
            current_pickupable.active_hand.Drop();

        // Change physics
        current_pickupable.gameObject.GetComponent<Rigidbody>().useGravity = true;
        current_pickupable.gameObject.GetComponent<Rigidbody>().isKinematic = false;

        // Position
        current_pickupable.transform.position = transform.position;

        // Attach the rigidbody of the pickupable to the fixed joint of the controller
        Rigidbody target_body = current_pickupable.GetComponent<Rigidbody>();
        joint.connectedBody = target_body;

        // Send haptic feedback
        Pulse(0.1f, 150, 75, pose.inputSource);
        //print("bzzzzzzz");

        // Set active hand
        current_pickupable.active_hand = this;
    }

    public void Hold()
    {
        // Get nearest holdable
        //current_holdable = GetNearestHoldable();

        // Null check (see if we actually have something to hold)
        if (!current_holdable)
            return;

        print("Holding " + current_holdable.gameObject.name);
        if (current_holdable.gameObject.name == "Needle")
        {
            // Detach needle from the body
            Destroy(s_body_joint);
        }

        // Set position
        Vector3 distance = injection_site.transform.position - current_holdable.transform.position;
        current_holdable.transform.position = injection_site.transform.position;
        current_holdable.gameObject.GetComponent<Rigidbody>().useGravity = false;
        current_holdable.gameObject.GetComponent<Rigidbody>().isKinematic = true;

        // Attach the rigidbody of the holdable to the fixed joint of the injection site
        Rigidbody target_body = current_holdable.GetComponent<Rigidbody>();
        site_joint.connectedBody = target_body;

        // Send haptic feedback
        Pulse(0.1f, 150, 75, pose.inputSource);
        //print("bzzzzzzz");

        // Set active hand
        current_holdable.active_hand = this;
    }

    public void Drop()
    {
        // Null check
        if (!current_pickupable)
            return;

        // Apply velocity
        Rigidbody target_body = current_pickupable.GetComponent<Rigidbody>();
        target_body.velocity = pose.GetVelocity();
        target_body.angularVelocity = pose.GetAngularVelocity();

        // Detach
        joint.connectedBody = null;

        //Clear
        current_pickupable.active_hand = null;
        current_pickupable = null;
    }

    public void StopHolding()
    {
        // Null check
        if (!current_holdable)
            return;

        print("Stop holding " + current_holdable.name);
        print(current_holdable + " first has tag = " + current_holdable.transform.tag);

        // Detach from the injection site
        site_joint.connectedBody = null;

        // Make Pickupable again
        //current_holdable.gameObject.GetComponent<Rigidbody>().useGravity = false;
        //current_holdable.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        current_holdable.transform.tag = "Pickupable";
        current_holdable.gameObject.AddComponent<Pickupable>();

        this.gameObject.layer = 0;
        current_holdable.gameObject.layer = 9;
        print(current_holdable + " now has tag = " + current_holdable.transform.tag);
        //current_holdable.is_holdable = false;

        //Clear
        current_holdable.active_hand = null;
        current_holdable = null;
    }

    private Pickupable GetNearestPickupable()
    {
        // Variable to store the nearest pickupable
        Pickupable nearest = null;
        float min_distance = float.MaxValue;
        float distance = 0.0f;

        // For each item that can be picked up (contact_pickupables)...
        foreach (Pickupable pickupable in contact_pickupables)
        {
            // Calculate the distance to that pickupable, and...
            distance = (pickupable.transform.position - transform.position).sqrMagnitude;

            // if the distance is the smallest, set that pickupable as the nearest
            if (distance < min_distance)
            {
                min_distance = distance;
                nearest = pickupable;
            }
        }

        return nearest;
    }

    //private Holdable GetNearestHoldable()
    //{
    //    // Variable to store the nearest holdable
    //    Holdable nearest = null;
    //    float min_distance = float.MaxValue;
    //    float distance = 0.0f;

    //    // For each item that can be held (contact_holdables)...
    //    foreach (Holdable holdable in contact_holdables)
    //    {
    //        // Calculate the distance to that holdable, and...
    //        distance = (holdable.transform.position - transform.position).sqrMagnitude;
            
    //        // if the distance is the smallest, set that holdable as the nearest
    //        if (distance < min_distance)
    //        {
    //            min_distance = distance;
    //            nearest = holdable;
    //        }
    //    }

    //    return nearest;
    //}

    private void Pulse(float duration, float frequency, float amplitude, SteamVR_Input_Sources source)
    {
        hapticAction.Execute(0, duration, frequency, amplitude, source);
    }
}
