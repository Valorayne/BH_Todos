﻿using System.Collections.Generic;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace TodoList.Components
{
    public sealed class TodoScrollView : FlowPanel
    {
        private const int OUTER_PADDING = 15;
        private const int INNER_PADDING = 5;
        
        private readonly List<TodoEntry> _entries = new List<TodoEntry>();
        
        public TodoScrollView(Resources resources, Settings settings)
        {
            FlowDirection = ControlFlowDirection.SingleTopToBottom;
            WidthSizingMode = SizingMode.Fill;
            HeightSizingMode = SizingMode.Fill;
            OuterControlPadding = new Vector2(OUTER_PADDING, OUTER_PADDING);
            ControlPadding = new Vector2(INNER_PADDING, INNER_PADDING);

            for (var i = 0; i < 50; i++)
                _entries.Add(new TodoEntry { Parent = this });
        }

        protected override void DisposeControl()
        {
            foreach (var entry in _entries)
                entry.Dispose();
            _entries.Clear();

            base.DisposeControl();
        }
    }
}