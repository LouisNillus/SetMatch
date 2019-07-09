using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NS_Menu
{
    namespace SceneChanger
    {
        public class MenuManager : MonoBehaviour
        {
            public void Loader(int index)
            {
                SceneManager.LoadScene(index);
            }

            public static void StaticLoader(int index)
            {
                SceneManager.LoadScene(index);
            }
        }
    }
}

