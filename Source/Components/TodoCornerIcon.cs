﻿using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;

namespace TodoList.Components
{
    public class TodoCornerIcon : ModuleEntity
    {
        private readonly CornerIcon _icon;
        private readonly Container _window;
        private readonly Settings _settings;

        public TodoCornerIcon(Resources resources, Container window, Settings settings)
        {
            _window = window;
            _settings = settings;
            _icon = RegisterForDisposal(CreateIcon(resources));
        }
        
        protected override void Initialize()
        {
            _icon.Click += OnIconClicked;
            _settings.ShowMenuIcon.SettingChanged += OnShowMenuIconChanged;
            OnShowMenuIconChanged(this, new ValueChangedEventArgs<bool>(false, _settings.ShowMenuIcon.Value));
        }

        private static CornerIcon CreateIcon(Resources resources)
        {
            return new CornerIcon
            {
                IconName = "Todo List",
                Icon = resources.GetTexture(Textures.CornerIcon),
                HoverIcon = resources.GetTexture(Textures.CornerIconHovered),
                Priority = 5
            };
        }

        private void OnIconClicked(object target, MouseEventArgs args)
        {
            if (_window.Visible)
                _window.Hide();
            else _window.Show();
        }

        private void OnShowMenuIconChanged(object target, ValueChangedEventArgs<bool> args)
        {
            if (args.NewValue)
                _icon.Show();
            else _icon.Hide();
        }

        protected override void CustomDispose()
        {
            _settings.ShowMenuIcon.SettingChanged -= OnShowMenuIconChanged;
        }
    }
}