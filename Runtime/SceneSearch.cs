using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LS.Attributes
{
    public class SceneSearch : PropertyAttribute
    {
        public Type type;
        public string typePropertyName;
        public string label;
        public int buttonWidth;
        public SceneSearch(string label = "Search", int buttonWidth = 60)
        {
            this.label = label;
            this.buttonWidth = buttonWidth;
        }

        public SceneSearch(string typePropertyName, string label = "Search", int buttonWidth = 60)
        {
            this.typePropertyName = typePropertyName;
            this.label = label;
            this.buttonWidth = buttonWidth;
        }

        public SceneSearch(Type type, string label = "Search", int buttonWidth = 60)
        {
            this.type = type;
            this.label = label;
            this.buttonWidth = buttonWidth;
        }
    }
}