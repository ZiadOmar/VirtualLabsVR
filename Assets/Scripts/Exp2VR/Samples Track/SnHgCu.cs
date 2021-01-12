using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnHgCu : MonoBehaviour
{
    public GameObject instructions;
    string[] instructionText = new string[18];

    // Start is called before the first frame update
    void Start()
    {
        instructionText[0] = "Hold the dispenser & Take a sample from one of the many flasks";
        instructionText[1] = "Drop the sample in the empty test container";
        instructionText[2] = "Hold the dispenser again & Take a sample from NaOH";
        instructionText[3] = "Drop the sample in the test container";
        instructionText[4] = "Throw away the used test flask in the trash can";
        instructionText[5] = "Hold the dispenser & Take another sample from the tested flask";
        instructionText[6] = "Drop the sample in the new test container";
        instructionText[7] = "Hold the dispenser again & Take a sample from HCl";
        instructionText[8] = "Drop the sample in the test container";
        instructionText[9] = "Hold the dispenser again & Take a sample from H2S";
        instructionText[10] = "Drop the sample in the test container";
        instructionText[11] = "Throw away the used test flask in the trash can";
        instructionText[12] = "Hold the dispenser & Take another sample from the tested flask";
        instructionText[13] = "Drop the sample in the new test container";
        instructionText[14] = "Hold the dispenser again & Take a sample from NaOH";
        instructionText[15] = "Drop the sample in the test container";
        instructionText[16] = "Throw away the used dispenser in the trash can";
        instructionText[17] = "Observe the color change and effect and answer the question";
        instructions = GameObject.FindGameObjectWithTag("instructions");
        instructions.GetComponent<Instructions>().instructionText = instructionText;
    }

    // Update is called once per frame
    void Update()
    {
        instructions.GetComponent<Instructions>().instructionText = instructionText;
    }
}
