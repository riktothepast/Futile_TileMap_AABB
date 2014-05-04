using UnityEngine;
using System.Collections.Generic;

public enum PageType
{
		None,
		MenuPage,
		GamePage
}

public class Game : MonoBehaviour
{

		public static Game instance;
		public FFont myfont;
		private PageType _currentPageType = PageType.None;
		private Page _currentPage = null;
		private FStage _stage;


		// Use this for initialization
		void Start ()
		{
				Application.targetFrameRate = 300;
				QualitySettings.vSyncCount = 0;
				RXDebug.Log ("Starting the game");
				instance = this;
				FSoundManager.Init ();
				Go.defaultEaseType = EaseType.Linear;
				Go.duplicatePropertyRule = DuplicatePropertyRuleType.RemoveRunningProperty;
				FutileParams fparams = new FutileParams (true, true, false, true);
		
				fparams.AddResolutionLevel (480.0f, 1.0f, 1.0f, ""); //iPhone
                
				fparams.origin = new Vector2 (0.0f, 0.0f);
		
				Futile.instance.Init (fparams);
		
				Futile.atlasManager.LoadAtlas ("Atlases/Factory");
                Futile.atlasManager.LoadAtlas("Atlases/SFB");
                Futile.atlasManager.LoadFont("font", "font", "Atlases/font", 0, 0);
				
				_stage = Futile.stage;
		
				GoToPage (PageType.GamePage);

		}
	
		public void GoToPage (PageType pageType)
		{
				if (_currentPageType == pageType)
						return;
		
				Page pageToCreate = null;
		
				if (pageType == PageType.GamePage) {
						pageToCreate = new GamePage ();
				}
		
				if (pageToCreate != null) {
						_currentPageType = pageType;	
			
						if (_currentPage != null) {
								_stage.RemoveChild (_currentPage);
						}
			
						_currentPage = pageToCreate;
						_stage.AddChild (_currentPage);
						_currentPage.Start ();
				}
		
		}

}
