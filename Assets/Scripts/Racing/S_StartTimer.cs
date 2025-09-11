using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class S_StartTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float countDown, visibleDuration, fadeDuration;
    [SerializeField] private GameObject qtmWheel;
    
    private float timer;
    private bool isQtmSpawned;
    
    public event Action OnTimerEnd;
    public void StartTimer()
    {

        timer = countDown;

        StartCoroutine(CountDown());
    }
    
    private IEnumerator CountDown()
    {
        // Count down from 3
        while (true)
        {
            if (timer > 0)
            {
                timerText.text = timer.ToString("F2");
                if (Mathf.Approximately(Mathf.Ceil(timer), 2) && !isQtmSpawned)
                {
                    isQtmSpawned = true;
                    qtmWheel.SetActive(true);
                }
            }
            else if (timer <= 0)
            {
            
                OnTimerEnd?.Invoke();
                timerText.text = "GO!";
                qtmWheel.SetActive(false);
                break;
            }
            timer -= Time.deltaTime;
            yield return null;
        }
        // Let text be visible for time
        yield return new WaitForSeconds(fadeDuration);
        
        timer = 0f;
        // Fade out text
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            timerText.alpha = Mathf.Lerp(timerText.alpha, 0.0f, timer / fadeDuration);
            yield return null;
        }

        Destroy(gameObject);
    }
}
