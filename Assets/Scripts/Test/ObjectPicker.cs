using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class ObjectPicker : MonoBehaviour
{
    public GameObject player;
    private bool looking = false;
    public float minDistance = 10.0f;
    private float distance;

    public bool interactableHit;
    public GameObject hitObject;

    // Use this for initialization
    void Start()
    {
        looking = false;
    }
    // Update is called once per frame
    void Update()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;
        RaycastHit hit;

        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            //Outline selected object
            if (hit.collider.gameObject.tag == "Interactable" && !interactableHit)
            {
                hitObject = hit.collider.gameObject;
                if (Vector3.Distance(Camera.main.transform.position, hitObject.transform.position) < 10f)
                {
                    distance = Vector3.Distance(player.transform.position, hitObject.transform.position);
                    if (looking)
                    {
                        if (distance <= minDistance)
                        {
                            if (Input.GetButtonDown("Fire1"))
                            {
                                interactableHit = true;
                            }
                        }
                    }
                }
            }
        }
        //Hold the interactable project
        if (interactableHit)
        {
            //To hold the object
            hitObject.transform.parent = Camera.main.transform;
            //hitObject.transform.parent = player.transform;

            //Make it easy to move the object
            hitObject.GetComponent<Rigidbody>().isKinematic = true;
            hitObject.GetComponent<Rigidbody>().useGravity = false;


            hitObject.GetComponent<Collider>().enabled = false;
        }

        if (Input.GetButtonUp("Fire1") && interactableHit)
        {
            //Reset all the variables  
            interactableHit = false;
            hitObject.transform.parent = null;

            hitObject.GetComponent<Collider>().enabled = true;

            hitObject.GetComponent<Rigidbody>().useGravity = true;
            hitObject.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    #region IGvrGazeResponder implementation
    /// Called when the user is looking on a GameObject with this script,
    /// as long as it is set to an appropriate layer (see GvrGaze).
    public void OnGazeEnter()
    {
        looking = true;
    }
    /// Called when the user stops looking on the GameObject, after OnGazeEnter
    /// /// was already called.
    public void OnGazeExit()
    {
        looking = false;
    }
    /// Called when the viewer's trigger is used, between OnGazeEnter and OnGazeExit.
    public void OnGazeTrigger()
    {
        hitObject.GetComponent<Renderer>().material.color = (looking && distance <= minDistance) ?
        Color.green : Color.red;
    }
    #endregion
}