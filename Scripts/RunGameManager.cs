using Photon.Pun;
using UnityEngine;
using System.Collections;

// 구현 기능
// 순차적으로 player를 만듦.

// 구현해야 하는 기능
// end에 도달시 끝났다고 check
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

        // 게임 끝남을 콜백 받음
        // result창을 setActive(true)
        // 5초뒤 다시 Looby Scene으로 돌아감.

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