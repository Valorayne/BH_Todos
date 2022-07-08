﻿using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using TodoList.Models;

namespace TodoList.Components
{
    public sealed class TodoEntry : Panel
    {
        private readonly Todo _todo;
        private readonly Action _saveScroll;
        private readonly TodoEntryHoverMenu _hoverMenu;

        public TodoEntry(Todo todo, Action saveScroll)
        {
            _todo = todo;
            _saveScroll = saveScroll;
            
            WidthSizingMode = SizingMode.Fill;
            Height = HEADER_HEIGHT;

            new TodoEntryContent(todo) { Parent = this, Location = Point.Zero };
            _hoverMenu = new TodoEntryHoverMenu(todo) { Parent = this, Visible = false };

            Data.TodoModified += OnTodoModified;
        }

        private bool _expanded;
        
        protected override void OnClick(MouseEventArgs e)
        {
            _saveScroll();
            Height = _expanded ? Height / 2 : Height * 2;
            _expanded = !_expanded;
            base.OnClick(e);
        }

        private void OnTodoModified(object sender, Todo todo)
        {
            if (todo == _todo && todo.Done && !Settings.ShowAlreadyDoneTasks.Value)
            {
                Hide();
                Parent.Invalidate();
            }
        }

        private void RepositionHoverMenu()
        {
            if (_hoverMenu != null)
                _hoverMenu.Location = new Point(Width - _hoverMenu.Width, 0);
        }

        protected override void OnResized(ResizedEventArgs e)
        {
            RepositionHoverMenu();
            base.OnResized(e);
        }

        protected override void OnMouseEntered(MouseEventArgs e)
        {
            _hoverMenu.Show();
            base.OnMouseEntered(e);
        }

        protected override void OnMouseLeft(MouseEventArgs e)
        {
            _hoverMenu.Hide();
            base.OnMouseLeft(e);
        }

        protected override void DisposeControl()
        {
            Data.TodoModified -= OnTodoModified;
            base.DisposeControl();
        }
    }
}