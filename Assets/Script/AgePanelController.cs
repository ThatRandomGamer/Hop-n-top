using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AgePanelController : MonoBehaviour
{
    [Header("UI")]
    public RectTransform agePanel;

    public Slider ageSlider;

    public TMP_Text ageText;

    private string AGE_KEY =
        "PlayerAge";

    void Start()
    {
        // Already recorded age
        if (PlayerPrefs.HasKey(AGE_KEY))
        {
            // agePanel.gameObject.SetActive(false);
            //return;
        }

        // Default display
        UpdateAgeText();
    }

    public void Update()
    {                                             //FO returns no decimal points
        ageText.text = "Age: " + ((int)ageSlider.value).ToString("F0");
        Debug.Log((int)ageSlider.value);
        Debug.Log(ageText.text);

    }
    public void UpdateAgeText()
    {
        // Debug.Log("test");
        // ageText.text = "Age: " + ((int)ageSlider.value).ToString("F0");

    }

    public void ConfirmAge()
    {
        int age = (int)(ageSlider.value);

        PlayerPrefs.SetInt(AGE_KEY, age);
        PlayerPrefs.Save();

        Debug.Log(age);
        StartCoroutine(ShrinkAndHide());
    }

    IEnumerator ShrinkAndHide()
    {
        Vector3 start = Vector3.one;

        Vector3 end = Vector3.zero;

        float time = 0f;

        while (time < 0.25f)
        {
            agePanel.localScale =
                Vector3.Lerp(
                    start,
                    end,
                    time / 0.25f
                );

            time +=
                Time.deltaTime;

            yield return null;
        }

        agePanel.gameObject.SetActive(false);
    }
}