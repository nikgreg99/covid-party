using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerMovement.acquiredPowerup += powerUpAcquired;
    }


    private void OnDisable()
    {
        PlayerMovement.acquiredPowerup -= powerUpAcquired;
    }

    [SerializeField] GameObject enemy;
    [SerializeField] GameObject player;
    [SerializeField] GameObject spawnSpot;
    [SerializeField] GameObject DNA;
    [SerializeField] GameObject goNextUI;
    [SerializeField] TMP_Text text;
    private string currentText;

    string[] instrunctions = new string[] {"Welcome to Covid Party!\nThe happiest place on Earth!", "Congratulations! You have been infected with a fun virus!\nAnd now you can pass it on everyone else!" ,
        "Isn't it exciting????!!!!!", "Anyway!\nLet's get started! Shall we?", "To Move use WASD\nTo switch cameras, press F", "To cough on your ENEMIES, left-click on your mouse",
        "Why don't you go ahead and cough on that FUCKER?"};
    int instrunctionsLength;
    int instrunctionsCounter = 0;

    string[] powerUp = new string[] { "Look at that!\nThe bar is filling up!", "And now you have some money!", "Why don't you approach that big DNA thingy?" };
    int powerUpLength;
    int powerUpCounter = 0;

    string[] endInstrunctions = new string[] { "Well done!\nThese allow your virus to evolve and become stronger!", "Well, I've taught everything you need to know.", "Now go on! Make us proud!\nInfect everyone you see!"};
    int endLength;
    int endCounter = 0;

    float timer = 7f;
    float counter = 0;

    private bool enemyHit = false;
    private bool firstHit = false;

    private bool isFullyInfected = false;
    private bool firstFullInfection = false;

    private bool isNearDNA = false;
    private bool firstDNA = false;

    private bool running = false;

    private bool startPowerUp = false;
    private bool startEnd = false;

    private bool acquiredPowerUp = false;
    private bool firstPowerUp = false;

    private bool done = false;

    private IEnumerator routine;

    void Start()
    {
        instrunctionsLength = instrunctions.Length - 1;
        powerUpLength = powerUp.Length - 1;
        endLength = endInstrunctions.Length - 1;
        currentText = instrunctions[instrunctionsCounter];
        routine = DisplayText();
        StartCoroutine(routine);
    }

    void powerUpAcquired(PowerUp powerup)
    {
        acquiredPowerUp = true;
    }

    void Update()
    {
        counter += Time.deltaTime;

        enemyHit = enemy.GetComponent<EnemyHealth>().IsHitAtLeastOnce;
        isFullyInfected = enemy.GetComponent<EnemyHealth>().Infected;
        isNearDNA = player.GetComponent<PlayerMovement>().isNearDNA;

        if ((Input.GetKeyDown(KeyCode.Return) || counter > timer) && instrunctionsCounter < instrunctionsLength)
        {
            if(running)
                StopCoroutine(routine);

            instrunctionsCounter++;
            currentText = instrunctions[instrunctionsCounter];
            routine = DisplayText();
            StartCoroutine(routine);
            counter = 0;
        }

        if (startPowerUp)
        {
            if ((Input.GetKeyDown(KeyCode.Return) || counter > timer) && powerUpCounter < powerUpLength)
            {
                if (running)
                    StopCoroutine(routine);

                powerUpCounter++;
                currentText = powerUp[powerUpCounter];
                routine = DisplayText();
                StartCoroutine(routine);
                counter = 0;
            }
        }

        if (startEnd)
        {
            if(endCounter == endLength)
            {
                done = true;
            }
            else if ((Input.GetKeyDown(KeyCode.Return) || counter > timer) && endCounter < endLength)
            {
                if (running)
                    StopCoroutine(routine);

                endCounter++;
                currentText = endInstrunctions[endCounter];
                routine = DisplayText();
                StartCoroutine(routine);
            }
        }

        if (enemyHit && !firstHit)
        {
            if (running)
                StopCoroutine(routine);

            if (instrunctionsCounter < instrunctionsLength)
                instrunctionsCounter = instrunctionsLength;

            currentText = "Nice! Keep hitting it until it is completely infected!";
            routine = DisplayText();
            StartCoroutine(routine);
            firstHit = true;
        }

        if (isFullyInfected && !firstFullInfection)
        {
            if (running)
                StopCoroutine(routine);

            currentText = "Look at that!\nThe bar is filling up!";
            routine = DisplayText();
            StartCoroutine(routine);
            firstFullInfection = true;
            Instantiate(DNA, spawnSpot.transform);
            startPowerUp = true;
            counter = 0;
        }

        if(isNearDNA && !firstDNA)
        {
            if (running)
                StopCoroutine(routine);

            startPowerUp = false;
            currentText = "Look at that! Go ahead!\nSpend your money!";
            routine = DisplayText();
            StartCoroutine(routine);
            firstDNA = true;
        }

        if (acquiredPowerUp && !firstPowerUp)
        {
            if (running)
                StopCoroutine(routine);

            currentText = "Well done!\nThese allow your virus to evolve and become stronger!";
            routine = DisplayText();
            StartCoroutine(routine);
            startEnd = true;
            firstPowerUp = true;
            counter = 0;
        }

        if (done && !running)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            goNextUI.SetActive(true);
        }

    }

    IEnumerator DisplayText()
    {
        running = true;

        text.text = "";

        foreach(char c in currentText.ToCharArray())
        {
            text.text += c;

            yield return new WaitForSecondsRealtime(0.04f);
        }

        running = false;

    }

    public void GoNext()
    {
        NextSceneManager.RequestNextScene(2);
       /* SceneManager.LoadScene("Loading");
        SceneManager.LoadSceneAsync(2);*/
    }

}
