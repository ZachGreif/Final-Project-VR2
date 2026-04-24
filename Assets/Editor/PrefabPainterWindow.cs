using UnityEngine;
using UnityEditor;

public class PrefabPainterWindow : EditorWindow
{
    private GameObject prefabToPaint;
    private Transform parentForPlacedObjects;
    private bool paintMode = false;
    private bool randomYRotation = false;
    private Vector2 randomScaleRange = new Vector2(1f, 1f);
    private Vector3 arenaCenter = Vector3.zero;

    [MenuItem("Tools/Prefab Painter")]
    public static void ShowWindow()
    {
        GetWindow<PrefabPainterWindow>("Prefab Painter");
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnGUI()
    {
        GUILayout.Label("Prefab Painter", EditorStyles.boldLabel);

        prefabToPaint = (GameObject)EditorGUILayout.ObjectField("Prefab", prefabToPaint, typeof(GameObject), false);
        parentForPlacedObjects = (Transform)EditorGUILayout.ObjectField("Parent", parentForPlacedObjects, typeof(Transform), true);
        randomYRotation = EditorGUILayout.Toggle("Random Y Rotation", randomYRotation);
        randomScaleRange = EditorGUILayout.Vector2Field("Random Scale Range", randomScaleRange);
        arenaCenter = EditorGUILayout.Vector3Field("Arena Center", arenaCenter);

        GUILayout.Space(10);

        if (GUILayout.Button(paintMode ? "Disable Paint Mode" : "Enable Paint Mode", GUILayout.Height(30)))
        {
            paintMode = !paintMode;
            SceneView.RepaintAll();
        }

        EditorGUILayout.HelpBox(
            "Click in the Scene view to place the prefab. Hold Shift and click an object to delete it. Press Escape to stop.",
            MessageType.Info
        );
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (!paintMode || prefabToPaint == null)
            return;

        Event e = Event.current;

        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Escape)
        {
            paintMode = false;
            e.Use();
            Repaint();
            return;
        }

        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Handles.color = Color.yellow;
            Handles.DrawWireDisc(hit.point, hit.normal, 0.35f);

            if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
            {
                if (e.shift)
                {
                    Undo.DestroyObjectImmediate(hit.collider.gameObject);
                }
                else
                {
                    PlacePrefab(hit.point);
                }

                e.Use();
            }
        }
    }

    private void PlacePrefab(Vector3 position)
    {
        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefabToPaint);
        if (instance == null) return;

        Undo.RegisterCreatedObjectUndo(instance, "Paint Prefab");

        instance.transform.position = position;

        if (randomYRotation)
        {
            instance.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        }
        else
        {
            Vector3 lookDir = arenaCenter - position;
            lookDir.y = 0f;

            if (lookDir.sqrMagnitude > 0.001f)
            {
                instance.transform.rotation = Quaternion.LookRotation(lookDir);
            }
        }

        float scale = Random.Range(randomScaleRange.x, randomScaleRange.y);
        instance.transform.localScale *= scale;

        if (parentForPlacedObjects != null)
        {
            instance.transform.SetParent(parentForPlacedObjects);
        }

        Selection.activeGameObject = instance;
    }
}