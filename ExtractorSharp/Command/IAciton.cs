using ExtractorSharp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Command {
    public interface IAction {

    }

    /// <summary>
    /// 多图层操作
    /// </summary>
    interface LayerAction : IAction {
        void Action();
    }

    /// <summary>
    /// 多IMG操作
    /// </summary>
    interface MutipleAciton :IAction{
        void Action(params Album[] Album);
    }

    /// <summary>
    /// 单IMG操作
    /// </summary>
    interface SingleAction : IAction {
        int[] Indexes { get; set; }
        void  Action(Album Album, int[] Indexes);
    }
}
