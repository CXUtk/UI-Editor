using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEditor.UILib.Events;
using Terraria;
using Terraria.GameInput;

namespace UIEditor.UILib {
    public class UIStateMachine {
        public int ActiveStateNumber => uiRunningStack.Count;
        private List<UIState> uiRunningStack = new List<UIState>();
        private List<UIState> uiStates = new List<UIState>();


        private UIElement _previousHoverElement;
        private UIElement _lastLeftDownElement;
        private UIElement _lastRightDownElement;
        private UIElement _lastFocusElement;

        private bool _wasMouseLeftDown;
        private bool _wasMouseRightDown;

        private long _timer;
        private UIState _currentFocus;
        private string _tooltip;
        private bool _shouldDrawIME;

        public UIStateMachine() {
            _wasMouseLeftDown = false;
            _previousHoverElement = null;
            _timer = 0;
            _tooltip = "";
            Main.OnResolutionChanged += Main_OnResolutionChanged;
        }

        private void Main_OnResolutionChanged(Vector2 obj) {
            foreach (var state in uiRunningStack) state.ShouldRecalculate = true;
            RecalculateAll();
        }

        public void Add(UIState state) {
            uiStates.Add(state);
            state.TimeGetFocus = _timer;
            state.ShouldRecalculate = true;
            state.Recalculate();
        }

        public void Remove(UIState state) {
            uiRunningStack.Remove(state);
        }

        public void Toggle(string name) {
            var state = uiStates.Find((s) => s.Name == name);
            if (state == null) throw new ArgumentNullException();
            state.IsActive ^= true;
            if (state.IsActive)
                state.TimeGetFocus = _timer;
        }

        public void HandleMouseEvent(GameTime gameTime) {
            bool mouseLeftDown = Main.mouseLeft && Main.hasFocus;
            bool mouseRightDown = Main.mouseRight && Main.hasFocus;
            // 响应鼠标事件的时候一定是从后往前，前端的窗口一定是第一个响应鼠标事件的
            // 如果有多个前端窗口，那么优先响应当前焦点窗口
            UIElement hoverElement = null;
            int sz = uiRunningStack.Count;
            for (int i = sz - 1; i >= 0; i--) {
                var state = uiRunningStack[i];
                if (state.IsActive) {
                    var element = state.ElementAt(Main.MouseScreen);
                    if (element != state) {
                        hoverElement = element;
                        if (!_wasMouseLeftDown && mouseLeftDown) {
                            _currentFocus = state;
                            _currentFocus.TimeGetFocus = _timer;
                        }
                        break;
                    }
                }
            }
            // if (hoverElement != null) Main.NewText(hoverElement.ToString());
            // 鼠标点击移动事件
            if (hoverElement != null) {
                _tooltip = hoverElement.Tooltip;
                Main.LocalPlayer.mouseInterface = true;
                Main.LocalPlayer.releaseUseItem = true;
            }
            if (hoverElement != null && hoverElement != _previousHoverElement)
                hoverElement.MouseEnter(new UIMouseEvent(hoverElement, gameTime.TotalGameTime, Main.MouseScreen));
            if (_previousHoverElement != null && hoverElement != _previousHoverElement)
                _previousHoverElement.MouseOut(new UIMouseEvent(_previousHoverElement, gameTime.TotalGameTime, Main.MouseScreen));

            if (!_wasMouseLeftDown && mouseLeftDown) {
                if (hoverElement == null || _lastFocusElement != hoverElement) {
                    _lastFocusElement?.UnFocus(new UIActionEvent(_lastLeftDownElement, gameTime.TotalGameTime));
                }
            }


            // 鼠标左键
            if (!_wasMouseLeftDown && mouseLeftDown && hoverElement != null) {
                hoverElement.MouseDown(new UIMouseEvent(hoverElement, gameTime.TotalGameTime, Main.MouseScreen));
                hoverElement.FocusOn(new UIActionEvent(hoverElement, gameTime.TotalGameTime));
                _lastLeftDownElement = hoverElement;
                _lastFocusElement = hoverElement;
            }

            if (_wasMouseLeftDown && Main.mouseLeftRelease) {
                _lastLeftDownElement?.MouseUp(new UIMouseEvent(hoverElement, gameTime.TotalGameTime, Main.MouseScreen));
                if (_wasMouseLeftDown && Main.mouseLeftRelease && hoverElement != null && _lastLeftDownElement == hoverElement) {
                    hoverElement.MouseClick(new UIMouseEvent(hoverElement, gameTime.TotalGameTime, Main.MouseScreen));
                }
                _lastLeftDownElement = null;
            }


            // 鼠标右键
            if (!_wasMouseRightDown && mouseRightDown && hoverElement != null) {
                hoverElement.MouseRightDown(new UIMouseEvent(hoverElement, gameTime.TotalGameTime, Main.MouseScreen));
                _lastRightDownElement = hoverElement;
            }

            if (_wasMouseRightDown && Main.mouseRightRelease) {
                _lastRightDownElement?.MouseRightUp(new UIMouseEvent(hoverElement, gameTime.TotalGameTime, Main.MouseScreen));
                if (hoverElement != null && _lastRightDownElement == hoverElement)
                    hoverElement.MouseRightClick(new UIMouseEvent(hoverElement, gameTime.TotalGameTime, Main.MouseScreen));
                _lastRightDownElement = null;
            }


            // 滚轮
            if (PlayerInput.ScrollWheelDeltaForUI != 0)
                hoverElement?.ScrollWheel(new UIScrollWheelEvent(hoverElement, gameTime.TotalGameTime, PlayerInput.ScrollWheelDeltaForUI));


            _previousHoverElement = hoverElement;
            _wasMouseLeftDown = Main.mouseLeft;
            _wasMouseRightDown = Main.mouseRight;
        }

        public void Update(GameTime gameTime) {
            _tooltip = "";
            _timer++;

            ReorderRunningStack();

            if (Main.hasFocus) {
                HandleMouseEvent(gameTime);
            }
            foreach (var state in uiRunningStack) {
                if (state.IsActive) {
                    state.Update(gameTime);
                }
            }

            RecalculateAll();
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

        private void RecalculateAll() {
            foreach (var state in uiRunningStack) {
                if (state.IsActive) {
                    Recalculate(state);
                }
            }
        }

        private void Recalculate(UIElement element) {
            if (element.ShouldRecalculate)
                element.Recalculate();
            else {
                foreach (var child in element.Children) {
                    if (child.IsActive) {
                        Recalculate(child);
                    }
                }
            }
        }

        public void Draw(SpriteBatch sb) {
            _shouldDrawIME = false;
            // 绘制一定要从前往后，维持父子关系
            foreach (var state in uiRunningStack) {
                if (state.IsActive) {
                    state.Draw(sb);
                }
            }
            DrawIME();
            DrawTooltip();
        }

        public void SetIME(Vector2 pos) {
            _imePosition = pos;
            _shouldDrawIME = true;
        }
        private Vector2 _imePosition;
        private void DrawIME() {
            if (!_shouldDrawIME) return;
            Main.instance.DrawWindowsIMEPanel(_imePosition);
        }

        private void DrawTooltip() {
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
