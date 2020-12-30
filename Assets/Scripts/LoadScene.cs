using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
