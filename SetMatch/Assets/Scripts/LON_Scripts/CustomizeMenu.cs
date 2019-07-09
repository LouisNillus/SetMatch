using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NS_Menu
{
    namespace Customize
    {
        public class CustomizeMenu : MonoBehaviour
        {

            [Header("Horizontal Scrolling")]
            public bool horizontalScrolling;
            public RectTransform clampLeft;
            public RectTransform clampRight;

            [Header("Vertical Scrolling")]
            public bool verticalScrolling;
            public RectTransform clampUp;
            public RectTransform clampDown;
            ScrollRect horizontalPanelToScroll;

            private void Start()
            {
                horizontalPanelToScroll = this.GetComponent<ScrollRect>();
            }

            private void Update()
            {
                if(horizontalScrolling == true)
                {
                    HorizontalClampScroll();
                }

                if(verticalScrolling == true)
                {
                    VerticalClampScroll();
                }
            }

            public void HorizontalClampScroll()
            {
                if (horizontalPanelToScroll.content.position.x <= clampRight.position.x)
                {
                    horizontalPanelToScroll.content.position = new Vector2(clampRight.position.x, horizontalPanelToScroll.content.position.y);
                }

                if (horizontalPanelToScroll.content.position.x >= clampLeft.position.x)
                {
                    horizontalPanelToScroll.content.position = new Vector2(clampLeft.position.x, horizontalPanelToScroll.content.position.y);
                }
            }

            public void VerticalClampScroll()
            {
                if (horizontalPanelToScroll.content.position.y >= clampUp.position.y)
                {
                    horizontalPanelToScroll.content.position = new Vector2(horizontalPanelToScroll.content.position.x, clampUp.position.y);
                }

                if (horizontalPanelToScroll.content.position.y <= clampDown.position.y)
                {
                    horizontalPanelToScroll.content.position = new Vector2(horizontalPanelToScroll.content.position.x, clampDown.position.y);
                }
            }



        }
    }
}

