using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ExtractorSharp.Composition.Menu {

    public static class MenuItemTools {

        public static void Sort(List<IMenuItem> children) {
            if(children == null) {
                return;
            }
            foreach(var child in children) {
                children.Sort((a, b) => a.Order - b.Order);
                children.ForEach(e => {
                    if(e is IChildrenItem c) {
                        Sort(c.Children);
                    }
                });
            }
        }

        public static List<IMenuItem> CreateMenuTree(List<IMenuItem> imports) {
            var tree = new List<IMenuItem>();
            foreach(var item in imports) {
                var nodes = item.Key.Split("/");
                var node = tree;

                for(var i = 0; i < nodes.Length - 1; i++) {
                    var key = nodes[i];
                    var child = node.Find(e => e.IsKey(key));
                    if(child == null) {
                        child = new DefaultMenuItem(key);
                        node.Add(child);
                    }
                    if(child is IChildrenItem c) {
                        node = c.Children;
                    }
                }
                var exist = node.Find(e => e.IsKey(nodes.Last()));
                var news = item.Clone();

                if(exist != null) {
                    node.Remove(exist);
                    news = news.Merge(exist);
                }
                node.Add(news);
            }
            Sort(tree);
            return tree;
        }

        public static IMenuItem Clone(this IMenuItem item) {
            if(item is IRoute) {
                return new DefaultRouteItem(item);
            }
            return new DefaultMenuItem(item);
        }

        public static bool IsKey(this IMenuItem item, string key) {
            var pattern = @"[_\[\d+\]]";
            var rs0 = Regex.Replace(item.Key, pattern, "");
            var rs1 = Regex.Replace(key, pattern, "");
            return rs0 == rs1;
        }

        /// <summary>
        /// 合并菜单属性
        /// </summary>
        /// <param name="item"></param>
        public static IMenuItem Merge(this IMenuItem item, IMenuItem news) {
            item.Order = Math.Max(news.Order, item.Order);
            item.ToolTip = news.ToolTip ?? item.ToolTip;
            var children = new List<IMenuItem>();
            var isTile = false;
            if(item is IChildrenItem childA) {
                children.AddRange(childA.Children ?? new List<IMenuItem>());
                isTile = isTile || childA.IsTile;
            }
            if(news is IChildrenItem childB) {
                children.AddRange(childB.Children ?? new List<IMenuItem>());
                isTile = isTile || childB.IsTile;
            }

            if(item is IRoute || news is IRoute) {
                item = new DefaultRouteItem(item) {
                    Children = children,
                    IsTile = isTile
                };
            } else {
                item = new DefaultMenuItem(item) {
                    Children = children,
                    IsTile = isTile
                };
            }

            return item;
        }


    }

}
