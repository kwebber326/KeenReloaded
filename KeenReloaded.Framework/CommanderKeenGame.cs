using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.KeenEventArgs;
using KeenReloaded.Framework.Weapons;
using KeenReloaded.Framework.Enemies;
using System.Diagnostics;
using KeenReloaded.Framework.Assets;
using KeenReloaded.Framework.Items;

namespace KeenReloaded.Framework
{
    public class CommanderKeenGame
    {
        public CommanderKeenGame(Map map)
        {
            _keysPressed = new Dictionary<string, bool>();
            _keysPressed.Add("Left", false);
            _keysPressed.Add("Right", false);
            _keysPressed.Add("Up", false);
            _keysPressed.Add("Down", false);
            _keysPressed.Add("ControlKey", false);
            _keysPressed.Add("Space", false);
            _keysPressed.Add("D1", false);
            _keysPressed.Add("D2", false);
            _keysPressed.Add("D3", false);
            _keysPressed.Add("D4", false);
            _keysPressed.Add("D5", false);
            _keysPressed.Add("D6", false);
            _keysPressed.Add("Return", false);
            _keysPressed.Add("ShiftKey", false);
            _keysPressed.Add("Menu", false);
            if (map != null && map.Objects != null)
                _keen = map.Objects.OfType<CommanderKeen>().FirstOrDefault();
            this.Map = map;
        }

        public event EventHandler<ObjectEventArgs> ObjectRemoved;
        public event EventHandler<ObjectEventArgs> ObjectCreated;
        private Dictionary<string, bool> _keysPressed;
        private CommanderKeen _keen;

        public Map Map
        {
            get;
            private set;
        }

        public void SetKeyPressed(string key, bool isPressed)
        {
            if (_keen != null)
                _keen.SetKeyPressed(key, isPressed);
        }

        public bool IsKeyPressed(string key)
        {
            if (_keen != null)
                return _keen.IsKeyPressed(key);

            return false;
        }

        public void UpdateGame()
        {
            if (this.Map != null && this.Map.Objects != null)
            {
                int length = this.Map.Objects.Count;
                for (int i = 0; i < length; i++)
                {
                    try
                    {
                        if (i < this.Map.Objects.Count)
                        {
                            var obj = this.Map.Objects[i];
                            obj.Update();
                        }
                    }

                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
            }
        }

        protected void OnObjectRemoved(ObjectEventArgs e)
        {
            if (ObjectRemoved != null)
                ObjectRemoved(this, e);
        }

        protected void OnObjectCreated(ObjectEventArgs e)
        {
            if (ObjectCreated != null)
                ObjectCreated(this, e);
        }

        public void RegisterItemEventsForObject(object obj)
        {
            if (obj is NeuralStunner)
            {
                var item = obj as NeuralStunner;
                item.CreatedObject += new EventHandler<ObjectEventArgs>(item_CreatedObject);
                item.RemovedObject += new EventHandler<ObjectEventArgs>(item_RemovedObject);
            }
            else if (obj is ThunderCloud)
            {
                var item = obj as ThunderCloud;
                item.BoltCreated += new EventHandler<ObjectEventArgs>(item_CreatedObject);
                item.BoltRemoved += new EventHandler<ObjectEventArgs>(item_RemovedObject);
            }
            else if (obj is ICreateRemove)
            {
                var item = obj as ICreateRemove;
                item.Create += new EventHandler<ObjectEventArgs>(item_CreatedObject);
                item.Remove += new EventHandler<ObjectEventArgs>(item_RemovedObject);
                if (obj is IFlag)
                {
                    var flag = (IFlag)obj;
                    flag.FlagCaptured += Flag_FlagCaptured;
                }
            }
        }

        public void DetachEventsForObject(object obj)
        {
            if (obj is NeuralStunner)
            {
                var item = obj as NeuralStunner;
                item.CreatedObject -= item_CreatedObject;
                item.RemovedObject -= item_RemovedObject;
            }
            else if (obj is ThunderCloud)
            {
                var item = obj as ThunderCloud;
                item.BoltCreated -= item_CreatedObject;
                item.BoltRemoved -= item_RemovedObject;
            }
            else if (obj is ICreateRemove)
            {
                var item = obj as ICreateRemove;
                item.Create -= item_CreatedObject;
                item.Remove -= item_RemovedObject;
                if (obj is IFlag)
                {
                    var flag = (IFlag)obj;
                    flag.FlagCaptured -= Flag_FlagCaptured;
                }
            }
        }

        private void Flag_FlagCaptured(object sender, FlagCapturedEventArgs e)
        {
            var flag = e.Flag;
            if (flag != null)
            {
                var newFlag = new Flag(this.Map.Grid, new System.Drawing.Rectangle(flag.LocationOfOrigin.X, flag.LocationOfOrigin.Y, flag.HitBox.Width, flag.HitBox.Height), flag.Color, flag.MaxPoints, flag.MinPoints, flag.PointsDegradedPerSecond, _keen);
                this.Map.Objects.Add(newFlag);
                this.RegisterItemEventsForObject(newFlag);

                OnObjectCreated(new ObjectEventArgs() { ObjectSprite = newFlag });
            }
        }

        void item_RemovedObject(object sender, ObjectEventArgs e)
        {
            ISprite trajectory = e.ObjectSprite as ISprite;
            if (trajectory != null)
            {
                ObjectEventArgs args = new ObjectEventArgs()
                {
                    ObjectSprite = trajectory
                };
                OnObjectRemoved(args);
            }
        }

        void item_CreatedObject(object sender, ObjectEventArgs e)
        {
            ISprite trajectory = e.ObjectSprite as ISprite;
            if (trajectory != null)
            {
                ObjectEventArgs args = new ObjectEventArgs()
                {
                    ObjectSprite = trajectory
                };
                OnObjectCreated(args);
            }

            IUpdatable obj = e.ObjectSprite as IUpdatable;
            if (obj != null)
            {
                bool isHill = obj is Hill;
                if (!isHill || !this.Map.Objects.Contains(obj))
                    this.Map.Objects.Add(obj);
            }
        }
    }
}
