using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    Image Health;
    float hp;
    float maxHp;

    private void Start()
    {
        Health = GetComponent<Image>();
    }

    private void Update()
    {
        if (hp/maxHp >= 0.7)
        {
            Health.color = new Color(0.4f, 0.8f, 0.4f);
        }
        else if (hp/maxHp >= 0.3)
        {
            Health.color = new Color(1, 1, 0.4f);
        }
        else
        {
            Health.color = new Color(1, 0.25f, 0);
        } 
    }

    public void SetHealth(float HP, float maxHP)
    {
        hp = HP;
        maxHp = maxHP;
        Health.transform.localScale = new Vector3 ((HP/maxHP), 1, 1);
    }
}
