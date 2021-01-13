using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class InteractableControl : MonoBehaviour
{
    public bool pullFrom;

    bool PHDone;

    public string sampleTaken;

    public GameObject[] answers;

    public GameObject instructions;

    GameObject Player;


    // Start is called before the first frame update
    void Start()
    {
        answers = GameObject.FindGameObjectsWithTag("AnswerText");
        Player = GameObject.FindGameObjectWithTag("Player");
        PHDone = false;
    }

    // Update is called once per frame
    void Update()
    {     
    }
    private void OnTriggerEnter(Collider other)
    {
        if (tag == "Substance" && other.tag == "Interactable" && other.name != "pH paper" && !other.GetComponent<Animator>().GetBool("StartSubstance") && pullFrom)
        {

            GetComponent<Animator>().SetBool("decrease", true);
            other.transform.position = new Vector3(transform.position.x, transform.position.y + 0.7f , transform.position.z);
            other.GetComponent<Animator>().SetBool("StartSubstance", true);

            other.GetComponent<InteractableControl>().sampleTaken = name;

            instructions.GetComponent<Instructions>().NextInstruction(1);

}

        if (tag == "Substance" && other.tag == "Interactable" && other.name != "pH paper" && other.GetComponent<Animator>().GetBool("StartSubstance") && !pullFrom)
        {
            GetComponent<Animator>().SetBool("increase", true);
            other.transform.position = new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z);
            other.GetComponent<Animator>().SetBool("StartSubstance", false);

            sampleTaken = other.GetComponent<InteractableControl>().sampleTaken;
            instructions.GetComponent<Instructions>().NextInstruction(2);  
            Debug.Log(sampleTaken);
            for (int i = 0; i < answers.Length; i++)
            {
                if(answers[i].name == sampleTaken)
                {
                    Debug.Log("found it " + answers[i]);
                    answers[i].GetComponent<UIControl>().rightAnswer = true;
                    Debug.Log(answers[i].GetComponent<UIControl>().rightAnswer);
                }
                answers[i].GetComponent<UIControl>().SetCorrectSubstance(sampleTaken);
            }
        }

        if(tag == "Dispose" && other.tag == "Interactable")
        {
            GameObject newDispenser = Instantiate(other.gameObject);
            newDispenser.transform.parent = GameObject.FindGameObjectWithTag("DispenserStand").transform;
            instructions.GetComponent<Instructions>().NextInstruction(3);
            Player.GetComponent<Pickup>().pHPointer.SetActive(true); // pick pH
            Destroy(other.gameObject);

            newDispenser.transform.position = Vector3.zero;

        }

        if (tag == "Substance" && other.tag == "Interactable" && other.name == "pH paper" && !pullFrom && !PHDone)
        {
            instructions.GetComponent<Instructions>().NextInstruction(5);
            
            switch (sampleTaken)
            {
                case "SO3":
                    other.GetComponent<MeshRenderer>().material.color = Color.green;
                    Debug.Log(other.transform.GetChild(0).GetComponent<MeshRenderer>().material.color);
                    break;
                case "S2O3":
                    other.GetComponent<MeshRenderer>().material.color = Color.green;
                    transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.yellow;
                    break;
                case "S2":
                    other.GetComponent<MeshRenderer>().material.color = Color.black;
                    break;
                case "NO2":
                    other.GetComponent<MeshRenderer>().material.color = Color.blue;
                    transform.GetChild(1).gameObject.SetActive(true);
                    break;


            }

            other.transform.position = new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z);
            other.transform.rotation = new Quaternion(90, 90, 180, 0);
            PHDone = true;
        }
    }

   

}
