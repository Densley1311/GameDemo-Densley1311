using UnityEngine;
using UnityEngine.UI;

public class SoulIcon : MonoBehaviour
{
    public Image iconImage;
    public int maxEnemies; // Change this to the number of enemies needed to fill the icon
    [SerializeField]
    public float enemyRaiseCount = 0;

    private void Start()
    {
        // Set the initial state to empty
        UpdateSoulIcon();
    }

    public void EnemyRaised()
    {
        if (enemyRaiseCount < maxEnemies)
        {
            enemyRaiseCount++;
            UpdateSoulIcon();
        }
    }

    public void UpdateSoulIcon()
    {
        float fillAmount = (float)enemyRaiseCount / maxEnemies;
        iconImage.fillAmount = fillAmount;

        // You can also change the image sprite when it's full or use animations here
    }
}
