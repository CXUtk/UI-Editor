using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEditor.UILib.Events;
using Terraria;

namespace UIEditor.UILib {
    public class UIStateMachine {
        public int ActiveStateNumber => uiRunningStack.Count;
        private List<UIState> uiRunningStack = new List<UIState>();
        private List<UIState> uiStates = new List<UIState>();
        private UIElement _previousHoverElement;

        private bool _wasMouseDown;
        private UIElement _lastDownElement;
        private long _timer;
        private UIState _currentFocus;
        private string _tooltip;

        public UIStateMachine() {
            _wasMouseDown = false;
            _previousHoverElement = null;
            _timer = 0;
            _tooltip = "";
        }

        public void Add<T>(T state) where T : UIState {
            uiStates.Add(state);
            state.TimeGetFocus = _timer;
            state.Recalculate();
        }

        public void Remove<T>(T state) where T : UIState {
            uiRunningStack.Remove(state);
        }

        public void Update(GameTime gameTime) {
            _tooltip = "";
            _timer++;
            Recalculate();
            ReorderRunningStack();


            bool mouseLeftDown = Main.mouseLeft && Main.hasFocus;
            // 响应鼠标事件的时候一定是从后往前，前端的窗口一定是第一个响应鼠标事件的
            int sz = uiRunningStack.Count;
            UIElement hoverElement = null;
            for (int i = sz - 1; i >= 0; i--) {
                var state = uiRunningStack[i];
                if (state.IsActive) {
                    var element = state.ElementAt(Main.MouseScreen);
                    if (element != state) {
                        hoverElement = element;
                        if (!_wasMouseDown && mouseLeftDown) {
                            _currentFocus = state;
                            _currentFocus.TimeGetFocus = _timer;
                        }
                        break;
                    }
                }
            }
            if (hoverElement != null)
                _tooltip = hoverElement.Tooltip;
            if (hoverElement != null && hoverElement != _previousHoverElement)
                hoverElement.MouseEnter(new UIMouseEvent(hoverElement, gameTime.TotalGameTime, Main.MouseScreen));
            if (_previousHoverElement != null && hoverElement != _previousHoverElement)
                _previousHoverElement.MouseOut(new UIMouseEvent(_previousHoverElement, gameTime.TotalGameTime, Main.MouseScreen));

            if (!_wasMouseDown && mouseLeftDown && hoverElement != null) {
                hoverElement.MouseDown(new UIMouseEvent(hoverElement, gameTime.TotalGameTime, Main.MouseScreen));
                _lastDownElement = hoverElement;
            }
            if (_wasMouseDown && Main.mouseLeftRelease && hoverElement != null) {
                hoverElement.MouseUp(new UIMouseEvent(hoverElement, gameTime.TotalGameTime, Main.MouseScreen));
                if (_wasMouseDown && Main.mouseLeftRelease && hoverElement != null && _lastDownElement == hoverElement) {
                    hoverElement.MouseClick(new UIMouseEvent(hoverElement, gameTime.TotalGameTime, Main.MouseScreen));
                }
            }

            _previousHoverElement = hoverElement;
            foreach (var state in uiRunningStack) {
                if (state.IsActive) {
                    state.Update(gameTime);
                }
            }
            Recalculate();

            _wasMouseDown = Main.mouseLeft;
        }

        private void ReorderRunningStack() {
            uiRunningStack.Clear();
            uiStates.Sort();
            foreach (var state in uiStates) {
                if (state.IsActive) {
                    uiRunningStack.Add(state);
                }
            }
        }

        private void Recalculate() {
            foreach (var state in uiRunningStack) {
                if (state.IsActive) {
                    state.Recalculate();
                }
            }
        }

        public void Draw(SpriteBatch sb) {
            // 绘制一定要从前往后，维持父子关系
            foreach (var state in uiRunningStack) {
                if (state.IsActive) {
                    state.Draw(sb);
                }
            }

            if (_tooltip != "") {
                var size = Main.fontMouseText.MeasureString(_tooltip);
                var drawPos = new Vector2(Main.mouseX, Main.mouseY) + new Vector2(25f, 25f);
                if (drawPos.Y > Main.screenHeight - 30f)
                    drawPos.Y = Main.screenHeight - 30f;
                if (drawPos.X > Main.screenWidth - size.X)
                    drawPos.X = Main.screenWidth - size.X - 30.0f;
                Drawing.DrawAdvBox(Main.spriteBatch, (int)drawPos.X - 5, (int)drawPos.Y - 10, (int)size.X + 10, (int)size.Y + 10,
                    Color.White * 0.75f, Drawing.DefaultBox2Texture, new Vector2(8, 8));
                Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, _tooltip, drawPos.X, drawPos.Y, Color.White, Color.Black, Vector2.Zero, 1f);
            }
        }

        //public void RegisterState<T>() where T : UIState {
        //    var name = typeof(T).FullName;
        //    if (stateDict.ContainsKey(name)) throw new ArgumentException("这个状态已经注册过了");
        //    var state = (T)Activator.CreateInstance(typeof(T), new[] { this });
        //    uiStates.Add(state);
        //    stateDict.Add(name, uiStates.Count);
        //}
    }
}
