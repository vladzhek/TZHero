using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UI.Level
{
    public class LevelCoverPoints : MonoBehaviour
    {
        [SerializeField] private List<Transform> _seekPoints;

        public List<Transform> GetLevelPoints()
        {
            return _seekPoints;
        }
        
#if UNITY_EDITOR
        [CustomEditor(typeof(LevelCoverPoints))]
        public class AddChildrenToListEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                LevelCoverPoints myScript = (LevelCoverPoints)target;

                if (GUILayout.Button("Add Children Transforms"))
                {
                    myScript.AddChildren();
                }

                DrawDefaultInspector();
            }
        }
        public void AddChildren()
        {
            _seekPoints.Clear();
            foreach (Transform child in transform)
            {
                _seekPoints.Add(child);
            }
        }
#endif

    }
}