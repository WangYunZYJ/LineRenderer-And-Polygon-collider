using HeroEditor.Common;
using HeroEditor.Common.Editor;
using UnityEditor;
using UnityEngine;

namespace Assets.HeroEditor.Common.Editor
{
    /// <summary>
    /// Add "Refresh" button to SpriteCollection script
    /// </summary>
    [CustomEditor(typeof(SpriteCollection))]
    public class SpriteCollectionEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var spriteCollection = (SpriteCollection) target;

            if (GUILayout.Button("Refresh"))
            {
                SpriteCollectionRefresh.Refresh(spriteCollection);
            }

            if (GUILayout.Button("Generate trails for melee weapons"))
            {
                SpriteCollectionRefresh.CreateMeleeWeaponTrails(spriteCollection);
            }

            if (GUILayout.Button("Read trails for melee weapons"))
            {
                SpriteCollectionRefresh.ReadMeleeWeaponTrails(spriteCollection);
            }
        }
    }
}