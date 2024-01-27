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
        currentCamera = cameras[0]; // �ʱ⿡ ù ��° ī�޶� Ȱ��ȭ
    }

    private void Update()
    {
        // �÷��̾�� ���� ī�޶� ������ �Ÿ� ���
        float distance = Vector3.Distance(player.position, currentCamera.transform.position);

        // �÷��̾ ī�޶󿡼� ���� �Ÿ� �̻� �־����� ���� ����� ī�޶�� ��ȯ
        if (distance > 10f)
        {
            SwitchToClosestCamera();
        }
    }

    private void SwitchToClosestCamera()
    {
        Camera closestCamera = null;
        float closestDistance = Mathf.Infinity;

        // �÷��̾���� �Ÿ��� ���Ͽ� ���� ����� ī�޶� ã��
        foreach (Camera camera in cameras)
        {
            float distance = Vector3.Distance(player.position, camera.transform.position);

            if (distance < closestDistance)
            {
                closestCamera = camera;
                closestDistance = distance;
            }
        }

        // ���� ����� ī�޶�� ��ȯ
        if (closestCamera != null)
        {
            currentCamera.enabled = false;
            currentCamera = closestCamera;
            currentCamera.enabled = true;
        }
    }
}