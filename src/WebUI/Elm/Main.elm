module Main exposing (..)

import Browser
import Html exposing (Html, a, div, form, h3, i, input, label, li, pre, span, text, ul)
import Html.Attributes as Attr
import Http



-- MAIN


main =
    Browser.element
        { init = init
        , update = update
        , subscriptions = subscriptions
        , view = view
        }



-- MODEL


type FormSection
    = Band
    | Name
    | Gender
    | Photo


type Gender
    = Male
    | Female


type alias Model =
    { section : FormSection
    , gender : Maybe Gender
    }


init : () -> ( Model, Cmd Msg )
init _ =
    ( { section = Band, gender = Nothing }
    , Cmd.none
    )



-- UPDATE


type Msg
    = ClickedNext


update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
    case msg of
        ClickedNext ->
            ( model, Cmd.none )



-- SUBSCRIPTIONS


subscriptions : Model -> Sub Msg
subscriptions model =
    Sub.none



-- VIEW


view : Model -> Html Msg
view model =
    case ( model.section, model.gender ) of
        ( Band, Nothing ) ->
            div []
                [ div [ Attr.class "bg-slate-800 flex rounded-md mb-5" ]
                    [ div [ Attr.class "bg-slate-800 border-r border-slate-500 rounded-l-md p-8" ]
                        [ ul []
                            [ li [ Attr.class "mb-8 flex items-center" ]
                                [ span [ Attr.class "block w-8 h-8 rounded-full bg-slate-500 mr-4 flex items-center justify-center" ] [ i [ Attr.class "fas text-slate-700 fa-circle" ] [] ]
                                , a [] [ text "Type" ]
                                ]
                            , li [ Attr.class "mb-8 flex items-center" ]
                                [ span [ Attr.class "block w-8 h-8 rounded-full border-slate-500 border-2 mr-4 flex items-center justify-center" ] []
                                , a [] [ text "Name" ]
                                ]
                            , li [ Attr.class "mb-8 flex items-center" ]
                                [ span [ Attr.class "block w-8 h-8 rounded-full border-slate-500 border-2 mr-4 flex items-center justify-center" ] []
                                , a [] [ text "Gender" ]
                                ]
                            , li [ Attr.class "mb-8 flex items-center" ]
                                [ span [ Attr.class "block w-8 h-8 rounded-full border-slate-500 border-2 mr-4 flex items-center justify-center" ] []
                                , a [] [ text "Photo" ]
                                ]
                            ]
                        ]
                    , div [ Attr.class "rounded-r-md p-8" ]
                        [ form []
                            [ div [ Attr.class "mb-8" ] [ h3 [ Attr.class "text-xl" ] [ text "Are they a solo artist or a band?" ] ]
                            , div [ Attr.class "flex" ]
                                [ label [ Attr.class "mr-8 bg-slate-900 rounded-md w-36 cursor-pointer p-4" ]
                                    [ span [ Attr.class "flex items-center justify-center py-8 px-12" ] [ i [ Attr.class "fas text-5xl text-slate-700 fa-user" ] [] ]
                                    , span [ Attr.class "block flex items-center" ]
                                        [ input [ Attr.name "band", Attr.class "mr-2 w-5 h-5 border-slate-700 text-slate-700", Attr.type_ "radio" ] []
                                        , span [ Attr.class "block" ] [ text "Solo Artist" ]
                                        ]
                                    ]
                                , label [ Attr.class "mr-8 bg-slate-900 rounded-md w-36 cursor-pointer p-4" ]
                                    [ span [ Attr.class "flex items-center justify-center py-8 px-12" ] [ i [ Attr.class "fas text-5xl text-slate-700 fa-users" ] [] ]
                                    , span [ Attr.class "block flex items-center" ]
                                        [ input [ Attr.name "band", Attr.class "mr-2 w-5 h-5 border-slate-700 text-slate-700", Attr.type_ "radio" ] []
                                        , span [ Attr.class "block" ] [ text "Band" ]
                                        ]
                                    ]
                                ]
                            ]
                        ]
                    ]
                , a [ Attr.class "text-white border-2 border-slate-500 rounded-md py-1 px-2 hover:bg-slate-700 ml-3" ] [ text "Back" ]
                ]

        ( Name, _ ) ->
            text "blah"

        ( Gender, _ ) ->
            text "blah"

        ( Photo, _ ) ->
            text "blah"

        ( Band, Just _ ) ->
            text "blah"
