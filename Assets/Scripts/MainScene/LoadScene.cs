using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PickExp>().interactableHit)
        {
            player.GetComponent<PickExp>().hitObject.GetComponent<Button>().onClick.Invoke();
        }

    }

    public void LoadExperiment(string sceneName)
    {
        StartCoroutine(LoadSceneWait(sceneName));
    }

    IEnumerator LoadSceneWait(string sceneName)
    {
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadScene(sceneName);
    }
}
