using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCircle : MonoBehaviour
{
    private SpellHandler spellHandler;
    [SerializeField] private GameObject circleVFX;
    [SerializeField] private GameObject errorVFX;
    [SerializeField] private SpriteRenderer[] sr;
    [SerializeField] private GameObject ellipse;
    [SerializeField] private float scaleSpeed = 0.5f;

    private Vector3 newScale;

    private void Awake()
    {
        spellHandler = FindObjectOfType<SpellHandler>();
    }

    private void OnEnable()
    {
        ResetCircle();
    }

    private void Update()
    {
        newScale = ellipse.transform.localScale - new Vector3(scaleSpeed, scaleSpeed, scaleSpeed) * Time.deltaTime * 2f;

        if (newScale.x < 0.7f)
        {
            //Resets the Circle, should miss
            GameObject errorVFXClone = Instantiate(errorVFX, transform.position, Quaternion.identity);
            Destroy(errorVFXClone, 1);

            spellHandler.AddMultiplier(0);
            gameObject.SetActive(false);
        }
        else
        {
            newScale = Vector3.Max(newScale, new Vector3(0.1f, 0.1f, 0.1f));
            ellipse.transform.localScale = newScale;
        }

        if (newScale.x <= .95f)
        {
            foreach (var item in sr)
            {
                item.color = new Color32(241, 196, 15, 200);
            }
        }
    }

    private void OnMouseDown()
    {
        GameObject circleVFXClone = Instantiate(circleVFX, transform.position, Quaternion.identity);
        Destroy(circleVFXClone, .25f);
        if (newScale.x <= .95f)
        {
            circleVFXClone.transform.localScale = new Vector3(2, 2, 2);
            AudioManager.Instance.PlayCriticalSFX();
            spellHandler.AddMultiplier(1.5f);
            gameObject.SetActive(false);
        }
        else
        {
            spellHandler.AddMultiplier(1f);
            gameObject.SetActive(false);
        }
    }

    private void ResetCircle()
    {
        newScale = new Vector3(1.8f, 1.8f, 1.8f);
        ellipse.transform.localScale = newScale;
        foreach (var item in sr)
        {
            item.color = new Color32(255, 255, 255, 200);
        }
    }
}