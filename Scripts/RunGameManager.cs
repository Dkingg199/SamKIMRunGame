using Photon.Pun;
using UnityEngine;
using System.Collections;
using static RunCountDown;

// ���� ���
// ���������� player�� ����.

// �����ؾ� �ϴ� ���
// end�� ���޽� �����ٰ� check
public class RunGameManager : MonoBehaviourPunCallbacks
{
    public delegate void AllGoalInDelegate();
    public AllGoalInDelegate AllGoalInCallback;

    [SerializeField] private GameObject playerPrefab = null;
    [SerializeField] private Vector3[] playerPosition = new Vector3[4];
    private bool[] isGoalIn = new bool[4];

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
        go.name = $"Player_{PhotonNetwork.LocalPlayer.ActorNumber}";
    }

    [PunRPC]
    public void ApplyGoalIn(int _actNum)
    {
        isGoalIn[_actNum - 1] = true;

        for(int i = 0; i < 4; ++i)
        {
            if (!isGoalIn[i]) return;
        }

        AllGoalInCallback?.Invoke();
    }

}