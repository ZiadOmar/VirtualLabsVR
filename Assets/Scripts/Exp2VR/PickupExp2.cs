using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PickupExp2 : MonoBehaviour
{
    public bool looking = false;
    public float minDistance = 10.0f;
    private float distance;
    public bool interactableHit;

    public bool chosen;

    public GameObject hitObject;
    GameObject previewItem;

    public Text objectName;

    public GameObject instructions;
    public GameObject IdentificationPanel;
    public Dictionary<string, GameObject> SamplesEffect = new Dictionary<string, GameObject>();
    public GameObject[] Samples;
    public bool SampleInProgress;
    public string sampleTaken;
    public bool NaOHDetect;
    public GameObject oldVial;
    public GameObject newVial;
    public GameObject Heat;
    public GameObject DispenserPointer;
    public GameObject pHPointer;

    Quaternion originalRotation;

    public GameObject AnswerIndicator;


    // Start is called before the first frame update
    void Start()
    {
        looking = false;
        SamplesEffect.Add("NH4", Samples[0]);
        SamplesEffect.Add("AgPb", Samples[1]);
        SamplesEffect.Add("SnHgCu", Samples[2]);
    }

    // Update is called once per frame
    void Update()
    {
        // Continue initializing after destroying of oldVial
        SamplesEffect["NH4"] = GameObject.FindGameObjectWithTag("NH4");
        SamplesEffect["AgPb"] = GameObject.FindGameObjectWithTag("AgPb");
        SamplesEffect["SnHgCu"] = GameObject.FindGameObjectWithTag("SnHgCu");
        Heat = GameObject.FindGameObjectWithTag("Heat");

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
                    distance = Vector3.Distance(Camera.main.transform.position, hitObject.transform.position);
                    hitObject.GetComponent<Outline>().OutlineWidth = 5;
                    originalRotation = hitObject.transform.rotation;
                    objectName.text = hitObject.name;

                    if (looking && distance <= minDistance)
                    {
                        // Bluetooth Controller: C
                        if (Input.GetButtonDown("Fire1"))
                        {
                            interactableHit = true;
                            if (hitObject.name == "dispenser")
                            {
                                DispenserPointer.SetActive(false);
                            }    
                        }
                    }
                }
            }
            else if (hit.collider.gameObject.tag == "AnswerText" && !interactableHit)
            {
                hitObject = hit.collider.gameObject;
                if (!chosen)
                    hitObject.GetComponent<TextMesh>().color = Color.blue;
                if (Input.GetButtonDown("Fire1"))
                {
                    if (hitObject.GetComponent<UIControlExp2>().rightAnswer)
                    {
                        hitObject.GetComponent<UIControlExp2>().RightAnswerChosen();
                        AnswerIndicator.transform.GetChild(0).gameObject.SetActive(true); //True
                        AnswerIndicator.transform.GetChild(1).gameObject.SetActive(false);
                        chosen = false;
                    }
                    else
                    {
                        hitObject.GetComponent<UIControlExp2>().WrongAnswerChosen();
                        AnswerIndicator.transform.GetChild(0).gameObject.SetActive(false);
                        AnswerIndicator.transform.GetChild(1).gameObject.SetActive(true); //False
                        chosen = false;
                    }
                }
            }
            else
            {
                if (!interactableHit)
                    objectName.text = "None";

                //for (int i = 0; i < GameObject.FindGameObjectsWithTag("Interactable").Length; i++)
                //    GameObject.FindGameObjectsWithTag("Interactable")[i].GetComponent<Outline>().OutlineWidth = 0;

                if (chosen)
                    for (int j = 0; j < GameObject.FindGameObjectsWithTag("AnswerText").Length; j++)
                        GameObject.FindGameObjectsWithTag("AnswerText")[j].GetComponent<TextMesh>().color = Color.white;
            }
        }


        for (int j = 0; j < GameObject.FindGameObjectsWithTag("AnswerText").Length; j++)
            if (hitObject != GameObject.FindGameObjectsWithTag("AnswerText")[j])
                GameObject.FindGameObjectsWithTag("AnswerText")[j].GetComponent<TextMesh>().color = Color.white;



        //Hold the interactable project
        if (interactableHit)
        {
            //To hold the object
            hitObject.transform.parent = Camera.main.transform;

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

            hitObject.transform.rotation = originalRotation;

            Destroy(previewItem);
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