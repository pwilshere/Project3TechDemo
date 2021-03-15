using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Icebender : MonoBehaviour
{
    public GameObject iceField;
    public Camera cam;
    private LayerMask mask;
    private GameObject spawnedIce;
    public GameObject icePreview;
    private GameObject preview; //storing preview
    public Image confirmButton;
    public GameObject iceProjectile;
    public GameObject gunBarrel;
    public float range = 10f;
    private bool icePreviewOn = false;
    AspectController aspectController;
    public float shootCost;
    public float abilityCost;
    void Start()
    {
        mask = LayerMask.GetMask("Player", "Summon");
        mask = ~mask; //makes sure that the ray doesn't hit the player or other summons
        aspectController = GetComponent<AspectController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (icePreviewOn)
        {
            PreviewMode();
            if (Input.GetMouseButtonDown(1)) //cancel
            {
                icePreviewOn = false;
                confirmButton.enabled = false;
                Destroy(preview);
            }
            
            if (Input.GetMouseButtonDown(0)) //confirm
            {
                icePreviewOn = false;
                confirmButton.enabled = false;
                Destroy(preview);

                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                //Debug.DrawRay(cam.transform.position, cam.transform.TransformDirection(Vector3.forward));

                if (Physics.Raycast(ray, out hit, range, mask))
                {
                    if (aspectController.currentMana > abilityCost)
                    {
                        aspectController.currentMana -= abilityCost;
                        SpawnIce(hit.point, hit.normal);
                    }
                }
            }
        } else
        {
            if (Input.GetMouseButtonDown(1))
            {
                icePreviewOn = true;
                preview = Instantiate(icePreview, transform.position - (Vector3.down * 10), transform.rotation);
                preview.name = "ability preview";
                confirmButton.enabled = true;
            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
                {
                    Shoot(hit.point);
                }
            }
        }
    }
    void Shoot(Vector3 target)
    {
        if(aspectController.currentMana < shootCost)
            return;
        else
        {
            aspectController.currentMana -= shootCost;
        }

        GameObject missile = Instantiate(iceProjectile, gunBarrel.transform.position, gunBarrel.transform.rotation);
        missile.transform.LookAt(target);
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
    void SpawnIce(Vector3 location, Vector3 surfaceAngle)
    {
        /*if (spawnedIce != null)
        {
            Destroy(spawnedIce);
        }*/
        spawnedIce = Instantiate(iceField, location,Quaternion.FromToRotation(Vector3.up,surfaceAngle));
    }
    public void CleanUp()
    {
        icePreviewOn = false;
        confirmButton.enabled = false;
        Destroy(preview);
    }
}
