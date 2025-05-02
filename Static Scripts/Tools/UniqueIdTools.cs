using UnityEngine;

public static class UniqueIdTools
{
    public static string _MakeUniqueId(Vector2 iPosition)
    {
        string xPart = iPosition.x.ToString("F2").Replace(".", "");
        string yPart = iPosition.y.ToString("F2").Replace(".", "");

        if (xPart.Length > 6)
            xPart = xPart.Substring(0, 5);
        if (yPart.Length > 6)
            yPart = yPart.Substring(0, 5);

        // we add currentScene to ensure the id is unique in different scenes
        int currentScene;
        currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

        return xPart + yPart + "_" + currentScene;
    }
    public static int _GetUniqueIdScene(string iUniqueId)
    {
        return int.Parse(iUniqueId.Split('_')[1]);
    }
    public static bool _IsUniqueIdInScene(string iUniqueId)
    {
        int uniqueIdScene = _GetUniqueIdScene(iUniqueId);
        return uniqueIdScene ==
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
    }
}
