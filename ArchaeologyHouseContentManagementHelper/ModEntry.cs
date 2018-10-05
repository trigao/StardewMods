﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StardewMods.ArchaeologyHouseContentManagementHelper.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using Harmony;
using StardewMods.Common;
using StardewMods.ArchaeologyHouseContentManagementHelper.Framework.Services;

namespace StardewMods.ArchaeologyHouseContentManagementHelper
{
    /// <summary>The mod entry point.</summary>
    internal class ModEntry : Mod
    {
        private MuseumInteractionDialogService menuInteractDialogService;
        private LostBookFoundDialogService lostBookFoundDialogService;
        private CollectionPageExMenuService collectionPageExMenuService;

        public static CommonServices CommonServices { get; private set; }

        /// <summary>The mod configuration from the player.</summary>
        public static ModConfig ModConfig { get; private set; }

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            if (helper == null)
            {
                Monitor.Log("Error: [modHelper] cannot be [null]!", LogLevel.Error);
                throw new ArgumentNullException(nameof(helper), "Error: [modHelper] cannot be [null]!");
            }

            // Set services and mod configurations
            CommonServices = new CommonServices(Monitor, helper.Translation, helper.Reflection, helper.Content);
            ModConfig = Helper.ReadConfig<ModConfig>();

            // Patch the game
            var harmony = HarmonyInstance.Create("StardewMods.ArchaeologyHouseContentManagementHelper");
            Patches.Patch.PatchAll(harmony);

            collectionPageExMenuService = new CollectionPageExMenuService();
            collectionPageExMenuService.Start();

            SaveEvents.AfterLoad += Bootstrap;
        }

        private void Bootstrap(object sender, EventArgs e)
        {
            menuInteractDialogService = new MuseumInteractionDialogService();
            lostBookFoundDialogService = new LostBookFoundDialogService();

            menuInteractDialogService.Start();
            lostBookFoundDialogService.Start();
        }
    }
}
