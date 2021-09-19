using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Rage;
using LSPD_First_Response.Mod.API;

namespace LSCallouts
{
    public class Main: Plugin
    {
        public override void Initialize()
        {
            Functions.OnOnDutyStateChanged += OnOnDutyStateChangedHandler;
            Game.LogTrivial("LSCallouts" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + " developed by Code99 has been initialised.");
            Game.LogTrivial("LSCallouts Information: LSCallouts will be fully loaded when you have gone on duty.");

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(LSPDFRResolveEventHandler);
        }

        public override void Finally()
        {
            Game.LogTrivial("LSCallouts Information: LSCallouts has been cleaned up.");
        }

        private static void OnOnDutyStateChangedHandler(bool OnDuty)
        {
            if (OnDuty)
            {
                RegisterLSCallouts();
                Game.DisplayNotification("LSCallouts | Version A1.0.0 | Alpha - Loaded");
            }
        }

        private static void RegisterLSCallouts()
        {
            // High Speed Pursuit Callouts
            Functions.RegisterCallout(typeof(Callouts.HighSpeedPursuit));
        }

        public static Assembly LSPDFRResolveEventHandler(object sender, ResolveEventArgs args)
        {
            foreach (Assembly assembly in Functions.GetAllUserPlugins())
            {
                if (args.Name.ToLower().Contains(assembly.GetName().Name.ToLower()))
                {
                    return assembly;
                }
            }
            return null;
        }
        public static bool IsLSPDFRPluginRunning(string Plugin, Version minversion = null)
        {
            foreach (Assembly assembly in Functions.GetAllUserPlugins())
            {
                AssemblyName an = assembly.GetName();
                if (an.Name.ToLower() == Plugin.ToLower())
                {
                    if (minversion == null || an.Version.CompareTo(minversion) >= 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
