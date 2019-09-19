using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    [SerializeField] float backgroundSpeed = 0.5f;

    private void Update()
    {
        Vector3 backgroundPosition = transform.position;
        backgroundPosition.x -= backgroundSpeed * Time.deltaTime;
        transform.position = backgroundPosition;
    }
}
