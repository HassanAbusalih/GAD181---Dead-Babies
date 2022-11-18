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
    public SwitchPokemon switchPokemon;
    bool encounter;
    float cooldown;


    // Start is called before the first frame update
    private void Start()
    {
        LoadPos();
        saveLoad.Load();
        myBoxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown < 5)
        {
            cooldown += Time.deltaTime;
        }
        if (!switchPokemon.isActive && !encounter && !switchPokemon.controls)
        {
            movementOfPlayer.x = Input.GetAxisRaw("Horizontal");
            movementOfPlayer.y = Input.GetAxisRaw("Vertical");
            PlayerAnimation();
            BattleEncounter();
        }
        else
        {
            movementOfPlayer.x = 0;
            movementOfPlayer.y = 0;
            anim.StopPlayback();
        }
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
    void BattleEncounter()
    {
        int battleEncounterRNG = Random.Range(1, 500);
        if (myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("1")))
        {
            if(battleEncounterRNG <= 5 && !encounter && cooldown > 4)
            {
                StartEncounter(Random.Range(1, 5));
            }
        }
        else if (myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("2")))
        {
            if (battleEncounterRNG <= 5 && !encounter && cooldown > 4)
            {
                StartEncounter(Random.Range(5, 10));
            }
        }
        else if (myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("3")))
        {
            if (battleEncounterRNG <= 5 && !encounter && cooldown > 4)
            {
                StartEncounter(Random.Range(10, 15));
            }
        }
        else if (myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("4")))
        {
            if (battleEncounterRNG <= 5 && !encounter && cooldown > 4)
            {
                StartEncounter(Random.Range(15, 20));
            }
        }
        else if (myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("5")))
        {
            if (battleEncounterRNG <= 5 && !encounter && cooldown > 4)
            {
                StartEncounter(Random.Range(20, 25));
            }
        }
        else if (saveLoad.isTrainer && !encounter)
        {
            encounter = true;
            saveLoad.PlayerSave();
            saveLoad.EnemySave(0);
            SavePos();
            StartCoroutine(LoadScene());
        }
    }

    void StartEncounter(int level)
    {
        encounter = true;
        saveLoad.isTrainer = false;
        battleAnim.SetBool("Encounter", true);
        saveLoad.PlayerSave();
        saveLoad.EnemySave(level);
        SavePos();
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);
        asyncOperation.allowSceneActivation = false;
        yield return new WaitForSeconds(1.1f);
        asyncOperation.allowSceneActivation = true;
    }

    void SavePos()
    {
        PlayerPrefs.SetFloat("X", transform.position.x);
        PlayerPrefs.SetFloat("Y", transform.position.y);
    }

    void LoadPos()
    {
        transform.position = new Vector2 (PlayerPrefs.GetFloat("X"), PlayerPrefs.GetFloat("Y"));
        PlayerPrefs.DeleteKey("X");
        PlayerPrefs.DeleteKey("Y");
    }
}
