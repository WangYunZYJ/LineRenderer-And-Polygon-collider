using UnityEngine;

namespace Assets.HeroEditor.Common.CharacterScripts
{
    /// <summary>
    /// Makes character to look at cursor side (flip by X scale).
    /// </summary>
    public class CharacterFlip : MonoBehaviour
    {
        public void Update()
        {
            transform.localScale = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x ? 1 : -1, 1, 1);
        }
    }
}