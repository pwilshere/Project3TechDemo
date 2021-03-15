using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AspectController : MonoBehaviour
{
    public Firebender firebender;
    private bool firePower = true;   
    public Earthbender earthbender;
    public bool earthPower = false;
    public Icebender icebender;
    public bool icePower = false;
    public float cooldown;
    public float cooldownTimer = -1;
    public Text aspectText;
    public static float maxMana = 100;
    public float currentMana;
    public float rechargeRate;
    public Text manaText;
    public float manaPickupValue;
    public float aspectChangeCost;
    public bool charging = false;
    public Image manaBar;
    public Image manaBarBackground;
    
    // Start is called before the first frame update
    void Start()
    {
        earthbender.enabled = false;
        icebender.enabled = false;
        firebender.enabled = false;
        ActivateFire();
        manaBar.rectTransform.sizeDelta = new Vector2(20,maxMana);
        manaBarBackground.rectTransform.sizeDelta = new Vector2(20,maxMana);
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownTimer < 0)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && firePower)
            {
                ActivateFire();
                currentMana -= aspectChangeCost;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && earthPower)
            {
                ActivateEarth();
                currentMana -= aspectChangeCost;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && icePower)
            {
                ActivateIce();
                currentMana -= aspectChangeCost;
            }
        } else
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (currentMana < maxMana && !charging)
        {
            currentMana += Time.deltaTime * rechargeRate;
            manaText.text = "Mana: " + Mathf.Round(currentMana);
            manaBar.fillAmount = currentMana / maxMana;
        }

    }
    void ActivateFire()
    {
        if (firebender.enabled)
        {
            return;
        }
        earthbender.CleanUp();
        icebender.CleanUp();
        earthbender.enabled = false;
        icebender.enabled = false;
        firebender.enabled = true;
        cooldownTimer = cooldown;
        aspectText.text = "Fire";
    }
    void ActivateEarth()
    {
        if (earthbender.enabled)
        {
            return;
        }
        
        firebender.CleanUp();
        icebender.CleanUp();
        earthbender.enabled = true;
        icebender.enabled = false;
        firebender.enabled = false;
        cooldownTimer = cooldown;
        aspectText.text = "Earth";
    }
    void ActivateIce()
    {
        if (icebender.enabled)
        {
            return;
        }
        
        earthbender.CleanUp();
        firebender.CleanUp();

        earthbender.enabled = false;
        icebender.enabled = true;
        firebender.enabled = false;
        cooldownTimer = cooldown;
        aspectText.text = "Ice";
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("earthPickup"))
        {
            earthPower = true;
            Destroy(other.gameObject);
            ActivateEarth();
        }
        if (other.CompareTag("icePickup"))
        {
            icePower = true;
            Destroy(other.gameObject);
            ActivateIce();
        }
        if (other.CompareTag("manaPickup"))
        {
            maxMana += manaPickupValue;
            manaBar.rectTransform.sizeDelta = new Vector2 (20,maxMana);
            manaBarBackground.rectTransform.sizeDelta = new Vector2 (20,maxMana);
            Destroy(other.gameObject);
        }
    }
}
