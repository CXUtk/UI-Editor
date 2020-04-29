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

namespace UIEditor.UILib {
    public class UIElement {
        public delegate void MouseEvent(UIMouseEvent e, UIElement sender);
        public delegate void ScrollEvent(UIScrollWheelEvent e, UIElement sender);
        public delegate void ActionEvent(UIActionEvent e, UIElement sender);

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

        [EditorPropertyIgnore]
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
        public bool BlockPropagation { get; set; }

        /// <summary>
        /// 这个UI元素是否会响应事件
        /// </summary>
        public bool NoEvent { get; set; }


        /// <summary>
        /// 鼠标移动上去时候显示的说明文字
        /// </summary>
        public string Tooltip { get; set; }

        [EditorPropertyIgnore]
        public bool IsFocused { get; set; }

        [EditorPropertyIgnore]
        public bool ShouldRecalculate { get; set; }



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
        public event MouseEvent OnMouseOver;
        public event MouseEvent OnMouseOut;
        public event MouseEvent OnMouseDown;
        public event MouseEvent OnMouseUp;
        public event MouseEvent OnClick;
        public event ScrollEvent OnScrollWheel;
        public event ActionEvent OnFocused;
        public event ActionEvent OnUnFocused;
        #endregion


        #region 派生属性
        public Rectangle OuterRectangleScreen {
            get {
                return _selfHitbox.GetOuterRectangle();
            }
        }
        public Rectangle BaseRectangleScreen {
            get {
                return new Rectangle((int)(_baseTopLeftScreen.X), (int)(_baseTopLeftScreen.Y), Width, Height);
            }
        }
        public Rectangle InnerRectangleScreen {
            get {
                return new Rectangle((int)(_baseTopLeftScreen.X), (int)(_baseTopLeftScreen.Y), Width, Height);
            }
        }


        public int Width {
            get {
                return (int)(SizeFactor.X * _parentRect.Width + Size.X);
            }
        }
        public int Height {
            get {
                return (int)(SizeFactor.Y * _parentRect.Height + Size.Y);
            }
        }

        public bool IsMouseHover {
            get {
                return _selfHitbox.Contains(Main.MouseScreen);
            }
        }

        private Vector2 PivotOffset {
            get {
                return new Vector2(Width * Pivot.X, Height * Pivot.Y);
            }
        }

        public Vector2 ScreenPositionToNode(Vector2 worldPos) {
            return worldPos - (_baseTopLeftScreen - Position + PivotOffset);
        }

        //public Vector2 NodePositionToScreen(Vector2 worldPos) {
        //    return _baseTopLeftScreen - PivotOffset;
        //}


        public IHitBox ScreenHitBox {
            get {
                return _selfHitbox;
            }
        }

        public Vector2 PostionScreen {
            get {
                return _baseTopLeftScreen + PivotOffset;
            }
            set {
                Position = ScreenPositionToNode(value);
            }
        }

        #endregion



        private Vector2 getBaseRectScreen() {
            Vector2 pos = _parentRect.TopLeft();
            pos += _parentRect.Size() * AnchorPoint;
            return pos;
        }
        public void RecalculateLocation() {
            _parentRect = Parent == null ? new Rectangle(0, 0, Main.screenWidth, Main.screenHeight) :
                 Parent.InnerRectangleScreen;
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
        private QuadrilateralHitbox _selfHitbox;
        private Matrix _selfTransform;
        private Rectangle _parentRect;

        private RasterizerState _selfRasterizerState;



        public void MouseEnter(UIMouseEvent e) {
            // Main.NewText("进入");
            OnMouseEnter?.Invoke(e, this);
            if (!BlockPropagation)
                Parent?.MouseEnter(e);
        }

        public void MouseOut(UIMouseEvent e) {
            //Main.NewText("离开");
            OnMouseOut?.Invoke(e, this);
            if (!BlockPropagation)
                Parent?.MouseOut(e);
        }

        public void MouseDown(UIMouseEvent e) {
            //Main.NewText("按下");
            OnMouseDown?.Invoke(e, this);
            if (!BlockPropagation)
                Parent?.MouseDown(e);
        }

        public void MouseUp(UIMouseEvent e) {
            //Main.NewText("抬起");
            OnMouseUp?.Invoke(e, this);
            if (!BlockPropagation)
                Parent?.MouseUp(e);
        }
        public void MouseClick(UIMouseEvent e) {
            //Main.NewText("点击");
            OnClick?.Invoke(e, this);
            if (!BlockPropagation)
                Parent?.MouseClick(e);
        }

        public void ScrollWheel(UIScrollWheelEvent e) {
            OnScrollWheel?.Invoke(e, this);
            if (!BlockPropagation)
                Parent?.ScrollWheel(e);
        }

        // 聚焦事件不会向后传播
        public void FocusOn(UIActionEvent e) {
            IsFocused = true;
            OnFocused?.Invoke(e, this);
        }

        public void UnFocus(UIActionEvent e) {
            IsFocused = false;
            OnUnFocused?.Invoke(e, this);
        }


        public UIElement ElementAt(Vector2 pos) {
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
            if (_selfHitbox.Contains(pos) && !NoEvent) {
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
            _selfRasterizerState = new RasterizerState() {
                CullMode = CullMode.None,
                ScissorTestEnable = true,
            };
            Tooltip = "";
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
            Matrix m1 = Matrix.CreateScale(Scale.X, Scale.Y, 1f) * Matrix.CreateTranslation(new Vector3(_realPosition.X + w * Pivot.X, _realPosition.Y + h * Pivot.Y, 0)) * prev;
            Matrix m2 = Matrix.CreateTranslation(new Vector3(-w * Pivot.X, -h * Pivot.Y, 0f)) * Matrix.CreateRotationZ(Rotation);
            return m2 * m1;
        }

        public virtual void DrawChildren(SpriteBatch sb) {
            foreach (var child in Children) {
                if (child.IsActive) {
                    child.Draw(sb);
                }
            }
        }
        public virtual void Draw(SpriteBatch sb) {
            Rectangle scissorRectangle = sb.GraphicsDevice.ScissorRectangle;
            if (IsVisible) {
                sb.End();
                sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
                    DepthStencilState.None, _selfRasterizerState, null, _selfTransform);
                DrawSelf(sb);
            }
            if (Overflow == OverflowType.Hidden) {
                sb.End();
                sb.GraphicsDevice.ScissorRectangle = Rectangle.Intersect(sb.GraphicsDevice.ScissorRectangle, GetClippingRectangle(sb));
                sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
                    DepthStencilState.None, _selfRasterizerState, null, _selfTransform);
            }
            DrawChildren(sb);
            if (Overflow == OverflowType.Hidden) {
                sb.End();
                var defaultstate = sb.GraphicsDevice.RasterizerState;
                sb.GraphicsDevice.ScissorRectangle = scissorRectangle;
                sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
                    DepthStencilState.None, defaultstate, null, _selfTransform);
            }
            if (DEBUG_MODE) {
                sb.End();
                sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
                    DepthStencilState.None, _selfRasterizerState, null, Main.UIScaleMatrix);
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

        public virtual void UpdateSelf(GameTime gameTime) { }
        public virtual void UpdateChildren(GameTime gameTime) {
            foreach (var child in Children) {
                if (child.IsActive) {
                    child.Update(gameTime);
                }
            }
        }

        public void Update(GameTime gameTime) {
            UpdateSelf(gameTime);
            UpdateChildren(gameTime);
        }

        public override string ToString() {
            return $"Type: {GetType().Name}, Name: {Name}";
        }
    }
}
