    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalEnumAttribute attribute = (ConditionalEnumAttribute)this.attribute;

        if (_cachedEnumProperty == null || _cachedEnumProperty.serializedObject != property.serializedObject)
        {
            _cachedEnumProperty = GetEnumProperty(property, attribute._enumField);
        }

        if (_cachedEnumProperty == null || !attribute._targetEnumValues.Contains(_cachedEnumProperty.enumValueIndex))
        {
            return 0;
        }

        return EditorGUI.GetPropertyHeight(property, true);
    }

    private SerializedProperty GetEnumProperty(SerializedProperty property, string enumField)
    {
        string path = property.propertyPath.Replace(property.name, enumField);
        return property.serializedObject.FindProperty(path);
    }
}
#endif