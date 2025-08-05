#if UNITY_EDITOR
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class BadBuildDebugger : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        //if (YourScript._isDebugMode)
        //{
        //    throw new BuildFailedException("Build blocked: Debug mode is still enabled!");
        //}
    }
}
#endif