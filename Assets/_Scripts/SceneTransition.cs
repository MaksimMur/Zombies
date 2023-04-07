 using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _loadingPercentage;
    [SerializeField]
    private Slider _loadingSlider;

    private static SceneTransition instance;
    private static bool _shouldPlayOpeningAnimation = false;

    private Animator _animator;
    private AsyncOperation _loadingSceneOperation;



    public static void SwitchToScene(string sceneName)
    {
        instance._animator.SetTrigger("sceneClosing");

        instance._loadingSceneOperation = SceneManager.LoadSceneAsync(sceneName);
        instance._loadingSceneOperation.allowSceneActivation = false;
    }

    private void Start()
    {
        instance = this;

        _animator = GetComponent<Animator>();

        if (_shouldPlayOpeningAnimation)
        {
            _animator.SetTrigger("sceneOpening");
        }
    }
    private void Update()
    {
        if (_loadingSceneOperation != null)
        {
            _loadingSlider.value = Mathf.RoundToInt(_loadingSceneOperation.progress * 100);
            _loadingPercentage.text = _loadingSlider.value.ToString() + "%";
        }
    }

    public void OnAnimatorOver()
    {
        _shouldPlayOpeningAnimation = true;
        instance._loadingSceneOperation.allowSceneActivation = true;
    }

}
