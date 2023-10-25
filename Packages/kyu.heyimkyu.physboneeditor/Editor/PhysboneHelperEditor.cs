using System;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Dynamics.PhysBone.Components;

public class PhysboneHelperEditor : UnityEditor.EditorWindow
{
    [UnityEditor.MenuItem("Tools/Kyu/Physbone Editor")]
    public static void ShowWindow()
    {
        GetWindow<PhysboneHelperEditor>("Physbone Editor");
    }

    public Vector2 scrollPosition { get; set; }
    public GameObject avatar { get; set; }

    private float m_boneOpacity;
    public float boneOpacity {
        get { return m_boneOpacity; }
        set
        {
            if (value == m_boneOpacity)
                return;

            m_boneOpacity = value;
            foreach (var bone in getPhysBones())
            {
                bone.boneOpacity = m_boneOpacity;
            }
        } 
    }
    private float m_angleOpacity;
    public float angleOpacity
    {
        get { return m_angleOpacity; }
        set
        {
            if (value == m_angleOpacity)
                return;

            m_angleOpacity = value;
            foreach (var bone in getPhysBones())
            {
                bone.limitOpacity = m_angleOpacity;
            }
        }
    }
    private bool m_visible;
    public bool visible
    {
        get { return m_visible; }
        set
        {
            if (value == m_visible)
                return;

            m_visible = value;
            foreach (var bone in getPhysBones())
            {
                bone.showGizmos = m_visible;
            }
        }
    }

    private void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        EditorGUILayout.Space();
        avatar = (GameObject)EditorGUILayout.ObjectField("Avatar", avatar, typeof(GameObject), true);
        EditorGUILayout.Space();

        if (GUILayout.Button("Fix PhyBone Roots"))
        {
            FixPhysBoneRoots();
        }

        if (GUILayout.Button("Fix Collider Roots"))
        {
            FixColliderRoots();
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Bone Opacity");
        boneOpacity = EditorGUILayout.Slider(boneOpacity, 0, 1);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Angle Opacity");
        angleOpacity = EditorGUILayout.Slider(angleOpacity, 0, 1);
        EditorGUILayout.EndHorizontal();

        visible = EditorGUILayout.ToggleLeft("Show Gizmos", visible);

        EditorGUILayout.EndScrollView();
    }

    private void FixColliderRoots()
    {
        foreach (var element in getCollider())
        {
            if (element.rootTransform != element.transform)
            { 
                element.rootTransform = element.transform;
                UnityEngine.Debug.Log("Fixed Root for Collider: " + element.name);
            }
        }
    }

    void FixPhysBoneRoots()
    {
        foreach (var element in getPhysBones())
        {
            if (element.rootTransform != element.transform)
            {
                element.rootTransform = element.transform;
                UnityEngine.Debug.Log("Fixed Root for PhysBone: " + element.name);
            }
        }
    }

    VRCPhysBone[] getPhysBones()
    {
        if (this.avatar == null)
            return new VRCPhysBone[]{ };

        return avatar.GetComponentsInChildren<VRCPhysBone>(true);
    }

    VRCPhysBoneCollider[] getCollider()
    {
        if (this.avatar == null)
            return new VRCPhysBoneCollider[] { };

        return avatar.GetComponentsInChildren<VRCPhysBoneCollider>(true);
    }
}