using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

partial struct CameraZoomInOutSystem : ISystem
{
    private float currentZoom;
    private float targetZoom;
    private const float DefaultZoom = 7.0f;
    private const float MapZoom = 200f; // Half of 300 to cover full map
    private const float ZoomSpeed = 10f; // Speed of zoom transition
    
    public void OnCreate(ref SystemState state)
    {
        currentZoom = DefaultZoom;
        targetZoom = DefaultZoom;
    }

    public void OnUpdate(ref SystemState state)
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
            return;

        // Toggle zoom with Z key
        if (keyboard.zKey.wasPressedThisFrame)
        {
            if (targetZoom == DefaultZoom)
            {
                targetZoom = MapZoom; // Zoom out to see whole map
                
                // Center camera on map origin when zooming out
                var camera = Camera.main;
                if (camera != null)
                {
                    camera.transform.position = new Vector3(0, 0, camera.transform.position.z);
                }
            }
            else
            {
                targetZoom = DefaultZoom; // Zoom back in
                // Camera will resume following player automatically
            }
        }

        // Smooth zoom transition
        if (Mathf.Abs(currentZoom - targetZoom) > 0.01f)
        {
            currentZoom = Mathf.Lerp(currentZoom, targetZoom, ZoomSpeed * SystemAPI.Time.DeltaTime);
            
            var camera = Camera.main;
            if (camera != null)
            {
                camera.orthographicSize = currentZoom;
            }
        }
    }
}
