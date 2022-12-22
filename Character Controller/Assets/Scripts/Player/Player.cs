using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    //Store a reference to all the sub player scripts
    [Header("Player Scripts")]
    internal Player_Movement playerMovement;
    internal Player_Collision playerCollision;

    //component references
    internal Animator playerAnim;
    internal Rigidbody playerRB;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("\"Player\" script starting...");

        playerMovement = GetComponent<Player_Movement>();
        playerCollision = GetComponent<Player_Collision>();

        playerAnim = GetComponent<Animator>();
        playerRB = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
