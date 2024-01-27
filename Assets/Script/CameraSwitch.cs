using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Transform player;
    public Camera[] cameras;
    private Camera currentCamera;

    private void Start()
    {
        currentCamera = cameras[0]; // 초기에 첫 번째 카메라를 활성화
    }

    private void Update()
    {
        // 플레이어와 현재 카메라 사이의 거리 계산
        float distance = Vector3.Distance(player.position, currentCamera.transform.position);

        // 플레이어가 카메라에서 일정 거리 이상 멀어지면 가장 가까운 카메라로 전환
        if (distance > 10f)
        {
            SwitchToClosestCamera();
        }
    }

    private void SwitchToClosestCamera()
    {
        Camera closestCamera = null;
        float closestDistance = Mathf.Infinity;

        // 플레이어와의 거리를 비교하여 가장 가까운 카메라 찾기
        foreach (Camera camera in cameras)
        {
            float distance = Vector3.Distance(player.position, camera.transform.position);

            if (distance < closestDistance)
            {
                closestCamera = camera;
                closestDistance = distance;
            }
        }

        // 가장 가까운 카메라로 전환
        if (closestCamera != null)
        {
            currentCamera.enabled = false;
            currentCamera = closestCamera;
            currentCamera.enabled = true;
        }
    }
}