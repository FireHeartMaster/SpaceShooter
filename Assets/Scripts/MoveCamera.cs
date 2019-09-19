using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] float cameraSpeed = 0.5f;

    private void Update()
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition.x += cameraSpeed * Time.deltaTime;
        transform.position = cameraPosition;
    }
}
