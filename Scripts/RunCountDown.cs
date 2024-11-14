using UnityEngine;
using System.Collections;

// ���� ��� : ī��Ʈ�ٿ�
public class RunCountDown : MonoBehaviour
{
    [SerializeField] private GameObject[] imgs = new GameObject[4];

    private void Start()
    {
        StartCoroutine(CountDownCoroutine());

        Debug.Log(1);
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
    }
}
