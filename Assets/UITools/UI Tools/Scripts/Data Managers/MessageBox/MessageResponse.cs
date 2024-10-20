﻿using UnityEngine;

namespace UITools.UI_Tools.Scripts.Data_Managers.MessageBox
{
    /// <summary>
    /// Message Box UI Tool - Data Class
    /// Placed on each button of a Message Box.
    /// 
    /// Developed by Dibbie.
    /// Website: http://www.simpleminded.x10host.com
    /// Email: mailto:strongstrenth@hotmail.com
    /// Discord Server: https://discord.gg/33PFeMv
    /// </summary>

    public class MessageResponse : MonoBehaviour
    {

        public MessageBoxResponse response;

        public void BtnMsgButton()
        {
            MessageBox.InvokeAction(response);
        }
    }
}
