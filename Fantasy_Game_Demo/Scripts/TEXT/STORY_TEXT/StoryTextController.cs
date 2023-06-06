using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoryTextController : MonoBehaviour
{
    public float displayDuration = 6f;
    public float fadeOutDuration = 2f;

    private Transform[] childObjects;
    private int currentChildIndex = 0;

    private void Start()
    {
        childObjects = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            childObjects[i] = transform.GetChild(i);
        }

        StartCoroutine(DisplayChildObjects());
    }

    private IEnumerator DisplayChildObjects()
    {
        while (currentChildIndex < childObjects.Length)
        {
            Transform currentChild = childObjects[currentChildIndex];
            currentChild.gameObject.SetActive(true);
            FadeOutAndHide(currentChild.gameObject);

            yield return new WaitForSeconds(displayDuration);

            currentChildIndex++;
        }
    }

    private void FadeOutAndHide(GameObject obj)
    {
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            StartCoroutine(FadeOutCoroutine(spriteRenderer));
        }
    }

    private IEnumerator FadeOutCoroutine(SpriteRenderer spriteRenderer)
    {
        Color originalColor = spriteRenderer.color;

        float elapsedTime = 0f;
        while (elapsedTime < fadeOutDuration)
        {
            float fadeAmount = 1f - elapsedTime / fadeOutDuration;
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, fadeAmount);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        spriteRenderer.gameObject.SetActive(false);
    }
}
