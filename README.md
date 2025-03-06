# Playhome reversed engineered community port for Android?

 Unity Engine used : Unity 5.5.1f1
 Visual studio used ; Visual Studio 2015
Source code extractor used :
assetripper (Extracting unity full souunrce code) https://github.com/AssetRipper/AssetRipper/issues , 
UABE(for extracting some contents from asset bundle) https://github.com/SeriousCache/UABE , 
dnspy and ilspy for recovering some scripts .

-------------------------------------------------------------------------------------------------------
 A halfbaked working trying to port on android there are some issues
 1. Shaders missing 
 2. Assets bundles are lock to target on platform windows looking for a fix the game can detect files but cant load due to the assets being made for windows and not for android it might take time to modify each asset if this one was fix ingame model characters would run ongoing
 3.  Need some code rebased
 4.  The whole source code is open community is free to modify
5. Need to copy the userdata and abdata on data/com.burger.hentai/files
6. the game is playable on windows but not on android due to assets not loading
7.  Added a console error viewer on the game to check for debugging issues
---------------------------------------------------------------------------------------------------------
![Screenshot_2024-10-31-14-58-41-305_com burger hentai](https://github.com/user-attachments/assets/50243274-3695-4fa0-bb45-6068c7f0856b)
![Screenshot_2024-10-31-14-58-25-827_com burger hentai](https://github.com/user-attachments/assets/99a76f08-0bbd-4369-a511-47e41b5396cd)
![image](https://github.com/user-attachments/assets/f60548a6-4a65-4c05-bc13-8b63eb49e23e)
![image](https://github.com/user-attachments/assets/ec9fa3c2-b5a9-4056-95f1-12c359786784)

------------------------------------------------------------------------------------------------------
Things todo!

1. Extract every assets bundles that can be seen from abdata and rebuild as assetbundle using unity 5.5 ( then change the target platform from windows to android) ❌ongoing
2.  rebuild shaders from unity 5.5.5f1 ✅ 60% converted all to standard shaders for now will try rebuilding the human skin shader
3.  Build it as a android ✅ (due to the game engine is dated 2016 you have to execute a command on cmd inside platform tool to install this apk on android 13 and above adb "install --bypass-low-target-sdk-block name of the apk.apk " oh dont forgt to switch platform if youre building it to android) ✅
4.  Adding mods? Translations? Upgrading the game engine to newer version? ❌

Updates:
1. the game now boots to scene
2. Clothes accessories shaders are working
3. Will rebuild the human skin shader
4. H scene is working
5. apk requires a android version android 12 below dues to unity engine issue <strike>will try to repack all assets onto 1 apk</strike>
6. 


How to build?
Download this whole package and open in unity 5.5.5f1 as project and build to windows or android (assets will not load still looking for a quick fix)

test build 
Playhome android port beta

Github :https://github.com/Anonymouse2400/Playhome-community-port

to install apk download this https://drive.google.com/file/d/1I1THe_LOYRJuHxW_zHyd-T2PCYMSptri/view?usp=drive_link you need platform tools(search on google) to bypass apk restriction enable usb debugging first and type adb install --bypass-low-target-sdk-block test.apk

extract all the contents of this https://drive.google.com/file/d/1JyHjF4woI8NdukulZYK1BUPapdb2YJmw/view?usp=drive_link to android/data/com.burger.hentai/files/abdata

also extract this https://drive.google.com/file/d/1lvrqJ5GD6Tv0vLzBAjAl6BekhcO6wK70/view?usp=drive_link to android/data/com.burger.hentai/files/

Tested on redmi note 13 4g android 15 sd 685,samsung a05s android 14 sd 680, and redmi 9c helio g35 android 10

I'll update the source code on github soon i need assistant from community as im continuing to port koikatsu even it is not finished yet

game size is 3.5 gb , runs on 32 bit

Whats next?
Well Try porting koikatsu and honey select on android though i did have started some!
