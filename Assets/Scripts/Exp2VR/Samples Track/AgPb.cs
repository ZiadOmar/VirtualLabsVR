using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgPb : MonoBehaviour
{
    public GameObject instructions;
    string[] instructionText = new string[17];

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
        instructionText[9] = "Heat the test sample and observe the effect";
        instructionText[10] = "Throw away the used test flask in the trash can";
        instructionText[11] = "Hold the dispenser & Take another sample from the tested flask";
        instructionText[12] = "Drop the sample in the new test container";
        instructionText[13] = "Hold the dispenser again & Take a sample from K2CrO4";
        instructionText[14] = "Drop the sample in the test container";
        instructionText[15] = "Throw away the used dispenser in the trash can";
        instructionText[16] = "Observe the color change and effect and answer the question";
        instructions= GameObject.FindGameObjectWithTag("instructions");
        instructions.GetComponent<Instructions>().instructionText = instructionText;
    }

    void Update()
    {
        instructions.GetComponent<Instructions>().instructionText = instructionText;
    }
}
