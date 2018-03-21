namespace Elmish.Toastr

open Elmish
open Fable.Core
open Fable.Core.JsInterop

[<StringEnum>]
type ToastPosition = 
    | [<CompiledName("toast-top-right")>] TopRight
    | [<CompiledName("toast-bottom-right")>] BottomRight
    | [<CompiledName("toast-top-left")>] TopLeft
    | [<CompiledName("toast-bottom-left")>] BottomLeft
    | [<CompiledName("toast-top-full-width")>] TopFullWidth
    | [<CompiledName("toast-bottom-full-width")>] BottomFullWidth
    | [<CompiledName("toast-top-center")>] TopCenter
    | [<CompiledName("toast-bottom-center")>] BottomCenter

[<StringEnum>]
type Easing = 
    | [<CompiledName("swing")>] Swing
    | [<CompiledName("linear")>] Linear

type ToastrOptions = 
    [<Emit("$0.timeOut{{=$1}}")>]
    abstract timeout : int with get,set
    abstract closeButton : bool with get,set
    abstract newestOnTop : bool with get,set
    abstract progressBar : bool with get,set
    abstract preventDuplicates : bool with get,set
    abstract showDuration : int with get, set
    abstract hideDuration : int with get,set
    [<Emit("$0.extendedTimeOut{{=$1}}")>]
    abstract extendedTimeout : int with get,set
    [<Emit("$0.positionClass{{=$1}}")>]
    abstract position : ToastPosition with get, set
    abstract escapeHtml : bool with get, set
    abstract closeHtml : string with get,set
    abstract closeDuration : int with get,set
    [<Emit("$0.rtl{{=$1}}")>]
    abstract rightToLeft : bool with get,set
    abstract showEasing : Easing with get, set
    abstract hideEasing : Easing with get,set
    abstract closeEasing : Easing with get,set
    abstract closeOnHover : bool with get,set
    abstract tapToDismiss : bool with get,set
    abstract onShown : (unit -> unit) with get,set
    abstract onHidden : (unit -> unit) with get,set
    abstract onclick : (unit -> unit) with get,set
    abstract onCloseClick : (unit -> unit) with get,set

[<RequireQualifiedAccess>]
module Toastr = 

    importAll "toastr/build/toastr.min.css"

    [<Pojo>]
    type ToastrMsg<'a> = { 
        Message : string; 
        Title: string; 
        Options: ToastrOptions 
        mutable Dispatcher : Option<'a -> unit>
    }

    let defaultMsg() = { 
        Message = ""; 
        Title = ""; 
        Options = createEmpty<ToastrOptions>
        Dispatcher = None 
    }
    let private successToastWithTitle (msg: string) (title: string) (options: ToastrOptions)   : unit = import "success" "toastr" 
    let private errorToastWithTitle (msg: string) (title: string) (options: ToastrOptions)   : unit = import "error" "toastr" 
    let private infoToastWithTitle (msg: string) (title: string) (options: ToastrOptions)   : unit = import "info" "toastr" 
    let private warningToastWithTitle (msg: string) (title: string) (options: ToastrOptions)  : unit = import "warning" "toastr" 

    /// Sets the message of toast
    let message msg = { defaultMsg() with Message = msg  }
    
    /// Sets the title of the toast
    let title title msg = { msg with Title = title }

    /// Defines the duration in ms after which the toast starts to disappear
    let timeout timeout msg = 
        let options = msg.Options
        options.timeout <- timeout
        msg

    /// Sets the position of the toastr relative to the screen
    let position pos msg = 
        let options = msg.Options
        options.position <- pos
        msg

    let extendedTimout t msg = 
        let options = msg.Options
        options.extendedTimeout <- t
        msg

    /// Configures a message to be sent to the dispatch loop when the toast is clicked
    let onClick (nextMsg: 'a) (msg: ToastrMsg<'a>) = 
        let options = msg.Options
        options.onclick <- fun () -> 
            match msg.Dispatcher with
            | Some dispatcher -> dispatcher nextMsg
            | None -> ()
        msg

    /// Configures a message to be sent to the dispatch loop when the toast is shown on screen
    let onShown (nextMsg: 'a) (msg: ToastrMsg<'a>) = 
        let options = msg.Options
        options.onShown <- fun () -> 
            match msg.Dispatcher with
            | Some dispatcher -> dispatcher nextMsg
            | None -> ()
        msg 

    let tapToDismiss msg = 
        let options = msg.Options
        options.tapToDismiss <- true
        msg

    /// Configures a message to be sent to the dispatch loop when the toast has disappeared

    let onHidden (nextMsg: 'a) (msg: ToastrMsg<'a>) = 
        let options = msg.Options
        options.onHidden <- fun () -> 
            match msg.Dispatcher with
            | Some dispatcher -> dispatcher nextMsg
            | None -> ()
        msg 

    /// Configures the toast to show a close button 
    let showCloseButton msg = 
        let options = msg.Options
        options.closeButton <- true
        msg
        
    /// Shows the progress bar of how long the toast will take before it disappears
    let withProgressBar msg = 
        let options = msg.Options
        options.progressBar <- true
        msg
    
    /// Configures a message to be sent to the dispatch loop when the close button of toast is clicked
    let closeButtonClicked (nextMsg: 'a) (msg: ToastrMsg<'a>) = 
        let options = msg.Options
        options.onCloseClick <- fun () -> 
            match msg.Dispatcher with
            | Some dispatcher -> 
                printfn "Dispatch found: %s" (toJson nextMsg)
                dispatcher nextMsg
            | None -> ()
        msg     

    let hideEasing e msg = 
        let options = msg.Options
        options.hideEasing <- e
        msg


    [<Emit("Object.assign({}, $0, $1)")>]
    let private mergeObjects x y = jsNative
    
    let mutable private options : ToastrOptions =  import "options" "toastr"
    
    /// Overrides global options
    let overrideOptions (opts: ToastrOptions) : unit = 
        options <- mergeObjects options opts
    
    /// Immediately remove current toasts without using animation
    let private remove() : unit = import "remove" "toastr"
    
    /// Remove current toasts using animation
    let private clear() : unit = import "clear" "toastr"
    
    /// Remove current toasts using animation
    let clearAll : Cmd<_> = 
        [fun _ -> clear()]

    /// Remove current toasts using animation
    let removeAll : Cmd<_> = 
        [fun _ -> remove()]

    /// Shows a success toast
    let success (msg: ToastrMsg<'msg>) : Cmd<'msg> = 
        [fun dispatch -> 
            msg.Dispatcher <- Some dispatch 
            successToastWithTitle msg.Message msg.Title msg.Options]
    
    /// Shows an error taost
    let error (msg: ToastrMsg<'msg>) :  Cmd<'msg> = 
        [fun dispatch -> 
            msg.Dispatcher <- Some dispatch 
            errorToastWithTitle msg.Message msg.Title msg.Options]
            
    /// Shows an info toast
    let info (msg: ToastrMsg<'msg>) : Cmd<'msg> = 
        [fun dispatch -> 
            msg.Dispatcher <- Some dispatch 
            infoToastWithTitle msg.Message msg.Title msg.Options]
            
    /// Shows a warning toast
    let warning (msg: ToastrMsg<'msg>) : Cmd<'msg> = 
        [fun dispatch -> 
            msg.Dispatcher <- Some dispatch 
            warningToastWithTitle msg.Message msg.Title msg.Options]
