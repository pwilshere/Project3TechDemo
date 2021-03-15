using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceController : MonoBehaviour
{
    public float size;
    public float scaleRate;
    public float duration;
    public float timer = 0;
    private bool fadeOut = false;
    void Start()
    {
        transform.localScale = new Vector3(0.1f,0.1f,0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > duration)
        {
            fadeOut = true;
        }
        if (transform.localScale.y < size && !fadeOut)
        {
            Vector3 newSize = transform.localScale;

            newSize += new Vector3(Time.deltaTime * scaleRate, Time.deltaTime * scaleRate, Time.deltaTime * scaleRate);
            transform.localScale = newSize;
        } else if (fadeOut)
        {
            Vector3 newSize = transform.localScale;

            newSize -= new Vector3(Time.deltaTime * scaleRate, Time.deltaTime * scaleRate, Time.deltaTime * scaleRate);
            transform.localScale = newSize;

            if (transform.localScale.x < 0.5f)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
