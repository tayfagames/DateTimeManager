using TayfaGames;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DateTimeManager))]
public class DateTimePickerEditor : Editor
{
    private SerializedProperty startDateTimeProperty;

    private void OnEnable()
    {
        startDateTimeProperty = serializedObject.FindProperty("startDateTime");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("progressMode"));

        EditorGUILayout.LabelField("Start Date Time");

        string buttonText;
        if (startDateTimeProperty.stringValue == "")
        {
            buttonText = "Set Date and Time";
        }
        else
        {
            buttonText = startDateTimeProperty.stringValue;
        }

        if (GUILayout.Button(buttonText))
        {
            DatePickerWindow.Show((selectedDate) =>
            {
                startDateTimeProperty.stringValue = selectedDate.ToString("MM/dd/yyyy HH:mm:ss");
                serializedObject.ApplyModifiedProperties();
            }, startDateTimeProperty.stringValue);
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("startOnAwake"));

        serializedObject.ApplyModifiedProperties();
    }
}
