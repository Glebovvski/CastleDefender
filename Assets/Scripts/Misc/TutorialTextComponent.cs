using TMPro;
using UnityEngine;

public class TutorialTextComponent : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public void SetText(string data) => text.text = data;
    public void Hide() => this.gameObject.SetActive(false);
    public void Show() => this.gameObject.SetActive(true);
}
