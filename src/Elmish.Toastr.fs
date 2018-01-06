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
    abstract extendedTimeOut : int with get,set
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

    type ToastrMsg = { 
        Message : string; 
        Title: string; 
        Options: ToastrOptions 
    }

    let defaultMsg = { Message = ""; Title = ""; Options = createEmpty<ToastrOptions> }

    let private successToast (msg: string) (options: ToastrOptions)  : unit = import "success" "toastr" 
    let private successToastWithTitle (msg: string) (title: string) (options: ToastrOptions)   : unit = import "success" "toastr" 

    let private errorToast (msg: string) (options: ToastrOptions)  : unit = import "error" "toastr" 
    let private errorToastWithTitle (msg: string) (title: string) (options: ToastrOptions)   : unit = import "error" "toastr" 

    let private infoToast (msg: string) (options: ToastrOptions)  : unit = import "info" "toastr" 
    let private infoToastWithTitle (msg: string) (title: string) (options: ToastrOptions)   : unit = import "info" "toastr" 

    let private warningToast (msg: string) (options: ToastrOptions) : unit = import "warning" "toastr" 
    let private warningToastWithTitle (msg: string) (title: string) (options: ToastrOptions)  : unit = import "warning" "toastr" 

    let message msg = { defaultMsg with Message = msg  }
    let title title msg = { msg with Title = title }

    let timeout timeout msg = 
        let options = msg.Options
        options.timeout <- timeout
        { msg with Options = options }

    let position pos msg = 
        let options = msg.Options
        options.position <- pos
        { msg with Options = options }

    let onClick f msg = 
        let options = msg.Options
        options.onclick <- f
        { msg with Options = options }

    let onShown f msg = 
        let options = msg.Options
        options.onShown <- f
        { msg with Options = options }  

    let tapToDismiss msg = 
        let options = msg.Options
        options.tapToDismiss <- true
        { msg with Options = options }  

    let onHidden f msg = 
        let options = msg.Options
        options.onHidden <- f
        { msg with Options = options } 

    let showCloseButton msg = 
        let options = msg.Options
        options.closeButton <- true
        { msg with Options = options }
        
    let withPrograssBar msg = 
        let options = msg.Options
        options.progressBar <- true
        { msg with Options = options }
    
    let closeButtonClicked f msg = 
        let options = msg.Options
        options.onCloseClick <- f
        { msg with Options = options }        

    let hideEasing e msg = 
        let options = msg.Options
        options.hideEasing <- e
        { msg with Options = options }

    let options (opt: ToastrOptions) msg = 
        { msg with Options = opt }

    [<Emit("Object.assign({}, $0, $1)")>]
    let private mergeObjects x y = jsNative
    
    /// Overrides global options
    let overrideOptions (options: ToastrOptions) : unit = 
        let mutable globalOptions : ToastrOptions = import "options" "toastr"
        globalOptions <- mergeObjects globalOptions options
    
    /// Immediately remove current toasts without using animation
    let remove() : unit = import "remove" "toastr"
    
    /// Remove current toasts using animation
    let clear() : unit = import "clear" "toastr"
    let success (msg: ToastrMsg) : Cmd<_> = 
        [fun _ ->
            if System.String.IsNullOrEmpty(msg.Title) 
            then successToast msg.Message msg.Options
            else successToastWithTitle msg.Message msg.Title msg.Options]

    let error (msg: ToastrMsg) :  Cmd<_> = 
        [fun _ ->
            if System.String.IsNullOrEmpty(msg.Title) 
            then errorToast msg.Message msg.Options
            else errorToastWithTitle msg.Message msg.Title msg.Options]

    let info (msg: ToastrMsg) : Cmd<_> = 
        [fun _ ->
            if System.String.IsNullOrEmpty(msg.Title) 
            then infoToast msg.Message msg.Options
            else infoToastWithTitle msg.Message msg.Title msg.Options]

    let warning (msg: ToastrMsg) : Cmd<_> = 
        [fun _ ->
            if System.String.IsNullOrEmpty(msg.Title) 
            then warningToast msg.Message msg.Options
            else warningToastWithTitle msg.Message msg.Title msg.Options]