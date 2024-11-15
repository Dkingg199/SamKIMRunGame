using Photon.Pun;
using UnityEngine;
using System.Collections;
using TMPro;

// ���� ���
// ���������� player�� ����.

// �����ؾ� �ϴ� ���
// end�� ���޽� �����ٰ� check
public class RunGameManager : MonoBehaviourPunCallbacks
{
    public delegate void AllGoalInDelegate();
    public delegate void AllMakeDelegate();
    public AllMakeDelegate AllMakeEndCallback;
    public AllGoalInDelegate AllGoalInCallback;


    [SerializeField] private GameObject playerPrefab = null;
    [SerializeField] private Vector3[] playerPosition = new Vector3[4];
    // [SerializeField] private Color[] playerColors = new Color[4];
    [SerializeField] private Material[] playerMaterials = new Material[4];
    [SerializeField] private RunTimer timer = null;
    [SerializeField] private GameObject resultUI = null;
    [SerializeField] private GameObject[] playerList = new GameObject[4];
    private bool[] isGoalIn = new bool[4];
    private string[] playerNickNames = new string[4];
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


        // ���� ������ �ݹ� ����
        // resultâ�� setActive(true)
        // 5�ʵ� �ٽ� Looby Scene���� ���ư�.

        // timer������ ȣ�����.
        timer.TimeEndCallback += TimeEnd;

        StartCoroutine(MakePlayerCoroutine());
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

        yield return null;

        GameObject go = PhotonNetwork.Instantiate(
                playerPrefab.name,
                playerPosition[PhotonNetwork.LocalPlayer.ActorNumber - 1],
                Quaternion.identity,
                0);
        go.AddComponent<RunPlayerCtrl>();

        photonView.RPC("ApplyPlayerList", RpcTarget.All);

        yield return new WaitForSeconds(delay);

        photonView.RPC("SynData", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
    }

    // ���� goalIn�ϴ��� check
    [PunRPC]
    public void ApplyGoalIn(int _actNum)
    {
        isGoalIn[_actNum - 1] = true;

        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; ++i)
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
    public void SynData(int _actNum)
    {
        //Debug.LogError("SynData : " + _actNum + " Player : " + playerList[_actNum - 1] + " SetColor : " + playerMaterials[_actNum - 1] + "Time.time : " + Time.time);

        //playerList[_actNum - 1].GetComponent<MeshRenderer>().material.SetColor("_Color", playerMaterials[_actNum - 1].color);
        //Debug.LogError(playerList[_actNum - 1].GetComponent<MeshRenderer>().material.color);
        playerList[_actNum - 1].GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
        // playerNickNames[_actNum - 1] = PhotonNetwork.NickName;
        // playerList[_actNum - 1].transform.GetChild(0).GetComponent<TextMeshPro>().text = playerNickNames[_actNum - 1];
    }

    [PunRPC]
    public void ApplyPlayerList()
    {
        // ���� �濡 ������ �ִ� �÷��̾��� ��
        Debug.LogError("CurrentRoom PlayerCount : " + PhotonNetwork.CurrentRoom.PlayerCount);

        // ���� �����Ǿ� �ִ� ��� ����� ��������
        //PhotonView[] photonViews = FindObjectsOfType<PhotonView>();
        PhotonView[] photonViews =
            FindObjectsByType<PhotonView>(FindObjectsSortMode.None);



        // �Ź� �������� �ϴ°� �����Ƿ� �÷��̾� ���ӿ�����Ʈ ����Ʈ�� �ʱ�ȭ
        System.Array.Clear(playerList, 0, playerList.Length);

        // ���� �����Ǿ� �ִ� ����� ��ü��
        // �������� �÷��̾���� ���ͳѹ��� ����,
        // ���ͳѹ��� �������� �÷��̾� ���ӿ�����Ʈ �迭�� ä��
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; ++i)
        {
            // Ű�� 0�� �ƴ� 1���� ����
            int key = i + 1;
            for (int j = 0; j < photonViews.Length; ++j)
            {
                // ���� PhotonNetwork.Instantiate�� ���ؼ� ������ ����䰡 �ƴ϶�� �ѱ�
                if (photonViews[j].isRuntimeInstantiated == false) continue;
                // ���� ���� Ű ���� ��ųʸ� ���� �������� �ʴ´ٸ� �ѱ�
                if (PhotonNetwork.CurrentRoom.Players.ContainsKey(key) == false) continue;

                // ������� ���ͳѹ�
                int viewNum = photonViews[j].Owner.ActorNumber;
                // �������� �÷��̾��� ���ͳѹ�
                int playerNum = PhotonNetwork.CurrentRoom.Players[key].ActorNumber;

                // ���ͳѹ��� ���� ������Ʈ�� �ִٸ�,
                if (viewNum == playerNum)
                {
                    // ���� ���ӿ�����Ʈ�� �迭�� �߰�
                    playerList[playerNum - 1] = photonViews[j].gameObject;
                    // ���ӿ�����Ʈ �̸��� �˾ƺ��� ���� ����
                    playerList[playerNum - 1].name = "Player_" + photonViews[j].Owner.NickName;

                    // playerList[playerNum - 1].GetComponent<RunPlayerCtrl>().SetMaterial(PhotonNetwork.LocalPlayer.ActorNumber);
                }


            }
        }
    }

    }