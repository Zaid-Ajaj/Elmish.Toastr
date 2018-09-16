# Elmish.Toastr [![Nuget](https://img.shields.io/nuget/v/Elmish.Toastr.svg?colorB=green)](https://www.nuget.org/packages/Elmish.Toastr)

[Toastr](https://github.com/CodeSeven/toastr) integration with Fable, implemented as [Elmish](https://github.com/fable-elmish/elmish) commands. 

[Live Demo](https://zaid-ajaj.github.io/Elmish.Toastr/)

### Nuget Packages

| Fable version | Package |
| ------------- | ------------- |
| 1.3.x  |  [![Nuget](https://img.shields.io/nuget/v/Elmish.Toastr.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/Elmish.Toastr)  |
| 2.0-beta  | [![Nuget](https://img.shields.io/nuget/vpre/Elmish.Toastr.svg?maxAge=0&colorB=brightgreen)](https://www.nuget.org/packages/Elmish.Toastr)   |

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

type Msg = 
    | ShowSuccess
    | Clicked
    | Closed

let successToast : Cmd<Msg> = 
    Toastr.message "Success message"
    |> Toastr.title "Shiny title"
    |> Toastr.position TopRight
    |> Toastr.timeout 3000
    |> Toastr.withProgressBar
    |> Toastr.hideEasing Easing.Swing
    |> Toastr.showCloseButton
    |> Toastr.closeButtonClicked Closed
    |> Toastr.onClick Clicked 
    |> Toastr.success

let update msg model = 
    match msg with
    | ShowSuccess -> 
        model, successToast

    | Clicked ->
        let infoToast = 
            Toastr.message "You clicked previous toast"
            |> Toastr.title "Clicked"
            |> Toastr.info
        model, infoToast

    | Closed ->
        let infoToast = 
            Toastr.message "You clicked the close button"
            |> Toastr.title "Close Clicked"
            |> Toastr.info
        model, infoToast
    | _ -> 
        model, Cmd.none
```
