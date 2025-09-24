using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;

public class S_StartTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float countDown, visibleDuration, fadeDuration;
    
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
                var timeCeil = Mathf.Ceil(timer);
                timerText.text = timeCeil.ToString(CultureInfo.InvariantCulture);
                if (Mathf.Approximately(timeCeil, visibleDuration) && !isQtmSpawned)
                {
                    isQtmSpawned = true;
                    if (S_MathManager.Instance.isActiveAndEnabled)
                        S_MathManager.Instance.DisplayQuestion();
                    
                }
            }
            else if (timer <= 0)
            {
            
                OnTimerEnd?.Invoke();
                timerText.text = "GO!";
                if (S_MathManager.Instance.isActiveAndEnabled)
                    S_MathManager.Instance.RaceStart();
                break;
            }
            timer -= Time.deltaTime;
            yield return null;
        }
        // Let text be visible for time
        yield return new WaitForSeconds(2);
        
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
