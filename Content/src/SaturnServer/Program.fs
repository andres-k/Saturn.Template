module Server

open Saturn.Pipeline
open Saturn.Application
open Giraffe
open Config

let endpointPipe = pipeline {
    plug head
    plug requestId
}

let app = application {
    pipe_through endpointPipe

    error_handler (fun ex _ -> HttpHandlers.renderHtml (InternalError.layout ex))
    router Router.router
    url "http://0.0.0.0:8085/"
    memory_cache
    use_static "static"
    use_gzip
    use_config (fun _ -> {connectionString = ""} ) //TODO: Set development time configuration
}

[<EntryPoint>]
let main _ =
    printfn "%s" (System.IO.Directory.GetCurrentDirectory())
    run app
    0 // return an integer exit code