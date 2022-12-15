using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    bool skipGarbage;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EndCredits());
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetKey(KeyCode.Space))
        {
            skipGarbage = true;
        }
    }

    IEnumerator EndCredits()
    {
        yield return new WaitUntil(() => skipGarbage || timer == 35);
        SceneManager.LoadScene(0);
    }
}
