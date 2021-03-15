using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Firebender : MonoBehaviour
{
    public Camera cam;
    private LayerMask mask;
    public float range = 10f;
    public Image confirmButton;
    public GameObject gunBarrel;
    public GameObject fireProjectile;
    public GameObject fireCharge;
    private GameObject currentCharge;
    private bool charging = false;
    private float chargeLevel = 0;
    public float chargeRate;
    public float maxChargeTime;
    AspectController aspectController;
    public float manaCost;
    void Start()
    {
        mask = LayerMask.GetMask("Player", "Summon");
        mask = ~mask; //makes sure that the ray doesn't hit the player or other summons
        confirmButton.enabled = false;
        aspectController = GetComponent<AspectController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!charging && Input.GetMouseButtonDown(0))
        {
            charging = true;
            aspectController.charging = true;
            currentCharge = Instantiate(fireCharge, gunBarrel.transform);
        }
        if (charging)
        {
            //charge over time
            if (chargeLevel < maxChargeTime)
            {
                chargeLevel += Time.deltaTime * chargeRate;
                currentCharge.transform.localScale = Vector3.one * chargeLevel / 5;
                
            }
            //update the charge position and size
            currentCharge.transform.position = gunBarrel.transform.position;
            currentCharge.transform.rotation = gunBarrel.transform.rotation;

            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
                {
                    Shoot(hit.point, chargeLevel);
                }
                charging = false;
                aspectController.charging = false;
                chargeLevel = 0;
                Destroy(currentCharge);
            }
        }
    }
    void Shoot(Vector3 target, float chargeLevel)
    {
        if (aspectController.currentMana < manaCost + 3*chargeLevel)
            return;
        aspectController.currentMana -= manaCost + 2*chargeLevel;

        GameObject missile = Instantiate(fireProjectile, gunBarrel.transform.position, gunBarrel.transform.rotation);
        missile.transform.LookAt(target);
        missile.GetComponent<FireProjectile>().power = chargeLevel;
    }
    public void CleanUp()
    {
        confirmButton.enabled = false;
    }
}
