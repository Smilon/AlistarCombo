﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace NerdBlitz
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        public static AIHeroClient _Player { get { return ObjectManager.Player; } }
        public static Spell.Skillshot Q;
        public static Spell.Active W;
        public static Spell.Active E;
        public static Spell.Active R;

        private static Slider _numR;
        public static int MinNumberR { get { return _numR.CurrentValue; } }

        private static Slider manaC;
        public static int MinNumberManaC { get { return manaC.CurrentValue; } }

        private static Slider manaH;
        public static int MinNumberManaH { get { return manaH.CurrentValue; } }		
		
        public static Menu BlitzMenu, ComboMenu, HarassMenu, FleeMenu;

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            TargetSelector2.init();
            Bootstrap.Init(null);

            Q = new Spell.Skillshot(SpellSlot.Q, 910, SkillShotType.Linear); // shortened range for better grabs instead of 100% range grabs
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Active(SpellSlot.E);
            R = new Spell.Active(SpellSlot.R, 600);

            BlitzMenu = MainMenu.AddMenu("NerdBlitz", "nerdblitz");
            BlitzMenu.AddGroupLabel("NerdBlitz");
            BlitzMenu.AddSeparator();
            BlitzMenu.AddLabel("Nerd Series - Downloading More Ram");
            BlitzMenu.AddLabel("Berb @ EloBuddy");
			BlitzMenu.AddSeparator();
			BlitzMenu.AddLabel("TO-DO ::");
			BlitzMenu.AddLabel("- Interrupt with Q/E/R");

            ComboMenu = BlitzMenu.AddSubMenu("Combo", "Combo");
            ComboMenu.AddGroupLabel("Combo Settings");
            ComboMenu.AddSeparator();
            ComboMenu.Add("useQCombo", new CheckBox("Use Q"));
			ComboMenu.Add("useWCombo", new CheckBox("Use W"));
			ComboMenu.Add("useECombo", new CheckBox("Use E"));
            ComboMenu.Add("useRCombo", new CheckBox("Use R"));
            ComboMenu.Add("igniteKill", new CheckBox("Ignite if Killable"));
            ComboMenu.AddSeparator();
            _numR = ComboMenu.Add("useRinEnemy", new Slider("Use R if enemy in range", 2, 1, 5));
            ComboMenu.AddSeparator();
            manaC = ComboMenu.Add("manamanager", new Slider("Minimum mana to do combo (%)", 20, 1, 100));

            HarassMenu = BlitzMenu.AddSubMenu("Harass", "Harass");
            HarassMenu.AddGroupLabel("Harass Settings");
            HarassMenu.AddSeparator();
			HarassMenu.Add("useEHarass", new CheckBox("Use E"));
            manaH = HarassMenu.Add("manamanager", new Slider("Minimum mana to harass (%)", 20, 1, 100));

            FleeMenu = BlitzMenu.AddSubMenu("Flee", "Flee");
            FleeMenu.AddGroupLabel("Flee Settings");
            FleeMenu.AddSeparator();
            FleeMenu.Add("useWFlee", new CheckBox("Use W"));

            Game.OnTick += Game_OnTick;
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                StateHandler.Combo();
            }
            else if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                StateHandler.Harass();
            }
            else if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                StateHandler.Flee();
            }
        }
    }
}