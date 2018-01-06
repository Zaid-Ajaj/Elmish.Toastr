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

let update msg model = 
    match msg with
    | Success -> 
        let cmd =   
          Toastr.message "Congrats! you did a great \"Job\""
          |> Toastr.title "Shiny title"
          |> Toastr.position TopRight
          |> Toastr.timeout 3000
          |> Toastr.withPrograssBar
          |> Toastr.hideEasing Easing.Swing
          |> Toastr.showCloseButton
          |> Toastr.closeButtonClicked (fun _ -> log "Close Clicked")
          |> Toastr.onClick (fun _ -> log "Clicked")
          |> Toastr.onShown (fun _ -> log "Shown")
          |> Toastr.onHidden (fun _ -> log "Hidden")
          |> Toastr.success
        
        model, cmd

    | Info ->
        let cmd =   
          Toastr.message "Just letting you know that you your account was blocked"
          |> Toastr.title "FYI"
          |> Toastr.position TopRight
          |> Toastr.timeout 3000
          |> Toastr.withPrograssBar
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
          |> Toastr.withPrograssBar
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
          |> Toastr.withPrograssBar
          |> Toastr.hideEasing Easing.Swing
          |> Toastr.showCloseButton
          |> Toastr.closeButtonClicked (fun _ -> log "Close Clicked")
          |> Toastr.onClick (fun _ -> log "Clicked")
          |> Toastr.onShown (fun _ -> log "Shown")
          |> Toastr.onHidden (fun _ -> log "Hidden")
          |> Toastr.warning
        
        model, cmd       

let codeSample = """
open Elmish
open Elmish.Toastr

let successToast : Cmd<_> = 
    Toastr.message "Success message"
    |> Toastr.title "Shiny title"
    |> Toastr.position TopRight
    |> Toastr.timeout 3000
    |> Toastr.withPrograssBar
    |> Toastr.hideEasing Easing.Swing
    |> Toastr.showCloseButton
    |> Toastr.closeButtonClicked (fun _ -> log "Close Clicked")
    |> Toastr.onClick (fun _ -> log "Clicked")
    |> Toastr.onShown (fun _ -> log "Shown")
    |> Toastr.onHidden (fun _ -> log "Hidden")
    |> Toastr.error

let errorToast : Cmd<_> =   
    Toastr.message "Oeps! Something went wrong"
    |> Toastr.title "So Sad!"
    (* other config *)
    |> Toastr.error

// etc...
"""  
let view _ dispatch = 
        let spacing = Style [Margin 5]
        div [ ]
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

              pre [ Style [ Margin 5;BackgroundColor "lightgray"; BorderRadius 20; Padding 20 ] ]
                  [ code [  ]
                         [ str codeSample ] ] ] 



Program.mkProgram init update view 
|> Program.withReact "root"
|> Program.run