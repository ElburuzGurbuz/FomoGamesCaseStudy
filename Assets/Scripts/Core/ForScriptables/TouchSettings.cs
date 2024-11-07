using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TouchSettings", menuName = "FomoGames/Touch Settings", order = 1)]
public class TouchSettings : ScriptableObject
{
    public SelectedTouchSystem SelectedTouchSystem;

    [HorizontalLine]
    public UpdateTypeForTouch UpdateType = UpdateTypeForTouch.Fixed;

    //[HorizontalLine,HideInInspector]
    //public JoystickType JoystickType;

    [HorizontalLine]
    public float SwipeTresholdPixel = 100; //px

    [HorizontalLine, Range(0.01f, 0.99f)]
    public float DragLerpRange = 0.1f;

    [HorizontalLine]
    public LayerMask RaycastLayer;

    [HorizontalLine]
    public float RayDistance;

    [HorizontalLine]
    public bool DebugRayLine = true;
}

public enum SelectedTouchSystem
{
    None,
    MouseButton,
    MouseButtonWithRaycast,
    Swipe,
    Drag
}
public enum ClickControl
{
    Empty, Down, Set, Up
}

public enum UpdateTypeForTouch
{
    Fixed,Update
}

//public enum JoystickType
//{
//    Fixed, Floating, Dynamic
//}