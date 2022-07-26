using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    [Header("ī�޶� �̵� ����Ʈ")]
    public Transform PositionTop;
    public Transform PositionBottom;
    public Transform PositionLeft;
    public Transform PositionRight;
    public Transform PositionFront;
    public Transform PositionQuater;

    [Header("ī�޶� �̵� �ӵ�")]
    public float CameraMoveSpeed;
    public float CameraRotateSpeed;
    public float GizmoMoveTime;

    bool isMoving;
    float lerpSpeed;

    Transform CurrCameraPosition;
    Transform DestCameraPosition;

    Vector2 CurrMousePosition;
    Vector2 MovedMousePosition;

    Vector3 CurrCameraRotation;
    Vector3 MovedCameraRotation;


    private void Awake()
    {
        isMoving = false;
        CurrCameraPosition = gameObject.transform;

        AutoCtrlCameraViewport();
    }

    private void Update()
    {
        if (isMoving) return;

        //���콺 ó�� �������� ��ġ ����
        if (Input.GetMouseButtonDown(1))
        {
            CurrMousePosition = Input.mousePosition;
            CurrCameraRotation = CurrCameraPosition.localEulerAngles;
            CurrCameraRotation.z = 0f;
        }

        //������ Ŭ���� �ϰ� �ִ� ���¿���,
        if (Input.GetMouseButton(1))
        {
            MovedMousePosition = Input.mousePosition;
            MovedCameraRotation.x = CurrCameraRotation.x - (MovedMousePosition.y - CurrMousePosition.y) * CameraRotateSpeed;
            MovedCameraRotation.y = CurrCameraRotation.y + (MovedMousePosition.x - CurrMousePosition.x) * CameraRotateSpeed;
            MovedCameraRotation.z = 0f;

            CurrCameraPosition.localEulerAngles = MovedCameraRotation;


            //����
            if (Input.GetKey(KeyCode.W))
            {
                CurrCameraPosition.localPosition += CurrCameraPosition.forward * CameraMoveSpeed;
            }
            //�Ĺ�
            else if (Input.GetKey(KeyCode.S))
            {
                CurrCameraPosition.localPosition -= CurrCameraPosition.forward * CameraMoveSpeed;
            }

            //������
            if (Input.GetKey(KeyCode.D))
            {
                CurrCameraPosition.localPosition += CurrCameraPosition.right * CameraMoveSpeed;
            }
            //����
            else if (Input.GetKey(KeyCode.A))
            {
                CurrCameraPosition.localPosition -= CurrCameraPosition.right * CameraMoveSpeed;
            }

            //��
            if (Input.GetKey(KeyCode.Q))
            {
                CurrCameraPosition.localPosition += CurrCameraPosition.up * CameraMoveSpeed;
            }
            //�Ʒ�
            else if (Input.GetKey(KeyCode.E))
            {
                CurrCameraPosition.localPosition -= CurrCameraPosition.up * CameraMoveSpeed;
            }
        }
    }

    void AutoCtrlCameraViewport()
    {
        float assetOptionPanelWidth = 360;

        float titlebarPanelHeight = 30;
        float contentBoxPanelHeight = 60;
        float timeLinePanelHeight = 340;

        float viewportX = assetOptionPanelWidth / Screen.width;
        float viewportY = timeLinePanelHeight / Screen.height;
        float viewportWidth = 1 - viewportX;
        float viewportHeight = 1 - (titlebarPanelHeight + contentBoxPanelHeight + timeLinePanelHeight) / Screen.height;

        Camera.main.rect = new Rect(viewportX, viewportY, viewportWidth, viewportHeight);
    }

    public void OnClick_MoveCamera(string dest)
    {
        if (isMoving) return;

        switch(dest)
        {
            case "TOP":
                DestCameraPosition = PositionTop;
                break;
            case "BOTTOM":
                DestCameraPosition = PositionBottom;
                break;
            case "LEFT":
                DestCameraPosition = PositionLeft;
                break;
            case "RIGHT":
                DestCameraPosition = PositionRight;
                break;
            case "FRONT":
                DestCameraPosition = PositionFront;
                break;
            case "QUATER":
                DestCameraPosition = PositionQuater;
                break;
            default:
                break;
        }

        StartCoroutine(MoveTransform());
    }

    IEnumerator MoveTransform()
    {
        isMoving = true;

        Vector3 currPos = CurrCameraPosition.localPosition;
        Vector3 currRot = CurrCameraPosition.localEulerAngles;

        float currMovingTime = 0;
        lerpSpeed = 0;

        while(true)
        {
            lerpSpeed += Time.deltaTime / GizmoMoveTime;
            currMovingTime += Time.deltaTime;

            CurrCameraPosition.localPosition = Vector3.Slerp(currPos, DestCameraPosition.localPosition, lerpSpeed);
            CurrCameraPosition.localEulerAngles = Vector3.Slerp(currRot, DestCameraPosition.localEulerAngles, lerpSpeed);

            if (currMovingTime > GizmoMoveTime)
            {
                CurrCameraPosition.localPosition = DestCameraPosition.localPosition;
                CurrCameraPosition.localEulerAngles = DestCameraPosition.localEulerAngles;

                isMoving = false;

                yield break;
            }

            yield return null;
        }
    }
}
