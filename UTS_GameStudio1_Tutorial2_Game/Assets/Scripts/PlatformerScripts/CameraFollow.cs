﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField]
    Player player;

    [SerializeField]
    Transform cameraFocus;

    [SerializeField]
    Vector3 cameraOffset;

    Vector3 moveVelocity;

    Vector2 moveVelocity2D;

    float velocity;

    [SerializeField]
    float smoothCameraTiming;

    Vector3 oldPosition;

    [SerializeField]
    float cameraPanSpeed;

    [SerializeField]
    BoxCollider2D bounds;

    Vector3 minBounds;

    Vector3 maxBounds;

    Camera mainCamera;

    float halfHeight;

    float halfWidth;

    private void Awake() {

        if (cameraFocus != null) {
            transform.position = cameraFocus.position + cameraOffset;
        }

        mainCamera = Camera.main;

        minBounds = bounds.bounds.min;
        maxBounds = bounds.bounds.max;

        halfHeight = mainCamera.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;

        FollowPlayer();
    }

    void Start() {

        if (player != null) {
            player.playerMoved += FollowPlayer;
            FollowPlayer();
        }
    }

    public void FollowPlayer() {

        if (!Utilities.VectorEquals(cameraFocus.position, oldPosition)) {

            Vector3 cameraPosition = new Vector3(cameraFocus.position.x, 0, 0) + cameraOffset;

            Vector3 smoothMove = Vector3.SmoothDamp(transform.position, cameraPosition, ref moveVelocity, smoothCameraTiming * Time.deltaTime);

            transform.position = smoothMove;

            float clampedX = Mathf.Clamp(transform.position.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
            float clampedY = Mathf.Clamp(transform.position.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);

            transform.position = new Vector3(clampedX, clampedY, transform.position.z);

            oldPosition = cameraFocus.position;
        }
    }

    public void MoveCamera(Vector2 newPosition) {

        Vector2 desiredLocation = newPosition * cameraPanSpeed;

        Vector2 smoothPosition = Vector2.SmoothDamp(newPosition, desiredLocation, ref moveVelocity2D, smoothCameraTiming);

        Vector2 coordinates = smoothPosition * Time.deltaTime;

        transform.Translate(coordinates);
    }
}
