using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image imgHP;
    [SerializeField] private TextMeshProUGUI textHP;
    [SerializeField] private float updateSpeedSeconds = 0.5f;

    private void Awake()
    {

        GetComponent<IHealthSystem>().onHpChanged += GetDamage;
    }
    private void GetDamage()
    {
        StartCoroutine("ChangeHealthBar");
    }

    public void HideHealthBarUI()
    { 
        GetComponentInChildren<Canvas>().gameObject.SetActive(false);
    }


    private IEnumerator ChangeHealthBar()
    {
        float prechange = imgHP.fillAmount;
        float nextCond = GetComponent<IHealthSystem>().CurrentHealth / GetComponent<IHealthSystem>().MaxHeatlh;

        float elapsed = 0;
        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            float v = Mathf.Lerp(prechange, nextCond, elapsed / updateSpeedSeconds);

            imgHP.fillAmount = v;
            textHP.text = Mathf.CeilToInt(v*100).ToString()+"hp";
            yield return null;
        }

        imgHP.fillAmount = nextCond;

    }
}
