using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    private Player player;

    [Header("Damping Config")]
    private float startRunningDamping = 2f;
    private float dampMaxTime = 1.5f;
    private float dampTimeCount;
    

    [Header("Camera Config")]
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineFramingTransposer transposer;

    void Start() {
        player = FindObjectOfType<Player>();
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    void Update() {
        DampHandler();
    }

    void DampHandler() {
        if(!player.isRunning && transposer.m_XDamping <= 0) {
            dampTimeCount = dampMaxTime;
            return;
        }

        float curDamp = dampTimeCount / dampMaxTime * startRunningDamping;
        if(curDamp < 0) curDamp = 0;
        if(dampTimeCount >= 0) dampTimeCount -= Time.deltaTime;

        transposer.m_XDamping = curDamp;
        transposer.m_YDamping = curDamp;
        transposer.m_ZDamping = curDamp;
    }
}
