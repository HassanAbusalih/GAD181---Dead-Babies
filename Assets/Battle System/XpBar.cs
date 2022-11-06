using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XpBar : MonoBehaviour
{

    Image xpBar;

    private void Start()
    {
        xpBar = GetComponent<Image>();
    }

    public void SetXpBar(float currentXp, float xpThreshhold)
    {
        xpBar.transform.localScale = new Vector3((currentXp / xpThreshhold), 1, 1);
    }
}
