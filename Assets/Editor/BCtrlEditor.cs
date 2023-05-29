using _Scripts;
using _Scripts.BCtrl;
using UnityEditor;
using UnityEngine;


namespace Editor {
    [CustomEditor(typeof(ArrowBulletSet))]
    [CanEditMultipleObjects]
    public class BCtrlEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            BulletController script = target as ArrowBulletSet;

            EditorGUILayout.BeginHorizontal();
            script.mainStepType =
                (StepType)EditorGUILayout.EnumPopup
                    ("Step Behaviour :", script.mainStepType);
            if (GUILayout.Button("Create")) {
                switch (script.mainStepType) {
                    case StepType.Linear:
                        script.mainStepBehaviour = new StepLinear();
                        (script.mainStepBehaviour as StepLinear).isUniformSpeed =
                            EditorGUILayout.Toggle((script.mainStepBehaviour as StepLinear).isUniformSpeed,
                                "Is Uniform Speed?");
                        break;  
                    case StepType.None:
                        break;
                }
            }
            EditorGUILayout.EndHorizontal();
            base.OnInspectorGUI();
        }
    }
}