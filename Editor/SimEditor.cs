using UnityEditor;
using UnityEngine;
using System.Collections;
using Ei.Agents.Sims;

[CustomEditor(typeof(Sim))]
public class MyTypeEditor : Editor
{
    Sim m_Instance;
    PropertyField[] m_fields;


    public void OnEnable() {
        m_Instance = target as Sim;
        m_fields = ExposeProperties.GetProperties(m_Instance);
    }

    public override void OnInspectorGUI() {

        if (m_Instance == null)
            return;

        this.DrawDefaultInspector();
        ExposeProperties.Expose(m_fields);
    }
}