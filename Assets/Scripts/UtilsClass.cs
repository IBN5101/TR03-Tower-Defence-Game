using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilsClass
{
    private static Camera mainCamera;

    public static Vector3 getMouseWorldPosition()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        Vector3 mouseWorldPostion = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPostion.z = 0f;
        return mouseWorldPostion;
    }
}
