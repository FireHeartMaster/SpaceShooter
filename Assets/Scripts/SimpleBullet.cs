using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBullet : Bullet
{

    //Should be called from the inside of a FixedUpdate
    public virtual void UpdateBulletPosition()
    {
        Vector3 position = transform.position;

        position += bulletDirection * bulletSpeed * Time.fixedDeltaTime;

        transform.position = position;
    }
    private void FixedUpdate()
    {
        UpdateBulletPosition();
    }
}

