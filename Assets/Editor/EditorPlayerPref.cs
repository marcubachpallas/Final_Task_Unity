using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorPlayerPref : EditorWindow
{
  public int width = 8;
  public float offset = 0;
  public Vector2Int maxGridSize = new Vector2Int(16, 16);

  [MenuItem("Tools/Player Prefs")]
  static void Init()
  {
    // Get existing open window or if none, make a new one:
    EditorPlayerPref window = (EditorPlayerPref)EditorWindow.GetWindow(typeof(EditorPlayerPref));
    window.titleContent = new GUIContent("Player Preference");
    window.Show();
  }


    public enum FieldType { String,Integer,Float }

    private FieldType fieldType = FieldType.String;
    private string setKey = "";
    private string setVal = "";
    private string error = null;

    void OnGUI() {

        EditorGUILayout.LabelField("Player Preference Editor", EditorStyles.boldLabel);
        EditorGUILayout.Separator();

        fieldType = (FieldType)EditorGUILayout.EnumPopup("Key Type", fieldType);
        setKey = EditorGUILayout.TextField("Key to Set", setKey);
        setVal = EditorGUILayout.TextField("Value to Set", setVal);

        if(error != null) {

            EditorGUILayout.HelpBox(error, MessageType.Error);

        }

        if(GUILayout.Button("Set Key")) {

            if(fieldType == FieldType.Integer) {

                int result;
                if(!int.TryParse(setVal, out result)) {

                    error = "Invalid input \"" + setVal + "\"";
                    return;

                }

                PlayerPrefs.SetInt(setKey, result);

            } else if(fieldType == FieldType.Float) {

                float result;
                if(!float.TryParse(setVal, out result)) {

                    error = "Invalid input \"" + setVal + "\"";
                    return;

                }

                PlayerPrefs.SetFloat(setKey, result);

            } else {

                PlayerPrefs.SetString(setKey, setVal);

            }

            PlayerPrefs.Save();
            error = null;

        }

        if (GUILayout.Button("Get Key"))
        {
            error = null;
            setVal = "-";
            int i;
            float f;
            string s;

            if (PlayerPrefs.HasKey(setKey))
            {
                i = PlayerPrefs.GetInt(setKey, int.MinValue);
                if (i == int.MinValue)
                {
                    f = PlayerPrefs.GetFloat(setKey, float.NaN);
                    if (float.IsNaN(f))
                    {
                        s = PlayerPrefs.GetString(setKey, string.Empty);
                        if (string.IsNullOrEmpty(s))
                        {
                            error = "Unknown type: " + setKey;
                        }
                        else
                        {
                            setVal = s.ToString();
                            fieldType = FieldType.String;
                        }
                    }
                    else
                    {
                        setVal = f.ToString();
                        fieldType = FieldType.Float;
                    }
                }
                else
                {
                    setVal = i.ToString();
                    fieldType = FieldType.Integer;
                }
            }
            else
            {
                error = "No matching Key " + setKey;
            }
          }

        if(GUILayout.Button("Delete Key")) {

            PlayerPrefs.DeleteKey(setKey);
            PlayerPrefs.Save();

        }

        if(GUILayout.Button("Delete All Keys")) {

            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();

        }

    }

}
