using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Mystic_Foods.Systems;

namespace Mystic_Foods.Managers
{
    public class GameManager
    {
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private CustomerManager _customerManager;
        private Sprite _sprite;

        public List<Food> _food = new();
        private readonly List<Filling> _fillings = new();
        private readonly List<Dough> _doughs = new();
        private readonly List<Wrapper> _wrapper = new();
        private readonly Dictionary<Filling, Vector2> _fillingOriginalPositions = new();
        private readonly Dictionary<Dough, Vector2> _doughOriginalPositions = new();
        private readonly Dictionary<Wrapper, Vector2> _wrapperOriginalPositions = new();
        private Socket _plate, _plate2, _steam1;
        private TrashBin _trashBin, _trashBin2;
        private Filling _placedFilling;
        private Dough _placedDough;
        public Vector2 _originSai = new Vector2(759 + 283 / 2, 218 + 154 / 2);
        public Vector2 _originPang = new Vector2(371 + 280 / 2, 215 + 183 / 2);
        private readonly Vector2 _originWrapper = new Vector2(1566, 635);
        private readonly float _fillingSpacing = 299;
        private readonly float _doughSpacing = 203;

        public static int IdFood;
        public static int IdFilling;
        public static int IdDough;
        public static bool HasFood { get; private set; }
        public static bool isChangeFood;
        public static bool readySteam = false;
        public static int countDia = 2;
        public static float countSteam = 3f;

        public GameManager()
        {
            DragDropManager.OnDrop += HandleDrop;
            DragDropManager.OnDragFailed += HandleDragFailed;
            HasFood = false;
        }

        public void LoadContent(ContentManager content)
        {
            #region load assets
            var Coconut_AmberTexture = content.Load<Texture2D>("foods/Coconut Amber");
            var Pandan_Taro_CreamTexture = content.Load<Texture2D>("foods/Pandan Taro Cream");
            var Lotus_Root_SpiritTexture = content.Load<Texture2D>("foods/Lotus Root Spirit");

            var Jasmine_MoonTexture = content.Load<Texture2D>("foods/Jasmine Moon");
            var Lotus_BlossomTexture = content.Load<Texture2D>("foods/Lotus Blossom");
            var Golden_MoonTexture = content.Load<Texture2D>("foods/Golden Moon");
            var Jasmine_MoonPlate = content.Load<Texture2D>("foods/12");
            var Lotus_BlossomPlate = content.Load<Texture2D>("foods/10");
            var Golden_MoonPlate = content.Load<Texture2D>("foods/11");

            var plateTexture = content.Load<Texture2D>("foods/Plate");
            var trashBinTexture = content.Load<Texture2D>("Etc/TrashBin");
            var wrappTexture = content.Load<Texture2D>("foods/1");
            var foodTexture = content.Load<Texture2D>("foods/3");

            var steam1Texture = content.Load<Texture2D>("Environments/tools/steamer1");
            #endregion

            #region add ingredient to list
            var Coconut_Amber = new Filling(Coconut_AmberTexture, _originSai, Filling.FillingType.Coconut_Amber);
            var Pandan_Taro_Cream = new Filling(Pandan_Taro_CreamTexture, new Vector2(_originSai.X + _fillingSpacing, _originSai.Y), Filling.FillingType.Pandan_Taro_Cream);
            var Lotus_Root_Spirit = new Filling(Lotus_Root_SpiritTexture, new Vector2(_originSai.X + 2 * _fillingSpacing, _originSai.Y), Filling.FillingType.Lotus_Root_Spirit);
            _fillings.Add(Coconut_Amber);
            _fillings.Add(Pandan_Taro_Cream);
            _fillings.Add(Lotus_Root_Spirit);
            _fillingOriginalPositions.Add(Coconut_Amber, _originSai);
            _fillingOriginalPositions.Add(Pandan_Taro_Cream, new Vector2(_originSai.X + _fillingSpacing, _originSai.Y));
            _fillingOriginalPositions.Add(Lotus_Root_Spirit, new Vector2(_originSai.X + 2 * _fillingSpacing, _originSai.Y));

            var Jasmine_Moon = new Dough(Jasmine_MoonTexture, Jasmine_MoonPlate, _originPang, Dough.DoughType.Jasmine_Moon);
            var Lotus_Blossom = new Dough(Lotus_BlossomTexture, Lotus_BlossomPlate, new Vector2(_originPang.X, _originPang.Y + _doughSpacing), Dough.DoughType.Lotus_Blossom);
            var Golden_Moon = new Dough(Golden_MoonTexture, Golden_MoonPlate, new Vector2(_originPang.X, _originPang.Y + 2 * _doughSpacing), Dough.DoughType.Golden_Moon);
            _doughs.Add(Jasmine_Moon);
            _doughs.Add(Lotus_Blossom);
            _doughs.Add(Golden_Moon);
            _doughOriginalPositions.Add(Jasmine_Moon, _originPang);
            _doughOriginalPositions.Add(Lotus_Blossom, new Vector2(_originPang.X, _originPang.Y + _doughSpacing));
            _doughOriginalPositions.Add(Golden_Moon, new Vector2(_originPang.X, _originPang.Y + 2 * _doughSpacing));

            var wrapper = new Wrapper(wrappTexture, _originWrapper);
            _wrapper.Add(wrapper);
            _wrapperOriginalPositions.Add(wrapper, _originWrapper);

            #endregion

            _plate = new Socket(plateTexture, new(1068, 639));
            _plate2 = new Socket(plateTexture, new(4200 - 958, 644));
            _steam1 = new Socket(steam1Texture, new(1800 + (steam1Texture.Width / 2), 460));
            _trashBin = new TrashBin(trashBinTexture, new Vector2(160, 800));
            _trashBin2 = new TrashBin(trashBinTexture, new Vector2(4000, 800));
        }
        private void HandleDrop(IDraggable item, ITargetable target)
        {
            #region check count of food
            //ถ้ามีอาหารเกิน 1 ชิ้น ให้รีเซ็ตตำแหน่งของ sai, pang, wrapper
            if (_food.Count >= 1 && (item is Filling || item is Dough || item is Wrapper))
            {
                if (item is Filling filling)
                {
                    filling.Position = _fillingOriginalPositions[filling];
                }
                else if (item is Dough dough)
                {
                    dough.Position = _doughOriginalPositions[dough];
                }
                else if (item is Wrapper wrapper)
                {
                    wrapper.Position = _wrapperOriginalPositions[wrapper];
                }
                return;
            }
            #endregion

            #region Plate
            if (target == _plate)
            {
                if (item is Filling newFilling)
                {
                    // อัปเดต _placedFilling และ IdFilling
                    _placedFilling = newFilling;
                    IdFilling = (int)newFilling.FillingKind;
                    // รีเซ็ต Filling อื่นที่อยู่บนจาน
                    var otherFillings = _fillings.Where(f => f != newFilling && f.Position == _plate.Position).ToList();
                    foreach (var filling in otherFillings)
                    {
                        filling.Position = _fillingOriginalPositions[filling];
                    }
                }
                else if (item is Dough newDough)
                {
                    // อัปเดต _placedDough และ IdDough
                    _placedDough = newDough;
                    IdDough = (int)newDough.DoughKind;
                    newDough.SetOnPlate(true);
                    // รีเซ็ต Dough อื่นที่อยู่บนจาน
                    var otherDoughs = _doughs.Where(d => d != newDough && d.Position == _plate.Position).ToList();
                    foreach (var dough in otherDoughs)
                    {
                        dough.Position = _doughOriginalPositions[dough];
                        dough.SetOnPlate(false);
                    }
                }

                // ตรวจสอบการสร้าง Food
                var fillingOnPlate = _fillings.FirstOrDefault(f => f.Position == _plate.Position);
                var doughOnPlate = _doughs.FirstOrDefault(d => d.Position == _plate.Position);
                var wrapperOnPlate = _wrapper.FirstOrDefault(w => w.Position == _plate.Position);

                if (fillingOnPlate != null && doughOnPlate != null && wrapperOnPlate != null)
                {
                    CreateFood(fillingOnPlate, doughOnPlate, wrapperOnPlate);
                    isChangeFood = false;
                }
            }
            #endregion

            #region Plate_2//สำหรับของตกแต่ง
            //if (target == _plate2)
            //{

            //}
            #endregion

            #region steamer
            else if (target == _steam1)
            {

                if (item is Food food && isChangeFood == false)
                {
                    readySteam = true;
                    DragDropManager.RemoveDraggable(food);
                    //System.Diagnostics.Debug.WriteLine($"Food placed on steam1: {food}, readySteam={readySteam}");

                }

            }
            #endregion

            #region TrashBin
            else if ((target == _trashBin || target == _trashBin2) && item is Food food && _food.Contains(food))
            {
                _food.Remove(food);
                DragDropManager.RemoveDraggable(food);
                IdFood = 0;
                HasFood = false;
                isChangeFood = false;
                readySteam = false;
                countSteam = 3f;
                DnDScene.isCountDownSteam = false;
            }
            else if ((target == _trashBin || target == _trashBin2) && item is Food changeFood && _food.Contains(changeFood))
            {
                _food.Remove(changeFood);
                (changeFood as IDraggable).UnregisterDraggable();
                HasFood = false;
                isChangeFood = false;
                DnDScene.isClickCook = false;
            }
            else if ((target == _trashBin || target == _trashBin2) && item is Filling filling)
            {
                filling.Position = _fillingOriginalPositions[filling];
                if (!_fillings.Any(f => f.Position == _plate.Position))
                {
                    IdFilling = 0;
                }
            }
            else if ((target == _trashBin || target == _trashBin2) && item is Dough dough)
            {
                dough.Position = _doughOriginalPositions[dough];
                dough.SetOnPlate(false);
                if (!_doughs.Any(d => d.Position == _plate.Position))
                {
                    IdDough = 0;
                }
            }
            else if ((target == _trashBin || target == _trashBin2) && item is Wrapper wrapper)
            {
                wrapper.Position = _wrapperOriginalPositions[wrapper];
            }
            #endregion
        }
        private void HandleDragFailed(IDraggable item)
        {
            if (item is Filling filling)
            {
                filling.Position = _fillingOriginalPositions[filling];
                if (!_fillings.Any(f => f.Position == _plate.Position))
                {
                    IdFilling = 0;
                }
            }
            else if (item is Dough dough)
            {
                dough.Position = _doughOriginalPositions[dough];
                dough.SetOnPlate(false);
                if (!_doughs.Any(d => d.Position == _plate.Position))
                {
                    IdDough = 0;
                }
            }
            else if (item is Wrapper wrapper)
            {
                wrapper.Position = _wrapperOriginalPositions[wrapper];
            }
        }
        private void CreateFood(Filling filling, Dough dough, Wrapper wrapper)
        {
            // อัปเดต IdFilling, IdDough, และ IdFood
            IdFilling = (int)filling.FillingKind;
            IdDough = (int)dough.DoughKind;
            IdFood = IdFilling * IdDough;

            // รีเซ็ตตำแหน่งของวัตถุดิบ
            int fillingIndex = _fillings.IndexOf(filling);
            int doughIndex = _doughs.IndexOf(dough);
            int wrapIndex = _wrapper.IndexOf(wrapper);
            filling.Position = new Vector2(_originSai.X + (fillingIndex * _fillingSpacing), _originSai.Y);
            dough.Position = new Vector2(_originPang.X, _originPang.Y + (doughIndex * _doughSpacing));
            wrapper.Position = new Vector2(_originWrapper.X, _originWrapper.Y);

            //สร้าง food
            var foodTexture = Globals.Content.Load<Texture2D>("foods/3");
            var newFood = new Food(foodTexture, _plate.Position);
            _food.Add(newFood);

            // รีเซ็ต IdFilling, IdDough, และ IdFood หลังสร้างอาหาร
            IdFilling = 0;
            IdDough = 0;
            GamePlayScene.TotalMoney -= 10;
            dough.SetOnPlate(false);
        }
        public void ChangeFood(Food food)
        {
            //delete the food
            _food.Remove(food);
            DragDropManager.RemoveDraggable(food);

            //change asset from food to changeFood
            var foodTexture = Globals.Content.Load<Texture2D>("foods/2");
            var changeFood = new Food(foodTexture, _steam1.Position);
            DragDropManager.AddDraggable(changeFood);
            _food.Add(changeFood);

            IdFood += 10;
            isChangeFood = true;
            HasFood = true;
            readySteam = false;
            countSteam = 3f;
            DnDScene.isCountDownSteam = false;
        }
        public void ServeFood()
        {
            if(IdFood == GamePlayScene._currentCustomer.IdOrder)
            {
                countDia = 1;
                GamePlayScene.pay = GamePlayScene.price * GamePlayScene.weight;
                GamePlayScene.TotalMoney += GamePlayScene.pay;
            }
            else
            {
                countDia = 0;
            }

            foreach (var food in _food.ToList())
            {
                _food.Remove(food);
                DragDropManager.RemoveDraggable(food);
            }

            _food.Clear();
            HasFood = false;
            isChangeFood = false;
            readySteam = false;
            IdFood = 0;
            countSteam = 3f;
            DnDScene.isCountDownSteam = false;
            DnDScene.isClickCook = false;
        }
        public void Update()
        {
            InputManager.Update();
            DragDropManager.Update();
        }
        public void Draw(Vector2 cameraPos)
        {
            _plate.Draw(cameraPos);
            _plate2.Draw(cameraPos);
            _trashBin.Draw(cameraPos);
            _trashBin2.Draw(cameraPos);
            _steam1.Draw(cameraPos);
            foreach (var wrapper in _wrapper)
            {
                wrapper.Draw(cameraPos);
            }
            foreach (var dough in _doughs)
            {
                dough.Draw(cameraPos);
            }
            foreach (var filling in _fillings)
            {
                filling.Draw(cameraPos);
            }
            foreach (var food in _food)
            {
                food.Draw(cameraPos);
            }
        }
    }
}