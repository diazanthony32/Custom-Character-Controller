using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Camera : MonoBehaviour
{
    internal Player playerScript;

    [SerializeField] internal GameObject _cameraHolder;


    [SerializeField] internal bool _isFirstPerson = true;
    //[SerializeField] internal bool _isMovementTiedToCamera = true;

    [SerializeField] internal float _camXSensitivity = 1.0f;
    [SerializeField] internal float _camYSensitivity = 1.0f;

    internal Vector3 _lookDirection;
    internal float _lookRotation;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("\"Player_Camera\" script starting...");
        playerScript = GetComponent<Player>();

        // error checking
        if (!playerScript || !_cameraHolder)
        {
            Debug.LogError("Something isn't right here... Please check if all values have been assigned (Player_Camera.cs)");
        }


        // enables and disables the approptiate camera inside tha camera holder
        if (_isFirstPerson)
        {
            //Debug.Log(_cameraHolder.GetComponentsInChildren<Camera>());
            Camera[] cameras = _cameraHolder.GetComponentsInChildren<Camera>();
            cameras[0].gameObject.SetActive(true);
            cameras[1].gameObject.SetActive(false);
        }
        else {
            //Debug.Log(_cameraHolder.GetComponentsInChildren<Camera>());
            Camera[] cameras = _cameraHolder.GetComponentsInChildren<Camera>();
            cameras[0].gameObject.SetActive(false);
            cameras[1].gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLook(InputValue value)
    {

        // gets the look vector based on player input
        _lookDirection = value.Get<Vector2>();

        if (_isFirstPerson)
        {
            // GOAL:
            // move camera in place with the player's inputed action 

            playerScript.transform.Rotate(Vector3.up * _lookDirection.x * _camXSensitivity);

            _lookRotation += (-_lookDirection.y * _camYSensitivity);
            _lookRotation = Mathf.Clamp(_lookRotation, -90.0f, 90.0f);

            _cameraHolder.transform.eulerAngles = new Vector3(_lookRotation, _cameraHolder.transform.eulerAngles.y, _cameraHolder.transform.eulerAngles.z);

        }
        else
        {
            // GOAL:
            // move camera in spherical pattern to keep a specific distance from the player's model  -- okay
            // provide basic camera collision detection
            // unclamp player rotation from camera movement -- okay
            // rotate player model based on movement inputs relative to camera


            // rotate player's 3rd person camera's x rotation around player model without affecting player movement
            _cameraHolder.transform.RotateAround(playerScript.gameObject.transform.position, Vector3.up, _lookDirection.x * _camXSensitivity);

            // rotate player's 3rd person camera's y rotation based on y axis movement clamped at a total rotation of 180 degrees from top to bottom
            _lookRotation += (-_lookDirection.y * _camYSensitivity);
            _lookRotation = Mathf.Clamp(_lookRotation, -115.0f, 65.0f);

            _cameraHolder.transform.eulerAngles = new Vector3(_lookRotation, _cameraHolder.transform.eulerAngles.y, _cameraHolder.transform.eulerAngles.z);
        }

    }
}
