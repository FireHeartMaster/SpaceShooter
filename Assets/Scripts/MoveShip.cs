using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class MoveShip : MonoBehaviour
{
    [SerializeField] protected Transform topCameraLimit;
    [SerializeField] protected Transform bottomCameraLimit;
    [SerializeField] protected Transform leftCameraLimit;
    [SerializeField] protected Transform rightCameraLimit;

    [HideInInspector] public Vector3 move;

    [SerializeField] protected float shipSpeed = 1f;

    [SerializeField] protected float snapDistance = 0.1f;

    [Space]
    [SerializeField] protected float dodgeDistance = 15f;

    [SerializeField] protected Energy shipEnergy;

    [SerializeField] protected float energyAmountToDodge = 40f;

    [SerializeField] protected Health shipHealth;

    private void Update()
    {      
        transform.localPosition += move * shipSpeed * Time.deltaTime;
        if (leftCameraLimit != null && rightCameraLimit != null && topCameraLimit != null && bottomCameraLimit != null)
        {
            transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, leftCameraLimit.localPosition.x + snapDistance, rightCameraLimit.localPosition.x - snapDistance),
                                                Mathf.Clamp(transform.localPosition.y, bottomCameraLimit.localPosition.y + snapDistance, topCameraLimit.localPosition.y - snapDistance),
                                                transform.localPosition.z);
        }
    }


    public void Dodge()
    {
        transform.localPosition += move.normalized * dodgeDistance;
        if(leftCameraLimit != null && rightCameraLimit != null && topCameraLimit != null && bottomCameraLimit != null)
        {
            transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, leftCameraLimit.localPosition.x + snapDistance, rightCameraLimit.localPosition.x - snapDistance),
                                                Mathf.Clamp(transform.localPosition.y, bottomCameraLimit.localPosition.y + snapDistance, topCameraLimit.localPosition.y - snapDistance),
                                                transform.localPosition.z);
        }
        
        shipHealth.PlayerDodgeInvincibility();
        shipEnergy.SpendEnergy(energyAmountToDodge);
    }
}
