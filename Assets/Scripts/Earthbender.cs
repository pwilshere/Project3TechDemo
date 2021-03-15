using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Earthbender : MonoBehaviour
{
    
    public Camera cam;
    private LayerMask mask;
    public float range = 10f;

    public GameObject earthBlock; //prefab
    public GameObject blockPreview; //preview model
    private GameObject blocks; //storing blocks

    private GameObject preview; //storing preview
    public Image confirmButton;
    public GameObject gunBarrel;
    public GameObject earthProjectile;
    private int index = 1;//navigating array
    public int blockLimit;

    private bool blockPreviewOn = false;
    private bool charging = false;
    private float chargeLevel;
    public float maxChargeTime;
    public float chargeRate = 1;
    public GameObject earthCharge;
    private GameObject currentCharge;
    AspectController aspectController;
    public float shootCost;
    public float abilityCost;



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
        if (blockPreviewOn)
        {
            PreviewMode();

            if (Input.GetMouseButtonDown(1))
            {
                blockPreviewOn = false;
                confirmButton.enabled = false;
                Destroy(preview);
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                blockPreviewOn = false;
                confirmButton.enabled = false;
                Destroy(preview);

                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, range, mask))
                {
                    if (aspectController.currentMana > abilityCost)
                    {
                        SpawnBlock(hit.point, hit.normal);
                        aspectController.currentMana -= abilityCost;
                    }
                }
            }
        } else //blockpreview off
        {
            if (Input.GetMouseButtonDown(1))
            {
                blockPreviewOn = true;
                preview = Instantiate(blockPreview, transform.position - (Vector3.down * 10), transform.rotation);
                preview.name = "ability preview";
                confirmButton.enabled = true;
            }
            if (!charging && Input.GetMouseButtonDown(0))
            {
                charging = true;
                aspectController.charging = true;
                currentCharge = Instantiate(earthCharge, gunBarrel.transform);
            }
            if (charging)
            {
                if (chargeLevel < maxChargeTime)
                {
                    chargeLevel += Time.deltaTime * chargeRate;
                    currentCharge.transform.localScale = Vector3.one * chargeLevel / 5;
                    
                }
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
    }
    void Shoot(Vector3 target, float power)
    {
        if (aspectController.currentMana < shootCost)
            return;
        else 
            aspectController.currentMana -= shootCost;

        GameObject missile = Instantiate(earthProjectile, gunBarrel.transform.position, gunBarrel.transform.rotation);
        missile.transform.LookAt(target);
        if (power > 1 && aspectController.currentMana > shootCost)
        {
            aspectController.currentMana -= shootCost;
            
            GameObject missile1 = Instantiate(earthProjectile, gunBarrel.transform.position, gunBarrel.transform.rotation);
            missile1.transform.LookAt(target);
            missile1.transform.Rotate(0f,5f,0f);
            GameObject missile2 = Instantiate(earthProjectile, gunBarrel.transform.position, gunBarrel.transform.rotation);
            missile2.transform.LookAt(target);
            missile2.transform.Rotate(0f,-5f,0f);
        }
        if (power > 2 && aspectController.currentMana > shootCost)
        {
            aspectController.currentMana -= shootCost;
            
            GameObject missile3 = Instantiate(earthProjectile, gunBarrel.transform.position, gunBarrel.transform.rotation);
            missile3.transform.LookAt(target);
            missile3.transform.Rotate(5f,0f,0f);
            GameObject missile4 = Instantiate(earthProjectile, gunBarrel.transform.position, gunBarrel.transform.rotation);
            missile4.transform.LookAt(target);
            missile4.transform.Rotate(-5f,0f,0f);
        }
        if (power > 2.9f && aspectController.currentMana > shootCost)
        {
            aspectController.currentMana -= shootCost;
            
            GameObject missile5 = Instantiate(earthProjectile, gunBarrel.transform.position, gunBarrel.transform.rotation);
            missile5.transform.LookAt(target);
            missile5.transform.Rotate(3f,3f,0f);
            GameObject missile6 = Instantiate(earthProjectile, gunBarrel.transform.position, gunBarrel.transform.rotation);
            missile6.transform.LookAt(target);
            missile6.transform.Rotate(-3f,3f,0f);
            GameObject missile7 = Instantiate(earthProjectile, gunBarrel.transform.position, gunBarrel.transform.rotation);
            missile7.transform.LookAt(target);
            missile7.transform.Rotate(-3f,-3f,0f);
            GameObject missile8 = Instantiate(earthProjectile, gunBarrel.transform.position, gunBarrel.transform.rotation);
            missile8.transform.LookAt(target);
            missile8.transform.Rotate(3f,-3f,0f);
        }
    }
    void PreviewMode()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range, mask))
        {
            preview.SetActive(true);
            preview.transform.position = hit.point;
            preview.transform.rotation = Quaternion.FromToRotation(Vector3.up,hit.normal);
        } else 
        {
            preview.SetActive(false);
        }
    }
    void SpawnBlock(Vector3 location, Vector3 surfaceAngle)
    {
        
        blocks = GameObject.Find("earthblock" + index);
        if (blocks != null)
        {
            blocks.GetComponent<BlockController>().SelfDestruct();
        }

        blocks = Instantiate(earthBlock, location,Quaternion.FromToRotation(Vector3.up,surfaceAngle));
        blocks.name = "earthblock" + index;
        
        index++;
        if (index > blockLimit)
        {
            index = 1;
        }
    }
    public void CleanUp()
    {
        blockPreviewOn = false;
        confirmButton.enabled = false;
        Destroy(preview);
    }
}
