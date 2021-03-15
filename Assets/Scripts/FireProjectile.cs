using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    public float speed;
    public float lifetime;
    private float timer = 0;
    public float power;
    public GameObject explosion;

    // Update is called once per frame
    void Update()
    {
        Vector3 move = transform.position;
        move += transform.forward*Time.deltaTime*speed;
        transform.position = move;

        timer += Time.deltaTime;
        if (timer > lifetime)
        {
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("projectile"))
            return;
        
        GameObject obj = Instantiate(explosion, transform.position, transform.rotation);
        obj.GetComponent<ExplodeController>().maxRadius = power*3;
        obj.transform.parent = null;
        
        Destroy(this.gameObject);
    }
}
