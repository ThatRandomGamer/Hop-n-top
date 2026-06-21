using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class FadeInFadeOut : MonoBehaviour
{
    public Image FadeImage;
    public float FadeDuration = 0.5f;
    public float DisplayDuration = 1.5f;
    [SerializeField] public string NextScreen = "TitleScreen";
    [SerializeField] public bool DoOnce;
    private bool isDone;

    private string isDoneKey =
        "IsDone";

    void Start()
    {
        StartCoroutine(FadeSequence());
        if (PlayerPrefs.HasKey(isDoneKey) && DoOnce == true)
        {
            //agePanel.gameObject.SetActive(false);
            SceneManager.LoadScene(NextScreen);

        }


    }
    IEnumerator FadeSequence()
    {
        yield return StartCoroutine(Fade(0, 1));
        yield return StartCoroutine(Fade(1, 0));
        yield return new WaitForSeconds(DisplayDuration);
        SceneManager.LoadScene(NextScreen);
    }
    IEnumerator Fade(float StartAlpha, float EndAlpha)
    {
        float time = 0;
        Color color = FadeImage.color;
        while (time < FadeDuration)
        {
            float alpha = Mathf.Lerp(StartAlpha, EndAlpha, time / FadeDuration);
            FadeImage.color = new Color(color.r, color.g, color.b, alpha);
            time += Time.deltaTime;
            yield return null;
        }
        FadeImage.color = new Color(color.r, color.g, color.b, EndAlpha);

        if (DoOnce == true)
        {
            isDone = true;
            if (isDone && DoOnce == true)
            {
                PlayerPrefs.SetInt(isDoneKey, 1);
                PlayerPrefs.Save();
            }
        }

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

}
