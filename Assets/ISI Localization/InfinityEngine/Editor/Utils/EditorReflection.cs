///#EXIT
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using InfinityEngine.Utils;
using System;
using System.Reflection;

/// <summary>
/// Provides access to useful methods whichs simplify the process of the reflection only in editor mode.
/// </summary>
public static class EditorReflection  {


    private static Type[] Decorators = ReflectionUtils.GetTypesInheritingFrom(typeof(CustomPropertyDrawer));
    
    /// <summary>
    /// Gets a value indicating whether the type has a property drawer
    /// </summary>
    /// <param name="type">The type</param>
    /// <returns><c>true</c> if it exists a property drawer for the type <c>false</c> otherwise</returns>
    public static bool HasPropertyDrawer(Type type)
    {

        var decorators = Decorators;
        CustomPropertyDrawer drawer;
        object drawerType;
        var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.Static;
        foreach (var decorator in decorators)
        {
            drawer = ReflectionUtils. GetAttribute<CustomPropertyDrawer>(decorator);
            if (drawer != null)
            {
                drawerType = drawer.GetType().GetField("m_Type", flags).GetValue(drawer);
                if (drawerType.ToString() == type.FullName)
                {
                    return true;
                }
            }
        }
        return false;
    }

}