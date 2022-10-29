using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2D;
    BoxCollider2D myBoxCollider2D;
    public Animator anim;
    public Animator battleAnim;
    public SaveLoad saveLoad;
    private float speed = 3.0f;
    Vector2 movementOfPlayer;


    // Start is called before the first frame update
    private void Start()
    {
        saveLoad.Load();
        myBoxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movementOfPlayer.x = Input.GetAxisRaw("Horizontal");
        movementOfPlayer.y = Input.GetAxisRaw("Vertical");
        PlayerAnimation();
        BattleEcnouter();
    }

    private void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + movementOfPlayer * speed * Time.fixedDeltaTime);
        
    }
    void PlayerAnimation()
    {
        anim.SetFloat("ySpeed", movementOfPlayer.y);
        anim.SetFloat("xSpeed", Mathf.Abs(movementOfPlayer.x));
        if(movementOfPlayer.x < 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (movementOfPlayer.x > 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }    
    }
    void BattleEcnouter()
    {
        if (myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Tall Grass")))
        {
            int battleEcnounterRng = Random.Range(1, 500);
            if(battleEcnounterRng <= 5)
            {
                Debug.Log("Encounter happening.");
                battleAnim.SetBool("Encounter", true);
                PlayerPrefs.DeleteAll();
                saveLoad.PlayerSave();
                saveLoad.isTrainer = false;
                saveLoad.EnemySave();
                StartCoroutine(LoadScene());
            }
        }
        else if (saveLoad.isTrainer)
        {
            saveLoad.PlayerSave();
            saveLoad.EnemySave();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Trainer"))
        {
            saveLoad.isTrainer = true;
        }
    }

    IEnumerator LoadScene()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);
        asyncOperation.allowSceneActivation = false;
        yield return new WaitForSeconds(1.1f);
        asyncOperation.allowSceneActivation = true;
    }

}
