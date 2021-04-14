using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{
    public const string photonAppId = "718020f9-c4e0-45e4-a866-fad4d6a2b09d";
    public const string photonAppVersion = "1.0";
    public const string photonChatAppId = "0651d30c-1b90-4f24-beab-11a61491636e";
    public const string photonChatAppVersion = "1.0";
    public const int loadingSceneIndex = 0;
    //public const int menuSceneIndex = 9;
    //public const int multiplayerGameLaunchSceneIndex = 7;
    //public const int playSceneIndex = 3;
    //public const int headlessPlaySceneIndex = 10;
    //public const int enterNameSceneIndex = 4;
    //public const int chooseCharacterSceneIndex = 5;
    public const int testSceneIndex = 2;
    //public const int headlessLaunchSceneIndex = 8;
    public const int downloadAssetsSceneIndex = 1;


    public const int lowResThreshold = 1700; //should be checked against screen height
    public const int uiMaxResThreshold = 1700; //should be checked against screen height
    public const int uiMedResThreshold = 1125; //should be checked against screen height
    //public const int uiMinResThreshold = 640; //should be checked against screen height

    public const float basePlayerMovementSpeed = 3f;
    public const int maxMedpacks = 3;
    public const int medpackHpAmount = 35;
    public const int armorPickupHp = 25;
    public const int maxArmor = 100;

    //photon player custom property keys
    public const string playerPropertyKey_chosenCharacter = "chosenCharacter";
    public const string playerPropertyKey_isHeadlessClient = "isHeadlessClient";

    //prefs keys
    public const string prefsKey_chosenCharacter = "chosenCharacter";
    public const string prefsKey_playerNickname = "playerNickname";
    public const string prefsKey_tutorialPassed = "tutorialPassed";
    public const string prefsKey_playerId = "playerId";
    public const string prefsKey_loginToken = "loginToken";
    public const string prefsKey_userEmail = "userEmail";

    //Photon event types
    public const byte eventCode_playerPickingUpPowerup = 1;
    public const byte eventCode_playerSwitchingWeapons = 2;
    public const byte eventCode_gameIsStarting = 3;
    public const byte eventCode_playerSendInterestGroupInfo = 4;
    public const byte eventCode_acknowledgePhotonViewInstantiated = 5;
    public const byte eventCode_locationAndRadiusUpdate = 6;
    public const byte eventCode_subscribedToYouNotification = 7; //This is for a player to tell another that he has subscribed to his (later) constant updates.
    public const byte eventCode_playerDroppedWeapon = 8;
    public const byte eventCode_playerOutOfAmmo = 9;
    public const byte eventCode_brSectorShouldStartPulsing = 10;
    public const byte eventCode_turnOnDamageColliders = 11;
    public const byte eventCode_reportDamage = 12;
    public const byte eventCode_reportDeath = 13;
    public const byte eventCode_sendProfileInfoToMasterClient = 14;

    //Item type id's - events will be sent that can apply to multiple types of items. Example: damage can be dealt to players, crates, etc
    public const byte item_type_id_character = 1;
    public const byte item_type_id_crate = 2;
    public const byte item_type_id_bot = 3;


}
