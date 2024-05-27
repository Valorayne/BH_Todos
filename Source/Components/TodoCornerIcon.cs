using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Todos.Source.Models;
using Todos.Source.Utils;
using Todos.Source.Utils.Reactive;

namespace Todos.Source.Components
{
    public sealed class TodoCornerIcon : CornerIcon
    {
        private readonly IDisposable _icon;
        private readonly SettingsModel _settings;
        private readonly TodoListModel _todoList;

        public TodoCornerIcon(SettingsModel settings, TodoListModel todoList)
        {
            _settings = settings;
            _todoList = todoList;

            Priority = 64530307;
            Visible = true;

            _icon = todoList.OpenTodos.CombineWith(settings.WindowMinimized, settings.ColorMenuIcon,
                    (openTodos, minimized, color) => color && minimized && openTodos > 0
                        ? Textures.CornerIconAlert
                        : Textures.CornerIcon)
                .Subscribe(this, texture => Icon = Resources.GetTexture(texture));
            todoList.OpenTodos.Subscribe(this,
                openTodos => BasicTooltipText = openTodos > 0
                    ? $"{openTodos} open Todo{(openTodos > 1 ? "s" : "")}"
                    : "No open Todos");
        }

        protected override void OnClick(MouseEventArgs e)
        {
            _settings.WindowMinimized.Toggle();
            base.OnClick(e);
        }

        protected override void DisposeControl()
        {
            _icon.Dispose();
            _settings.Unsubscribe(this);
            _todoList.Unsubscribe(this);
            base.DisposeControl();
        }
    }
}
