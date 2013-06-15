using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Parsers
{
    public enum Status
    {
            ST_OK,
            ST_FAIL
    }

    //public delegate void CallEnterState(Status status);
    //public delegate IState<InputReader, TSymbVal> CallTransitionState<InputReader, TSymbVal>(
    //IState<InputReader, TSymbVal> previousState,
    //SymbolReader<InputReader, TSymbVal> reader, 
    //Status status);
    //public delegate void CallLeaveState(Status status);

    public interface IState<InputReader, TSymbVal, TSymbType>
    {
        //CallEnterState EnterState { get; set; }
        //CallTransitionState<InputReader> TransitionState { get; set; }
        //CallLeaveState LeaveState { get; set; }
        IState<InputReader, TSymbVal, TSymbType> NextState(SymbolReader<InputReader, TSymbVal, TSymbType> reader);
    }
}
