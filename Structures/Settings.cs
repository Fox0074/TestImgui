using ClickableTransparentOverlay;
using ClickableTransparentOverlay.Win32;
using System.Numerics;

namespace DriverUserInterface.Structures
{
    public class Settings
    {
        public string AuthKey;
        public bool UseExperimentalVersion;
        public Language Lang = Language.Ru;
        public bool ShowDebug;
        public bool AimBot = false;
        public float AimFov = 100f;
        public bool AimActive = true;
        public VK AimButton = VK.LBUTTON;
        public bool LootIsDraw = false;
        public bool ShowContainers = true;
        public bool NoRecoil = false;
        public bool Stamina = false;
        public bool DrawBones = true;
        public bool DrawBoxes = false;
        public bool DrawNick = false;
        public bool DrawRadar = false;
        public float RadarMashtab = 1f;
        public Vector2 RadarOffset = new Vector2(0, 0);
        public bool DrawHealth = false;
        public bool ShowTemmates = false;
        public bool ShowUnknownItems = false;
        public bool DrawShadows = true;
        public bool ShowWeapon = false;
        public bool DrawExits = false;
        public bool ShowQuestItems = false;
        public bool LootInCorpse = false;
        public VK BattleModeButton = VK.NUMPAD0;

        public AimTarget AimTarget = AimTarget.Head;
        public Vector3 AimCustomOffset = new Vector3(0, 0, 1f);
        public Vector4 AimColor = new Vector4(1f, 0f, 0f, 1);
        public Vector4 UsecColor = new Vector4(1, 1, 0, 1);
        public Vector4 BearColor = new Vector4(1, 1, 0, 1);
        public Vector4 BotPlayersColor = new Vector4(1, 1, 0, 1);
        public Vector4 LootColor = new Vector4(.71f, .85f, 0, 1);
        public Vector4 RareLootColor = new Vector4(.88f, .30f, 0, 1);
        public Vector4 CorpseColor = new Vector4(.71f, 1f, 0.9f, 1);
        public Vector4 BotColor = new Vector4(.95f, .85f, .04f, 1);
        public Vector4 BossColor = new Vector4(.71f, .9f, .99f, 1);
        public Vector4 SvitaColor = new Vector4(.71f, .9f, .99f, 1);
        public Vector4 DefaultColor = new Vector4(1f, 1f, 1f, 1);
        public Vector4 ContainerColor = new Vector4(1f, 1f, 1f, 1);
        public Vector4 ExitColor = new Vector4(1f, 1f, 1f, 1);

        public float DrawDistance = 200f;
        public float LootDrawDistance = 50f;
        public float MinLootPrice_k = 20f;
        public bool ShowItemsWithoutCost = true;
        public List<string> LootFilter = new List<string>();
        public List<string> RareLootFilter = new List<string>();

        public bool UseAutoUpdateJson = true;
        public DateTime LastLootJsonUpdateTime;
    }

    public enum AimTarget { Custom, Head, RArm, LArm, LFoot, RFoot, Chest, Belly }
}
