using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaserShoot : LaserBeam
{
    [SerializeField] public float maxWidth = 0.4f;
    private float timeAdded = 0.2f;
    private float timeMaxAdd = 0.15f;
    private float elapsedTime;
    private bool isAdded;
    
    protected override void Awake()
    {
        base.Awake();
        damagePerSecond = 5;
        damageInterval = 0.2f;
        step = maxWidth;
    }

    private void OnEnable()
    {
        width = 0;
        elapsedTime = 0f;
        isAdded = true;
    }

    protected override void UpdateBeamVisual(float length)
    {
        if (elapsedTime < timeAdded + timeMaxAdd && isAdded)
        {
            elapsedTime += GameModel.UnscaledDeltaTime;
        }
        else if (elapsedTime >= timeAdded + timeMaxAdd && isAdded)
        {
            elapsedTime = timeAdded + timeMaxAdd;
            isAdded = false;
        }
        else
        {
            elapsedTime -= GameModel.UnscaledDeltaTime;
            if (elapsedTime <= 0)
            {
                gameObject.SetActive(false);
            }
        }

        if (elapsedTime >= timeAdded) width = maxWidth;
        else width = maxWidth * (elapsedTime / timeAdded);
        
        base.UpdateBeamVisual(length);
    }
}