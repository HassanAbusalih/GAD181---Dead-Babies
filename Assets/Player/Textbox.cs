using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Textbox : MonoBehaviour
{
    [SerializeField] List<string> textList = new List<string>();
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject textBox;
    Collider2D textCollider;
    Collider2D playerCollider;
    bool isActive;
    int counter;

    private void Start()
    {
        textCollider = GetComponent<Collider2D>();
        playerCollider = FindObjectOfType<PlayerMovement>().GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (textCollider.IsTouching(playerCollider))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            { 
                if (!isActive)
                {
                    isActive = true;
                    textBox.SetActive(true);
                    text.gameObject.SetActive(true);
                    //FindObjectOfType<PlayerMovement>().encounter = true;
                }
                NextLine();
            }

        }
        else
        {
            textBox.SetActive(false);
            text.gameObject.SetActive(false);
            isActive = false;
            counter = 0;
        }
    }

    void NextLine()
    {
        if (counter < textList.Count)
        {
            text.text = textList[counter];
            counter++;
        }
        else
        {
            textBox.SetActive(false);
            text.gameObject.SetActive(false);
            isActive = false;
            counter = 0;
            //FindObjectOfType<PlayerMovement>().encounter = false;
        }
    }
}
