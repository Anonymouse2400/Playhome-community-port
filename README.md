# Playhome community port

 Unity Engine used : Unity 5.5.1f1
 Visual studio used ; Visual Studio 2015
Source code extractor used : assetripper (Extracting unity full souunrce code) https://github.com/AssetRipper/AssetRipper/issues , UABE(for extracting some contents from asset bundle) https://github.com/SeriousCache/UABE , dnspy and ilspy for recovering some source code.

-------------------------------------------------------------------------------------------------------
 A halfbaked working trying to port on android there are some issues
 1. Shaders missing 
 2. Assets bundles are lock to target on platform windows looking for a fix the game can detect files but cant load due to the assets being made for windows and not for android it might take time to modify each asset if this one was fix ingame model characters would run ongoing
 3.  Need some code rebased
 4.  The whole source code is open community is free to modify
5. Need to copy the userdata and abdata on data/com.burger.hentai/files
6. the game is playable on windows but not on android due to assets not loading
7. 
---------------------------------------------------------------------------------------------------------
![Screenshot_2024-10-31-14-58-41-305_com burger hentai](https://github.com/user-attachments/assets/50243274-3695-4fa0-bb45-6068c7f0856b)
![Screenshot_2024-10-31-14-58-25-827_com burger hentai](https://github.com/user-attachments/assets/99a76f08-0bbd-4369-a511-47e41b5396cd)

------------------------------------------------------------------------------------------------------
Things todo!

1. Extract every assets bundles that can be seen from abdata and rebuild as assetbundle using unity 5.5 ( then change the target platform from windows to android) ❌ongoing
2.  rebuild shaders from unity 5.5.5f1 ❌
3.  Build it as a android ✅

Whats next?
Try porting koikatsu and honey select on android!
