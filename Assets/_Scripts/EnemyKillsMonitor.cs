using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class EnemyKillsMonitor : MonoBehaviour
{
    private static EnemyKillsMonitor instance;
    [SerializeField]
    private Text _counter;
    private static int _countKills=0;

    private void Awake()
    {
        instance = this;
        PlayerPrefs.SetInt("Kills", 0);
    }
    public static void AddKills()
    {
        instance.ShowKills(++_countKills);
        PlayerPrefs.SetInt("Kills",_countKills);
    }
   
    public void ShowKills(int kills)
    {
        _counter.text = kills.ToString("00");
    }
}
