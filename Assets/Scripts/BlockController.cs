using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    public float size = 4f;
    public float scaleRate = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(size,0.1f,size);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.y < size)
        {
            float newY = transform.localScale.y;

            newY += Time.deltaTime * scaleRate;
            transform.localScale = new Vector3(size,newY,size);
        }
    }

    public void SelfDestruct()
    {
        Destroy(this.gameObject);
    }
}
