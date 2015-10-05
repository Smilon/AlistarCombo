using System;
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

namespace NerdStar
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        public static AIHeroClient _Player { get { return ObjectManager.Player; } }
        public static Spell.Active Q;
        public static Spell.Targeted W;

        public static Menu AliMenu, ComboMenu, MiscMenu;

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            TargetSelector2.init(null);
            Bootstrap.Init(null);

            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Targeted(SpellSlot.W, 620); // reduced the range so it will rarely miss the combo

            AliMenu = MainMenu.AddMenu("NerdStar", "NerdStar");
            AliMenu.AddGroupLabel("NerdStar");
            AliMenu.AddSeparator();
            AliMenu.AddLabel("Nerd Series - Downloading More Ram");
            AliMenu.AddLabel("Berb @ EloBuddy");
            AliMenu.AddSeparator();
            AliMenu.AddLabel("What is this?");
            AliMenu.AddLabel("This is a very simple Alistar Combo addon. All it does it do the Q>W combo for you!");

            ComboMenu = AliMenu.AddSubMenu("Combo", "Combo");
            ComboMenu.AddGroupLabel("Combo Settings");
            ComboMenu.AddSeparator();
            ComboMenu.Add("doCombo", new KeyBind("Do W+Q Combo", false, KeyBind.BindTypes.HoldActive, 'a'));

            MiscMenu = AliMenu.AddSubMenu("Misc", "Misc");
            MiscMenu.AddGroupLabel("Misc. Settings");
            MiscMenu.AddSeparator();
            MiscMenu.Add("interrupt", new CheckBox("Use Spells to Interrupt"));			

            Game.OnTick += Game_OnTick;
			Interrupter.OnInterruptableSpell += StateHandler.Interrupter_OnInterruptableSpell;

            EloBuddy.Chat.Print("ALISTAR COMBO NÃO FUNCIONANDO DIREITO");
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) || Program.ComboMenu["doCombo"].Cast<KeyBind>().CurrentValue)
            {
                StateHandler.Combo();
                Orbwalker.MoveTo(Game.CursorPos);
            }
        }
    }
}
