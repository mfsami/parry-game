using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public Health playerHealth;

    private void Update()
    {
        if (playerHealth != null)
        {
            healthText.text = "Health: " + playerHealth.health.ToString();
        }
    }
}
