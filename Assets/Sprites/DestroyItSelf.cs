using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyItSelf : MonoBehaviour
{
    [SerializeField] float timeToDisapear = 3f;

    private void Start()
    {
        Destroy(gameObject, timeToDisapear);
    }
}
