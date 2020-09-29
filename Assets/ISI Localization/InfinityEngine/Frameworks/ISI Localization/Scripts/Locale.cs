//$(DONT-EXTRACT-DOC)
/*************************************************************************************************************************************
* Developed by Mamadou Cisse                                                                                                        *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                                    *
*************************************************************************************************************************************/
using System.Linq;

namespace InfinityEngine.Localization
{


    using UnityEngine;
    using System;
    using InfinityEngine.Extensions;

    /// <summary>
    /// Represents java.util.Locale object (Used
    /// </summary>
    [Serializable]
    public class Locale
    {
        public string Name { get; set; }
        public string LanguageCode { get; set; }
        public string Country { get; set; }

        #region Static Instances

        ///<summary> 
        ///Bengali
        ///</summary>	
        public static Locale Bengali = new Locale("Bengali", "bn", "");
        ///<summary> 
        ///Bosnian
        ///</summary>	
        public static Locale Bosnian = new Locale("Bosnian", "bs", "");
        ///<summary> 
        ///Catalan
        ///</summary>	
        public static Locale Catalan = new Locale("Catalan", "ca", "");
        ///<summary> 
        ///Czech
        ///</summary>	
        public static Locale Czech = new Locale("Czech", "cs", "");
        ///<summary> 
        ///Welsh
        ///</summary>	
        public static Locale Welsh = new Locale("Welsh", "cy", "");
        ///<summary> 
        ///Danish
        ///</summary>	
        public static Locale Danish = new Locale("Danish", "da", "");
        ///<summary> 
        ///German
        ///</summary>	
        public static Locale German = new Locale("German", "de", "");
        ///<summary> 
        ///English
        ///</summary>	
        public static Locale English = new Locale("English", "en", "");
        ///<summary> 
        ///Spanish
        ///</summary>	
        public static Locale Spanish = new Locale("Spanish", "es", "");
        ///<summary> 
        ///Finnish
        ///</summary>	
        public static Locale Finnish = new Locale("Finnish", "fi", "");
        ///<summary> 
        ///French
        ///</summary>	
        public static Locale French = new Locale("French", "fr", "");
        ///<summary> 
        ///Hindi
        ///</summary>	
        public static Locale Hindi = new Locale("Hindi", "hi", "");
        ///<summary> 
        ///Croatian
        ///</summary>	
        public static Locale Croatian = new Locale("Croatian", "hr", "");
        ///<summary> 
        ///Hungarian
        ///</summary>	
        public static Locale Hungarian = new Locale("Hungarian", "hu", "");
        ///<summary> 
        ///Indonesian
        ///</summary>	
        public static Locale Indonesian = new Locale("Indonesian", "in", "");
        ///<summary> 
        ///Italian
        ///</summary>	
        public static Locale Italian = new Locale("Italian", "it", "");
        ///<summary> 
        ///Japanese
        ///</summary>	
        public static Locale Japanese = new Locale("Japanese", "ja", "");
        ///<summary> 
        ///Korean
        ///</summary>	
        public static Locale Korean = new Locale("Korean", "ko", "");
        ///<summary> 
        ///Dutch
        ///</summary>	
        public static Locale Dutch = new Locale("Dutch", "nl", "");
        ///<summary> 
        ///Polish
        ///</summary>	
        public static Locale Polish = new Locale("Polish", "pl", "");
        ///<summary> 
        ///Portuguese
        ///</summary>	
        public static Locale Portuguese = new Locale("Portuguese", "pt", "");
        ///<summary> 
        ///Russian
        ///</summary>	
        public static Locale Russian = new Locale("Russian", "ru", "");
        ///<summary> 
        ///Slovak
        ///</summary>	
        public static Locale Slovak = new Locale("Slovak", "sk", "");
        ///<summary> 
        ///Albanian
        ///</summary>	
        public static Locale Albanian = new Locale("Albanian", "sq", "");
        ///<summary> 
        ///Serbian
        ///</summary>	
        public static Locale Serbian = new Locale("Serbian", "sr", "");
        ///<summary> 
        ///Swedish
        ///</summary>	
        public static Locale Swedish = new Locale("Swedish", "sv", "");
        ///<summary> 
        ///Swahili
        ///</summary>	
        public static Locale Swahili = new Locale("Swahili", "sw", "");
        ///<summary> 
        ///Tamil
        ///</summary>	
        public static Locale Tamil = new Locale("Tamil", "ta", "");
        ///<summary> 
        ///Thai
        ///</summary>	
        public static Locale Thai = new Locale("Thai", "th", "");
        ///<summary> 
        ///Turkish
        ///</summary>	
        public static Locale Turkish = new Locale("Turkish", "tr", "");
        ///<summary> 
        ///Vietnamese
        ///</summary>	
        public static Locale Vietnamese = new Locale("Vietnamese", "vi", "");
        ///<summary> 
        ///Chinese
        ///</summary>	
        public static Locale Chinese = new Locale("Chinese", "zh", "");


        #endregion Static Instances

        /// <summary>
        /// Creates new instance of <c>Locale</c>
        /// </summary>
        /// <param name="name">The name of the Locale example : English</param>
        /// <param name="language">The Language of the Locale example : en</param>
        /// <param name="country">The country code of the Locale example : US</param>
        public Locale(string name, string language, string country)
        {
            this.Name = name;
            this.LanguageCode = language;
            this.Country = country;
        }

        /// <summary>
        /// Gets all 32 available locales (without country code).
        /// </summary>
        public static Locale[] AllLocales
        {
            get
            {
                var type = typeof(Locale);
                return type.GetFields()
                           .Where(field => field.FieldType == type)
                           .Select(field => (Locale)field.GetValue(null))
                           .ToArray();
            }
        }

        /// <summary>
        /// returns this name, language and country
        /// </summary>
        public string Informations => $"{Name} : {LanguageCode} : {Country}";

        /// <summary>
        /// Finds the locale with the given name.
        /// </summary>
        /// <param name="name">The name</param>
        /// <returns>a <c>Locale</c> object if there is a object found <c>null</c> otherwise</returns>
        public static Locale WithName(string name)
        {
            return AllLocales.FirstOrDefault(elem => elem.Name == name);
        }

        public override string ToString() => Name;

    }
}