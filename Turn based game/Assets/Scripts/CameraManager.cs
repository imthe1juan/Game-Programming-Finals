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

    public void TargetTakingAction(Transform target, bool isEnemy)
    {
        defaultCamera.gameObject.SetActive(false);
        targetCamera.m_Follow = target;
        if (isEnemy)
        {
            targetCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(1, .5f, -10);
        }
        else
        {
            targetCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(-1, .5f, -10);
        }
    }
}