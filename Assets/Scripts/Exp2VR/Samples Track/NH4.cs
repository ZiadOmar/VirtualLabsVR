using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NH4 : MonoBehaviour
{
    public GameObject instructions;
    string[] instructionText = new string[8];


    // Start is called before the first frame update
    void Start()
    {
        instructionText[0] = "Hold the dispenser & Take a sample from one of the many flasks"; 
        instructionText[1] = "Drop the sample in the empty test container";
        instructionText[2] = "Hold the dispenser again & Take a sample from NaOH";
        instructionText[3] = "Drop the sample in the test container";
        instructionText[4] = "Hold the dispenser again & Take a sample from HCl";
        instructionText[5] = "Drop the sample in the test container";
        instructionText[6] = "Throw away the used dispenser in the trash can";
        instructionText[7] = "Observe the effect and answer the question";
        instructions = GameObject.FindGameObjectWithTag("instructions");
        instructions.GetComponent<Instructions>().instructionText = instructionText;
    }

    void Update()
    {
        instructions.GetComponent<Instructions>().instructionText = instructionText;
    }
}
