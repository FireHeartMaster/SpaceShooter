using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralBullet : Bullet
{
    Vector3 previousPosition;

    Vector3 initialPosition;

    public float currentRadius = 0;
    [SerializeField] float radiusIncreasingSpeed = 1f;

    public float currentAngle = 0;
    [SerializeField] float angleIncreasingSpeed = Mathf.PI / 2f;

    private void Awake()
    {
        initialPosition = transform.position;
    }

    //Should be called from the inside of a FixedUpdate
    public virtual void UpdateBulletPosition()
    {
        //Vector3 position = transform.position;

        //position += bulletDirection * bulletSpeed * Time.fixedDeltaTime;

        //transform.position = position;

        currentRadius += radiusIncreasingSpeed * Time.fixedDeltaTime;
        currentAngle += angleIncreasingSpeed * Time.fixedDeltaTime;

        transform.position += new Vector3(currentRadius * Mathf.Cos(currentAngle), currentRadius * Mathf.Sin(currentAngle), transform.position.z);

        transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.Cross(Vector3.forward, transform.position - previousPosition));

        previousPosition = transform.position;
    }
    private void FixedUpdate()
    {
        UpdateBulletPosition();
    }
}

