using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSlash : MonoBehaviour
{ 
    private bool facingLeft;
    private bool facingLeftValidate;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        Player playerScript = player?.GetComponent<Player>();
        facingLeft = playerScript.PlayerFaceing();
        facingLeftValidate = playerScript.PlayerFaceing();

    }

    void Update()
    {
        Player playerScript = player?.GetComponent<Player>();
        facingLeft = playerScript.PlayerFaceing();

        if (playerScript != null && facingLeft != facingLeftValidate)
    {
            //Debug.Log(facingLeft);

            float xRotation = -90f;
            float yRotation = 0f;
            float zRotation = facingLeft ? 180f : 0f;

            transform.rotation = Quaternion.Euler(xRotation, yRotation, zRotation);

            facingLeft = playerScript.PlayerFaceing();
            facingLeftValidate = playerScript.PlayerFaceing();
        }
    }
}

