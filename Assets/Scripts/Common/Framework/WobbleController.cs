using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WobbleController : MonoBehaviour
{
    public float minX = -1.0f;
    public float maxX = 1.0f;
    public float minY = -1.0f;
    public float maxY = 1.0f;

    public float wobbleTime = 1.0f;
    public AnimationCurve curveX;
    public AnimationCurve curveY;

    public float startAtTime = 0.5f;
    public bool wobbleOnStart = true;
    public bool loop = true;

    bool doWobble = false;

    // Use this for initialization
    void Start()
    {
        if (wobbleOnStart)
        {
            Wobble();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Wobble()
    {
        StartCoroutine(DoWobble());
    }

    public IEnumerator DoWobble()
    {

        float elapsedTime = startAtTime;
        bool moveUp = true;

        float startTime = Time.time;

        doWobble = true;
        while (doWobble)
        {
            //wobble from start to max
            while (elapsedTime < wobbleTime)
            {
                float easingX = curveX.Evaluate(elapsedTime / wobbleTime);
                float easingY = curveY.Evaluate(elapsedTime / wobbleTime);
                float newX;
                float newY;

                if (moveUp)
                {
                    newX = Mathf.Lerp(minX, maxX, easingX);
                    newY = Mathf.Lerp(minY, maxY, easingY);
                }
                else
                {
                    newX = Mathf.Lerp(maxX, minX, easingX);
                    newY = Mathf.Lerp(maxY, minY, easingY);
                    //newX = Mathf.Lerp(minX, maxX, 1 - easingX);
                    //newY = Mathf.Lerp(minY, maxY, 1 - easingY);
                }

                transform.localPosition = new Vector2(newX, newY);

                yield return new WaitForSeconds(0.1f);

                elapsedTime += Time.deltaTime;
            }

            doWobble = false;

            if (loop)
            {
                elapsedTime = 0.0f;
                doWobble = true;
                moveUp = !moveUp;
            }
        }
    }
}