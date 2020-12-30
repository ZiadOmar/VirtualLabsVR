using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class InteractableControlExp2 : MonoBehaviour
{
    public bool pullFrom;

    public string sampleTakenName;

    public GameObject[] answers;

    public GameObject instructions;

    GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        answers = GameObject.FindGameObjectsWithTag("AnswerText");
        instructions = GameObject.FindGameObjectWithTag("instructions");
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        sampleTakenName = Player.GetComponent<PickupExp2>().sampleTaken;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (tag == "Substance" && other.tag == "Interactable" && other.name != "pH paper" && !other.GetComponent<Animator>().GetBool("StartSubstance") && pullFrom)
        {

            GetComponent<Animator>().SetBool("decrease", true);
            other.transform.position = new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z);
            other.GetComponent<Animator>().SetBool("StartSubstance", true);

            if (!Player.GetComponent<PickupExp2>().SampleInProgress)
            {
                Player.GetComponent<PickupExp2>().sampleTaken = name;
                instructions.GetComponent<Instructions>().NextInstruction(1);
            }
            else
            {
                instructions.GetComponent<Instructions>().NextInstruction(instructions.GetComponent<Instructions>().currentStep);

                switch (sampleTakenName)
                {
                    case "NH4":
                        Player.GetComponent<PickupExp2>().SamplesEffect["AgPb"].GetComponent<AgPb>().enabled = false;
                        Player.GetComponent<PickupExp2>().IdentificationPanel.GetComponentInChildren<Text>().text = "A smell of Ammonia is detected";
                        if (name == "NaOH")
                        {
                            Player.GetComponent<PickupExp2>().NaOHDetect = true;
                        }
                        break;
                    case "Ag":
                    case "Pb":
                        Player.GetComponent<PickupExp2>().SamplesEffect["NH4"].GetComponent<NH4>().enabled = false;
                        Player.GetComponent<PickupExp2>().IdentificationPanel.GetComponentInChildren<Text>().text = "Smell of Ammonia is NOT detected";
                        if (name == "NaOH")
                        {
                            Player.GetComponent<PickupExp2>().NaOHDetect = true;
                        }
                        break;
                }
            }
        }

        if (tag == "Substance" && other.tag == "Interactable" && other.GetComponent<Animator>().GetBool("StartSubstance") && !pullFrom)
        {

            GetComponent<Animator>().SetBool("increase", true);
            other.transform.position = new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z);
            other.GetComponent<Animator>().SetBool("StartSubstance", false);

            if (!Player.GetComponent<PickupExp2>().SampleInProgress)
            {
                instructions.GetComponent<Instructions>().NextInstruction(2);
                Player.GetComponent<PickupExp2>().SampleInProgress = true;
            }
            else
            {
                instructions.GetComponent<Instructions>().NextInstruction(instructions.GetComponent<Instructions>().currentStep);

                switch (sampleTakenName)
                {
                    case "NH4":
                        if (Player.GetComponent<PickupExp2>().NaOHDetect)
                        {
                            Player.GetComponent<PickupExp2>().NaOHDetect = false;
                            Player.GetComponent<PickupExp2>().IdentificationPanel.SetActive(true);
                            StartCoroutine("WaitForIdentification");
                        }
                        break;
                    case "Ag": 
                    case "Pb":
                        if (Player.GetComponent<PickupExp2>().NaOHDetect)
                        {
                            Player.GetComponent<PickupExp2>().NaOHDetect = false;
                            Player.GetComponent<PickupExp2>().IdentificationPanel.SetActive(true);
                            StartCoroutine("WaitForIdentification");
                            this.tag = "Interactable"; //used vial
                            CreateVial(gameObject);
                        }
                     
                        if (instructions.GetComponent<Instructions>().currentStep == 9) //Change Sample Color
                        {
                            Player.GetComponent<PickupExp2>().Heat.transform.GetChild(0).gameObject.SetActive(true);
                            transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.white;
                            if (sampleTakenName == "Ag")
                            {
                                Player.GetComponent<PickupExp2>().IdentificationPanel.GetComponentInChildren<Text>().text = "The sample doesn't dissolve with Heat";
                                Player.GetComponent<PickupExp2>().IdentificationPanel.SetActive(true);
                            }
                            else //Pb
                            {
                                Player.GetComponent<PickupExp2>().IdentificationPanel.GetComponentInChildren<Text>().text = "The sample dissolves with Heat";
                                Player.GetComponent<PickupExp2>().IdentificationPanel.SetActive(true);                           
                            }
                            this.tag = "Interactable"; //used vial
                            CreateVial(gameObject);
                            StartCoroutine("WaitForIdentification");
                            instructions.GetComponent<Instructions>().NextInstruction(instructions.GetComponent<Instructions>().currentStep);
                        }

                        if (instructions.GetComponent<Instructions>().currentStep == 15) //Change Sample Color
                        {
                            if (sampleTakenName == "Ag")
                                transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color32(173, 0, 1, 255); //Dark Red
                            else //Pb
                                transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color32(255, 255, 0, 255); //Yellow      
                        }
                        break;
                }
            }


            for (int i = 0; i < answers.Length; i++)
            {
                if (answers[i].name == sampleTakenName)
                {
                    Debug.Log("found it" + answers[i]);
                    answers[i].GetComponent<UIControlExp2>().rightAnswer = true;
                    Debug.Log(answers[i].GetComponent<UIControlExp2>().rightAnswer);
                }
                answers[i].GetComponent<UIControlExp2>().SetCorrectSubstance(sampleTakenName);
            }
        }

        if (tag == "Dispose" && other.tag == "Interactable" && other.name == "dispenser")
        {

            GameObject newDispenser = Instantiate(other.gameObject);
            newDispenser.transform.parent = GameObject.FindGameObjectWithTag("DispenserStand").transform;
            Destroy(other.gameObject);

            switch (sampleTakenName)
            {
                case "NH4":
                    instructions.GetComponent<Instructions>().NextInstruction(instructions.GetComponent<Instructions>().currentStep);
                    Player.GetComponent<PickupExp2>().SamplesEffect["NH4"].transform.GetChild(0).gameObject.SetActive(true);
                    break;

                case "Ag":
                case "Pb":
                    instructions.GetComponent<Instructions>().NextInstruction(instructions.GetComponent<Instructions>().currentStep);
                    break;
            }

            newDispenser.transform.position = Vector3.zero;

        }

        if (tag == "Dispose" && other.tag == "Interactable" && other.name == "vial")
        {
            instructions.GetComponent<Instructions>().NextInstruction(instructions.GetComponent<Instructions>().currentStep);
            Destroy(other.gameObject);
            Player.GetComponent<PickupExp2>().SampleInProgress = false;
        }
    }

    public void CreateVial(GameObject oldVial)
    {
         Player.GetComponent<PickupExp2>().newVial = Instantiate(Player.GetComponent<PickupExp2>().oldVial);
         Player.GetComponent<PickupExp2>().newVial.transform.parent = oldVial.transform.parent;
         Player.GetComponent<PickupExp2>().newVial.transform.position = oldVial.transform.position;
         Player.GetComponent<PickupExp2>().newVial.tag = "Substance";
         Player.GetComponent<PickupExp2>().newVial.name = "vial";
    }

    IEnumerator WaitForIdentification()
    {
        yield return new WaitForSecondsRealtime(5f);
        Player.GetComponent<PickupExp2>().IdentificationPanel.SetActive(false);
        Player.GetComponent<PickupExp2>().Heat.transform.GetChild(0).gameObject.SetActive(false);
    }
}

