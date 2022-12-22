using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Input : MonoBehaviour
{

    internal Player playerScript;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("\"Player_Input\" script starting...");

        playerScript = GetComponent<Player>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
