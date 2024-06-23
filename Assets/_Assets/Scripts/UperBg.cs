using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UperBgLogic : MonoBehaviour
{
    public GameObject Miner;
    public GameObject Player;

    public void Hide()
    {
        Miner.SetActive(false);
        Player.SetActive(true);
    }
}
