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

    [SerializeField] Color energySliderFillNormalColor;
    [SerializeField] Color energySliderFillSlowRecoveryColor;

    [SerializeField] Image sliderFillImage;

    private void Awake()
    {
        currentEnergy = maxEnergy;

        if(energySlider != null)
        {
            energySlider.maxValue = maxEnergy;
        }
    }

    private void Update()
    {
        if(currentEnergy >= maxEnergy)
        {
            currentEnergy = maxEnergy;

            slowRecover = false;
            canShoot = true;

            if (energySlider != null)
                sliderFillImage.color = energySliderFillNormalColor;
        }

        if (canRecoverEnergy && currentEnergy < maxEnergy)
        {
            currentEnergy += (!slowRecover ? energyRecoveryRate : slowEnergyRecoveryRate) * Time.deltaTime;
        }

        if (energySlider != null)
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

            if (energySlider != null)
                sliderFillImage.color = energySliderFillSlowRecoveryColor;
        }
    }


    public void ReinitializeEnergy()
    {
        currentEnergy = maxEnergy;

        if (energySlider != null)
        {
            energySlider.maxValue = maxEnergy;
        }
    }
}
