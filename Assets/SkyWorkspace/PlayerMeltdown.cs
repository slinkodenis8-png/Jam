using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerMeltdown : MonoBehaviour
{
    public event Action OnPlayerMelted;

    [Header("Counter Settings")]
    [SerializeField] private int maxValue = 100;
    public int currentValue;
    public int meltSpeed = 1;
    [SerializeField] private int framesBetweenDecrement = 25;

    [Header("Scaling Settings")]
    [SerializeField] private Vector3 minScale = Vector3.one * 0.1f;
    [SerializeField] private Vector3 maxScale = Vector3.one;

    private int frameCounter;
    private Vector3 originalScale;


    [SerializeField] private Transform targetObject;
    [SerializeField] private Image healthBar;


    [SerializeField] private ParticleSystem waterParticles;
    private float defaultParticleRate = 10f;
    [SerializeField] private float minParticleRate = 10f;
    [SerializeField] private float maxParticleRate = 10f;

    private ParticleSystem.EmissionModule emissionModule;

    void Start()
    {
        if (targetObject == null)
            targetObject = transform;

        originalScale = targetObject.localScale;

        if (waterParticles != null)
        {
            emissionModule = waterParticles.emission;
        }

        ResetCounter();
    }


    void FixedUpdate()
    {
        frameCounter++;

        if (frameCounter >= framesBetweenDecrement)
        {
            frameCounter = 0;
            TickMelt();
        }

        //UpdateObjectScale();
    }

    private void TickMelt()
    {
        if (currentValue <= 0) return;

        SubtractFromCounter(meltSpeed);

        if (currentValue <= 0)
        {
            currentValue = 0;
            OnPlayerMelted?.Invoke();
        }
    }

    private void UpdateObjectScale()
    {
        if (targetObject == null) return;

        float progress = (float)currentValue / maxValue; // 1.0 → 0.0

        Vector3 newScale = Vector3.Lerp(minScale, maxScale, progress);

        targetObject.localScale = newScale;

        healthBar.fillAmount = progress;

        var rate = emissionModule.rateOverTime;
        rate.constant = Math.Clamp(defaultParticleRate * meltSpeed, minParticleRate, maxParticleRate);
        emissionModule.rateOverTime = rate;
    }

    public void SetCounterValue(int newValue)
    {
        currentValue = Mathf.Clamp(newValue, 0, maxValue);

        UpdateObjectScale();
    }

    public void ResetCounter()
    {
        currentValue = maxValue;
        frameCounter = 0;
        if (targetObject != null)
            targetObject.localScale = maxScale;
    }



    public void AddToCounter(int amount)
    {
        SetCounterValue(currentValue + amount);
        if (currentValue > maxValue)
            currentValue = maxValue;
    }

    public void SubtractFromCounter(int amount)
    {
        SetCounterValue(currentValue - amount);
    }

    public void SetFramesBetweenDecrement(int newFrameCount)
    {
        framesBetweenDecrement = Mathf.Max(1, newFrameCount);
        frameCounter = 0;
    }

    public void SetDecrementAmount(int newAmount)
    {
        meltSpeed = Mathf.Max(1, newAmount);
    }

    public void StopCounter()
    {
        frameCounter = framesBetweenDecrement;
    }

    public void StartCounter()
    {
        frameCounter = 0;
    }

    // public void SetMinScale(Vector3 newMinScale)
    // {
    //     minScale = newMinScale;
    // }

    // public void SetMaxScale(Vector3 newMaxScale)
    // {
    //     maxScale = newMaxScale;
    //     UpdateObjectScale();
    // }
}
