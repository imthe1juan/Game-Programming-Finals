using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Circle : MonoBehaviour
{
    [SerializeField] private GameObject errorVFX;
    [SerializeField] private SpriteRenderer[] sr;
    [SerializeField] private MoveScaling moveScaling;
    [SerializeField] private GameObject ellipse;
    [SerializeField] private float scaleSpeed = 0.5f;
    public bool isEnemy;
    private Vector3 newScale;

    private void Update()
    {
        newScale = ellipse.transform.localScale - new Vector3(scaleSpeed, scaleSpeed, scaleSpeed) * Time.deltaTime * 2f;

        if (newScale.x < 0.7f)
        {
            //Resets the Circle, should miss
            GameObject errorVFXClone = Instantiate(errorVFX, transform.position, Quaternion.identity);
            Destroy(errorVFXClone, 1);

            ScaleMove(0);
            ResetCircle();
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
        Debug.Log("Clicked");
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
        newScale = new Vector3(1.8f, 1.8f, 1.8f);
        ellipse.transform.localScale = newScale;
        foreach (var item in sr)
        {
            item.color = new Color32(255, 255, 255, 200);
        }
    }

    public virtual void ScaleMove(int scaler)
    {
        if (scaler == 0 && isEnemy)
        {
            GameObject vfxClone = Instantiate(moveScaling.vfx, transform.position, Quaternion.identity);
            Destroy(vfxClone.gameObject, .25f);
            vfxClone.gameObject.transform.localScale = new Vector3(2, 2, 2);
        }
        if (scaler > 0)
        {
            GameObject vfxClone = Instantiate(moveScaling.vfx, transform.position, Quaternion.identity);
            Destroy(vfxClone.gameObject, .25f);
            if (scaler == 2 && !isEnemy)
            {
                vfxClone.gameObject.transform.localScale = new Vector3(2, 2, 2);
            }
        }

        moveScaling.ScaleMove(scaler);
    }
}