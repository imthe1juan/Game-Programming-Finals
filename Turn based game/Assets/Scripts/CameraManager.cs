using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    [SerializeField] private CinemachineVirtualCamera defaultCamera;
    [SerializeField] private CinemachineVirtualCamera targetCamera;

    [SerializeField] private Transform testTarget;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            DefaultCameraPos();
        }
    }

    public void DefaultCameraPos()
    {
        defaultCamera.gameObject.SetActive(true);
    }

    public void TargetTakingAction(Character target, Move preselectedMove, bool isEnemy)
    {
        defaultCamera.gameObject.SetActive(false);
        targetCamera.m_Follow = target.transform;
        if (isEnemy)
        {
            targetCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(1, 0, -10);
        }
        else
        {
            if (preselectedMove.moveName == "'Rest'" || preselectedMove.moveName == "'Nature's Embrace'") //If move is not rest
            {
                targetCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(0, 0, -10);
            }
            else
            {
                targetCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(-1, 0, -10);
            }
        }
    }
}