using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSimpleMove : MonoBehaviour
{
    public Vector3 moveDirection = Vector2.right;

    [SerializeField] float bulletSpeed = 10f;

    private void Update()
    {
        MoveBulletInALine(moveDirection);
    }


    void SetBullet(Vector3 direction)
    {
        moveDirection = direction;

        transform.rotation = Quaternion.LookRotation(direction);
    }
    void MoveBulletInALine(Vector2 direction)
    {
        Vector3 position = transform.position;

        position += bulletSpeed * Time.deltaTime * moveDirection;
        transform.position = position;
    }
}
