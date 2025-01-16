using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private GameObject player;
    
    private Vector3 playerPosition;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = player.transform.position;
        mainCamera.transform.position = new Vector3(playerPosition.x, transform.position.y, playerPosition.z - 10);
    }
}
