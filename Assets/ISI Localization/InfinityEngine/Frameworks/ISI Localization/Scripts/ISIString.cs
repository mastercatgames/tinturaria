#pragma warning disable IDE1006 // Naming Styles

/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS                             		                                                    *                                                                                                          *
*************************************************************************************************************************************/

namespace InfinityEngine.Localization
{
    /// <summary>
    ///    Encapsulates a localized string
    /// </summary>
    public struct ISIString
    {

        /// <summary>
        /// The key
        /// </summary>
        public string key { get; private set; }

        public ISIString(string key)
        {
            this.key = key;
        }

        /// <summary>
        /// Returns a formated version of this value
        /// </summary>
        /// <param name="arg">objects to insert into this string</param>
        /// <returns>fprmated version of this </returns>
        public string Format(params object[] arg)
        {
            return ISILocalization.GetFormatedValueOf(key, arg);
        }

        /// <summary>
        /// Uses implicitly this object as a string object.
        /// </summary>
        /// <param name="self">this struct</param>
        public static implicit operator string(ISIString self)
        {
            return ISILocalization.GetValueOf(self.key);
        }

    }
}