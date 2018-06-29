using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackoutEffectsController : MonoBehaviour {

    public SpriteRenderer blackoutBackground;
    public SpriteRenderer blackoutForeground;

    public float blackoutIntroTime = 0.5f;

    public FloatVariable timeScale;

    public void OnBlackoutStart()
    {
        StartCoroutine(StartBlackout());

    }

    public void OnBlackoutEnd()
    {
        blackoutBackground.gameObject.SetActive(false);
        blackoutForeground.gameObject.SetActive(false);
    }

    public IEnumerator StartBlackout()
    {
        Color blackoutColor = blackoutBackground.color;
        blackoutColor.a = 1.0f;        
        blackoutBackground.color = blackoutColor;

        Color blackoutFColor = blackoutForeground.color;
        blackoutFColor.a = 1.0f;
        blackoutForeground.color = blackoutFColor;

        blackoutBackground.gameObject.SetActive(true);
        blackoutForeground.gameObject.SetActive(true);

        Time.timeScale = 0.0f;
        

        yield return new WaitForSecondsRealtime(blackoutIntroTime * 0.5f);

        float startTime = Time.realtimeSinceStartup;
        float endTime = startTime + blackoutIntroTime * 0.5f;

        while (Time.realtimeSinceStartup < endTime)
        {
            float alpha = Mathf.InverseLerp(endTime, startTime, Time.realtimeSinceStartup);
            blackoutFColor.a = alpha;
            blackoutForeground.color = blackoutFColor;

            yield return null;
        }

        blackoutFColor.a = 0.0f;
        blackoutForeground.color = blackoutFColor;

        Time.timeScale = timeScale.Value;
    }
}
