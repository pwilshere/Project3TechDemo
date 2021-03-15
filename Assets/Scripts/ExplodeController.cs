using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeController : MonoBehaviour
{
    public float maxRadius = 10;
    private float radius = 0;
    public float growthRate = 1;


    // Update is called once per frame
    void Update()
    {
        if (radius < maxRadius)
        {
            radius += Time.deltaTime * growthRate;
            transform.localScale = Vector3.one * radius;
        } else
        {
            Destroy(this.gameObject);
        }
    }
}
