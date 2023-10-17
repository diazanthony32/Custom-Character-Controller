using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Input : MonoBehaviour
{

    internal Player playerScript;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("\"Player_Input\" script starting...");

        playerScript = GetComponent<Player>();

        // error checking
        if (!playerScript)
        {
            Debug.LogError("Something isn't right here... Please check if all values have been assigned (Player_Input.cs)");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
