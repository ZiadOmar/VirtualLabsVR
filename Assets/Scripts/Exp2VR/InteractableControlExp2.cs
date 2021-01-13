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

    int HgColor = 0;

    // Start is called before the first frame update
    void Start()
    {
        answers = GameObject.FindGameObjectsWithTag("AnswerText");
        instructions = GameObject.FindGameObjectWithTag("instructions");
        Player = GameObject.FindGameObjectWithTag("Player");
        HgColor = 0;
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
                        Player.GetComponent<PickupExp2>().SamplesEffect["SnHgCu"].GetComponent<SnHgCu>().enabled = false;
                        Player.GetComponent<PickupExp2>().SamplesEffect["NH4"].GetComponent<NH4>().enabled = true;
                        Player.GetComponent<PickupExp2>().IdentificationPanel.GetComponentInChildren<Text>().text = "A smell of Ammonia is detected";
                        if (name == "NaOH")
                        {
                            Player.GetComponent<PickupExp2>().NaOHDetect = true;
                        }
                        break;
                    case "Ag":
                    case "Pb":
                        Player.GetComponent<PickupExp2>().SamplesEffect["NH4"].GetComponent<NH4>().enabled = false;
                        Player.GetComponent<PickupExp2>().SamplesEffect["SnHgCu"].GetComponent<SnHgCu>().enabled = false;
                        Player.GetComponent<PickupExp2>().SamplesEffect["AgPb"].GetComponent<AgPb>().enabled = true;
                        Player.GetComponent<PickupExp2>().IdentificationPanel.GetComponentInChildren<Text>().text = "Smell of Ammonia is NOT detected";
                        if (name == "NaOH")
                        {
                            Player.GetComponent<PickupExp2>().NaOHDetect = true;
                        }
                        break;
                    case "Sn":
                    case "Hg":
                    case "Cu":
                        Player.GetComponent<PickupExp2>().SamplesEffect["NH4"].GetComponent<NH4>().enabled = false;
                        Player.GetComponent<PickupExp2>().SamplesEffect["AgPb"].GetComponent<AgPb>().enabled = false;
                        Player.GetComponent<PickupExp2>().SamplesEffect["SnHgCu"].GetComponent<SnHgCu>().enabled = true;
                        Player.GetComponent<PickupExp2>().IdentificationPanel.GetComponentInChildren<Text>().text = "Smell of Ammonia is NOT detected";
                        if (name == "NaOH" && instructions.GetComponent<Instructions>().currentStep == 3)
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
                    case "Sn":
                    case "Hg":
                    case "Cu":
                        if (Player.GetComponent<PickupExp2>().NaOHDetect)
                        {
                            Player.GetComponent<PickupExp2>().NaOHDetect = false;
                            Player.GetComponent<PickupExp2>().IdentificationPanel.SetActive(true);
                            StartCoroutine("WaitForIdentification");
                            this.tag = "Interactable"; //used vial
                            CreateVial(gameObject);
                        }

                        if (instructions.GetComponent<Instructions>().currentStep == 11) //Change Sample Color
                        {
                            switch (sampleTakenName)
                            {
                                case "Sn":
                                    transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color32(154, 48, 0, 255); //Dark Brown
                                    break;
                                case "Hg":
                                    transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color32(255, 255, 255, 255); //White then Yellow then Brown then Black
                                    StartCoroutine(WaitForHgColor(HgColor));
                                    break;
                                case "Cu":
                                    transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color32(75, 24, 0, 255); //Black Brown
                                    break;
                            }
                            this.tag = "Interactable"; //used vial
                            CreateVial(gameObject);
                            //instructions.GetComponent<Instructions>().NextInstruction(instructions.GetComponent<Instructions>().currentStep);
                        }
                        if (instructions.GetComponent<Instructions>().currentStep == 16) //Change Sample Color
                        {
                            switch (sampleTakenName)
                            {
                                case "Sn":
                                    transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color32(255, 255, 255, 255); //White
                                    Player.GetComponent<PickupExp2>().IdentificationPanel.GetComponentInChildren<Text>().text = "The sample dissolves";
                                    Player.GetComponent<PickupExp2>().IdentificationPanel.SetActive(true);
                                    StartCoroutine("WaitForIdentification");
                                    break;
                                case "Hg":
                                    transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color32(248, 61, 0, 255); //Reddish Brown then Black
                                    StartCoroutine(WaitForHgColor(0));
                                    Player.GetComponent<PickupExp2>().IdentificationPanel.GetComponentInChildren<Text>().text = "The sample doesn't dissolve";
                                    Player.GetComponent<PickupExp2>().IdentificationPanel.SetActive(true);
                                    StartCoroutine("WaitForIdentification");
                                    break;
                                case "Cu":
                                    transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color32(32, 0, 255, 255); //Blue
                                    Player.GetComponent<PickupExp2>().IdentificationPanel.GetComponentInChildren<Text>().text = "The sample doesn't dissolve";
                                    Player.GetComponent<PickupExp2>().IdentificationPanel.SetActive(true);
                                    StartCoroutine("WaitForIdentification");
                                    break;
                            }
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
                case "Sn":
                case "Hg":
                case "Cu":
                    instructions.GetComponent<Instructions>().NextInstruction(instructions.GetComponent<Instructions>().currentStep);
                    break;
            }
            newDispenser.transform.position = Vector3.zero;
        }

        if (tag == "Dispose" && other.tag == "Interactable" && other.name == "vial")
        {
            Player.GetComponent<PickupExp2>().newVial.SetActive(true);
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
        Player.GetComponent<PickupExp2>().newVial.SetActive(false);
    }

    IEnumerator WaitForIdentification()
    {
        yield return new WaitForSecondsRealtime(5f);
        Player.GetComponent<PickupExp2>().IdentificationPanel.SetActive(false);
        Player.GetComponent<PickupExp2>().Heat.transform.GetChild(0).gameObject.SetActive(false);
    }

    IEnumerator WaitForHgColor(int HgColorChange)
    {
        yield return new WaitForSecondsRealtime(2f);
        if (instructions.GetComponent<Instructions>().currentStep == 16)
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color32(0, 0, 0, 255); //Reddish Brown then Black
        }
        else
        {
            switch (HgColorChange)
            {
                case 0:
                    transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color32(255, 255, 0, 255); //Yellow then Brown
                    HgColor++;
                    StartCoroutine(WaitForHgColor(HgColor));
                    break;             
                case 1:
                    transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color32(142, 32, 0, 255); //Brown then Black
                    HgColor++;
                    StartCoroutine(WaitForHgColor(HgColor));
                    break;
                case 2:
                    transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color32(0, 0, 0, 255); // Black
                    HgColor++;
                    break;
            }
        }
    }
}

