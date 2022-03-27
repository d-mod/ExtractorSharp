using System.Collections.Generic;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Services.Constants {
    public static class StoreKeys {


        #region APP
        /// <summary>
        /// 当前文件列表所显示的
        /// </summary>
        public const string DISPLAY_LIST = "/files/display-list";
        /// <summary>
        /// <see cref="IEnumerable{T}"/>
        /// 文件列表所选的集合
        /// </summary>
        public const string SELECTED_FILE_RANGE = "/files/selected-range";
        /// <summary>
        /// <see cref="IEnumerable{int}"/>
        /// 文件列表所选的下标
        /// </summary>
        public const string SELECTED_FILE_INDICES = "/files/selected-indices";

        /// <summary>
        /// <see cref="Album"/>
        /// 文件列表所选的第一个项
        /// </summary>
        public const string SELECTED_FILE = "/files/selected-item";


        /// <summary>
        /// <see cref="Album"/>
        /// 文件列表所选的第一个项
        /// </summary>
        public const string SELECTED_FILE_INDEX = "/files/selected-index";
        /// <summary>
        /// <see cref="IEnumerable{T}"/>
        /// </summary>
        public const string SELECTED_IMAGE_RANGE = "/images/selected-range";

        /// <summary>
        /// <see cref="IEnumerable{int}"/>
        /// 文件列表所选的下标
        /// </summary>
        public const string SELECTED_IMAGE_INDICES = "/images/selected-indices";

        /// <summary>
        /// <see cref="Sprite"/>
        /// 贴图列表所选的第一个项
        /// </summary>
        public const string SELECTED_IMAGE = "/images/selected-item";
        /// <summary>
        /// <see cref="IEnumerable{T}"/>
        /// </summary>
        public const string SELECTED_IMAGE_INDEX = "/images/selected-index";
        /// <summary>
        /// 退出程序
        /// </summary>
        public const string APP_EXIT = "/app/exit";
        /// <summary>
        /// 启动新窗口
        /// </summary>
        public const string APP_NEW_WINDOW = "/app/new-window";

        public const string APP_DIR = "/app/dir";

        public const string APP_VERSION = "/app/version";

        public const string APP_SESSION_ID = "/app/session-id";

        public const string CONFIG_DIR = "/config/dir";

        public const string FILES_SELECT_ALL = "/files/select-all";

        public const string FILES_UNSELECT_ALL = "/files/unselect-all";

        public const string FILES_SELECT_REVERSE = "/files/select-reverse";


        public const string IMAGES_SELECT_ALL = "/images/select-all";

        public const string IMAGES_UNSELECT_ALL = "/images/unselect-all";


        public const string IMAGES_SELECT_REVERSE = "/images/select-reverse";

        #endregion

        #region DATA

        /// <summary>
        /// <see cref="IEnumerable{T}"/>
        /// 文件列表
        /// </summary>
        public const string FILES = "/data/files";

        /// <summary>
        /// 文件列表正被锁定中
        /// </summary>
        public const string IS_LOCKED = "/data/is-locked";

        /// <summary>
        /// <see cref="bool"/>
        /// 是否已保存
        /// </summary>
        public const string IS_SAVED = "/data/is-saved";
        /// <summary>
        /// <see cref="string"/>
        /// 文件保存的路径
        /// </summary>
        public const string SAVE_PATH = "/data/save-path";
        /// <summary>
        /// <see cref="IEnumerable{String}"/>
        /// 最近打开过的文件路径
        /// </summary>
        public const string RECENTS = "/data/recents";
        /// <summary>
        /// <see cref="bool"/>
        /// 选择保存文件的路径时是否点了取消
        /// </summary>
        public const string SAVE_CANCEL = "/data/save-cancel";
        /// <summary>
        /// <see cref="IEnumerable{String}"/>
        /// 刚加载的临时文件(临时)
        /// </summary>
        public const string LOAD_FILES = "/load/files";

        /// <summary>
        /// <see cref="object"/>
        /// 脚本上次返回值
        /// </summary>
        public const string SCRIPT_LAST_RESULT = "/data/script-last-result";
        #endregion

        #region MERGE
        /// <summary>
        /// <see cref="IEnumerable{T}"/>
        /// 拼合队列
        /// </summary>
        public const string MERGE_QUEUES = "/merge/queues";

        /// <summary>
        /// <see cref="Dictionary{TKey, TValue}"/>
        /// 拼合排序规则
        /// </summary>
        public const string MERGE_RULES = "/merge/rules";

        /// <summary>
        /// <see cref="int"/>
        /// 拼合进度
        /// </summary>
        public const string MERGE_PROGRESS = "/merge/progress";

        #endregion

    }
}
