using System.Threading;
using UnityEngine;
using System.Collections;
using TMPro;

// ��� ����
// Ÿ�̸Ӱ� �귯���� ���?

// �����ؾ� �ϴ� ���

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
        // ī��Ʈ �ٿ��� ������ ����Ǵ� Ÿ�̸�
        if (cnt != null)
        {
            cnt.CountdownFinished = StartTimer;
        }
    }

    // Ÿ�̸� ����
    private void StartTimer()
    {
        timeGo.SetActive(true);

        StartCoroutine(TimerCountdown());
    }

    // Ÿ�̸Ӱ� �귯��.
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

        // ���� ������ ��������(�÷��̾ �� �����ų�, �ð��� �ٵ�) => Ÿ�̸� end 
    }
}
