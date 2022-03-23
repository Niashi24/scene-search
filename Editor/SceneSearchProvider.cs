using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using System;
using UnityEngine.SceneManagement;
using System.Linq;
using Object = UnityEngine.Object;

namespace LS.Attributes.Editor
{
    public class SceneSearchProvider : ScriptableObject, ISearchWindowProvider
    {
        Type objectType;
        public SerializedProperty serializedProperty;

        public SceneSearchProvider Init(
            Type objectType,
            SerializedProperty serializedProperty
        ) {
            this.objectType = objectType;
            this.serializedProperty = serializedProperty;
            return this;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            serializedProperty.objectReferenceValue = (UnityEngine.Object)searchTreeEntry.userData;
            serializedProperty.serializedObject.ApplyModifiedProperties();
            return true;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> list = new List<SearchTreeEntry>();

            PopulateSearchTree(list);

            return list;
        }

        private void PopulateSearchTree(List<SearchTreeEntry> list)
        {
            list.Add(new SearchTreeGroupEntry(new GUIContent("Select Object")));

            var foundObjects = FindObjectsInScene(objectType);
            foreach (var obj in foundObjects)
            {
                SearchTreeEntry entry;
                // if (obj is Component)
                    entry = new SearchTreeEntry(
                        new GUIContent($"{obj.name} ({obj.GetType().Name})", EditorGUIUtility.ObjectContent(obj, obj.GetType()).image)
                    );
                // else
                //     entry = null;
                entry.userData = obj;
                entry.level = 1; //level 1 is base layer
                list.Add(entry);
            }
        }

        private static List<Object> FindObjectsInScene(Type t)
        {
            if (t.IsInterface)
                return FindComponentsInScene(t).Cast<Object>().ToList();
            return FindObjectsOfType(t).ToList();
        }

        private static List<Component> FindComponentsInScene(Type t)
        {
            List<Component> objects = new List<Component>();
            GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var rootGameObject in rootGameObjects)
            {
                var components = rootGameObject.GetComponentsInChildren(t);

                foreach (var component in components)
                    objects.Add(component);
            }
            return objects;
        }
    }
}