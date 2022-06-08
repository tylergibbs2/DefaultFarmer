using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System.Reflection;

namespace DefaultFarmer
{
    public class ModEntry : Mod
    {
        public static IDataHelper Data;

        private IClickableMenu oldSubMenu;

        public override void Entry(IModHelper helper)
        {
            Data = helper.Data;

            helper.Events.Display.MenuChanged += MenuChanged;
            helper.Events.GameLoop.UpdateTicked += UpdateTicked;
        }

        public static void LoadDefaults(CharacterCustomizationDefaults menu)
        {
            FarmerCustomizationData data = Data.ReadGlobalData<FarmerCustomizationData>("farmer-defaults");
            if (data is null)
                return;

            TextBox nameBox = (TextBox)menu.GetType().BaseType.GetField("nameBox", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(menu);
            TextBox farmnameBox = (TextBox)menu.GetType().BaseType.GetField("farmnameBox", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(menu);
            TextBox favThingBox = (TextBox)menu.GetType().BaseType.GetField("favThingBox", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(menu);

            nameBox.Text = data.Name;

            if (menu.source != CharacterCustomization.Source.NewFarmhand)
            {
                farmnameBox.Text = data.FarmName;
                menu.GetType().BaseType.GetField("skipIntro", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(menu, data.SkipIntro);
                menu.skipIntroButton.sourceRect.X = data.SkipIntro ? 236 : 227;
                Game1.player.catPerson = data.CatPerson;
                Game1.player.whichPetBreed = data.Pet;
            }

            favThingBox.Text = data.FavThing;

            Game1.player.changeGender(male: data.Gender);
            Game1.player.changeEyeColor(data.EyeColor);
            Game1.player.changeHairColor(data.HairColor);
            Game1.player.changePants(data.PantsColor);
            Game1.player.changeSkinColor(data.Skin);
            Game1.player.changeHairStyle(data.Hair);
            Game1.player.changeShirt(data.Shirt);
            Game1.player.changePantStyle(data.Pants, is_customization_screen: true);
            Game1.player.changeAccessory(data.Accessory);

            menu.eyeColorPicker.setColor(data.EyeColor);
            menu.hairColorPicker.setColor(data.HairColor);
            menu.pantsColorPicker.setColor(data.PantsColor);
        }

        public static void SaveDefaults(CharacterCustomizationDefaults menu)
        {
            bool skipIntro = (bool)menu.GetType().BaseType.GetField("skipIntro", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(menu);

            TextBox nameBox = (TextBox)menu.GetType().BaseType.GetField("nameBox", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(menu);
            TextBox farmnameBox = (TextBox)menu.GetType().BaseType.GetField("farmnameBox", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(menu);
            TextBox favThingBox = (TextBox)menu.GetType().BaseType.GetField("favThingBox", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(menu);

            FarmerCustomizationData data = new()
            {
                Name = nameBox.Text,
                FarmName = farmnameBox.Text,
                FavThing = favThingBox.Text,
                Gender = Game1.player.IsMale,
                EyeColor = menu.eyeColorPicker.getSelectedColor(),
                HairColor = menu.hairColorPicker.getSelectedColor(),
                PantsColor = menu.pantsColorPicker.getSelectedColor(),
                Skin = Game1.player.skin.Value,
                Hair = Game1.player.hair.Value,
                Shirt = Game1.player.shirt.Value,
                Pants = Game1.player.pants.Value,
                Accessory = Game1.player.accessory.Value,
                CatPerson = Game1.player.catPerson,
                Pet = Game1.player.whichPetBreed,
                SkipIntro = skipIntro
            };

            Data.WriteGlobalData("farmer-defaults", data);
        }

        public void UpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (Game1.activeClickableMenu is not TitleMenu)
                return;

            if (oldSubMenu != TitleMenu.subMenu)
                oldSubMenu = TitleMenu.subMenu;

            if (TitleMenu.subMenu is CharacterCustomization && TitleMenu.subMenu is not CharacterCustomizationDefaults)
            {
                CharacterCustomization.Source source = (TitleMenu.subMenu as CharacterCustomization).source;
                if (source is CharacterCustomization.Source.NewGame || source is CharacterCustomization.Source.NewFarmhand || source is CharacterCustomization.Source.HostNewFarm)
                    TitleMenu.subMenu = new CharacterCustomizationDefaults(source);
            }
        }

        public void MenuChanged(object sender, MenuChangedEventArgs e)
        {
            if (e.NewMenu is CharacterCustomization && e.NewMenu is not CharacterCustomizationDefaults)
            {
                CharacterCustomization.Source source = (e.NewMenu as CharacterCustomization).source;
                if (source is CharacterCustomization.Source.NewFarmhand)
                    Game1.activeClickableMenu = new CharacterCustomizationDefaults(source);
            }
        }
    }
}
