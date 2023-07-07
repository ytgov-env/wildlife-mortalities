﻿using System.Reflection;

namespace WildlifeMortalities.Test.Helpers;

public static class TypeExtensions
{
    public static PropertyInfo[] GetPublicNoneBaseProperties(this Type type) =>
        type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

    public static PropertyInfo[] GetPublicBaseOnlyProperties(this Type type) =>
        type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
}