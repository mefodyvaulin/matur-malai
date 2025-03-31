using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;

public class MefodijDoSomething : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    bool isUp = true;
    float step = 0.01f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.y >= 0 && transform.position.y <= 10)
        {
            if (isUp)
                transform.position += new Vector3(0, step, 0);
            else
                transform.position += new Vector3(0, -1 * step, 0);
        }
        else
        {
            isUp = !isUp;
            transform.localScale = new Vector3(transform.localScale.x * 2, transform.localScale.y, transform.localScale.z);
            if (isUp)
                transform.position += new Vector3(0, step, 0);
            else
                transform.position += new Vector3(0, -1 * step, 0);
        }
    }
}
