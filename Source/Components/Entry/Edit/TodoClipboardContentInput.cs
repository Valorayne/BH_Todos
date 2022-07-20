﻿using System;
using Blish_HUD.Controls;
using Todos.Source.Models;

namespace Todos.Source.Components.Entry.Edit
{
    public class TodoClipboardContentInput : TextBox
    {
        private readonly TodoModel _todo;

        public TodoClipboardContentInput(TodoModel todo)
        {
            _todo = todo;
            Text = _todo.ClipboardContent.Value;
            TextChanged += OnTextChanged;
        }
        
        private void OnTextChanged(object sender, EventArgs e) => _todo.ClipboardContent.Value = Text;

        protected override void DisposeControl()
        {
            TextChanged -= OnTextChanged;
            base.DisposeControl();
        }
    }
}