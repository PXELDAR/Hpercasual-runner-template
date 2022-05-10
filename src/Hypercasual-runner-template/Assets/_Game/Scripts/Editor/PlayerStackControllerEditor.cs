using UnityEditor;
using UnityEngine;

namespace PXELDAR
{
    [CustomEditor(typeof(PlayerStackController))]
    [ExecuteAlways]
    public class PlayerStackControllerEditor : Editor
    {
        //===================================================================================

        private bool _debugging;

        private int _addStack;
        private int _removeStack;

        //===================================================================================

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            PlayerStackController myController = (PlayerStackController)target;

            //Debugging title and toggle
            GUILayout.Label("Debugging", EditorStyles.boldLabel);
            _debugging = EditorGUILayout.Toggle("Show Debug Menu", _debugging);

            if (_debugging)
            {
                GUILayout.Label("Stack Debug", EditorStyles.boldLabel);

                //Add stack layout
                GUILayout.BeginHorizontal();
                _addStack = EditorGUILayout.IntSlider(_addStack, 0, 50);
                if (GUILayout.Button("Increase Stack",
                    GUILayout.Width(120), GUILayout.Height(20)))
                {
                    myController.AddStack(_addStack);
                }
                GUILayout.EndHorizontal();


                //Remove stack layout
                GUILayout.BeginHorizontal();
                _removeStack = EditorGUILayout.IntSlider(_removeStack, 0, 50);
                if (GUILayout.Button("Decrease Stack",
                    GUILayout.Width(120), GUILayout.Height(20)))
                {
                    myController.LoseStack(_removeStack);
                }
                GUILayout.EndHorizontal();


                GUILayout.Label("Stack Level Debug", EditorStyles.boldLabel);

                //Add stack level
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Increase Stack Level",
                    GUILayout.Width(150), GUILayout.Height(50)))
                {
                    myController.IncreaseStackLevel();
                }

                //Decrease stack level
                if (GUILayout.Button("Decrease Stack Level",
                    GUILayout.Width(150), GUILayout.Height(50)))
                {
                    myController.DecreaseStackLevel();
                }
                GUILayout.EndHorizontal();
            }
        }

        //===================================================================================

    }
}