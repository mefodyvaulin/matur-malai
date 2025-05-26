using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class AbstractBuff : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 50f;
    private bool isActive = false;
    
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
        if (!other.GetComponent<PlayerMovement>()) return;
        
        isActive = true;
        StartCoroutine(BuffAndDestroyCoroutine());
        GetComponent<Collider>().enabled = false;
        GetComponentInChildren<MeshRenderer>().enabled = false;
    }
    
    private IEnumerator BuffAndDestroyCoroutine()
    {
        yield return StartCoroutine(DoBuff());
        Destroy(gameObject);
    }


    private void ShouldDie()
    {
        if (GameModel.PlayerPosition.z - transform.position.z > 100f && !isActive)
            Destroy(gameObject);
    }

    protected abstract IEnumerator DoBuff();
}
