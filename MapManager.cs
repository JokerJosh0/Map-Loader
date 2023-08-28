using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Reflection;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    [Tooltip("This is the collider recognised by the loader (set this as your player body collider)")]
    public Collider recognisedCollider;

    [System.Serializable]
    public struct Component 
    {
        [Tooltip("This will be used in your script or loader. E.g. Map.Change(uniqueId);")]
        public string uniqueId;
        public List<GameObject> enable;
        public List<GameObject> disable;
        [Tooltip("Disable this map on start?")]
        public bool disableOnStart;
    }

    [System.Serializable]
    public struct Loader
    {
        [Tooltip("The gameObject that the loader will derive from.")]
        public GameObject LoaderGameObject;
        [Tooltip("The ID of the map that you want to change.")]
        public string IdToChange;
        [Tooltip("Once a player enters, the map will enable, but upon exit it will disable")]
        public bool dynamicHandle;
    }


    public List<Component> Maps;
    public List<Loader> Loaders;

    private void Awake()
    {
        instance = this;

        for (int i = 0; i < Maps.Count; i++)
        {
            if (Maps[i].disableOnStart)
            {
                foreach (GameObject x in Maps[i].enable)
                {
                    x.SetActive(false);
                }
            }
        }
    }

    private bool wasMainCameraColliding = false;
    Loader selectedLoader;
    private void Update()
    {
        bool isMainCameraColliding = false;

        for (int i = 0; i < Loaders.Count; i++)
        {
            if (Physics.CheckBox(Loaders[i].LoaderGameObject.transform.position, Loaders[i].LoaderGameObject.transform.localScale, Loaders[i].LoaderGameObject.transform.rotation))
            {
                Collider[] colliders = Physics.OverlapBox(Loaders[i].LoaderGameObject.transform.position, Loaders[i].LoaderGameObject.transform.localScale, Loaders[i].LoaderGameObject.transform.rotation);

                foreach (Collider collider in colliders)
                {
                    if (collider == recognisedCollider)
                    {
                        selectedLoader = Loaders[i];
                        isMainCameraColliding = true;
                    }
                }
            }
        }
        if (wasMainCameraColliding && !isMainCameraColliding)
        {
            Vector3 currentColliderPosition = recognisedCollider.transform.position;
            Vector3 loaderToPreviousCollider = selectedLoader.LoaderGameObject.transform.position;
            Vector3 loaderToCurrentCollider = currentColliderPosition - selectedLoader.LoaderGameObject.transform.position;

            float dotProduct = Vector3.Dot(loaderToPreviousCollider, loaderToCurrentCollider);

            if (dotProduct < 0)
            {
                Map.Change(selectedLoader.IdToChange, selectedLoader.dynamicHandle);
            }
        }

        wasMainCameraColliding = isMainCameraColliding;
    }
}

public static class Map
{
    public static void Change(string ID, bool dynamicHandle = false)
    {
        MapManager.Component foundComponent = MapManager.instance.Maps.Find(component => component.uniqueId.ToLower() == ID.ToLower());

        if (!dynamicHandle)
        {
            foreach (GameObject x in foundComponent.enable)
            {
                if (x == null) return;
                x.SetActive(true);
            }
            foreach (GameObject x in foundComponent.disable)
            {
                if (x == null) return;
                x.SetActive(false);
            }
        }else
        {
            foreach (GameObject x in foundComponent.enable)
            {
                if (x == null) return;
                x.SetActive(!x.activeSelf);
            }
            foreach (GameObject x in foundComponent.disable)
            {
                if (x == null) return;
                x.SetActive(!x.activeSelf);
            }
        }
    }
}
#if UNITY_EDITOR

[CustomEditor(typeof(MapManager))]
public class MapManagerEditor : Editor
{
    private bool showMapsFoldout = false;
    private int hoveredBoxControlID = -1;
    private int selectedBoxControlID = -1;
    private Dictionary<int, MapManager.Loader> controlIDToLoaderDict = new Dictionary<int, MapManager.Loader>();


    private void OnSceneGUI()
    {
        MapManager mapManager = (MapManager)target;

        foreach (MapManager.Loader loaderData in mapManager.Loaders)
        {
            GameObject loaderObject = loaderData.LoaderGameObject;
            Vector3 center = loaderObject.transform.position;
            Quaternion rotation = loaderObject.transform.rotation;
            Vector3 extents = loaderObject.transform.lossyScale * 0.5f;
            Handles.Label(center, loaderData.IdToChange);
            DrawBox(loaderData, center, rotation, extents);
        }

        HandleBoxEvents();
    }

    private void DrawBox(MapManager.Loader loaderData, Vector3 center, Quaternion rotation, Vector3 extents)
    {
        int controlID = GUIUtility.GetControlID(FocusType.Passive);
        controlIDToLoaderDict[controlID] = loaderData;
        Handles.color = hoveredBoxControlID == controlID ? Color.blue : Color.cyan;
        Handles.matrix = Matrix4x4.TRS(center, rotation, Vector3.one);

        Handles.DrawWireCube(Vector3.zero, extents * 2f);

        float maxExtent = Mathf.Max(extents.x, extents.y, extents.z);

        Handles.matrix = Matrix4x4.identity;

        HandleUtility.AddControl(controlID, HandleUtility.DistanceToRectangle(center, rotation, maxExtent));
    }

    private void HandleBoxEvents()
    {
        Event e = Event.current;
        if (e.type == EventType.MouseMove)
        {
            int controlID = HandleUtility.nearestControl;
            if (hoveredBoxControlID != controlID)
            {
                hoveredBoxControlID = controlID;
                Repaint();
            }
        }
        if (e.type == EventType.MouseDown && e.button == 0)
        {
            int controlID = HandleUtility.nearestControl;
            if (hoveredBoxControlID == controlID)
            {
                selectedBoxControlID = controlID;
                if (controlIDToLoaderDict.TryGetValue(selectedBoxControlID, out MapManager.Loader selectedLoader))
                {
                    Selection.activeGameObject = selectedLoader.LoaderGameObject;
                }
                Repaint();
            }
            e.Use();
        }
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MapManager mapManager = (MapManager)target;

        EditorGUILayout.Space();

        if (EditorApplication.isPlaying)
        {
            EditorGUILayout.Space();

            showMapsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(showMapsFoldout, "Maps Editor");
            if (showMapsFoldout)
            {
                EditorGUI.indentLevel++;
                CreateMapButtons();
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
    }

    private void CreateMapButtons()
    {
        MapManager mapManager = (MapManager)target;
        foreach (MapManager.Component component in mapManager.Maps)
        {
            if (GUILayout.Button($"Map: {component.uniqueId}"))
            {
                Map.Change(component.uniqueId);
            }
        }
    }
}
#endif
