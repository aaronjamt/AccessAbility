﻿using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace AccessAbility
{
    internal class Donate
    {
        internal static string donate_clickable_text = "<#00000000>------------<#ff0080ff><size=80%>♡ Donate";
        internal static string donate_clickable_hint = "If you'd like to support me";

        internal static string donate_modal_text_static_1 = "<size=80%><#ffff00ff><u>Support AccessAbility</u><size=75%><#cc99ffff>\nHave you have been enjoying my creations\nand you wish to support me?";
        internal static string donate_modal_text_static_2 = "<size=70%><#ff0080ff>With much love, ♡ Zeph<#00000000>------";

        internal static string donate_modal_text_dynamic = "";
        internal static string donate_modal_hint_dynamic = "";

        internal static void Refresh_Text()
        {
            if (donate_modal_text_dynamic == "")
            {
                _ = Get_Donate_Modal_Text();
            }
        }

        internal static string Open_Donate_Browser()
        {
            Process.Start("https://www.patreon.com/xeph_yr");
            return "";
        }

        private static async Task Get_Donate_Modal_Text()
        {
            //Logger.log.Debug("reply: " + donate_modal_text_dynamic);
            string reply_text = "Loading...";
            string reply_hint = "";

            using (WebClient client = new WebClient())
            {
                try
                {
                    reply_text = await client.DownloadStringTaskAsync("https://raw.githubusercontent.com/zeph-yr/Shoutouts/main/README.md");
                }
                catch
                {
                    reply_text = "Loading failed. Pls ping Zeph on Discord, TY!";
                    Plugin.Log.Debug("Failed to fetch Donate info");
                }
                try
                {
                    reply_hint = await client.DownloadStringTaskAsync("https://raw.githubusercontent.com/zeph-yr/Shoutouts/main/hoverhints.txt");
                }
                catch
                {
                    Plugin.Log.Debug("Failed to fetch Donate info");
                }
            }

            donate_modal_text_dynamic = reply_text;

            int hint_start = reply_hint.IndexOf("[ACCESSABILITY]");
            int hint_end = reply_hint.IndexOf("###", hint_start);

            if (hint_start != -1)
            {
                //Logger.log.Debug("reply: " + reply_hint);
                //Logger.log.Debug("start: " + hint_start + " end: " + hint_end);
                donate_modal_hint_dynamic = reply_hint.Substring(hint_start + 9, hint_end - hint_start - 9); // Yes. And no, it's not wrong.
            }
        }
    }
}