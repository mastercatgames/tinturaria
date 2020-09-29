/*************************************************
* Developed by Mamadou Cisse                     *
* Mail => mciissee@gmail.com                     *
* Twitter => http://www.twitter.com/IncMce       *
* Unity Asset Store catalog: http://u3d.as/riS	 *
*************************************************/


namespace InfinityEngine.Utils
{
    using UnityEngine;

    /// <summary>
    ///   Static reference to resources placed in 'InfinityEngine/Editor/Resources' folder.
    /// </summary>
    public static class AssetReferences
    {

        #region Fields

        #region Textures

        private static Texture2D logo;
        private static Texture2D helpIconEnable;
        private static Texture2D helpIconDisable;
        private static Texture2D searchIcon;
        private static Texture2D minusIcon;
        private static Texture2D plusIcon;
        private static Texture2D saveIcon;
        private static Texture2D loadIcon;
        private static Texture2D editIcon;
        private static Texture2D fbICon;
        private static Texture2D twitterIcon;
        private static Texture2D docIcon;
        private static Texture2D rateIcon;
        private static Texture2D moreIcon;
        private static Texture2D supportIcon;
        private static Texture2D achievementIconEnable;
        private static Texture2D leaderboardIconEnable;
        private static Texture2D achievementIconDisable;
        private static Texture2D leaderboardIconDisable;
        private static Texture2D google_translate_icon;

        #endregion Textures

        private static GUISkin infinityEditorSkin;
        private static Font fontAwesomeFont;

        #endregion Fields

        #region Properties

        #region Textures

        public static Texture2D Logo => logo ?? (logo = Resources.Load<Texture2D>("Textures/logo"));
        public static Texture2D HelpIconEnable => helpIconEnable ?? (helpIconEnable = Resources.Load<Texture2D>("Textures/help_icon_enable"));
        public static Texture2D HelpIconDisable => helpIconDisable ?? (helpIconDisable = Resources.Load<Texture2D>("Textures/help_icon_disable"));
        public static Texture2D SearchIcon => searchIcon ?? (searchIcon = Resources.Load<Texture2D>("Textures/search_icon"));
        public static Texture2D MinusIcon => minusIcon ?? (minusIcon = Resources.Load<Texture2D>("Textures/minus_icon"));
        public static Texture2D PlusIcon => plusIcon ?? (plusIcon = Resources.Load<Texture2D>("Textures/plus_icon"));
        public static Texture2D SaveIcon => saveIcon ?? (saveIcon = Resources.Load<Texture2D>("Textures/save_icon"));
        public static Texture2D LoadIcon => loadIcon ?? (loadIcon = Resources.Load<Texture2D>("Textures/load_icon"));
        public static Texture2D EditIcon => editIcon ?? (editIcon = Resources.Load<Texture2D>("Textures/edit_icon"));
        public static Texture2D FbICon => fbICon ?? (fbICon = Resources.Load<Texture2D>("Textures/facebook_icon"));
        public static Texture2D TwitterIcon => twitterIcon ?? (twitterIcon = Resources.Load<Texture2D>("Textures/twitter_icon"));
        public static Texture2D DocIcon => docIcon ?? (docIcon = Resources.Load<Texture2D>("Textures/doc_icon"));
        public static Texture2D RateIcon => rateIcon ?? (rateIcon = Resources.Load<Texture2D>("Textures/rate_icon"));
        public static Texture2D MoreIcon => moreIcon ?? (moreIcon = Resources.Load<Texture2D>("Textures/more_games_icon"));
        public static Texture2D SupportIcon => supportIcon ?? (supportIcon = Resources.Load<Texture2D>("Textures/support_icon"));

        public static Texture2D AchievementIconEnable => achievementIconEnable ?? (achievementIconEnable = Resources.Load<Texture2D>("Textures/achievement_enable"));
        public static Texture2D LeaderboardIconEnable => leaderboardIconEnable ?? (leaderboardIconEnable = Resources.Load<Texture2D>("Textures/leaderboard_enable"));
        public static Texture2D AchievementIconDisable => achievementIconDisable ?? (achievementIconDisable = Resources.Load<Texture2D>("Textures/achievement_disable"));
        public static Texture2D LeaderboardIconDisable => leaderboardIconDisable ?? (leaderboardIconDisable = Resources.Load<Texture2D>("Textures/leaderboard_disable"));
        public static Texture2D GoogleTranslateIcon => google_translate_icon ?? (google_translate_icon = Resources.Load<Texture2D>("Textures/google_translate_icon"));

        #endregion Textures

        public static GUISkin InfinityEditorSkin => infinityEditorSkin ?? (infinityEditorSkin = Resources.Load<GUISkin>("Skins/infinity-editor-skin"));

        public static GUIStyle AccordionHeader => InfinityEditorSkin.GetStyle("AccordionHeader");

        public static GUIStyle FontAwesome => InfinityEditorSkin.GetStyle("FontAwesome");

        public static GUIStyle GetFontAwesomeStyle(GUIStyle copy)
        {
            var style = new GUIStyle(copy) { font = FontAwesomeFont };
            return style;
        }

        public static Font FontAwesomeFont => fontAwesomeFont ?? (fontAwesomeFont = Resources.Load<Font>("Fonts/FontAwesome"));

        #endregion Properties

    }
}