using UnityEngine;

public static class GeneralTools
{
    public static void _RemoveAllChildren(GameObject iParent)
    {
        for (int i = iParent.transform.childCount - 1; i >= 0; i--)
        {
            GameObject iChild = iParent.transform.GetChild(i).gameObject;
            Object.Destroy(iChild);
        }
    }
    public static Vector2 _GetDirection(Directions iDirection)
    {
        if (iDirection == Directions.Up)
            return Vector2.up;
        else if (iDirection == Directions.down)
            return Vector2.down;
        else if (iDirection == Directions.right)
            return Vector2.right;
        else //(iDirection == Directions.left)
            return Vector2.left;
    }
    public static Color _MakeColor(Vector4 iColor)
    {
        return new Color(iColor.x, iColor.y, iColor.z, iColor.w);
    }
    public enum Directions
    {
        Up = 0, right = 1, left = 2, down = 3
    }
}
