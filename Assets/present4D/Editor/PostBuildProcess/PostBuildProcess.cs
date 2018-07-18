using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class PostBuildProcess : MonoBehaviour {

    [PostProcessBuildAttribute(1)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        Debug.Log("Post Process: pathToBuiltProject");
    }
}
