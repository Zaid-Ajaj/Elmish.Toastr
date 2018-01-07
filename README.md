# Elmish.Toastr [![Nuget](https://img.shields.io/nuget/v/Elmish.Toastr.svg?colorB=green)](https://www.nuget.org/packages/Elmish.Toastr)

[Toastr](https://github.com/CodeSeven/toastr) integration with Fable, implemented as [Elmish](https://github.com/fable-elmish/elmish) commands. 

[Live Demo](https://zaid-ajaj.github.io/Elmish.Toastr/)


## Installation
- Install this library from nuget
```
paket add nuget Elmish.Toastr --project /path/to/Project.fsproj
```
- Install toastr from npm
```
npm install toastr --save
```
- Because this library directly uses and imports the CSS dependency from the npm toastr package, you will need the appropriate CSS loaders for Webpack: `css-loader` and `style-loader`, install them :
```
npm install css-loader style-loader --save-dev
```
- Now from your Webpack, use the loaders:
```
{
    test: /\.(sa|c)ss$/,
    use: [
        "style-loader",
        "css-loader"
    ]
}
```

## Usage
See the demo app for reference. Create toastr commands and use them in the Elmish dispatch loop

```fs
open Elmish
open Elmish.Toastr

let successToast : Cmd<_> = 
    Toastr.message "Success message"
    |> Toastr.title "Shiny title"
    |> Toastr.position TopRight
    |> Toastr.timeout 3000
    |> Toastr.withProgressBar
    |> Toastr.hideEasing Easing.Swing
    |> Toastr.showCloseButton
    |> Toastr.closeButtonClicked (fun _ -> log "Close Clicked")
    |> Toastr.onClick (fun _ -> log "Clicked")
    |> Toastr.onShown (fun _ -> log "Shown")
    |> Toastr.onHidden (fun _ -> log "Hidden")
    |> Toastr.success

let update msg model = 
    match msg with
    | ShowSuccess -> model, successToast
    | OtherMsg -> model, Cmd.none
```
