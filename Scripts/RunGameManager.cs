using Photon.Pun;
using UnityEngine;
using System.Collections;
using static RunCountDown;
using Unity.VisualScripting;

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
    [SerializeField] private Color[] playerColors = new Color[4];
    [SerializeField] private RunTimer timer = null;
    [SerializeField] private GameObject resultUI = null;
    
    private GameObject[] playerList = new GameObject[4];
    private bool[] isGoalIn = new bool[4];
    private bool timeEnd = false;
    private bool allGoalIn = false;


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

        // timer������ ȣ�����.
        timer.TimeEndCallback += TimeEnd;

    }

    private void Update()
    {
        if (timeEnd || allGoalIn)
        {
            // �����ִٰ� ��� resultâ�� �ٿ�.
            resultUI.SetActive(true);
        }
    }

    // �÷��̾ ����.
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
        playerList[PhotonNetwork.LocalPlayer.ActorNumber - 1] = go;

        photonView.RPC("SynColor", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
    }


    // ���� goalIn�ϴ��� check
    [PunRPC]
    public void ApplyGoalIn(int _actNum)
    {
        isGoalIn[_actNum - 1] = true;

        for(int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; ++i)
        {
            if (!isGoalIn[i]) return;
        }

        allGoalIn = true;
        AllGoalInCallback?.Invoke();
    }

    private void TimeEnd()
    {
        timeEnd = true;
    }

    [PunRPC]
    public void SynColor(int _actNum)
    {
        playerList[_actNum-1].GetComponent<MeshRenderer>().material.color = playerColors[_actNum-1];
    }
}