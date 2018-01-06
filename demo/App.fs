module App

open Elmish
open Elmish.React
open Elmish.Toastr

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.Core

[<Emit("console.log($0)")>]
let log (x: 'a) : unit = jsNative

type Model = string

let init() = "Elmish.Toastr", Cmd.none

type Message = 
    | Success
    | Info
    | Error
    | Warning
    | SimpleMsg
    | SimpleMsgWithTitle
    | SimpleProgressBar
    | Timeout
    | OnClickSample
    | CloseButton


let update msg model = 
    match msg with
    | Success -> 
        let cmd =   
          Toastr.message "Your account is created"
          |> Toastr.title "Shiny title"
          |> Toastr.position TopRight
          |> Toastr.timeout 3000
          |> Toastr.withProgressBar
          |> Toastr.hideEasing Easing.Swing
          |> Toastr.showCloseButton
          |> Toastr.onClick (fun _ -> log "Clicked")
          |> Toastr.success
        
        model, cmd

    | Info ->
        let cmd =   
          Toastr.message "Just letting you know that you your account was blocked"
          |> Toastr.title "FYI"
          |> Toastr.position TopRight
          |> Toastr.timeout 3000
          |> Toastr.withProgressBar
          |> Toastr.hideEasing Easing.Swing
          |> Toastr.showCloseButton
          |> Toastr.closeButtonClicked (fun _ -> log "Close Clicked")
          |> Toastr.onClick (fun _ -> log "Clicked")
          |> Toastr.onShown (fun _ -> log "Shown")
          |> Toastr.onHidden (fun _ -> log "Hidden")
          |> Toastr.info
        
        model, cmd  

    | Error ->
        let cmd =   
          Toastr.message "Oeps! Something went wrong"
          |> Toastr.title "So Sad!"
          |> Toastr.position TopRight
          |> Toastr.timeout 3000
          |> Toastr.withProgressBar
          |> Toastr.hideEasing Easing.Swing
          |> Toastr.showCloseButton
          |> Toastr.closeButtonClicked (fun _ -> log "Close Clicked")
          |> Toastr.onClick (fun _ -> log "Clicked")
          |> Toastr.onShown (fun _ -> log "Shown")
          |> Toastr.onHidden (fun _ -> log "Hidden")
          |> Toastr.error
        
        model, cmd         
    | Warning ->
        let cmd =   
          Toastr.message "Something is about to go down..."
          |> Toastr.title "Heads up"
          |> Toastr.position TopRight
          |> Toastr.timeout 3000
          |> Toastr.withProgressBar
          |> Toastr.hideEasing Easing.Swing
          |> Toastr.showCloseButton
          |> Toastr.closeButtonClicked (fun _ -> log "Close Clicked")
          |> Toastr.onClick (fun _ -> log "Clicked")
          |> Toastr.onShown (fun _ -> log "Shown")
          |> Toastr.onHidden (fun _ -> log "Hidden")
          |> Toastr.warning
        
        model, cmd       
    | SimpleMsg ->
        let cmd = 
            Toastr.message "Your account is created"
            |> Toastr.success
        model, cmd

    | SimpleMsgWithTitle ->
        let cmd = 
            Toastr.message "Your accound is created"
            |> Toastr.title "Server"
            |> Toastr.success
        model, cmd

    | SimpleProgressBar -> 
        let cmd = 
            Toastr.message "Countdown has started"
            |> Toastr.title "Be aware"
            |> Toastr.withProgressBar
            |> Toastr.warning
        model, cmd

    | Timeout ->
        let cmd = 
            Toastr.message "Hide message in 5 seconds."
            |> Toastr.title "Delayed"
            |> Toastr.timeout 5000
            |> Toastr.withProgressBar
            |> Toastr.info

        model, cmd
    | CloseButton ->
        let cmd = 
            Toastr.message "I have a close button"
            |> Toastr.title "Close"
            |> Toastr.showCloseButton
            |> Toastr.error
        model, cmd

    | OnClickSample ->
        let cmd = 
            Toastr.message "See your browser console"
            |> Toastr.title "Click Me"
            |> Toastr.onClick (fun _ -> printfn "Clicked")
            |> Toastr.info

        model, cmd

   
   

let simpleMessage = """
Toastr.message "Your account is created."
|> Toastr.success
"""  

let simpleMsgTitle = """
Toastr.message "Your accound is created"
|> Toastr.title "Server"
|> Toastr.success
"""

let simpleProgressBar = """
Toastr.message "Countdown has started"
|> Toastr.title "Be aware"
|> Toastr.withProgressBar
|> Toastr.warning
"""

let timeoutSample = """
Toastr.message "Hide message in 5 seconds."
|> Toastr.title "Delayed"
|> Toastr.timeout 5000
|> Toastr.withProgressBar
|> Toastr.info
"""
let closeButtonSample = """
Toastr.message "I have a close button"
|> Toastr.title "Close"
|> Toastr.showCloseButton
|> Toastr.error
"""
let onClickSample = """
Toastr.message "See your browser console"
|> Toastr.title "Click me"
|> Toastr.onClick (fun _ -> printfn "Clicked")
|> Toastr.info
"""

let view _ dispatch = 

    let sample title snippet msg = 
       let btn = 
        button [ Style [ MarginLeft 20 ]; ClassName "btn btn-info"; OnClick (fun _ -> dispatch msg) ] 
               [ str "Run" ]
       
       div [ Style [ TextAlign "left"; Display "table"; Margin "0 auto" ] ]
           [ h4 [ ] [ str title; span [] [ btn ] ]
             pre [ ]
                 [ code [ ]
                        [ str snippet ] ] ]

    let spacing = Style [Margin 5]
    div [ Style [ TextAlign "center" ] ]
        [ h1 [ ] [ str "Elmish.Toastr" ]
          button 
           [ spacing; ClassName "btn btn-success"; OnClick (fun _ -> dispatch Success) ] 
           [ str "Success" ] 
          button 
           [ spacing; ClassName "btn btn-info";OnClick (fun _ -> dispatch Info) ] 
           [ str "Info" ] 
          button 
           [ spacing; ClassName "btn btn-warning";OnClick (fun _ -> dispatch Warning) ] 
           [ str "Warning" ] 
          button 
           [ spacing; ClassName "btn btn-danger"; OnClick (fun _ -> dispatch Error) ] 
           [ str "Error" ]
          br []
          hr [ ]
          sample "Simple message" simpleMessage SimpleMsg
          hr [ ]
          sample "Message with title" simpleMsgTitle SimpleMsgWithTitle
          hr [ ]
          sample "Progress bar" simpleProgressBar SimpleProgressBar 
          hr [ ]
          sample "Close Button" closeButtonSample CloseButton
          hr [ ]
          sample "Timeout" timeoutSample Timeout 
          hr [ ]
          sample "Interactive" onClickSample OnClickSample
          hr [ ] ]



Program.mkProgram init update view 
|> Program.withReact "root"
|> Program.run