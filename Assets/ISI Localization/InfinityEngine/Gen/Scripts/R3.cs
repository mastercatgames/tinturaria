#pragma warning disable IDE1006 // Naming Styles
using UnityEngine;
namespace InfinityEngine.Localization {
/// <summary>This class is generated automaticaly by InfinityEngine, it contains constants used by many scripts.  DO NOT EDIT IT !</summary>
	public static class R3 {
		public static class strings {
			public const string Names = "HelloWorld";

			public static ISIString HelloWorld = new ISIString("HelloWorld");
		}
		public static class audios {
			public const string Names = "Hello";

			public static AudioClip Hello => ISILocalization.GetAudio("Hello");
		}
		public static class sprites {
			public const string Names = "LocalizedSprite";

			public static Sprite LocalizedSprite => ISILocalization.GetSprite("LocalizedSprite");
		}
	}
}
