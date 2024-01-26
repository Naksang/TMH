using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 카메라가 따라갈 대상 (플레이어)
    public float smoothSpeed = 0.005f; // 카메라 이동 스무딩 정도 0.00xf => x값은 플레이어 속도
    Vector2 offset; // 카메라와 대상 사이의 오프셋

    public float leftLimit; // 카메라의 왼쪽 한계
    public float rightLimit; // 카메라의 오른쪽 한계
    float topLimit; // 카메라의 위쪽 한계
    float bottomLimit; // 카메라의 아래쪽 한계

    private void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = new Vector3(
                Mathf.Clamp(smoothedPosition.x, leftLimit, rightLimit),
                Mathf.Clamp(smoothedPosition.y, bottomLimit, topLimit),
                smoothedPosition.z
            );
        }
    }
}
