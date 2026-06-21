using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] public AudioSource clip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Clicked()
    {
        clip.Play();
    }
}
