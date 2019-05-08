using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  [Header("Settings")]
  public float mouseSensitivity = 10;
  public Transform target;
  public float distFromTarget = 2;
  public float minDistFromTarget = 5;
  public float maxDistFromTarget = 40;
  public Vector2 pitchMinMax = new Vector2(-40,85);
  public bool stopForPause = false;

  public float rotationSmoothTime = 8f;
  Vector3 rotationSmoothVelocity;
  Vector3 currentRotation;

  private bool fixedCamera;

  float yaw;
  float pitch;

  [Header("Collision Vars")]

  [Header("Transparency")]
  public bool changeTransparency = true;
  public MeshRenderer targetRenderer;

  [Header("Speeds")]
  public float moveSpeed = 5;
  public float returnSpeed = 9;
  public float wallPush = 0.7f;

  [Header("Distances")]
  public float closestDistanceToPlayer = 2;
  public float evenCloserDistanceToPlayer = 1;

  [Header("Mask")]
  public LayerMask collisionMask;

  private bool pitchLock = false;
  private bool lockCursor;

  public bool getLockCursor() {
    return lockCursor;
  }

  public void setLockCursor(bool _lockCursor) {
    lockCursor = _lockCursor;
  }

  private void Start() {
    lockCursor = true;
    stopForPause = false;
  }

  private void LateUpdate() {

    if (target == null || transform == null){
      Debug.LogWarning("Ignoring camera late update, already destroyed player instance.");
      return;
    }

    if (lockCursor) {
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
    }
    else {
      Cursor.lockState = CursorLockMode.None;
      Cursor.visible = true;
    }

    // not updating when game paused
    if (stopForPause) {
      return;
    }

    wheelCheck();
    CollisionCheck(target.position - transform.forward * distFromTarget);
    WallCheck ();

    if (pitchLock) {
      yaw += Input.GetAxis ("Mouse X") * mouseSensitivity;
      pitch = pitchMinMax.y;

      currentRotation = Vector3.Lerp (currentRotation, new Vector3 (pitch, yaw), rotationSmoothTime * Time.deltaTime);
    }
    else {
      yaw += Input.GetAxis ("Mouse X") * mouseSensitivity;
      pitch -= Input.GetAxis ("Mouse Y") * mouseSensitivity;
      pitch = Mathf.Clamp (pitch, pitchMinMax.x, pitchMinMax.y);
      currentRotation = Vector3.Lerp (currentRotation, new Vector3 (pitch, yaw), rotationSmoothTime * Time.deltaTime);
    }

    transform.eulerAngles = currentRotation;
    Vector3 e = transform.eulerAngles;
    e.x = 0;

    if (Input.GetMouseButtonDown(1)) fixedCamera = true;
    if (Input.GetMouseButtonUp(1)) fixedCamera = false;
    if (fixedCamera) target.eulerAngles = e;
  }

  private void WallCheck() {

    Ray ray = new Ray (target.position, -target.forward);
    RaycastHit hit;

    if (Physics.SphereCast (ray, 0.2f, out hit, 0.7f, collisionMask)) {
      pitchLock = true;
    }
    else {
      pitchLock = false;
    }
  }

  private void CollisionCheck (Vector3 retPoint) {

    RaycastHit hit;

    if (Physics.Linecast (target.position, retPoint, out hit, collisionMask)) {

      Vector3 norm = hit.normal * wallPush;
      Vector3 p = hit.point + norm;

      TransparencyCheck ();

      if (Vector3.Distance (Vector3.Lerp (transform.position, p, moveSpeed * Time.deltaTime), target.position) <= evenCloserDistanceToPlayer) {

      } else {
        transform.position = Vector3.Lerp (transform.position, p, moveSpeed * Time.deltaTime);
      }

      return;
    }

    FullTransparency ();

    transform.position = Vector3.Lerp (transform.position, retPoint, returnSpeed * Time.deltaTime);
    pitchLock = false;
  }

  private void TransparencyCheck() {

    if (changeTransparency) {

      if (Vector3.Distance (transform.position, target.position) <= closestDistanceToPlayer) {

        Color temp = targetRenderer.sharedMaterial.color;
        temp.a = Mathf.Lerp (temp.a, 0.2f, moveSpeed * Time.deltaTime);

        targetRenderer.sharedMaterial.color = temp;

      } else {

        if (targetRenderer.sharedMaterial.color.a <= 0.99f) {

          Color temp = targetRenderer.sharedMaterial.color;
          temp.a = Mathf.Lerp (temp.a, 1, moveSpeed * Time.deltaTime);

          targetRenderer.sharedMaterial.color = temp;
        }
      }
    }
  }

  private void FullTransparency() {

    if (changeTransparency) {

      if (targetRenderer.material.color.a <= 0.99f) {

        Color temp = targetRenderer.material.color;
        temp.a = Mathf.Lerp (temp.a, 1, moveSpeed * Time.deltaTime);

        targetRenderer.material.color = temp;
      }
    }
  }

  private void wheelCheck(){

     float y = Input.mouseScrollDelta.y;

     if (y > 0 && distFromTarget > minDistFromTarget) {
       distFromTarget -= 2;
     }

     else if (y < 0 && distFromTarget < maxDistFromTarget) {
       distFromTarget += 2;
     }
  }
}
