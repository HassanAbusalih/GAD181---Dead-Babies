using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progression : MonoBehaviour
{
    Trainer trainer;

    private void Start()
    {
        trainer = GetComponent<Trainer>();
    }

    void Update()
    {
        if (PlayerPrefs.GetInt($"{trainer.trainerBase.trainerName}1") == 0)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
