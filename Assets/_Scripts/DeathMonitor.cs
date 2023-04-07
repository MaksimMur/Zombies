using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathMonitor : MonoBehaviour
{

    [SerializeField]
    private GameObject _deathMonitorButtons;

    [SerializeField]
    private Text _killedEnemies;
    public void GetDeathMonitor()
    {
        ShowDeathMonitor();
    }

    public void LeaveFromGame()
    {
        Application.Quit();
    }
    public void OpenButtons()
    {
        _deathMonitorButtons.SetActive(true);
    }
    public void CloseButtons()
    {
        _deathMonitorButtons.SetActive(false);
    }

    public static DeathMonitor instance;
    private static bool _shouldPlayOpeningAnimation = false;

    private Animator _animator;
    private AsyncOperation _loadingSceneOperation;

    public static void ShowDeathMonitor()
    {
        instance._killedEnemies.text = PlayerPrefs.GetInt("Kills").ToString("00");
        instance._animator.SetTrigger("DeathStart");
        instance._loadingSceneOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        instance._loadingSceneOperation.allowSceneActivation = false;
    }

    private void Start()
    {
        instance = this;

        _animator = GetComponent<Animator>();

        if (_shouldPlayOpeningAnimation)
        {
            _animator.SetTrigger("TryAgain");
        }
    }

    public void TryAgain()
    {
        _shouldPlayOpeningAnimation = true;
        instance._loadingSceneOperation.allowSceneActivation = true;
    }
}
