using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MovesetManager : MonoBehaviour
{
    public static MovesetManager Instance;
    public List<Button> movesetButtonList;

    [SerializeField] private TMP_Text chosenActionText;
    [SerializeField] private Transform movesetParent;
    [SerializeField] private Transform pickTarget;
    [SerializeField] private GameObject characterAction;

    private void Awake()
    {
        Instance = this;
    }

    public void DisableMoveset()
    {
        for (int i = 0; i < movesetButtonList.Count; i++)
        {
            movesetButtonList[i].onClick.RemoveAllListeners();
        }

        characterAction.gameObject.SetActive(true);
        movesetParent.gameObject.SetActive(false);
        pickTarget.gameObject.SetActive(false);
        chosenActionText.gameObject.SetActive(false);
    }

    public void EnableMoveset()
    {
        characterAction.gameObject.SetActive(false);
        movesetParent.gameObject.SetActive(true);
    }

    public void SetCharacterAction(string text)
    {
        characterAction.GetComponentInChildren<TMP_Text>().text = text;
    }

    public void MoveSelected(string text)
    {
        pickTarget.gameObject.SetActive(true);
        chosenActionText.gameObject.SetActive(true);
        characterAction.GetComponentInChildren<TMP_Text>().text = text;
    }
}