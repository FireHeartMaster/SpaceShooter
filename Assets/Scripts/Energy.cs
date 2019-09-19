using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Energy : MonoBehaviour
{
    [SerializeField] float maxEnergy = 100f;
    public float currentEnergy;

    [SerializeField] float energyRecoveryRate = 35f;
    [SerializeField] float slowEnergyRecoveryRate = 20f;


    public bool canRecoverEnergy = true;
    public bool canLooseEnergy = true;

    public bool canShoot = true;

    public bool slowRecover = false;

    [SerializeField] Slider energySlider;

    [SerializeField] Color sliderFillNormalColor;
    [SerializeField] Color sliderFillSlowRecoveryColor;

    [SerializeField] Image sliderFillImage;

    private void Awake()
    {
        currentEnergy = maxEnergy;
    }

    private void Update()
    {
        if(currentEnergy >= maxEnergy)
        {
            currentEnergy = maxEnergy;

            slowRecover = false;
            canShoot = true;

            sliderFillImage.color = sliderFillNormalColor;
        }

        if (canRecoverEnergy && currentEnergy < maxEnergy)
        {
            currentEnergy += (!slowRecover ? energyRecoveryRate : slowEnergyRecoveryRate) * Time.deltaTime;
        }

        energySlider.value = currentEnergy;
    }

    public void SpendEnergy(float energyAmount)
    {
        if (canLooseEnergy && canShoot)
        {
            currentEnergy -= energyAmount;
        }

        if (currentEnergy <= 0)
        {
            currentEnergy = 0;
            slowRecover = true;
            canShoot = false;

            sliderFillImage.color = sliderFillSlowRecoveryColor;
        }
    }

}
