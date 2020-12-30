using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Instructions : MonoBehaviour
{
    public Text instructions;

    public string[] instructionText;

    public int currentStep;
    // Start is called before the first frame update
    void Start()
    {
        currentStep = 0;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = instructionText[currentStep];
    }
    public void NextInstruction(int step)
    {
        if(SceneManager.GetSceneByName("Experiment1VR").isLoaded)
        {
            if (step == currentStep)
            {
                currentStep++;
                Debug.Log(currentStep);

            }
        }

        if (SceneManager.GetSceneByName("Experiment2VR").isLoaded)
        {
            currentStep++;
            Debug.Log(currentStep);
        }   
    }
}
