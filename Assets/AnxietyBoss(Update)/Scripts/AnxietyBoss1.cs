using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnxietyBoss : MonoBehaviour
{
    [SerializeField] Transform[] libraryPos;
    [SerializeField] Transform[] classPos;
    [SerializeField] Transform[] cafeteriaPos;

    [SerializeField] float intervalTeleport = 5;
    [SerializeField] bool isVulnerable = false;
    [SerializeField] float recovering = 5;
    Animator animator;

    [SerializeField] int currentLevel = 1;
    [SerializeField] bool isSwitchingLevel = false;
    public GameObject blindness;

    bool teleported = false;

    int randomPos;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        SwitchCaseBossPhase();
    }

    // Update is called once per frame
    void Update()
    {
        bool keyPressed = Input.GetKeyDown(KeyCode.Q);
        if (keyPressed)
        {
            StopAllCoroutines();
            BossBlind();
        }

        if (!isVulnerable)
        {
            Teleporting();
        }
    }

    void SwitchCaseBossPhase()
    {
        switch (currentLevel)
        {
            case 1:
                Debug.Log("case 1");
                transform.position = libraryPos[0].transform.position;
                Teleporting();
                break;
            case 2:
                Debug.Log("case 2");
                this.gameObject.SetActive(true);
                transform.position = classPos[0].transform.position;
                Teleporting();
                break;
            case 3:
                Debug.Log("case 3");
                this.gameObject.SetActive(true);
                transform.position = cafeteriaPos[0].transform.position;
                Teleporting();
                break;
            case 4:
                Debug.Log("End");
                break;
        }
    }
    void Teleporting()
    {
        if (!teleported)
        {
            Debug.Log("teleporting");
            teleported = true;
            StartCoroutine(NextPosition());
        }
    }
    void BossBlind() //Call when taking Damage
    {
        isVulnerable = true;
        Debug.Log("Boss Blind");
        animator.SetBool("isBlind", true);
        StartCoroutine(RecoveryState());
        StartCoroutine(TeleportToNewPos());
    }

    void UpgradingCurrent() //To prevent 2x increment
    {
        if (isSwitchingLevel)
        {
            currentLevel += 1;
            Debug.Log("currentLevel is Now" + currentLevel);
        }
        
    }
    private void OnCollisionEnter (Collision collision)
    {
        if (isVulnerable && collision.gameObject.CompareTag("Player"))
        {
            if (!isSwitchingLevel)
            {
                isSwitchingLevel = true;
                UpgradingCurrent();

            }
            //this.gameObject.SetActive(false); 
        }
        else//testing if its colliding
        {
            Debug.Log("He's not blind!");
        }
    }
    IEnumerator TeleportToNewPos()
    {
        yield return new WaitForSeconds(recovering);
        if (currentLevel == 1)
        {
            randomPos = Random.Range(0, libraryPos.Length);
            transform.position = libraryPos[randomPos].transform.position;
            teleported = false;
            Debug.Log("telorting to libraryPos #" + randomPos);
        }
        else if (currentLevel == 2)
        {
            randomPos = Random.Range(0, classPos.Length);
            transform.position = classPos[randomPos].transform.position;
            teleported = false;
            Debug.Log("telorting to classPos #" + randomPos);
        }
        else if (currentLevel == 3)
        {
            randomPos = Random.Range(0,cafeteriaPos.Length);
            transform.position = cafeteriaPos[randomPos].transform.position;
            teleported = false;
            Debug.Log("telorting to cafeteriaPos #" + randomPos);
        }
        
    }

    IEnumerator NextPosition()
    {
        yield return new WaitForSeconds(intervalTeleport);

        switch (currentLevel)
        {
            case 1:
                randomPos = (randomPos + 1) % libraryPos.Length;
                this.transform.position = libraryPos[randomPos].position;
                teleported = false;
                break;
            case 2:
                randomPos = (randomPos + 1) % classPos.Length;
                this.transform.position = classPos[randomPos].position;
                teleported = false;
                break;
            case 3:
                randomPos = (randomPos + 1) % cafeteriaPos.Length;
                this.transform.position = cafeteriaPos[randomPos].position;
                teleported = false;
                break;
            default:
                Debug.LogError("Over the amount of Key Areas");
                break;
        }
    }

    IEnumerator RecoveryState()
    {
        yield return new WaitForSeconds(recovering);
        isVulnerable = false;
        isSwitchingLevel = false;
        blindness.SetActive(true);
        animator.SetBool("isBlind", false);
        StartCoroutine(PlayerRecovery());
        Debug.Log("All State is now false");
    }

    IEnumerator PlayerRecovery()
    {
        yield return new WaitForSeconds(3);
        blindness.SetActive(false);
    }
}
