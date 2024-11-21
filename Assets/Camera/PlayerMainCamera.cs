using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMainCamera : MonoBehaviour
{
    public GameObject playerObject;         // プレイヤーオブジェクト
    public float followDelay = 0.2f;        // 追従遅延時間（秒）
    public Vector3 positionOffset = new Vector3(0, 2, -5); // カメラ位置のオフセット (X, Y, Z)
    public float collisionOffset = 0.5f;    // 障害物との距離調整（カメラと障害物との接触を防ぐためのオフセット）
    public float rayY = 0f;
    public float rayZ = 0f;
    public float rayLength = 0f;

    private Vector3 targetPosition;         // 目標位置（カメラが追従する位置）
    private Quaternion targetRotation;      // 目標回転（プレイヤーを注視する回転）
    private Quaternion currentRotation;     // 現在の回転
    private Vector3 velocity = Vector3.zero; // 補間用の速度（SmoothDamp用）

    void Start()
    {
        // 初期の目標位置をプレイヤーのオフセットを考慮して設定
        targetPosition = playerObject.transform.position + playerObject.transform.TransformDirection(positionOffset);
        currentRotation = transform.rotation; // 最初の回転を記録
    }

    void LateUpdate()
    {
        FollowPosition();
        FollowRotation();
    }

    void FollowPosition()
    {
        // プレイヤーの回転を考慮し、常にプレイヤーの後ろの位置にカメラを設定
        Vector3 desiredPosition = playerObject.transform.position + playerObject.transform.TransformDirection(positionOffset);

        // 障害物がある場合、カメラを障害物の手前に調整
        Vector3 ray = new Vector3(transform.position.x, transform.position.y + rayY, transform.position.z + rayZ);
        Debug.DrawRay(ray, transform.forward * rayLength, Color.red, 1);
        RaycastHit hit;
        if (Physics.Raycast(ray, (desiredPosition - playerObject.transform.position).normalized, out hit, positionOffset.magnitude))
        {
            // 障害物がカメラの進行方向にある場合、障害物の手前でカメラを設定
            desiredPosition = hit.point - (hit.normal * collisionOffset);
        }

        // 目標位置に SmoothDamp で遅延追従
        targetPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, followDelay);

        // カメラの位置を更新
        transform.position = targetPosition;
    }

    void FollowRotation()
    {
        // プレイヤーを常に注視する目標回転を計算
        targetRotation = Quaternion.LookRotation(playerObject.transform.position - transform.position);

        // プレイヤーの上下（pitch）と左右（yaw）を分ける
        float targetPitch = targetRotation.eulerAngles.x;
        float targetYaw = targetRotation.eulerAngles.y;

        // 現在の回転を取得
        float currentPitch = transform.rotation.eulerAngles.x;
        float currentYaw = transform.rotation.eulerAngles.y;

        // 上下の回転は即時に反映（遅延なし）
        float pitch = Mathf.LerpAngle(currentPitch, targetPitch, Time.deltaTime * 10f); // 10fはスムーズな補間のスピード調整

        // 左右の回転は遅延を適用
        float yaw = Mathf.LerpAngle(currentYaw, targetYaw, Time.deltaTime / followDelay);

        // 新しい回転を設定
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}

