using Ei.Agents.Sims;
using NodeCanvas.BehaviourTrees;
using Rotorz.ReorderableList;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SimObject))]
public class SimObjectEditor : Editor
{

    SerializedProperty actions;
    // SerializedProperty offsets;
    // SerializedProperty lookAts;

    void OnEnable() {
        actions = serializedObject.FindProperty("Actions");
        // offsets = serializedObject.FindProperty("positionOffsets");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        ReorderableListGUI.Title("Actions");
        ReorderableListGUI.ListField(actions);

        serializedObject.ApplyModifiedProperties();
    }
}

[CustomPropertyDrawer(typeof(SimAction))]
public class SimObjectInteractiveActionEditor : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label) {
        var innerEntriesProp = prop.FindPropertyRelative("Modifiers");
        var planEntries = prop.FindPropertyRelative("Plan");
        return ReorderableListGUI.CalculateListFieldHeight(innerEntriesProp) +
               ReorderableListGUI.CalculateListFieldHeight(planEntries) + 85;
    }

    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label) {

        var y = position.y;
        var height = position.height;

        position.height = 16;
        EditorGUI.PropertyField(position, prop.FindPropertyRelative("Name"));
        position.y += 20;
        EditorGUI.PropertyField(position, prop.FindPropertyRelative("Uses"));
        position.y += 20;
        EditorGUI.PropertyField(position, prop.FindPropertyRelative("DurationInMinutes"));

        position.y += 20;
        position.height = 23;
        EditorGUI.LabelField(position, "Modifiers", LabelHelper.HeaderStyle);
        position.y += 20;

        var planEntries = prop.FindPropertyRelative("Modifiers");
        position.height = ReorderableListGUI.CalculateListFieldHeight(planEntries);
        ReorderableListGUI.ListFieldAbsolute(position, planEntries);        

        position.y += position.height - 5;
        position.height = 23;
        EditorGUI.LabelField(position, "Plan", LabelHelper.HeaderStyle);
        position.y += 20;

        planEntries = prop.FindPropertyRelative("Plan");
        position.height = ReorderableListGUI.CalculateListFieldHeight(planEntries);
        ReorderableListGUI.ListFieldAbsolute(position, planEntries, ReorderableListFlags.ShowIndices);

    }
}


[CustomPropertyDrawer(typeof(ModifierAdvertisement))]
public class SimObjectInteractiveActionModifierEditor : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label) {
        var innerEntriesProp = prop.FindPropertyRelative("PersonalityModifiers");
        return ReorderableListGUI.CalculateListFieldHeight(innerEntriesProp) + 30;
    }

    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label) {
        var y = position.y;
        var x = position.x;
        var width = position.width;

        position.y += 2;
        position.height = 20;
        position.width = 80;
        var type = prop.FindPropertyRelative("Type");
        type.enumValueIndex = (int)(ModifierType)EditorGUI.EnumPopup
            (position, (ModifierType)Enum.GetValues(typeof(ModifierType)).GetValue(type.enumValueIndex));

        position.y = y;
        position.x = 175;
        position.width = 40;
        position.height = 18;

        var delta = prop.FindPropertyRelative("Delta");
        delta.floatValue = EditorGUI.FloatField(position, delta.floatValue);

        position.x = x;
        position.width = width;
        position.y += 20;
        position.height = 23;
        EditorGUI.LabelField(position, "Personality", LabelHelper.HeaderStyle);
        position.y += 20;

        var innerEntriesProp = prop.FindPropertyRelative("PersonalityModifiers");
        position.height = ReorderableListGUI.CalculateListFieldHeight(innerEntriesProp);
        ReorderableListGUI.ListFieldAbsolute(position, innerEntriesProp);
    }
}

[CustomPropertyDrawer(typeof(PersonalityModifier))]
public class SimObjectInteractiveActionPersonalityModifierEditor : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label) {
        return 18;
    }

    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label) {
        var y = position.y;
        position.y += 2;
        position.height = 20;
        position.width = 80;
        var type = prop.FindPropertyRelative("Type");
        type.enumValueIndex = (int) (PersonalityType) EditorGUI.EnumPopup
            (position, (PersonalityType) Enum.GetValues(typeof(PersonalityType)).GetValue(type.enumValueIndex));

        position.y = y;
        position.x = 175;
        position.width = 40;
        position.height = 18;

        var delta = prop.FindPropertyRelative("Delta");
        delta.floatValue = EditorGUI.FloatField(position, delta.floatValue);
        // EditorGUI.PropertyField(position, prop.FindPropertyRelative("Delta"));
    }
}

//[CustomPropertyDrawer(typeof(SeedInfo))]
//public class SimObjectSeedInfoEditor : PropertyDrawer
//{
//    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label) {
//        return 100;
//    }

//    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label) {
//        position.height = 16;
//        EditorGUI.PropertyField(position, prop.FindPropertyRelative("Count"));
//        position.y += 20;
//        EditorGUI.PropertyField(position, prop.FindPropertyRelative("MaxSeedCount"));
//        position.y += 20;
//        EditorGUI.PropertyField(position, prop.FindPropertyRelative("MinSeedCount"));
//        position.y += 20;
//        EditorGUI.PropertyField(position, prop.FindPropertyRelative("NextSeed"));
//        position.y += 20;
//        EditorGUI.PropertyField(position, prop.FindPropertyRelative("SeedPeriodInMinutes"));
//    }
//}


