using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveTo : MonoBehaviour
{
    [SerializeField] private Transform _moveToPosition; // ������� �������
    [SerializeField] private Transform _moveToRotation;  // ������� �������
    [SerializeField] private float movementSpeed = 2f;  // �������� ��������� �������
    [SerializeField] private float rotationSpeed = 2f;  // �������� ��������� ��������

    public void MoveCameraTo()
    {
        StartCoroutine(MoveAndRotateCamera());
    }

    private IEnumerator MoveAndRotateCamera()
    {
        while (Vector3.Distance(transform.position, _moveToPosition.position) > 0.1f ||
               Quaternion.Angle(transform.rotation, _moveToRotation.rotation) > 0.1f)
        {
            // ����������� ��������� �������
            transform.position = Vector3.Lerp(transform.position, _moveToPosition.position, Time.deltaTime * movementSpeed);

            // ����������� ��������� ��������
            transform.rotation = Quaternion.Lerp(transform.rotation, _moveToRotation.rotation, Time.deltaTime * rotationSpeed);

            yield return null;
        }

        // ���������� ��������
        yield break;
    }
}
