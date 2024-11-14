using UnityEngine;
using System.Collections;

// 구현 기능 : 카운트다운
public class RunCountDown : MonoBehaviour
{
    // 카운트 다운이 끝난걸 알려주기 위한 델리게이트
    public delegate void CountdownFinishedDelegate();
    public CountdownFinishedDelegate CountdownFinished;

    [SerializeField] private GameObject[] imgs = new GameObject[4];

    private void Start()
    {
        StartCoroutine(CountDownCoroutine());
    }

    // 호출되면 카운트다운 시작
    private IEnumerator CountDownCoroutine()
    {
        for (int i = 0; i < imgs.Length; ++i)
        {
            imgs[i].SetActive(true);

            yield return new WaitForSeconds(1f);

            imgs[i].SetActive(false);
        }

        // 시작했다는걸 알림 -> 타이머 재생, 개인별 기록을 기록하기 위한 타이머, 플레이어 이동가능 => true
        CountdownFinished?.Invoke();
    }
}
