using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public class DuEditor : Editor
    {
        public class DuProperty
        {
            internal string propertyPath;
            internal string title;
            internal string tooltip;
            internal SerializedProperty property;
            internal bool isChanged;

            internal Editor parentEditor;

            //--------------------------------------------------------------------------------------------------------------
            // Helpers

            public SerializedProperty FindInnerProperty(string relativePropertyPath)
            {
                return property.FindPropertyRelative(relativePropertyPath);
            }

            public bool IsTrue => property.propertyType == SerializedPropertyType.Boolean ? property.boolValue : false;

            public int enumValueIndex => property?.enumValueIndex ?? 0;

            public int valInt
            {
                get => property.intValue;
                set => property.intValue = value;
            }

            public float valFloat
            {
                get => property.floatValue;
                set => property.floatValue = value;
            }

            public string valString
            {
                get => property.stringValue;
                set => property.stringValue = value;
            }

            public Vector3 valVector3
            {
                get => property.vector3Value;
                set => property.vector3Value = value;
            }

            public Vector3Int valVector3Int
            {
                get => property.vector3IntValue;
                set => property.vector3IntValue = value;
            }

            public Quaternion valQuaternion
            {
                get => property.quaternionValue;
                set => property.quaternionValue = value;
            }

            public AnimationCurve valAnimationCurve
            {
                get => property.animationCurveValue;
                set => property.animationCurveValue = value;
            }

            public Color valColor
            {
                get => property.colorValue;
                set => property.colorValue = value;
            }

            public SerializedProperty valUnityEvent => property.FindPropertyRelative("m_PersistentCalls.m_Calls");

            public GameObject GameObjectReference => property.objectReferenceValue as GameObject;

            public bool ObjectReferenceExists => Dust.IsNotNull(property.objectReferenceValue);
        }

        //--------------------------------------------------------------------------------------------------------------

        public DuProperty FindProperty(string propertyPath)
            => FindProperty(propertyPath, "", "");

        public DuProperty FindProperty(string propertyPath, string title)
            => FindProperty(propertyPath, title, "");

        public DuProperty FindProperty(string propertyPath, string title, string tooltip)
        {
            var duProperty = new DuProperty
            {
                propertyPath = propertyPath,
                title = title,
                tooltip = tooltip,
                property = serializedObject.FindProperty(propertyPath),
                isChanged = false,
                parentEditor = this
            };
            return duProperty;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static DuProperty FindProperty(SerializedProperty parentProperty, string propertyPath)
            => FindProperty(null, parentProperty, propertyPath, "", "");

        public static DuProperty FindProperty(SerializedProperty parentProperty, string propertyPath, string title)
            => FindProperty(null, parentProperty, propertyPath, title, "");

        public static DuProperty FindProperty(DuEditor parentEditor, SerializedProperty parentProperty, string propertyPath, string title)
            => FindProperty(parentEditor, parentProperty, propertyPath, title, "");

        public static DuProperty FindProperty(DuEditor parentEditor, SerializedProperty parentProperty, string propertyPath, string title, string tooltip)
        {
            var duProperty = new DuProperty
            {
                propertyPath = propertyPath,
                title = title,
                tooltip = tooltip,
                property = parentProperty.FindPropertyRelative(propertyPath),
                isChanged = false,
                parentEditor = parentEditor,
            };
            return duProperty;
        }

        //--------------------------------------------------------------------------------------------------------------

        public class SerializedEntity
        {
            internal Object target;
            internal SerializedObject serializedObject;
        }

        // If I change some parameters for list of targets then I need to create SerializedObject for each target.
        // BUT only for self-target I need to use self-serializedObject object!
        public SerializedEntity[] GetSerializedEntitiesByTargets()
        {
            return GetSerializedEntitiesByTargets(this);
        }

        public static SerializedEntity[] GetSerializedEntitiesByTargets(DuEditor targetEditor)
        {
            var serializedTargets = new SerializedEntity[targetEditor.targets.Length];

            for (int i = 0; i < targetEditor.targets.Length; i++)
            {
                serializedTargets[i] = new SerializedEntity
                {
                    target = targetEditor.targets[i],
                    serializedObject = targetEditor.targets[i] == targetEditor.target
                        ? targetEditor.serializedObject :
                        new SerializedObject(targetEditor.targets[i])
                };
            }

            return serializedTargets;
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void AddComponentToSelectedObjects(System.Type duComponentType)
        {
            if (Selection.gameObjects.Length == 0)
                return;

            foreach (var gameObject in Selection.gameObjects)
            {
                Undo.AddComponent(gameObject, duComponentType);
            }
        }

        public static void AddComponentToSelectedOrNewObject(string gameObjectName, System.Type duComponentType)
        {
            if (Selection.gameObjects.Length > 0)
            {
                foreach (var gameObject in Selection.gameObjects)
                {
                    Undo.AddComponent(gameObject, duComponentType);
                }
            }
            else
            {
                AddComponentToNewObject(gameObjectName, duComponentType);
            }
        }

        public static Component AddComponentToNewObject(string gameObjectName, System.Type duComponentType)
            => AddComponentToNewObject(gameObjectName, duComponentType, true);

        public static Component AddComponentToNewObject(string gameObjectName, System.Type duComponentType, bool fixUndoState)
        {
            var gameObject = new GameObject();
            gameObject.name = gameObjectName;

            if (Dust.IsNotNull(Selection.activeGameObject))
                gameObject.transform.parent = Selection.activeGameObject.transform;

            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
            gameObject.transform.localScale = Vector3.one;

            var component = gameObject.AddComponent(duComponentType);

            if (fixUndoState)
                Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);

            Selection.activeGameObject = gameObject;
            return component;
        }

        protected static bool IsAllowExecMenuCommandOnce(MenuCommand menuCommand)
        {
            return Selection.objects.Length == 0 || menuCommand.context == Selection.objects[0];
        }

        //--------------------------------------------------------------------------------------------------------------

        public static bool PropertyField(DuProperty duProperty, string label)
            => PropertyField(duProperty, label, "");

        public static bool PropertyField(DuProperty duProperty, string label, string tooltip)
        {
            if (Dust.IsNull(duProperty.property))
            {
                Dust.Debug.Warning("DuProperty[" + duProperty.propertyPath + "] is null");
                return false;
            }

            EditorGUI.BeginChangeCheck();
            DustGUI.Field(new GUIContent(label, tooltip), duProperty.property);
            duProperty.isChanged = EditorGUI.EndChangeCheck();
            return duProperty.isChanged;
        }

        public static bool PropertyFieldOrLock(DuProperty duProperty, bool isLocked, string label)
            => PropertyFieldOrLock(duProperty, isLocked, label, "");

        public static bool PropertyFieldOrLock(DuProperty duProperty, bool isLocked, string label, string tooltip)
        {
            if (isLocked) DustGUI.Lock();
            PropertyField(duProperty, label, tooltip);
            if (isLocked) DustGUI.Unlock();
            return duProperty.isChanged;
        }

        public static bool PropertyFieldOrHide(DuProperty duProperty, bool isHidden, string label)
            => PropertyFieldOrHide(duProperty, isHidden, label, "");

        public static bool PropertyFieldOrHide(DuProperty duProperty, bool isHidden, string label, string tooltip)
        {
            if (isHidden)
                return false;

            return PropertyField(duProperty, label, tooltip);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static bool PropertyField(DuProperty duProperty)
        {
            return PropertyField(duProperty, duProperty.title, duProperty.tooltip);
        }

        public static bool PropertyFieldOrLock(DuProperty duProperty, bool isLocked)
        {
            return PropertyFieldOrLock(duProperty, isLocked, duProperty.title, duProperty.tooltip);
        }

        public static bool PropertyFieldOrHide(DuProperty duProperty, bool isHidden)
        {
            return PropertyFieldOrHide(duProperty, isHidden, duProperty.title, duProperty.tooltip);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static bool PropertyFieldRange(DuProperty duProperty)
            => PropertyFieldRange(duProperty, "Range Min", "Range Max");

        public static bool PropertyFieldRange(DuProperty duProperty, string titleRangeMin, string titleRangeMax)
        {
            if (Dust.IsNull(duProperty.property))
            {
                Dust.Debug.Warning("DuProperty[" + duProperty.propertyPath + "] is null");
                return false;
            }

            duProperty.isChanged |= PropertyField(duProperty.FindInnerProperty("m_Min"), titleRangeMin);
            duProperty.isChanged |= PropertyField(duProperty.FindInnerProperty("m_Max"), titleRangeMax);
            return duProperty.isChanged;
        }

        public static bool PropertySeedRandomOrFixed(DuProperty duProperty)
            => PropertySeedRandomOrFixed(duProperty, DuConstants.RANDOM_SEED_DEFAULT);

        public static bool PropertySeedRandomOrFixed(DuProperty duProperty, int defValue)
        {
            bool isChanged = false;

            int seed = duProperty.valInt;

            bool curUseSeed = seed > 0;
            bool newUseSeed = DustGUI.Field("Use Fixed Seed", curUseSeed);

            if (curUseSeed != newUseSeed)
            {
                if (newUseSeed)
                    duProperty.valInt = duProperty.valInt == 0 ? defValue : -duProperty.valInt;
                else
                    duProperty.valInt = -duProperty.valInt;

                isChanged = true;
            }

            if (newUseSeed)
                isChanged = PropertySeedFixed(duProperty);

            return isChanged;
        }

        public static bool PropertySeedFixed(DuProperty duProperty)
        {
            int seedMin = DuConstants.RANDOM_SEED_MIN;
            int seedMax = DuConstants.RANDOM_SEED_MAX;
            int seedEditorMin = DuConstants.RANDOM_SEED_MIN_IN_EDITOR;
            int seedEditorMax = DuConstants.RANDOM_SEED_MAX_IN_EDITOR;

            EditorGUI.BeginChangeCheck();
            DustGUI.ExtraIntSlider.Create(seedEditorMin, seedEditorMax, 1, seedMin, seedMax).Draw("Seed", duProperty.property);
            duProperty.isChanged = EditorGUI.EndChangeCheck();
            return duProperty.isChanged;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static bool PropertySlider(DuProperty duProperty, float leftValue, float rightValue)
        {
            if (Dust.IsNull(duProperty.property))
            {
                Dust.Debug.Warning("DuProperty[" + duProperty.propertyPath + "] is null");
                return false;
            }

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.Slider(duProperty.property, leftValue, rightValue, new GUIContent(duProperty.title, duProperty.tooltip));
            duProperty.isChanged = EditorGUI.EndChangeCheck();
            return duProperty.isChanged;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static bool PropertyExtendedIntSlider(DuProperty duProperty, int leftValue, int rightValue, int stepValue)
            => PropertyExtendedIntSlider(duProperty, leftValue, rightValue, stepValue, int.MinValue, int.MaxValue);

        public static bool PropertyExtendedIntSlider(DuProperty duProperty, int leftValue, int rightValue, int stepValue, int leftLimit)
            => PropertyExtendedIntSlider(duProperty, leftValue, rightValue, stepValue, leftLimit, int.MaxValue);

        public static bool PropertyExtendedIntSlider(DuProperty duProperty, int leftValue, int rightValue, int stepValue, int leftLimit, int rightLimit)
        {
            var slider = new DustGUI.ExtraIntSlider(leftValue, rightValue, stepValue, leftLimit, rightLimit);
            slider.LinkEditor(duProperty.parentEditor);
            duProperty.isChanged = slider.Draw(new GUIContent(duProperty.title, duProperty.tooltip), duProperty.property);
            return duProperty.isChanged;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static bool PropertyExtendedSlider(DuProperty duProperty, float leftValue, float rightValue, float stepValue)
            => PropertyExtendedSlider(duProperty, leftValue, rightValue, stepValue, float.MinValue, float.MaxValue);

        public static bool PropertyExtendedSlider(DuProperty duProperty, float leftValue, float rightValue, float stepValue, float leftLimit)
            => PropertyExtendedSlider(duProperty, leftValue, rightValue, stepValue, leftLimit, float.MaxValue);

        public static bool PropertyExtendedSlider(DuProperty duProperty, float leftValue, float rightValue, float stepValue, float leftLimit, float rightLimit)
        {
            var slider = new DustGUI.ExtraSlider(leftValue, rightValue, stepValue, leftLimit, rightLimit);
            slider.LinkEditor(duProperty.parentEditor);
            duProperty.isChanged = slider.Draw(new GUIContent(duProperty.title, duProperty.tooltip), duProperty.property);
            return duProperty.isChanged;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static bool PropertyExtendedSlider01(DuProperty duProperty)
        {
            return PropertyExtendedSlider(duProperty, 0f, 1f, 0.01f, 0f, 1f);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static bool PropertyFieldCurve(DuProperty duProperty)
            => PropertyFieldCurve(duProperty, 100);

        public static bool PropertyFieldCurve(DuProperty duProperty, int height)
        {
            return PropertyFieldCurve(duProperty, duProperty.title);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public static bool PropertyFieldCurve(DuProperty duProperty, string label)
            => PropertyFieldCurve(duProperty, label, 100);

        public static bool PropertyFieldCurve(DuProperty duProperty, string label, int height)
        {
            if (Dust.IsNull(duProperty.property))
            {
                Dust.Debug.Warning("DuProperty[" + duProperty.propertyPath + "] is null");
                return false;
            }

            EditorGUI.BeginChangeCheck();
            DustGUI.Field(label, duProperty.property, 0, 90);
            duProperty.isChanged = EditorGUI.EndChangeCheck();
            return duProperty.isChanged;
        }

        //--------------------------------------------------------------------------------------------------------------

        public static bool PropertyField(SerializedProperty property, string label)
            => PropertyField(property, label, "");

        public static bool PropertyField(SerializedProperty property, string label, string tooltip)
        {
            EditorGUI.BeginChangeCheck();
            DustGUI.Field(new GUIContent(label, tooltip), property);
            return EditorGUI.EndChangeCheck();
        }

        //--------------------------------------------------------------------------------------------------------------

        public static bool PropertyFieldOrLock(SerializedProperty property, bool isLocked, string label)
            => PropertyFieldOrLock(property, isLocked, label, "");

        public static bool PropertyFieldOrLock(SerializedProperty property, bool isLocked, string label, string tooltip)
        {
            if (isLocked) DustGUI.Lock();

            bool isChanged = PropertyField(property, label, tooltip);

            if (isLocked) DustGUI.Unlock();

            return isChanged;
        }

        //--------------------------------------------------------------------------------------------------------------

        public static int Popup(string label, int selectedIndex, string[] displayedOptions)
        {
            return EditorGUILayout.Popup(label, selectedIndex, displayedOptions);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void Space()
        {
            DustGUI.SpaceLine(0.5f);
        }
    }
}
