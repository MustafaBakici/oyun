
using UnityEngine;

public class MouseUtils 
{
    public static Vector2 GetMousePosition2d()
        { return  (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition); }
}
