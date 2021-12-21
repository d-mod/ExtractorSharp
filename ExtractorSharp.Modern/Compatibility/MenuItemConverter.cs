using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtractorSharp.Composition.Menu;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ExtractorSharp.Composition;
using System.ComponentModel.Composition;
using ExtractorSharp.Core;
using System.Text.RegularExpressions;

namespace ExtractorSharp.Compatibility {

    [Export]
    class MenuItemConverter {


        [Import]
        private Language Language;


        public List<object> CreateMenu(List<IMenuItem> imports, List<CommandBinding> commandBindings) {
            if(commandBindings == null) {
                commandBindings = new List<CommandBinding>();
            }
            return CreateChildren(MenuItemTools.CreateMenuTree(imports), commandBindings);
        }



        private List<object> CreateChildren(List<IMenuItem> items,List<CommandBinding> commandBindings) {
            var list = new List<object>();
            foreach(var item in items) {

                var menuItem = new MenuItem {
                    Header = this.Language[item.Key],
                    Tag = item.Key
                };


                if(item is IRoute route) {

                    var keyGesture = ParseGesture(route.ShortCutKey);
                    var inputGestures = new InputGestureCollection();
                    if(keyGesture != null) {
                        inputGestures.Add(keyGesture);
                    }

                    var command = new RoutedCommand(route.Command, typeof(MenuItem), inputGestures);

                    var commandBinding = new CommandBinding(command);

                    commandBinding.CanExecute += (o, e) => {
                        e.CanExecute = route.CanExecute();
                        e.Handled = true;
                        e.ContinueRouting = false;
                    };
                    commandBinding.Executed += route.Execute;

                    menuItem.Command = command;
                    menuItem.CommandBindings.Add(commandBinding);
                    commandBindings.Add(commandBinding);
                }




                if(item is IChildrenItem child) {
                    var children = this.CreateChildren(child.Children,commandBindings);
                    if(child.IsTile) {                        //如果有分割线则不添加
                        if(list.Count > 0 && !(list.Last() is Separator)) {
                            list.Add(new Separator());
                        }
                        list.AddRange(children);
                        list.Add(new Separator());
                        continue;
                    } else {
                        if(item is INotifyPropertyChanged) {
                            menuItem.DataContext = item;
                            var binding = new Binding("Children");
                            menuItem.SetBinding(ItemsControl.ItemsSourceProperty, binding);
                        } else {
                            menuItem.ItemsSource = children;
                        }
                    }
                }
                list.Add(menuItem);
            }
            //移除最后的分割线
            if(list.Count > 0 && (list.Last() is Separator s)) {
                list.Remove(s);
            }
            SetIcon(list);
            return list;
        }


        private static void SetIcon(IEnumerable items) {
            foreach(var item in items) {
                if(item is MenuItem menuItem && menuItem.Icon == null) {
                    var key = menuItem.Tag?.ToString();
                    menuItem.Icon = menuItem.TryFindResource($"Icons.{key}");
                    SetIcon(menuItem.Items);
                }
            }
        }

        public static KeyGesture ParseGesture(string keyToken) {
            if(!string.IsNullOrEmpty(keyToken)) {
                var tokens = keyToken.Split("+");
                var modifiers = GetModifierKey(tokens);
                var last = tokens.Last();
                var key = GetKey(last);
                var gesture = new KeyGesture(key, modifiers, keyToken);
                return gesture;
            }
            return null;
        }

        public static ModifierKeys GetModifierKey(string[] tokens) {
            var modifiers = ModifierKeys.None;
            if(tokens != null && tokens.Length > 0) {

                foreach(var token in tokens) {
                    switch(token.ToUpper()) {
                        case "ALT":
                            modifiers |= ModifierKeys.Alt;
                            break;
                        case "CTRL":
                        case "CONTROL":
                            modifiers |= ModifierKeys.Control;
                            break;
                        case "SHIFT":
                            modifiers |= ModifierKeys.Shift;
                            break;
                        case "WINDOWS":
                            modifiers |= ModifierKeys.Windows;
                            break;
                    }
                }
            }
            return modifiers;
        }

        public static Key GetKey(string token) {
            var key = (Key)(-1);
            if(token == string.Empty) {
                return Key.None;
            }
            token = token.ToUpper();
            if(token.Length == 1 && char.IsLetterOrDigit(token[0])) {
                if(char.IsDigit(token[0]) && token[0] >= '0' && token[0] <= '9') {
                    return (Key)34 + token[0] - 48;
                }
                if(char.IsLetter(token[0]) && token[0] >= 'A' && token[0] <= 'Z') {
                    return (Key)44 + token[0] - 65;
                }
                throw new ArgumentException("");
            }
            if(Regex.IsMatch(token, "^F\\d+$")) {
                return (Key)Enum.Parse(typeof(Key), token);
            }
            return key;
        }

    }
}
