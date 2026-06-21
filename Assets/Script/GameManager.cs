using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool jumpTutorialActive = false;
    public bool jumpTutorialDone = false;

    void Awake()
    {
        instance = this;
    }
}