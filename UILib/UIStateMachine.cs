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
        public UIElement FocusedElement { get { return _lastFocusElement; } }
        public UIElement LastRightClickElement { get { return _lastRightClickElement; } }
        private readonly List<UIState> uiRunningStack = new List<UIState>();
        private readonly List<UIState> uiStates = new List<UIState>();



        private UIElement _previousHoverElement;
        private UIElement _lastLeftDownElement;
        private UIElement _lastLeftClickElement;
        private UIElement _lastRightClickElement;
        private UIElement _lastRightDownElement;
        private UIElement _lastFocusElement;

        private double _lastLeftClickTime = 0;
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
                hoverElement.MouseLeftDown(new UIMouseEvent(hoverElement, gameTime.TotalGameTime, Main.MouseScreen));
                hoverElement.FocusOn(new UIActionEvent(hoverElement, gameTime.TotalGameTime));
                hoverElement.DragStart(new UIMouseEvent(hoverElement, gameTime.TotalGameTime, Main.MouseScreen));
                _lastLeftDownElement = hoverElement;
                _lastFocusElement = hoverElement;
            }

            if (_wasMouseLeftDown && Main.mouseLeftRelease) {
                _lastLeftDownElement?.MouseLeftUp(new UIMouseEvent(hoverElement, gameTime.TotalGameTime, Main.MouseScreen));
                _lastLeftDownElement?.DragEnd(new UIDragEndEvent(_lastLeftDownElement, hoverElement, gameTime.TotalGameTime, Main.MouseScreen));
                if (_wasMouseLeftDown && Main.mouseLeftRelease && hoverElement != null && _lastLeftDownElement == hoverElement) {
                    if (gameTime.TotalGameTime.TotalMilliseconds - _lastLeftClickTime > 200) {
                        hoverElement.MouseLeftClick(new UIMouseEvent(hoverElement, gameTime.TotalGameTime, Main.MouseScreen));
                    } else {
                        hoverElement.MouseDoubleClick(new UIMouseEvent(hoverElement, gameTime.TotalGameTime, Main.MouseScreen));
                    }
                    _lastLeftClickElement = hoverElement;
                    _lastLeftClickTime = gameTime.TotalGameTime.TotalMilliseconds;
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
                if (hoverElement != null && _lastRightDownElement == hoverElement) {
                    hoverElement.MouseRightClick(new UIMouseEvent(hoverElement, gameTime.TotalGameTime, Main.MouseScreen));
                    _lastRightClickElement = hoverElement;
                }
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
            DrawDragElement(sb);
            DrawIME();
            DrawTooltip();

        }

        public void DrawDragElement(SpriteBatch sb) {
            var mousePos = Main.MouseScreen;
            Drawing.DrawAdvBox(sb, new Rectangle((int)mousePos.X - 50, (int)mousePos.Y - 25, 100, 50),
                Color.Yellow, UIEditor.Instance.SkinManager.GetTexture("BoxFrame_Default"), new Vector2(4, 4));
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
                    Color.White * 0.75f, UIEditor.Instance.SkinManager.GetTexture("Box2_Default"), new Vector2(8, 8));
                Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, _tooltip, drawPos.X, drawPos.Y, Color.White, Color.Black, Vector2.Zero, 1f);
            }
        }
    }
}
