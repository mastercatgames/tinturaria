/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                             		                                                    *                                                                                                          *
*************************************************************************************************************************************/

///#EXIT

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

/// <summary>
///  Sprite slicer script
/// </summary>
public class SpriteSlicer : EditorWindow {

    #region Attributes

    private const int Width = 720;
    private const int Height = 420;
    private const int OptionsAreaWidth = 250;
    private static GUIStyle separatorStyle;
    public static GUIStyle SeparatorStyle
    {
        get
        {
            if (separatorStyle == null)
            {
                separatorStyle = new GUIStyle();
                separatorStyle.normal.background = EditorGUIUtility.FindTexture("AvatarBlendBackground");
            }
            return separatorStyle;
        }
    }

    private int _colon = 1;
    private int _row = 1;

    private int row = 1;
    private int colon = 1;

    private static List<Texture2D> Previews;
    private static Texture2D OriginalTexture;
    private Vector2 scroll;
    private float zoom = 1.0f;
    private bool isClosed;

    #endregion Attributes

    void OnGUI()
    {
        if (OriginalTexture == null)
            Close();

        GUI.Box(new Rect(OptionsAreaWidth, 0, 2, Height), GUIContent.none, SeparatorStyle);

        GUILayout.BeginArea(new Rect(0, 0, OptionsAreaWidth, Height));
        GUILayout.Label("Options", EditorStyles.boldLabel);

        _colon = EditorGUILayout.IntField("Colon", _colon);
        _row = EditorGUILayout.IntField("Row", _row);
        _colon = Mathf.Clamp(_colon, 1, _colon);
        _row = Mathf.Clamp(_row, 1, _row);

        if (GUILayout.Button("Preview"))
        {
            colon = _colon;
            row = _row;
            Slice();
        }
        if(GUILayout.Button("Create Assets"))
        {
            if(Previews.Count > 1)
            {
                CreateAssets();
                isClosed = true;
            }else
            {
                ShowNotification(new GUIContent("The number of colon or row must be greater than 1."));
            }
        }
        GUILayout.EndArea();

        if (!isClosed)
        {
            var w = Width - OptionsAreaWidth;
            GUILayout.BeginArea(new Rect(OptionsAreaWidth, 0, w, Height));
            scroll = GUILayout.BeginScrollView(scroll);
            GUILayout.Label("Preview", EditorStyles.boldLabel);
            zoom = EditorGUILayout.Slider("Zoom", zoom, .5f, 2.0f);

            var index = 0;
            for (var i = 0; i < row; i++)
            {
                GUILayout.BeginHorizontal();
                for (var j = 0; j < colon; j++)
                {
                    var tex = Previews[index];
                    GUILayout.Box(new GUIContent(index.ToString(), tex), GUILayout.Width(tex.width * zoom), GUILayout.Height(tex.height * zoom));
                    index++;
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
        }
        GUILayout.EndArea();
        if (isClosed)
            Close();
    }

    /// <summary>
    /// Slice the original texture
    /// </summary>
    private void Slice()
    {
        Previews.Clear();
        var w = OriginalTexture.width / colon;
        var h = OriginalTexture.height / row;

        var y = 0;
        var x = 0;
        for (var i = 0; i < row; i++)
        {
            y = 0;
            for (var j = 0; j < colon; j++)
            {
                var pixels = OriginalTexture.GetPixels(y, x, w , h);
                var tex = new Texture2D(w , h);
                tex.SetPixels(pixels);
                tex.Apply();
                Previews.Add(tex);

                y += w;
            }
            x += h;
        }
    }

    /// <summary>
    /// Generates the files
    /// </summary>
    private void CreateAssets()
    {
        var path = AssetDatabase.GetAssetPath(OriginalTexture);
        var name = Path.GetFileNameWithoutExtension(path);
        var fullName = Path.GetFileName(path);
        var extension = Path.GetExtension(path);
        path = path.Replace(fullName, "");

        for(var i = 0; i < Previews.Count; i++)
        {
            var tex = Previews[i];
            var bytes = tex.EncodeToPNG();
            
            File.WriteAllBytes(string.Concat(path, name, "_part_", i+1, extension), bytes);
            DestroyImmediate(tex);
        }
        AssetDatabase.Refresh();
        Previews.Clear();
    }

    private static bool IsReadable(Texture2D tex)
    {
        if (tex == null)
            return false;

        var assetPath = AssetDatabase.GetAssetPath(tex);
        var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        var isReadable = false;
        if (importer != null)
        {
            isReadable = importer.isReadable;
            if (!isReadable)
            {
                isReadable = EditorUtility.DisplayDialog("Warning", "This texture is not readable, whould you make it readable", "Yes", "No");
                if (isReadable)
                {
                    importer.textureType = TextureImporterType.Default;
                    importer.isReadable = true;
                    AssetDatabase.ImportAsset(assetPath);
                    AssetDatabase.Refresh();
                }
            }
           
        }

        return isReadable;
    }

    [MenuItem("Assets/InfinityEngine/Split Texture")]
    private static void Open()
    {
        OriginalTexture = Selection.activeObject as Texture2D;
        if (IsReadable(OriginalTexture))
        {
            Previews = new List<Texture2D>();
            Previews.Add(OriginalTexture);
            var window = GetWindow<SpriteSlicer>("Sprite Slicer");
            window.minSize = new Vector2(Width, Height);
            window.Show();
        }

    }

    [MenuItem("Assets/InfinityEngine/Split Texture", true)]
    private static bool CanOpen()
    {
        var obj = Selection.activeObject;
        if (obj != null)
            return obj is Texture2D;

        return false;
    }
}
