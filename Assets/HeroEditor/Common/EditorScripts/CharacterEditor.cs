using System;
using System.Linq;
using Assets.HeroEditor.Common.CharacterScripts;
using Assets.HeroEditor.Common.ExampleScripts;
using HeroEditor.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.HeroEditor.Common.EditorScripts
{
    /// <summary>
    /// Character editor UI and behaviour.
    /// </summary>
    public class CharacterEditor : CharacterEditorBase
    {
        [Header("Other")]
        public FirearmCollection FirearmCollection;
        public bool UseEditorColorField = true;
        public string PrefabFolder;
        public string TestRoomSceneName;

        private static Character _temp;

        /// <summary>
        /// Called automatically on app start.
        /// </summary>
        public void Awake()
        {
            RestoreTempCharacter();
        }

        #if UNITY_EDITOR

        /// <summary>
        /// Save character to prefab.
        /// </summary>
        public void Save()
        {
            PrefabFolder = UnityEditor.EditorUtility.SaveFilePanel("Save character prefab", PrefabFolder, "New character", "prefab");

            if (PrefabFolder.Length > 0)
            {
                Save("Assets" + PrefabFolder.Replace(Application.dataPath, null));
            }
        }

        /// <summary>
        /// Load character from prefab.
        /// </summary>
        public void Load()
        {
	        PrefabFolder = UnityEditor.EditorUtility.OpenFilePanel("Open character prefab", PrefabFolder, "prefab");

            if (PrefabFolder.Length > 0)
            {
                Load("Assets" + PrefabFolder.Replace(Application.dataPath, null));
            }

			//FeatureTip();
		}

		public override void Save(string path)
        {
            Character.transform.localScale = Vector3.one;
            UnityEditor.PrefabUtility.CreatePrefab(path, Character.gameObject);
            Debug.LogFormat("Prefab saved as {0}", path);
        }

        public override void Load(string path)
        {
	        var character = UnityEditor.AssetDatabase.LoadAssetAtPath<Character>(path);

	        Character.GetComponent<Character>().Firearm.Params = character.Firearm.Params; // TODO: Workaround
			Load(character);
		}

	    #else

        public override void Save(string path)
        {
            throw new NotSupportedException();
        }

        public override void Load(string path)
        {
            throw new NotSupportedException();
        }

        #endif

        /// <summary>
        /// Test character with demo setup.
        /// </summary>
        public void Test()
        {
            #if UNITY_EDITOR

            if (UnityEditor.EditorBuildSettings.scenes.All(i => !i.path.Contains(TestRoomSceneName)))
            {
	            UnityEditor.EditorUtility.DisplayDialog("Hero Editor", string.Format("Please add '{0}.scene' to Build Settings!", TestRoomSceneName), "OK");
				return;
            }

            #endif

            var controller = Character.gameObject.AddComponent<CharacterController>();

            controller.center = new Vector3(0, 90);
            controller.height = 200;
            controller.radius = 60;
            Character.GetComponent<WeaponControls>().enabled = true;
            Character.gameObject.AddComponent<CharacterControl>();
            DontDestroyOnLoad(Character);
            _temp = Character as Character;
            SceneManager.LoadScene(TestRoomSceneName);

	        //FeatureTip();
		}

		protected override void SetFirearmParams(SpriteGroupEntry entry)
        {
            if (entry == null) return;

            if (FirearmCollection.Firearms.Count(i => i.Name == entry.Name) != 1) throw new Exception("Please check firearms params for: " + entry.Name);

			((Character) Character).Firearm.Params = FirearmCollection.Firearms.Single(i => i.Name == entry.Name);
		}

	    protected override void OpenPalette(GameObject palette, Color selected)
        {
            #if UNITY_EDITOR

            if (UseEditorColorField)
            {
                EditorColorField.Open(selected);
            }
            else

            #endif

            {
                Editor.SetActive(false);
                palette.SetActive(true);
            }
        }

        private void RestoreTempCharacter()
        {
            if (_temp == null) return;

	        Character.GetComponent<Character>().Firearm.Params = _temp.Firearm.Params; // TODO: Workaround
			Load(_temp);
	        Destroy(_temp.gameObject);
            _temp = null;
        }

		private void FeatureTip()
	    {
			#if UNITY_EDITOR

		    if (UnityEditor.EditorUtility.DisplayDialog("Hero Editor", "This feature is available only in PRO asset version!", "Navigate", "Cancel"))
		    {
			    var url = "";

			    switch (SceneManager.GetActiveScene().name)
			    {
				    case "CharacterEditor [FH]": url = "http://u3d.as/QCQ"; break;
				    case "CharacterEditor [MH]": url = "http://u3d.as/14qb"; break;
				    case "CharacterEditor [SFH]": url = "http://u3d.as/18Pr"; break;
				    default: return;
			    }

			    Application.OpenURL(url);
		    }

			#endif
		}
    }
}