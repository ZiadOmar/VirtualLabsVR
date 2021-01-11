using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PickExp : MonoBehaviour
{
    public bool looking = false;
    public bool interactableHit;
    public GameObject hitObject;

    // Start is called before the first frame update
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
            if (hit.collider.gameObject.tag == "Interactable" && !interactableHit)
            {
                hitObject = hit.collider.gameObject;
                    if (looking)
                    {
                        if (Input.GetButtonDown("Fire1"))
                        {
                            interactableHit = true;
                        }
                    }
            }
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
    ///// Called when the viewer's trigger is used, between OnGazeEnter and OnGazeExit.
    //public void OnGazeTrigger()
    //{    
    //}
    #endregion
}