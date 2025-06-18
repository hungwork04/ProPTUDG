using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReloadUI : MonoBehaviour
{
    public static ReloadUI instance;

    [SerializeField] private TextMeshProUGUI reloadText;

    private void Awake()
    {
        if (instance == null) instance = this;
        reloadText.gameObject.SetActive(false);
    }

    public void ShowReloadText()
    {
        reloadText.gameObject.SetActive(true);
    }

    public void HideReloadText()
    {
        reloadText.gameObject.SetActive(false);
    }
}
