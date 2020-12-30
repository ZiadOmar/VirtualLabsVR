using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class GazeTrigger : MonoBehaviour
{
    GameObject Player;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    public void OnPointerEnter(BaseEventData eventData)
    {
        Player.GetComponent<PickupExp2>().OnGazeEnter();
    }

    public void OnPointerExit(BaseEventData eventData)
    {
        Player.GetComponent<PickupExp2>().OnGazeExit();
    }

  
}

