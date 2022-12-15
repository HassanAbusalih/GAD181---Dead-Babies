using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progression : MonoBehaviour
{
    [SerializeField] Trainers trainerBase;

    void Update()
    {
        if (PlayerPrefs.GetInt($"{trainerBase.trainerName}1") == 0)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
