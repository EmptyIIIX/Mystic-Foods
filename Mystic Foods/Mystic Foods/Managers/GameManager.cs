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
        private readonly List<Food> _food = new();
        private readonly List<Filling> _fillings = new();
        private readonly List<Dough> _doughs = new();
        private readonly List<Wrapper> _wrapper = new();
        private readonly Dictionary<Filling, Vector2> _fillingOriginalPositions = new();
        private readonly Dictionary<Dough, Vector2> _doughOriginalPositions = new();
        private readonly Dictionary<Wrapper, Vector2> _wrapperOriginalPositions = new();
        private Socket _plate;
        private TrashBin _trashBin;
        private Filling _placedFilling;
        private Dough _placedDough;
        private readonly Vector2 _originSai = new Vector2(600, 200);
        private readonly Vector2 _originPang = new Vector2(300, 200);
        private readonly Vector2 _originWrapper = new Vector2(1362, 592);
        private readonly float _fillingSpacing = 350; // ระยะห่างระหว่าง Filling
        private readonly float _doughSpacing = 250; // ระยะห่างระหว่าง Dough

        public static int IdFood;
        public static int IdFilling; // IdFill = _filling.fillingType
        public static int IdDough;
        public static bool HasFood { get; private set; }
        public static int IsCurrectOrder = -1;

        public GameManager()
        {
            DragDropManager.OnDrop += HandleDrop;
            DragDropManager.OnDragFailed += HandleDragFailed;
            HasFood = false;
        }

        public void LoadContent(ContentManager content)
        {
            // โค้ดเดิม ไม่ต้องแก้ไข
            var foodTexture = content.Load<Texture2D>("foods/3");
            var plateTexture = content.Load<Texture2D>("foods/Plate");
            var cheeseTexture = content.Load<Texture2D>("foods/4");
            var meatTexture = content.Load<Texture2D>("foods/5");
            var vegetableTexture = content.Load<Texture2D>("foods/6");
            var wheatTexture = content.Load<Texture2D>("foods/7");
            var cornTexture = content.Load<Texture2D>("foods/8");
            var riceTexture = content.Load<Texture2D>("foods/9");
            var trashBinTexture = content.Load<Texture2D>("Etc/TrashBin");
            var wrappTexture = content.Load<Texture2D>("foods/1");

            var cheese = new Filling(cheeseTexture, _originSai, Filling.FillingType.Cheese);
            var meat = new Filling(meatTexture, new Vector2(_originSai.X + _fillingSpacing, _originSai.Y), Filling.FillingType.Meat);
            var vegetable = new Filling(vegetableTexture, new Vector2(_originSai.X + 2 * _fillingSpacing, _originSai.Y), Filling.FillingType.Vegetable);
            _fillings.Add(cheese);
            _fillings.Add(meat);
            _fillings.Add(vegetable);
            _fillingOriginalPositions.Add(cheese, _originSai);
            _fillingOriginalPositions.Add(meat, new Vector2(_originSai.X + _fillingSpacing, _originSai.Y));
            _fillingOriginalPositions.Add(vegetable, new Vector2(_originSai.X + 2 * _fillingSpacing, _originSai.Y));

            var wheat = new Dough(wheatTexture, _originPang, Dough.DoughType.Wheat);
            var corn = new Dough(cornTexture, new Vector2(_originPang.X, _originPang.Y + _doughSpacing), Dough.DoughType.Corn);
            var rice = new Dough(riceTexture, new Vector2(_originPang.X, _originPang.Y + 2 * _doughSpacing), Dough.DoughType.Rice);
            _doughs.Add(wheat);
            _doughs.Add(corn);
            _doughs.Add(rice);
            _doughOriginalPositions.Add(wheat, _originPang);
            _doughOriginalPositions.Add(corn, new Vector2(_originPang.X, _originPang.Y + _doughSpacing));
            _doughOriginalPositions.Add(rice, new Vector2(_originPang.X, _originPang.Y + 2 * _doughSpacing));

            var wrapper = new Wrapper(wrappTexture, _originWrapper);
            _wrapper.Add(wrapper);
            _wrapperOriginalPositions.Add(wrapper, _originWrapper);

            _plate = new Socket(plateTexture, new(858, 586));
            _trashBin = new TrashBin(trashBinTexture, new Vector2(1700, 500));
        }

        private void HandleDrop(IDraggable item, ITargetable target)
        {
            // ถ้ามีอาหารเกิน 1 ชิ้น ให้รีเซ็ตตำแหน่งของ sai, pang, wrapper
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
                    // รีเซ็ต Dough อื่นที่อยู่บนจาน
                    var otherDoughs = _doughs.Where(d => d != newDough && d.Position == _plate.Position).ToList();
                    foreach (var dough in otherDoughs)
                    {
                        dough.Position = _doughOriginalPositions[dough];
                    }
                }

                // ตรวจสอบการสร้าง Food
                var fillingOnPlate = _fillings.FirstOrDefault(f => f.Position == _plate.Position);
                var doughOnPlate = _doughs.FirstOrDefault(d => d.Position == _plate.Position);
                var wrapperOnPlate = _wrapper.FirstOrDefault(w => w.Position == _plate.Position);

                if (fillingOnPlate != null && doughOnPlate != null && wrapperOnPlate != null)
                {
                    CreateFood(fillingOnPlate, doughOnPlate, wrapperOnPlate);
                }
            }
            else if (target == _trashBin && item is Food food && _food.Contains(food))
            {
                _food.Remove(food);
                (food as IDraggable).UnregisterDraggable();
                IdFood = 0;
                HasFood = false;
            }
            else if (target == _trashBin && item is Filling filling)
            {
                filling.Position = _fillingOriginalPositions[filling];
                if (!_fillings.Any(f => f.Position == _plate.Position))
                {
                    IdFilling = 0;
                }
            }
            else if (target == _trashBin && item is Dough dough)
            {
                dough.Position = _doughOriginalPositions[dough];
                if (!_doughs.Any(d => d.Position == _plate.Position))
                {
                    IdDough = 0;
                }
            }
            else if (target == _trashBin && item is Wrapper wrapper)
            {
                wrapper.Position = _wrapperOriginalPositions[wrapper];
            }
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
            var foodTexture = Globals.Content.Load<Texture2D>("foods/3");
            var newFood = new Food(foodTexture, _plate.Position);
            _food.Add(newFood);

            // อัปเดต IdFilling, IdDough, และ IdFood
            IdFilling = (int)filling.FillingKind;
            IdDough = (int)dough.DoughKind;
            IdFood = IdFilling * IdDough;
            HasFood = true;

            // รีเซ็ตตำแหน่งของวัตถุดิบ
            int fillingIndex = _fillings.IndexOf(filling);
            int doughIndex = _doughs.IndexOf(dough);
            int wrapIndex = _wrapper.IndexOf(wrapper);
            filling.Position = new Vector2(_originSai.X + (fillingIndex * _fillingSpacing), _originSai.Y);
            dough.Position = new Vector2(_originPang.X, _originPang.Y + (doughIndex * _doughSpacing));
            wrapper.Position = new Vector2(_originWrapper.X, _originWrapper.Y);

            // รีเซ็ต IdFilling, IdDough, และ IdFood หลังสร้างอาหาร
            IdFilling = 0;
            IdDough = 0;
            GamePlayScene.TotalMoney -= 10;
        }

        public void ServeFood()
        {

            if(IdFood == GamePlayScene._currentCustomer.IdOrder)
            {
                IsCurrectOrder = 1;
                GamePlayScene.pay = GamePlayScene.price * GamePlayScene.weight;
                GamePlayScene.TotalMoney += GamePlayScene.pay;
            }
            else
            {
                IsCurrectOrder= 0;
            }
            if (_food.Count > 0)
            {
                _food.Clear();
                HasFood = false;
                IdFood = 0;
            }
        }

        public void Update()
        {
            InputManager.Update();
            DragDropManager.Update();
        }

        public void Draw()
        {
            _plate.Draw();
            _trashBin.Draw();
            foreach (var wrapper in _wrapper)
            {
                wrapper.Draw();
            }
            foreach (var dough in _doughs)
            {
                dough.Draw();
            }
            foreach (var filling in _fillings)
            {
                filling.Draw();
            }
            foreach (var food in _food)
            {
                food.Draw();
            }
        }
    }
}