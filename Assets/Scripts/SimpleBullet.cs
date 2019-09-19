using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBullet : Bullet
{
    Vector3 previousPosition;
    //Should be called from the inside of a FixedUpdate
    public virtual void UpdateBulletPosition()
    {
        Vector3 position = transform.position;

        position += bulletDirection * bulletSpeed * Time.fixedDeltaTime;

        transform.position = position;

        transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.Cross(Vector3.forward, transform.position - previousPosition));

        previousPosition = transform.position;
    }
    private void FixedUpdate()
    {
        UpdateBulletPosition();
    }
}

