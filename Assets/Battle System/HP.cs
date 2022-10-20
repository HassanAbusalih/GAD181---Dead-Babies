using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    Image Health;

    private void Start()
    {
        Health = GetComponent<Image>();
    }

    public void SetHealth(float HP, float maxHP)
    {
        Health.transform.localScale = new Vector3 ((HP/maxHP), 1, 1);
    }
}
