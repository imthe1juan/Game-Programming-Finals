using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Circle : MonoBehaviour
{
    [SerializeField] protected MoveScaling moveScaling;
    [SerializeField] private GameObject ellipse;
    [SerializeField] private float scaleSpeed = 0.5f;

    public bool isEnemy;
    private Vector3 newScale;

    private void Update()
    {
        newScale = ellipse.transform.localScale - new Vector3(scaleSpeed, scaleSpeed, scaleSpeed) * Time.deltaTime * 1.5f;

        if (newScale.x < 0.7f)
        {
            //Resets the Circle, should miss
            ScaleMove(0);
            ellipse.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
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
            ScaleMove(2);
            ResetCircle();
        }
        else
        {
            ScaleMove(1);
            ResetCircle();
        }
    }

    private void ResetCircle()
    {
        ellipse.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
    }

    public virtual void ScaleMove(int scaler)
    {
        moveScaling.ScaleMove(scaler);
    }
}