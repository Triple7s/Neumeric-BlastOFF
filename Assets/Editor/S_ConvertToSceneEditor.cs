#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class ConvertToSceneEditor : MonoBehaviour
{
    [MenuItem("Tools/Convert Selected GameObject To Scene")]
    static void ConvertSelected()
    {
        GameObject selected = Selection.activeGameObject;
        if (selected == null)
        {
            Debug.LogWarning("No GameObject selected!");
            return;
        }

        // 1. Create a new scene
        Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // 2. Move selected GameObject into the new scene
        SceneManager.MoveGameObjectToScene(selected, newScene);

        // 3. Save it as a scene asset
        string path = "Assets/" + selected.name + ".unity";
        EditorSceneManager.SaveScene(newScene, path);
        Debug.Log($"Scene saved: {path}");
    }
}
#endif