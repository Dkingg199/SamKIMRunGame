using System.Threading;
using UnityEngine;
using System.Collections;
using TMPro;

// 기능 구현
// 타이머가 흘러가는 기능?

// 구현해야 하는 기능

public class RunTimer : MonoBehaviour
{
    [SerializeField] private RunCountDown cnt = null;
    [SerializeField] private GameObject timeGo = null;
    private float currentTime;

    private void Awake()
    {
        currentTime = 5f;
    }

    private void Start()
    {
        // 카운트 다운이 끝나면 실행되는 타이머
        if (cnt != null)
        {
            cnt.CountdownFinished = StartTimer;
        }
    }

    // 타이머 실행
    private void StartTimer()
    {
        timeGo.SetActive(true);

        StartCoroutine(TimerCountdown());
    }

    // 타이머가 흘러감.
    private IEnumerator TimerCountdown()
    {
        TextMeshProUGUI timerText = timeGo.GetComponent<TextMeshProUGUI>();

        while (currentTime > 0)
        {
            timerText.text = currentTime.ToString();
            yield return new WaitForSeconds(1f);
            currentTime--;
        }
        timerText.text = currentTime.ToString();
        yield return new WaitForSeconds(1f);

        timeGo.SetActive(false);

        // 만약 게임이 끝났을때(플레이어가 다 들어오거나, 시간이 다됨) => 타이머 end 
    }
}
