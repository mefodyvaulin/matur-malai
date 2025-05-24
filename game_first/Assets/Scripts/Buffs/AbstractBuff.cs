using System;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class AbstractBuff : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 50f;
    
    private void Update()
    {
        RotateAround();
        ShouldDie();
    }

    private void RotateAround()
    {
        transform.Rotate(Vector3.up * (rotationSpeed * GameModel.UnscaledDeltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            DoBuff();
            Destroy(gameObject);
        }
    }

    private void ShouldDie()
    {
        if (GameModel.PlayerPosition.z - transform.position.z > 100f)
            Destroy(gameObject);
    }

    protected abstract void DoBuff();
}
