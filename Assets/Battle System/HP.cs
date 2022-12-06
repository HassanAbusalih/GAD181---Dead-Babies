using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    Image Health;
    float beforeDamage;
    float hp;
    float maxHp;

    private void Start()
    {
        Health = GetComponent<Image>();
    }

    private void Update()
    {
        if (Health.transform.localScale.x >= 0.7)
        {
            Health.color = new Color(0.4f, 0.8f, 0.4f);
        }
        else if (Health.transform.localScale.x >= 0.3)
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
        if (hp != 0 && HP < hp)
        {
            beforeDamage = hp;
        }
        hp = HP;
        maxHp = maxHP;
        if (hp == maxHp)
        {
            Health.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            StartCoroutine(AnimateHP());
        }
    }

    IEnumerator AnimateHP()
    {
        float reduceBy = (beforeDamage - hp) / 300;  
        for (float i = beforeDamage; i > hp; i-= reduceBy)
        {
            Health.transform.localScale = new Vector3((i/maxHp), 1, 1);
            yield return new WaitForSeconds(0.001f);
        }
        if (hp == 0)
        {
            Health.transform.localScale = new Vector3(0, 1, 1);
        }
    }
}
