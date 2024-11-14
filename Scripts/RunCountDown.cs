using UnityEngine;
using System.Collections;

// ���� ��� : ī��Ʈ�ٿ�
public class RunCountDown : MonoBehaviour
{
    // ī��Ʈ �ٿ��� ������ �˷��ֱ� ���� ��������Ʈ
    public delegate void CountdownFinishedDelegate();
    public CountdownFinishedDelegate CountdownFinished;

    [SerializeField] private GameObject[] imgs = new GameObject[4];

    private void Start()
    {
        StartCoroutine(CountDownCoroutine());
    }

    // ȣ��Ǹ� ī��Ʈ�ٿ� ����
    private IEnumerator CountDownCoroutine()
    {
        for (int i = 0; i < imgs.Length; ++i)
        {
            imgs[i].SetActive(true);

            yield return new WaitForSeconds(1f);

            imgs[i].SetActive(false);
        }

        // �����ߴٴ°� �˸� -> Ÿ�̸� ���, ���κ� ����� ����ϱ� ���� Ÿ�̸�, �÷��̾� �̵����� => true
        CountdownFinished?.Invoke();
    }
}
