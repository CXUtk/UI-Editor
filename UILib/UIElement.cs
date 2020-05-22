using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEditor.UILib.Events;
using UIEditor.UILib.Hitbox;
using Terraria;
using Newtonsoft.Json;
using UIEditor.Editor.Attributes;
using UIEditor.UILib.Enums;

namespace UIEditor.UILib {
    public class UIElement : ICloneable {
        public delegate void MouseEvent(UIMouseEvent e, UIElement sender);
        public delegate void ScrollEvent(UIScrollWheelEvent e, UIElement sender);
        public delegate void ActionEvent(UIActionEvent e, UIElement sender);
        public delegate void DrawEvent(UIDrawEvent e, UIElement sender);
        public delegate void DragStartEvent(UIMouseEvent e, UIElement sender);
        public delegate void DragEndEvent(UIDragEndEvent e, UIElement sender);
        public delegate void ValueChangeEvent<T>(UIValueChangeEvent<T> e, UIElement sender);

        public static bool DEBUG_MODE = true;


        #region 基础属性
        /// <summary>
        /// UI元素是否处于激活状态，如果不激活则不会显示也不会响应任何事件
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// UI元素是否能见，如果不能见就不绘制，但是会响应事件
        /// </summary>
        public bool IsVisible { get; set; }

        [JsonIgnore]
        /// <summary>
        /// 该UI节点的父节点
        /// </summary>
        public UIElement Parent { get; set; }

        [EditorPropertyIgnore]
        /// <summary>
        /// 该UI节点的直接子节点
        /// </summary>
        public List<UIElement> Children { get; set; }

        /// <summary>
        /// 该UI节点的基准点位置，计算位置旋转等时会以此位置为原点，
        /// X和Y的值一般为0到1的浮点数，代表节点的比例位置
        /// </summary>
        public Vector2 Pivot { get { return _pivot; } set { CheckRecalculate(_pivot, value); _pivot = value; } }
        private Vector2 _pivot;

        /// <summary>
        /// UI元素的锚点，也就是其基准点相对于父节点的位置，
        /// X和Y的值一般为0到1的浮点数，代表父节点的比例位置
        /// </summary>
        public Vector2 AnchorPoint { get { return _anchorPoint; } set { CheckRecalculate(_anchorPoint, value); _anchorPoint = value; } }
        private Vector2 _anchorPoint;

        /// <summary>
        /// 该UI元素的宽度，高度
        /// </summary>
        public Vector2 Size { get { return _size; } set { CheckRecalculate(_size, value); _size = value; } }
        private Vector2 _size;

        /// <summary>
        /// 该UI元素相对于父节点的宽度，高度
        /// </summary>
        public Vector2 SizeFactor { get { return _sizeFactor; } set { CheckRecalculate(_sizeFactor, value); _sizeFactor = value; } }
        private Vector2 _sizeFactor;

        /// <summary>
        /// 该UI元素与于自身锚点的相对位置
        /// </summary>
        public Vector2 Position { get { return _position; } set { CheckRecalculate(_position, value); _position = value; } }
        private Vector2 _position;

        /// <summary>
        /// UI元素绕基准点旋转的弧度，注意，如果设置了旋转，就不要设置溢出隐藏了
        /// </summary>
        public float Rotation { get { return _rotation; } set { CheckRecalculate(_rotation, value); _rotation = value; } }
        private float _rotation;

        /// <summary>
        /// UI元素的放大倍率
        /// </summary>
        public Vector2 Scale { get { return _scale; } set { CheckRecalculate(_scale, value); _scale = value; } }
        private Vector2 _scale;

        /// <summary>
        /// UI元素的名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// UI元素的溢出行为
        /// </summary>
        public OverflowType Overflow { get; set; }

        /// <summary>
        /// UI元素是否阻止事件向其父元素传播
        /// </summary>
        public PropagationFlags PropagationRule { get; set; }

        /// <summary>
        /// 这个UI元素是否会响应事件
        /// </summary>
        public bool NoEvent { get; set; }


        /// <summary>
        /// 鼠标移动上去时候显示的说明文字
        /// </summary>
        public string Tooltip { get; set; }

        /// <summary>
        /// 标记该节点或容器是否处于用户焦点
        /// </summary>
        [EditorPropertyIgnore]
        [JsonIgnore]
        public bool IsFocused { get; set; }

        /// <summary>
        /// 标记该节点以及其子节点是否应该重新计算位置
        /// </summary>
        [EditorPropertyIgnore]
        [JsonIgnore]
        public bool ShouldRecalculate { get; set; }

        /// <summary>
        /// 标记该节点是否处于Preview模式，如果处于这个模式就会在编辑器的Viewer里特殊处理
        /// </summary>
        [EditorPropertyIgnore]
        [JsonIgnore]
        internal bool IsPreview { get; set; }

        /// <summary>
        /// 标记该元素是否被某些容器选中
        /// </summary>
        [EditorPropertyIgnore]
        [JsonIgnore]
        internal bool IsSelected { get; set; }

        /// <summary>
        /// 如果需要使用着色器，会更改这个元素的绘制模式，会严重影响性能
        /// </summary>
        [EditorPropertyIgnore]
        [JsonIgnore]
        public bool UseShader { get; set; }

        //public int MarginLeft { get; set; }
        //public int MarginRight { get; set; }
        //public int MarginTop { get; set; }
        //public int MarginBottom { get; set; }


        //public int PaddingLeft { get; set; }
        //public int PaddingRight { get; set; }
        //public int PaddingTop { get; set; }
        //public int PaddingBottom { get; set; }
        #endregion


        #region 事件
        public event MouseEvent OnMouseEnter;
        public event MouseEvent OnMouseOut;
        public event MouseEvent OnMouseDown;
        public event MouseEvent OnMouseUp;
        public event MouseEvent OnClick;
        public event MouseEvent OnDoubleClick;
        public event MouseEvent OnMouseRightDown;
        public event MouseEvent OnMouseRightUp;
        public event MouseEvent OnRightClick;
        public event ScrollEvent OnScrollWheel;
        public event ActionEvent OnFocused;
        public event ActionEvent OnUnFocused;
        public event DrawEvent PostDrawSelf;
        public event MouseEvent OnDragStart;
        public event DragEndEvent OnDragEnd;
        #endregion


        #region 派生属性
        [EditorPropertyIgnore]
        [JsonIgnore]
        public Rectangle OuterRectangleScreen {
            get {
                return _selfHitbox.GetOuterRectangle();
            }
        }
        [EditorPropertyIgnore]
        [JsonIgnore]
        public Rectangle BaseRectangleScreen {
            get {
                return new Rectangle((int)(_baseTopLeftScreen.X), (int)(_baseTopLeftScreen.Y), Width, Height);
            }
        }
        [EditorPropertyReadOnly]
        [JsonIgnore]
        public Rectangle InnerRectangleScreen {
            get {
                return new Rectangle((int)(_baseTopLeftScreen.X), (int)(_baseTopLeftScreen.Y), Width, Height);
            }
        }

        [EditorPropertyReadOnly]
        [JsonIgnore]
        public int Width {
            get {
                return (int)(SizeFactor.X * _parentRect.Width + Size.X);
            }
        }

        [EditorPropertyReadOnly]
        [JsonIgnore]
        public int Height {
            get {
                return (int)(SizeFactor.Y * _parentRect.Height + Size.Y);
            }
        }
        /// <summary>
        /// 如果鼠标位置在这个UI元素上面
        /// </summary>
        [EditorPropertyIgnore]
        [JsonIgnore]
        public bool IsMouseHover { get; private set; }

        [JsonIgnore]
        public Vector2 PivotOffset {
            get {
                return new Vector2(Width * Pivot.X, Height * Pivot.Y);
            }
        }

        /// <summary>
        /// 把屏幕坐标转化为相对于这个UI元素的子节点的坐标，可以指定锚点
        /// </summary>
        /// <param name="worldPos"></param>
        /// <param name="anchor"></param>
        /// <returns></returns>
        public Vector2 ScreenPositionToNodeAR(Vector2 worldPos, Vector2 anchor) {
            return worldPos - (_baseTopLeftScreen + new Vector2(Width * anchor.X, Height * anchor.Y));
        }

        /// <summary>
        ///  把屏幕坐标转化为相对于这个UI元素的父节点的子节点坐标，并且可以指定锚点相对位置
        /// </summary>
        /// <param name="worldPos"></param>
        /// <param name="anchor"></param>
        /// <returns></returns>
        public Vector2 ScreenPositionToParentAR(Vector2 worldPos) {
            if (Parent != null) return Parent.ScreenPositionToNodeAR(worldPos, AnchorPoint);
            return worldPos;
        }

        /// <summary>
        /// 把屏幕坐标转化为为相对于这个UI元素的父节点的子节点坐标，锚点位置默认是左上角
        /// </summary>
        /// <param name="worldPos"></param>
        /// <returns></returns>
        public Vector2 ScreenPositionToParent(Vector2 worldPos) {
            if (Parent != null) return Parent.ScreenPositionToNodeAR(worldPos, new Vector2(0, 0));
            return worldPos;
        }

        /// <summary>
        /// 把相对于基准点的位置转化为屏幕位置
        /// </summary>
        /// <param name="nodePos"></param>
        /// <returns></returns>
        public Vector2 NodePositionToScreenAR(Vector2 nodePos) {
            return PositionScreen + nodePos;
        }

        /// <summary>
        /// 把基准点相对于父节点的位置转化为屏幕位置
        /// </summary>
        /// <param name="nodePos"></param>
        /// <returns></returns>
        public Vector2 ParentNodePositionToScreenAR(Vector2 nodePos) {
            return _parentRect.TopLeft() + new Vector2(Width * AnchorPoint.X, Height * AnchorPoint.Y) + nodePos;
        }
        //public Vector2 NodePositionToScreen(Vector2 worldPos) {
        //    return _baseTopLeftScreen - PivotOffset;
        //}

        [JsonIgnore]
        public IHitBox ScreenHitBox {
            get {
                return _selfHitbox;
            }
        }

        /// <summary>
        /// 基准点位置相对于屏幕的位置
        /// </summary>
        [JsonIgnore]
        public Vector2 PositionScreen {
            get {
                return _baseTopLeftScreen + PivotOffset;
            }
            set {
                Position = ScreenPositionToParentAR(value);
            }
        }

        [JsonIgnore]
        public Vector2 TopLeft {
            get {
                return Position - PivotOffset;
            }
            set {
                Position = value + PivotOffset;
            }
        }

        protected bool MouseDownedLeft { get; private set; }

        protected int MouseDownTimeLeft { get; private set; }
        #endregion



        private Vector2 getBaseRectScreen() {
            Vector2 pos = _parentRect.TopLeft();
            pos += _parentRect.Size() * AnchorPoint;
            return pos;
        }
        public void RecalculateLocation() {
            _parentRect = Parent?.InnerRectangleScreen ?? new Rectangle(0, 0, Main.screenWidth, Main.screenHeight);
            _baseTopLeftScreen = getBaseRectScreen() + Position - PivotOffset;
            _realPosition = (Parent == null) ? Position : new Vector2(Parent.Width, Parent.Height) * AnchorPoint
                + Position - new Vector2(Width * Pivot.X, Height * Pivot.Y);
        }
        public void RecalculateSelf() {
            RecalculateLocation();
            _selfTransform = Main.UIScaleMatrix;
            if (Parent != null) _selfTransform = Parent._selfTransform;
            _selfTransform = ApplyTransform(_selfTransform);
            _selfHitbox.Reset(Width, Height);
            _selfHitbox.Transform(_selfTransform);
        }


        public virtual void RecalculateChildren() {
            foreach (var element in Children) {
                if (element.IsActive) {
                    element.Recalculate();
                }
            }
        }
        protected void CheckRecalculate(object prev, object post) {
            if (!prev.Equals(post)) ShouldRecalculate = true;
        }

        public virtual void Recalculate() {
            RecalculateSelf();
            ShouldRecalculate = false;
            RecalculateChildren();
        }


        private Vector2 _baseTopLeftScreen;
        private Vector2 _realPosition;
        private readonly QuadrilateralHitbox _selfHitbox;
        private Matrix _selfTransform;
        private Rectangle _parentRect;
        private static readonly RasterizerState OverflowHiddenRasterizerState = new RasterizerState {
            CullMode = CullMode.None,
            ScissorTestEnable = true
        };


        #region 事件
        public virtual void MouseEnter(UIMouseEvent e) {
            // Main.NewText("进入");
            IsMouseHover = true;
            OnMouseEnter?.Invoke(e, this);
            if (PropagationRule.HasFlag(PropagationFlags.MouseEnter))
                Parent?.MouseEnter(e);
        }

        public virtual void MouseOut(UIMouseEvent e) {
            //Main.NewText("离开");
            IsMouseHover = false;
            OnMouseOut?.Invoke(e, this);
            if (PropagationRule.HasFlag(PropagationFlags.MouseOut))
                Parent?.MouseOut(e);
        }

        public virtual void MouseLeftDown(UIMouseEvent e) {
            //Main.NewText("按下");
            MouseDownedLeft = true;
            OnMouseDown?.Invoke(e, this);
            if (PropagationRule.HasFlag(PropagationFlags.MouseLeftDown))
                Parent?.MouseLeftDown(e);
        }

        public virtual void MouseRightDown(UIMouseEvent e) {
            //Main.NewText("右键按下");
            OnMouseRightDown?.Invoke(e, this);
            if (PropagationRule.HasFlag(PropagationFlags.MouseRightDown))
                Parent?.MouseRightDown(e);
        }

        public virtual void MouseLeftUp(UIMouseEvent e) {
            // Main.NewText("抬起");
            MouseDownedLeft = false;
            OnMouseUp?.Invoke(e, this);
            if (PropagationRule.HasFlag(PropagationFlags.MouseLeftUp))
                Parent?.MouseLeftUp(e);
        }

        public virtual void MouseRightUp(UIMouseEvent e) {
            // Main.NewText("右键抬起");
            OnMouseRightUp?.Invoke(e, this);
            if (PropagationRule.HasFlag(PropagationFlags.MouseRightUp))
                Parent?.MouseRightUp(e);
        }
        public virtual void MouseLeftClick(UIMouseEvent e) {
            //Main.NewText("点击");
            OnClick?.Invoke(e, this);
            if (PropagationRule.HasFlag(PropagationFlags.MouseLeftClick))
                Parent?.MouseLeftClick(e);
        }
        public virtual void MouseDoubleClick(UIMouseEvent e) {
            //Main.NewText("点击");
            OnDoubleClick?.Invoke(e, this);
            if (PropagationRule.HasFlag(PropagationFlags.MouseLeftDouble))
                Parent?.MouseDoubleClick(e);
        }
        public virtual void MouseRightClick(UIMouseEvent e) {
            //Main.NewText("点击");
            OnRightClick?.Invoke(e, this);
            if (PropagationRule.HasFlag(PropagationFlags.MouseRightClick))
                Parent?.MouseRightClick(e);
        }

        public virtual void ScrollWheel(UIScrollWheelEvent e) {
            OnScrollWheel?.Invoke(e, this);
            if (PropagationRule.HasFlag(PropagationFlags.ScrollWheel))
                Parent?.ScrollWheel(e);
        }

        public virtual void FocusOn(UIActionEvent e) {
            IsFocused = true;
            OnFocused?.Invoke(e, this);
            if (PropagationRule.HasFlag(PropagationFlags.FocusOn))
                Parent?.FocusOn(e);
        }

        public virtual void UnFocus(UIActionEvent e) {
            IsFocused = false;
            OnUnFocused?.Invoke(e, this);
            if (PropagationRule.HasFlag(PropagationFlags.UnFocus))
                Parent?.UnFocus(e);
        }

        public virtual void DragStart(UIMouseEvent e) {
            OnDragStart?.Invoke(e, this);
            if (PropagationRule.HasFlag(PropagationFlags.DragStart))
                Parent?.DragStart(e);
        }
        public virtual void DragEnd(UIDragEndEvent e) {
            OnDragEnd?.Invoke(e, this);
            if (PropagationRule.HasFlag(PropagationFlags.DragEnd))
                Parent?.DragEnd(e);
        }

        #endregion

        /// <summary>
        /// 获取在屏幕坐标为pos的地点的最上层响应事件的UI元件
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public virtual UIElement ElementAt(Vector2 pos) {
            UIElement target = null;
            int sz = Children.Count;
            for (int i = sz - 1; i >= 0; i--) {
                var child = Children[i];
                if (child.IsActive && child._selfHitbox.Contains(pos)) {
                    var tmp = child.ElementAt(pos);
                    if (tmp != null) {
                        target = tmp;
                        break;
                    }
                }
            }
            if (target != null) return target;
            if (_selfHitbox.Contains(pos) && !NoEvent && !IsPreview) {
                return this;
            }
            return target;
        }




        public UIElement() {
            Name = "UI元素";
            Pivot = new Vector2(0.5f, 0.5f);
            AnchorPoint = Vector2.Zero;
            Position = new Vector2(0, 0);
            Scale = new Vector2(1f, 1f);
            Parent = null;
            Children = new List<UIElement>();
            IsActive = true;
            IsVisible = true;
            Rotation = 0;
            NoEvent = false;
            PropagationRule = PropagationFlags.PASS_ALL;
            Tooltip = "";
            UseShader = false;
            _selfHitbox = new QuadrilateralHitbox();
            Recalculate();
        }

        public void AppendChild(UIElement element) {
            element.SplitFromParent();
            element.Parent = this;
            Children.Add(element);
            ShouldRecalculate = true;
        }

        public void RemoveChild(UIElement element) {
            Children.Remove(element);
            ShouldRecalculate = true;
        }

        public UIElement GetChildByName(string name) {
            return Children.FirstOrDefault((element) => element.Name.Equals(name));
        }

        public void SplitFromParent() {
            Parent?.RemoveChild(this);
        }


        public Rectangle GetClippingRectangle(SpriteBatch sb) {
            return Rectangle.Intersect(sb.GraphicsDevice.ScissorRectangle, OuterRectangleScreen);
        }

        private Matrix ApplyTransform(Matrix prev) {
            int w = Width, h = Height;
            var m1 = Matrix.CreateScale(Scale.X, Scale.Y, 1f) * Matrix.CreateTranslation(new Vector3((int)(_realPosition.X + w * Pivot.X),
                (int)(_realPosition.Y + h * Pivot.Y), 0)) * prev;
            var m2 = Matrix.CreateTranslation(new Vector3((int)(-w * Pivot.X), (int)(-h * Pivot.Y), 0f)) * Matrix.CreateRotationZ(Rotation);
            return m2 * m1;
        }

        public virtual void DrawChildren(SpriteBatch sb) {
            foreach (var child in Children) {
                if (child.IsActive) {
                    child.Draw(sb);
                }
            }
        }
        public void SpriteBatchBegin(SpriteBatch sb, BlendState blendState) {
            sb.Begin(SpriteSortMode.Deferred, blendState, SamplerState.AnisotropicClamp,
                       DepthStencilState.None, sb.GraphicsDevice.RasterizerState, null, _selfTransform);
        }

        public virtual void Draw(SpriteBatch sb) {
            Rectangle scissorRectangle = sb.GraphicsDevice.ScissorRectangle;
            var defaultstate = sb.GraphicsDevice.RasterizerState;
            if (IsVisible) {
                sb.End();
                if (UseShader) {
                    sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp,
                        DepthStencilState.None, OverflowHiddenRasterizerState, null, _selfTransform);
                } else {
                    sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp,
                        DepthStencilState.None, OverflowHiddenRasterizerState, null, _selfTransform);
                }
                DrawSelf(sb);
                if (UseShader) {
                    sb.End();
                    sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp,
                        DepthStencilState.None, OverflowHiddenRasterizerState, null, _selfTransform);
                }
                PostDrawSelf?.Invoke(new UIDrawEvent(this, Main._drawInterfaceGameTime.TotalGameTime, sb), this);
            }

            if (Overflow == OverflowType.Hidden) {
                sb.End();
                sb.GraphicsDevice.ScissorRectangle = Rectangle.Intersect(scissorRectangle, GetClippingRectangle(sb));
                sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp,
                    DepthStencilState.None, OverflowHiddenRasterizerState, null, _selfTransform);
            }
            DrawChildren(sb);
            if (Overflow == OverflowType.Hidden) {
                sb.End();
                sb.GraphicsDevice.ScissorRectangle = scissorRectangle;
                sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp,
                    DepthStencilState.None, defaultstate, null, Main.UIScaleMatrix);
            } else {
                sb.End();
                sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp,
                    DepthStencilState.None, defaultstate, null, Main.UIScaleMatrix);
            }
            if (DEBUG_MODE) {
                _selfHitbox.Draw(sb);
                Drawing.StrokeRect(sb, GetClippingRectangle(sb), 1, Color.Yellow);
                if (IsFocused) Drawing.StrokeRect(sb, GetClippingRectangle(sb), 2, Color.Red);
            }
        }

        public virtual void DrawSelf(SpriteBatch sb) {
            if (UIElement.DEBUG_MODE) {
                sb.Draw(Main.magicPixel, new Rectangle(1, 1, Width - 2, Height - 2), Color.White * 0.4f);
            }
        }

        public virtual void UpdateSelf(GameTime gameTime) {
            if (MouseDownedLeft) {
                MouseDownTimeLeft++;
            } else {
                MouseDownTimeLeft = 0;
            }
        }
        public virtual void UpdateChildren(GameTime gameTime) {
            foreach (var child in Children.Where(child => child.IsActive)) {
                child.Update(gameTime);
            }
        }

        public void Update(GameTime gameTime) {
            UpdateSelf(gameTime);
            UpdateChildren(gameTime);
        }

        public override string ToString() {
            return $"Type: {GetType().Name}, Name: {Name}";
        }
        public virtual object Clone() {
            var obj = (UIElement)Activator.CreateInstance(GetType());
            obj.Position = this.Position;
            obj.Pivot = this.Pivot;
            obj.AnchorPoint = this.AnchorPoint;
            obj.Size = this.Size;
            obj.SizeFactor = this.SizeFactor;
            obj.Name = this.Name;
            obj.Rotation = this.Rotation;
            obj.Scale = this.Scale;
            obj.IsActive = this.IsActive;
            obj.IsVisible = this.IsVisible;
            obj.IsPreview = this.IsPreview;
            return obj;
        }
    }
}
