using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spells : MonoBehaviour
{
    public PlayerMovement player;

    [Header("Attacks")]
    public Transform firePointRight;
    public Transform firePointLeft;

    [Header("Prefabs")]
    public GameObject bulletPrefab;
    public GameObject zombiePrefab;
    public GameObject target1Prefab;

    [SerializeField]
    public float bulletForce;

    public Animator animator;

    

    [Header("Cooldowns")]
    [SerializeField]
    private int basic_spell_cooldown;
    [SerializeField]
    private int basic_spell_cooldown_reset;

    [SerializeField]
    private int animate_spell_cooldown;
    [SerializeField]
    private int animate_spell_cooldown_reset;

    [SerializeField]
    public bool onGrave = false;
    [SerializeField]
    private GameObject graveGameobject;

    public SoulIcon soulIcon;
    public Tutorial_Controller tutorial;
    public GameObject attack_box;
    public GameObject tutorial_box;

    [Header("Upgrades")]
    public bool canHeal = false;

    public GameObject[] nearbyAllies;
    [SerializeField]
    private int heal_spell_cooldown;
    [SerializeField]
    private int heal_spell_cooldown_reset;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && basic_spell_cooldown == 0)
        {
            Shoot();
        }

        if(Input.GetButtonDown("Jump") && onGrave)
        {
            tutorial.has_raisedDeath = true;
            animate_spell_cooldown = animate_spell_cooldown_reset;
            animator.SetBool("Raise", true);
            player.moveSpeed = 0.5f;
            Invoke("AnimateDead", 0.5f);
            
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && canHeal && heal_spell_cooldown == 0)
        {
            animator.SetBool("Raise", true);
            heal_spell_cooldown = heal_spell_cooldown_reset;
            player.moveSpeed = 0.5f;
            Invoke("HealAllies", 0.1f);
        }

        if (basic_spell_cooldown == 0)
        {
            animator.SetBool("Shooting", false);
        }
        if (animate_spell_cooldown == 0 && heal_spell_cooldown == 0)
        {
            animator.SetBool("Raise", false);
        }
    }
    
    private void FixedUpdate()
    {
        if (basic_spell_cooldown > 0)
        {
            basic_spell_cooldown--;
        }

        if (animate_spell_cooldown > 0)
        {
            animate_spell_cooldown--;
        }

        if (heal_spell_cooldown > 0)
        {
            heal_spell_cooldown--;
        }
    }

    void Shoot()
    {
        animator.SetBool("Shooting", true);
        Invoke("BasicSpell", 0.5f);
        tutorial.has_shot = true;
        basic_spell_cooldown = basic_spell_cooldown_reset;
    }

    void BasicSpell()
    {
        Transform firePoint;

        // Get the mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the direction from the firePoint to the mouse position
        Vector2 mouseDirection = (mousePosition - gameObject.transform.position);
        

        if (mouseDirection.x < 0)
        {
            firePoint = firePointLeft;
        } else
        {
            firePoint = firePointRight;
        }
        Vector2 direction = (mousePosition - firePoint.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Bullet"), LayerMask.NameToLayer("Allies"));
        rb.AddForce(direction.normalized * bulletForce, ForceMode2D.Impulse);
    }

    void AnimateDead()
    {
        GameObject newZombie = Instantiate(zombiePrefab, graveGameobject.transform.position, graveGameobject.transform.rotation);
        newZombie.GetComponent<ZombieController>().waypoint = target1Prefab.transform;
        soulIcon.EnemyRaised();
        Destroy(graveGameobject);
        player.moveSpeed = 1f;
    }

    void HealAllies()
    {
        nearbyAllies = GameObject.FindGameObjectsWithTag("Ally");
        float minDistanceToHeal = 2;

        foreach (GameObject entity in nearbyAllies)
        {
            float distanceToAlly = Vector3.Distance(transform.position, entity.transform.position);
            if (distanceToAlly < minDistanceToHeal)
            {
                entity.GetComponent<Health>().Heal(5);
            }
        }
        player.moveSpeed = 1f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Grave")
            onGrave = true;
            graveGameobject = collision.gameObject;

        if (collision.gameObject == attack_box)
        {
            tutorial.inAttackBox = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Grave")
            onGrave = false;
            graveGameobject = null;

        if (collision.gameObject == attack_box)
        {
            tutorial.inAttackBox = false;
        }

        if (collision.gameObject == tutorial_box)
        {
            tutorial.popupWindowSprite.color = Color.clear;
        }
    }
}
