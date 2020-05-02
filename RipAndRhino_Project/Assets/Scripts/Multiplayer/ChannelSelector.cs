using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RhinoGame
{
    public class ChannelSelector : MonoBehaviour, IPointerClickHandler
    {
        public string Channel;
        public ChatGui ChatGui;

        public void SetChannel(string channel)
        {
            this.Channel = channel;
            Text t = GetComponentInChildren<Text>();
            t.text = this.Channel;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ChatGui.ShowChannel(this.Channel);
        }
    }
}
