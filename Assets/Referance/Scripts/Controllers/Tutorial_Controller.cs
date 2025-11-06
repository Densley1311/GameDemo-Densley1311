using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Controller : MonoBehaviour
{
    public Sprite[] popups;
    public Image popupWindowSprite;

    [Header("Booleans")]
    public bool tutorial_active = true;
    public bool intro_active = true;
    public bool has_walked = false;
    public bool has_shot = false;
    public bool has_raisedDeath = false;
    public bool has_commanded = false;
    public bool can_upgrade = false;
    public bool has_upgraded = false;

    public bool inAttackBox = false;

    [Header("Timers")]
    public int tutorialTimer = 0;
    public int time2ndPopup;
    public int time3thPopup;

    [Header("Objects")]
    public Health tutorial_peasant;
    public Spells player;
    public GameObject waypoint1;
    public SoulIcon SoulIcon;

    // Start is called before the first frame update
    void Start()
    {
        popupWindowSprite.color = Color.white;
        popupWindowSprite.sprite = popups[0];
    }

    private void FixedUpdate()
    {
        if (tutorial_active)
        {
            if (tutorialTimer < time3thPopup)
            {
                IntroSequence();
            }

            if (!intro_active)
            {
                if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)) && !has_walked)
                {
                    popupWindowSprite.color = Color.clear;
                    has_walked = true;
                }

                if (inAttackBox)
                {
                    popupWindowSprite.sprite = popups[3];
                    popupWindowSprite.color = Color.white;

                    if (has_shot)
                    {
                        popupWindowSprite.color = Color.clear;
                    }
                }

                if (!has_raisedDeath && has_shot)
                {
                    if (tutorial_peasant.tutorial_NPC_Died == true)
                    {
                        popupWindowSprite.color = Color.white;
                        popupWindowSprite.sprite = popups[4];

                        if (player.onGrave && !has_raisedDeath)
                        {
                            popupWindowSprite.sprite = popups[5];
                        }
                    }
                }

                if (has_raisedDeath && !has_commanded)
                {
                    waypoint1.SetActive(true);
                    popupWindowSprite.sprite = popups[6];

                    if (Input.GetMouseButton(1))
                    {
                        has_commanded = true;
                        popupWindowSprite.color = Color.clear;
                    }
                }

                if (has_commanded)
                {
                    if (SoulIcon.enemyRaiseCount == SoulIcon.maxEnemies)
                    {
                        can_upgrade = true;
                        popupWindowSprite.sprite = popups[7];
                        popupWindowSprite.color = Color.white;
                    }
                    if (Input.GetButtonDown("Fire3") && can_upgrade)
                    {
                        has_upgraded = true;
                        tutorial_active = false;
                    }
                }
            }
        }
        else
        {
            if (waypoint1.active == false)
            {
                waypoint1.SetActive(true);
            }
        }
    }

    private void IntroSequence()
    {
        tutorialTimer++;
        if (tutorialTimer > time2ndPopup)
        {
            popupWindowSprite.sprite = popups[1];
            if (tutorialTimer >= time3thPopup)
            {
                popupWindowSprite.sprite = popups[2];
                intro_active = false;
            }
        }
    }
}