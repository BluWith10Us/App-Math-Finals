using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI timerText;

    float timer;

    void Update()
    {
        // TIMER
        timer += Time.deltaTime;
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);

        timerText.text = $"TIME: {minutes:00}:{seconds:00}";

        // HP DISPLAY
        if (PlayerStats.Instance != null)
            hpText.text = $"HP: {PlayerStats.Instance.lives}";
    }
}
