using SixLabors.ImageSharp;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;

namespace Common.Structures
{
    //T1 = entity, T2 = enemy
    public class BaseGameManager<T1,T2> where T1 : BaseEntity where T2 : BaseEntity
    {
        public KernalInterface Driver;
        public BaseTableManager<T1> LootManager;
        public BasePlayer Player;
        public BaseTableManager<T2> EnemyManager;
        public List<string> WhiteNameList = new List<string>();
        public List<string> BlackNameList = new List<string>();
        public bool EnableNameFilter;
        public bool EnableLootDraw;
        public float AutoUpdateTimeout;
        public int Width = 1920;
        public int Height = 1080;
        public float DrawDistance;

        protected List<Rectangle> _textRects = new List<Rectangle>();

        public BaseGameManager(KernalInterface driver)
        {
            Driver = driver;

            EnableNameFilter = true;
        }

        public virtual void Loop()
        {
            UpdatePlayerPosition();
            UpdateCameraMatrix();
            EnemyManager.UpdateData();

            DrawEntities(EnemyManager.TableItems as List<BaseEntity>);

            if (EnableLootDraw)
            {
                DrawEntities(LootManager.TableItems as List<BaseEntity>);
            }

            if (AutoUpdateTimeout != 0)
                Thread.Sleep((int)(AutoUpdateTimeout * 1000));
        }

        protected virtual void DrawEntities(List<BaseEntity> Entities)
        {

        }

        public virtual bool CheckFilter(string entityName)
        {
            if (!EnableNameFilter)
                return true;

            if (WhiteNameList.Count == 0)
            {
                if (BlackNameList.Count == 0)
                    return true;
                else
                    return !BlackNameList.Contains(entityName);

            }


            return WhiteNameList.Any(x => x.Contains(entityName));
        }

        public virtual void UpdateGamePointers()
        {

        }

        public virtual void UpdatePlayerPosition()
        {
            
        }

        public virtual void UpdateCameraMatrix()
        {
            Player.Camera.CameraMatrix = Driver.ReadVirtualMemory<Matrix4x4>(Player.Camera.Adress);
        }

        protected virtual Color GetColorByType(BaseEntityType type)
        {
            switch (type)
            {
                case BaseEntityType.Player:
                    return Color.Red;
                case BaseEntityType.Bot:
                    return Color.Blue;
                case BaseEntityType.LootItem:
                    return Color.Yellow;
                default:
                    return Color.Purple;
            }
        }
    }
}
