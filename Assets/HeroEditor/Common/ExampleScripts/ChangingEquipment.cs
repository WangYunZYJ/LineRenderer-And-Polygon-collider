using System.Linq;
using Assets.HeroEditor.Common.CharacterScripts;
using HeroEditor.Common;
using HeroEditor.Common.Enums;
using UnityEngine;

namespace Assets.HeroEditor.Common.ExampleScripts
{
	/// <summary>
	/// Changing equipment at runtime examples.
	/// </summary>
	public class ChangingEquipment : MonoBehaviour
	{
		public SpriteCollection SpriteCollection;
		public Character Character;

		public void EquipMeleeWeapon1H(string sname, string collection)
		{
			Character.PrimaryMeleeWeapon = SpriteCollection.MeleeWeapon1H.Single(i => i.Name == sname && i.Collection == collection).Sprite;
			Character.WeaponType = WeaponType.Melee1H;
			Character.Initialize();
		}

		public void EquipShield(string sname, string collection)
		{
			Character.Shield = SpriteCollection.Shield.Single(i => i.Name == sname && i.Collection == collection).Sprite;
			Character.Initialize();
		}

		public void EquipBow(string sname, string collection)
		{
			Character.Bow = SpriteCollection.Bow.Single(i => i.Name == sname && i.Collection == collection).Sprites;
			Character.Initialize();
		}

		public void EquipArmor(string sname, string collection)
		{
			Character.Armor = SpriteCollection.Armor.Single(i => i.Name == sname && i.Collection == collection).Sprites;
			Character.Initialize();
		}

		public void EquipHelmet(string sname, string collection)
		{
			Character.Helmet = SpriteCollection.Helmet.Single(i => i.Name == sname && i.Collection == collection).Sprite;
			Character.HairMaskType = HairMaskType.HelmetMask; // None, HelmetMask, HeadMask
			Character.HelmetMask = SpriteCollection.HelmetMask.Single(i => i.Name == sname && i.Collection == collection).Sprite;
			Character.Initialize();
		}
	}
}