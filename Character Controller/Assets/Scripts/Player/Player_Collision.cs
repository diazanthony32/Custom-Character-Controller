using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Collision : MonoBehaviour
{

    internal Player playerScript;

    [Header("Ground: ")]
    [SerializeField] internal LayerMask m_groundLayer;                          // A mask determining what is ground to the character
    [SerializeField] internal Transform _groundMarker;                          // Bottom transform of the bottom of the player 
    [SerializeField] internal float _groundCheckRadius = 0.25f;                 // How wide should the ground check be from the players feet

    private Collider[] _hitColliders = new Collider[10];
    internal bool isGrounded => Physics.OverlapSphereNonAlloc(_groundMarker.position, _groundCheckRadius, _hitColliders, m_groundLayer, QueryTriggerInteraction.Ignore) > 0;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("\"Player_Collision\" script starting...");

        playerScript = GetComponent<Player>();

        // error checking
        if (!playerScript || !_groundMarker || m_groundLayer < 0)
        {
            Debug.LogError("Something isn't right here... Please check if all values have been assigned");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawSphere(_groundMarker.position, _groundCheckRadius);

    }
}
