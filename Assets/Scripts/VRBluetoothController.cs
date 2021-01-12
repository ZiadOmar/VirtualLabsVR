using UnityEngine;
using System.Collections;
[RequireComponent(typeof(CharacterController))]
public class VRBluetoothController : MonoBehaviour
{
    public VRTeleporter teleporter;
    // VR Main Camera
    private Transform vrCamera;
    // Speed to move the player
    public float speed = 3f;
    // CharacterController script
    CharacterController myCC;
    // Use this for initialization
    void Start()
    {
        // Find the CharacterController
        myCC = gameObject.GetComponent<CharacterController>();
        // Find the Main Camera
        vrCamera = Camera.main.transform;
    }
    // Update is called once per frame
    void Update()
    {
        // Move with SimpleMove based on Horizontal adn Vertical input
        myCC.SimpleMove(speed * vrCamera.TransformDirection(Vector3.forward * Input.GetAxis("Vertical") +
       Vector3.right * Input.GetAxis("Horizontal")));

        // Bluetooth Controller: A
        if (Input.GetButtonDown("Fire2"))
        {
            teleporter.ToggleDisplay(true);
        }

        if (Input.GetButtonUp("Fire2"))
        {
            teleporter.Teleport();
            teleporter.ToggleDisplay(false);
        }
        
    }
}