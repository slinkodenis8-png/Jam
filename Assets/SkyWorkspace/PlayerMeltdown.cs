using UnityEngine;
using System;

public class PlayerMeltdown : MonoBehaviour
{
    public event Action OnPlayerMelted;

    [Header("Counter Settings")]
    [SerializeField] private int initialValue = 100;
    [SerializeField] private int currentValue;
    [SerializeField] private int decrementAmount = 1;
    [SerializeField] private int framesBetweenDecrement = 25;

    [Header("Scaling Settings")]
    [SerializeField] private Transform targetObject;
    [SerializeField] private Vector3 minScale = Vector3.one * 0.1f;
    [SerializeField] private Vector3 maxScale = Vector3.one;

    private int frameCounter;
    private Vector3 originalScale;

    void Start()
    {
        if (targetObject == null)
            targetObject = transform;

        originalScale = targetObject.localScale;
        ResetCounter();
    }

    void FixedUpdate()
    {
        frameCounter++;

        if (frameCounter >= framesBetweenDecrement)
        {
            frameCounter = 0;
            DecrementCounter();
        }

        //UpdateObjectScale();
    }

    private void DecrementCounter()
    {
        if (currentValue <= 0) return;

        SubtractFromCounter(decrementAmount);

        if (currentValue <= 0)
        {
            currentValue = 0;
            OnPlayerMelted?.Invoke();
        }
    }

    private void UpdateObjectScale()
    {
        if (targetObject == null) return;

        float progress = (float)currentValue / initialValue; // 1.0 → 0.0

        Vector3 newScale = Vector3.Lerp(minScale, maxScale, progress);

        targetObject.localScale = newScale;
    }

    public void SetCounterValue(int newValue)
    {
        currentValue = Mathf.Max(0, newValue);

        UpdateObjectScale();
    }

    public void ResetCounter()
    {
        currentValue = initialValue;
        frameCounter = 0;
        if (targetObject != null)
            targetObject.localScale = maxScale;
    }

    

    public void AddToCounter(int amount)
    {
        SetCounterValue(currentValue + amount);
        if (currentValue > initialValue)
            currentValue = initialValue;
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
        decrementAmount = Mathf.Max(1, newAmount);
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
