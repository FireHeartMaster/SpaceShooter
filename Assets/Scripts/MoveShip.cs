using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class MoveShip : MonoBehaviour
{
    [SerializeField] Transform topCameraLimit;
    [SerializeField] Transform bottomCameraLimit;
    [SerializeField] Transform leftCameraLimit;
    [SerializeField] Transform rightCameraLimit;

    [HideInInspector] public Vector3 move;

    [SerializeField] float shipSpeed = 1f;

    [SerializeField] float snapDistance = 0.1f;

    [Space]
    [SerializeField] float dodgeDistance = 15f;

    [SerializeField] Energy playerEnergy;

    [SerializeField] float energyAmountToDodge = 40f;

    [SerializeField] Health playerHealth;

    private void Update()
    {      
        transform.localPosition += move * shipSpeed * Time.deltaTime;
        transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, leftCameraLimit.localPosition.x + snapDistance, rightCameraLimit.localPosition.x - snapDistance),
                                                Mathf.Clamp(transform.localPosition.y, bottomCameraLimit.localPosition.y + snapDistance, topCameraLimit.localPosition.y - snapDistance),
                                                transform.localPosition.z);
    }


    public void Dodge()
    {
        transform.localPosition += move.normalized * dodgeDistance;
        transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, leftCameraLimit.localPosition.x + snapDistance, rightCameraLimit.localPosition.x - snapDistance),
                                                Mathf.Clamp(transform.localPosition.y, bottomCameraLimit.localPosition.y + snapDistance, topCameraLimit.localPosition.y - snapDistance),
                                                transform.localPosition.z);
        playerHealth.PlayerDodgeInvincibility();
        playerEnergy.SpendEnergy(energyAmountToDodge);
    }
}
