/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                             		                                                    *                                                                                                          *
*************************************************************************************************************************************/


namespace InfinityEngine.Localization
{
    /// <summary>
    /// Extension class of <see cref="Language"/> enum.
    /// </summary>
    public static class LanguageExtensions
    {
        /// <summary>
        /// Gets the locale object corresponding to this <see cref="Language"/>
        /// </summary>
        /// <param name="language">This language</param>
        /// <returns>A Locale object</returns>
        public static Locale ToLocale(this Language language)
        {
            return Locale.WithName(language.ToString());
        }
        public static string Code(this Language language)
        {
            return language.ToLocale().LanguageCode;
        }
    }
}