using Photon.Pun;
using UnityEngine;
using System.Collections;

// ���� ���
// ���������� player�� ����.

// �����ؾ� �ϴ� ���
// end�� ���޽� �����ٰ� check
public class RunGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerPrefab = null;
    [SerializeField] private Vector3[] playerPosition = new Vector3[4];

    private void Start()
    {
        if (playerPrefab != null)
        {
            //
            DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;
            if (!pool.ResourceCache.ContainsKey(playerPrefab.name))
                pool.ResourceCache.Add(playerPrefab.name, playerPrefab);
            //
        }

        StartCoroutine(MakePlayerCoroutine());

        // ���� ������ �ݹ� ����
        // resultâ�� setActive(true)
        // 5�ʵ� �ٽ� Looby Scene���� ���ư�.

    }

    private IEnumerator MakePlayerCoroutine()
    {
        float delay = 1f * PhotonNetwork.LocalPlayer.ActorNumber;

        yield return new WaitForSeconds(delay);

        GameObject go = PhotonNetwork.Instantiate(
                playerPrefab.name,
                playerPosition[PhotonNetwork.LocalPlayer.ActorNumber - 1],
                Quaternion.identity,
                0);
        go.AddComponent<RunPlayerCtrl>();
    }


}