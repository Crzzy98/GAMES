using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script handles damage behavior unrelated to health of the character or object
public class Damageable : MonoBehaviour
{
    [SerializeField]public bool shouldFlashWhenHit;
    private bool isFlashing;
    private Image childImageRenderer;
    private Color originalColor;

    //Variables used in sound effect behavior
    [SerializeField]public AudioClip soundFile; // Assign the sound file in the Inspector
    private AudioSource audioSource;

    private void Start()
    {
        childImageRenderer = GetComponentInChildren<Image>();
        originalColor = childImageRenderer.color;
        audioSource = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Check for current object being hit by player attacks
        if (collision.gameObject.CompareTag("Chaser_Spell")
            || collision.gameObject.CompareTag("Projectile"))
        {
            if (shouldFlashWhenHit)
                if(!isFlashing)
                    StartCoroutine(flashCurrentObjectRed());

            playSoundEffect();
        }
    }
    //Method flashes the current gameobject red
    //Meant for use when current gameobjec t comes in contact with a projectile
    private System.Collections.IEnumerator flashCurrentObjectRed()
    {
        Debug.Log("ROUTINE EXECUTED");
        isFlashing = true;
        float flashTime = 1.0f;
        float flashInterval = 0.2f;
        int flashCount = (int)(flashTime / (2 * flashInterval));

        for (int i = 0; i < 1; i++)
        {
            // Set the color of all child SpriteRenderers to red
            setImageColor(childImageRenderer, Color.red);
            yield return new WaitForSeconds(flashInterval);

            // Restore the original colors of child SpriteRenderers
            resetImageColor(childImageRenderer, originalColor);
            yield return new WaitForSeconds(flashInterval);
        }

        isFlashing = false;
    }
    //Method sets the color of the current objects child image
    private void setImageColor(Image image, Color color)
    {
        image.color = color;
    }
    //Method RESETS the color of the current objects child image
    private void resetImageColor(Image image,Color originalColor)
    {
        image.color = originalColor;
    }

    public void playSoundEffect()
    {
        audioSource.PlayOneShot(soundFile);
    }
}
