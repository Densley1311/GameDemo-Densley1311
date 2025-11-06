using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade_Controller : MonoBehaviour
{
    public SoulIcon SoulIcon;
    public int upgradesUnlocked = 0;

    [Header("Prefabs")]
    public GameObject magic_bolt;
    public GameObject skelton;

    public Sprite[] popups;
    public Image popUp;

    // Update is called once per frame
    private void Start()
    {
        magic_bolt.GetComponent<Basic_Spell>().damage = 4;
        skelton.GetComponent<Health>().maxHealth = 20;
        skelton.GetComponent<ZombieController>().attackDamage = 5;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire3") && SoulIcon.enemyRaiseCount == SoulIcon.maxEnemies)
        {
            popUp.color = Color.white;
            UnlockUpgrade();
        }
    }

    void UnlockUpgrade()
    {
        SoulIcon.maxEnemies = SoulIcon.maxEnemies * 2;
        SoulIcon.enemyRaiseCount = 0;
        SoulIcon.UpdateSoulIcon();
        upgradesUnlocked += 1;

        switch (upgradesUnlocked)
        {
            case 1:
                magic_bolt.GetComponent<Basic_Spell>().damage = 10;
                popUp.sprite = popups[upgradesUnlocked - 1];
                break;
            case 2:
                gameObject.GetComponent<Spells>().canHeal = true;
                popUp.sprite = popups[upgradesUnlocked - 1];
                break;
            case 3:
                skelton.GetComponent<Health>().maxHealth = skelton.GetComponent<Health>().maxHealth * 2;
                popUp.sprite = popups[upgradesUnlocked - 1];
                break;
            case 4:
                skelton.GetComponent<ZombieController>().attackDamage = skelton.GetComponent<ZombieController>().attackDamage * 2;
                popUp.sprite = popups[upgradesUnlocked - 1];
                break;
            default:
                break;
        }

        Invoke("removePopupWindow", 5f);
    }

    void removePopupWindow()
    {
        popUp.color = Color.clear;
    }
}
