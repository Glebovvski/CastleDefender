using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private bool isAlwaysActive = false;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image healthBar;

    public void Init()
    {
        if (!isAlwaysActive)
            healthBar.gameObject.SetActive(false);
        canvas.worldCamera = Camera.main;
        healthBar.fillAmount = 1;
    }

    private void Update()
    {
        if (healthBar.gameObject.activeSelf)
            this.transform.LookAt(Camera.main.transform);
    }

    public void UpdateHealth(float health, bool shouldShow = true)
    {
        if (!isAlwaysActive && shouldShow) healthBar.gameObject.SetActive(true);
        healthBar.fillAmount = health;
    }
}
