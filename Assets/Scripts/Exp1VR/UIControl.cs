using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIControl : MonoBehaviour
{
    public bool rightAnswer;

    public string correctSubstance;
    GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    public void SetCorrectSubstance(string sampleTaken)
    {
        sampleTaken = correctSubstance;
    }

    // Update is called once per frame
    void Update()
    {
        if(correctSubstance == name)
        {
            rightAnswer = true;
        }      
    }
    public void RightAnswerChosen()
    {
        StartCoroutine(FlashGreen(4));
    }

    public void WrongAnswerChosen()
    {
        StartCoroutine(FlashRed(4));
    }

    IEnumerator FlashGreen(int loops)
    {
        Debug.Log(System.DateTime.Now.ToString());

        GetComponent<TextMesh>().color = Color.green;

        yield return new WaitForSecondsRealtime(2f);


        if (loops != 0)
            StartCoroutine(FlashWhite(loops - 1));
        else
        {
            AllowCursor();  // To allow Cursor            
            Player.GetComponent<Pickup>().AnswerIndicator.transform.GetChild(0).gameObject.SetActive(false);
            Player.GetComponent<Pickup>().chosen = true;
            GameObject.FindGameObjectWithTag("EndExperiment").transform.GetChild(0).gameObject.SetActive(true);
            StartCoroutine("LoadMainScene");
        }
    }

    IEnumerator FlashRed(int loops)
    {
        Debug.Log(System.DateTime.Now.ToString());

        GetComponent<TextMesh>().color = Color.red;

        yield return new WaitForSecondsRealtime(2f);

        if (loops != 0)
            StartCoroutine(FlashWhite(loops - 1));

        Player.GetComponent<Pickup>().AnswerIndicator.transform.GetChild(1).gameObject.SetActive(false);
    }
    IEnumerator FlashWhite(int loops)
    {
        Debug.Log(System.DateTime.Now.ToString());

        GetComponent<TextMesh>().color = Color.white;

        yield return new WaitForSecondsRealtime(1);


        if (loops != 0 && rightAnswer)
        {
            StartCoroutine(FlashGreen(loops - 1));
        }
        else if (loops != 0 && !rightAnswer)
        {
            StartCoroutine(FlashRed(loops - 1));
        }
    }

    public void AllowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    IEnumerator LoadMainScene()
    {
        yield return new WaitForSecondsRealtime(5f);
        SceneManager.LoadScene("MainScene");
    }
}
