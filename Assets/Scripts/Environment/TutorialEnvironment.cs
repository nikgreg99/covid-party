using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnvironment : MonoBehaviour
{
    public delegate void TutorialAction();
    public static TutorialAction tutorialReady;

    void Start()
    {
        tutorialReady();
    }
}
