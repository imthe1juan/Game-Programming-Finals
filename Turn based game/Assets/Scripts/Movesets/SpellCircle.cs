using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCircle : MonoBehaviour
{
    private SpellHandler spellHandler;
    [SerializeField] private GameObject ellipse;
    [SerializeField] private float scaleSpeed = 0.5f;

    public bool isEnemy;
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
        newScale = ellipse.transform.localScale - new Vector3(scaleSpeed, scaleSpeed, scaleSpeed) * Time.deltaTime * 1.5f;

        if (newScale.x < 0.7f)
        {
            //Resets the Circle, should miss
            spellHandler.AddMultiplier(0);
            gameObject.SetActive(false);
        }
        else
        {
            newScale = Vector3.Max(newScale, new Vector3(0.1f, 0.1f, 0.1f));
            ellipse.transform.localScale = newScale;
        }
    }

    private void OnMouseDown()
    {
        if (newScale.x <= .95f)
        {
            spellHandler.AddMultiplier(1);
            gameObject.SetActive(false);
        }
        else
        {
            spellHandler.AddMultiplier(.5f);

            gameObject.SetActive(false);
        }
    }

    private void ResetCircle()
    {
        ellipse.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
    }
}