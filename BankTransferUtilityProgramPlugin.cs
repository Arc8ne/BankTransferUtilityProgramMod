using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using HarmonyLib;
using GHPluginCoreLib;
using System.Reflection;

namespace BankTransferUtilityProgramMod
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    public class BankTransferUtilityProgramPlugin : BaseUnityPlugin
    {
        private Harmony harmony = new Harmony(pluginGuid);

        public static BankTransferUtilityProgramPlugin instance = null;

        public const string pluginGuid = "gh.bank.transfer.utility.program.plugin";

        public const string pluginName = "Bank Transfer Utility Program Plugin";

        public const string pluginVersion = "0.1.0";

        public string pluginDirectoryPath = "";

        public string pluginDataDirectoryPath = "";

        private void RegisterGreyOSPrograms()
        {
			GHPluginCore.instance.greyOSProgramManager.RegisterProgram(
				new BankTransferUtilityProgram()
			);
		}

        public void Awake()
        {
            instance = this;

            GHPluginCore.instance.Init();

            this.pluginDirectoryPath = AppDomain.CurrentDomain.BaseDirectory + "/BepInEx/plugins/" + pluginName.Replace(" ", "_");

            this.pluginDataDirectoryPath = this.pluginDirectoryPath + "/Data";

            harmony.PatchAll();

            this.RegisterGreyOSPrograms();
        }
    }
}
