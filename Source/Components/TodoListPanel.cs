﻿using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TodoList.Components.Menu;

namespace TodoList.Components
{
    public sealed class TodoListPanel : FlowPanel
    {
        private const int SCROLL_BAR_WIDTH = 15;

        private readonly Scrollbar _scrollBar;
        private readonly TodoListMenuBar _menuBar;
        private readonly TodoScrollView _scrollView;

        public TodoListPanel()
        {
            FlowDirection = ControlFlowDirection.SingleTopToBottom;
            WidthSizingMode = SizingMode.Fill;
            HeightSizingMode = SizingMode.Fill;

            _menuBar = new TodoListMenuBar { Parent = this };
            _scrollView = new TodoScrollView(SaveScroll) { Parent = this };
            _scrollBar = new Scrollbar(_scrollView) { Parent = this };
        }

        private float? _scrollTarget;

        private void SaveScroll()
        {
            _scrollTarget = _scrollBar.ScrollDistance * (_scrollView.Bottom - _scrollView.ContentBounds.Y);
        }
        
        public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
        {
            base.PaintBeforeChildren(spriteBatch, bounds);
            if (_scrollTarget.HasValue)
            {
                _scrollBar.ScrollDistance = _scrollTarget.Value / (_scrollView.Bottom - _scrollView.ContentBounds.Y);
                _scrollTarget = null;
            }
        }

        private void ResizeComponents()
        {
            if (_scrollBar != null)
            {
                _scrollBar.Height = Height - _menuBar.Height;
                _scrollBar.Location = new Point(Width - SCROLL_BAR_WIDTH, TodoListMenuBar.HEIGHT);
            }

            if (_scrollView != null)
            {
                _scrollView.Height = Height - _menuBar.Height;
                _scrollView.Width = Width - SCROLL_BAR_WIDTH;
                _scrollView.Location = new Point(0, TodoListMenuBar.HEIGHT);
            }
        }

        protected override void OnResized(ResizedEventArgs e)
        {
            ResizeComponents();
            base.OnResized(e);
        }
    }
}