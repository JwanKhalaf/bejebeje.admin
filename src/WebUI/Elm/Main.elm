module Main exposing (..)

import Browser
import Html exposing (Html, a, div, form, h3, i, input, label, li, pre, span, text, ul)
import Html.Attributes as Attr
import Html.Events exposing (onCheck, onClick)
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


type Form
    = Step1Type (Maybe ArtistType)
    | BandForm BandSteps
    | SoloArtistForm SoloSteps


type BandSteps
    = BandStep2Name String
    | BandStep3Photo


type SoloSteps
    = SoloStep2Name String String
    | SoloStep3Gender String String Gender
    | SoloStep4Photo


type ArtistType
    = SoloArtist
    | Band


type Gender
    = Male
    | Female


type alias Model =
    Form


init : () -> ( Model, Cmd Msg )
init _ =
    ( Step1Type Nothing
    , Cmd.none
    )



-- UPDATE


type Msg
    = BandSelected
    | SoloArtistSelected
    | ClickedNext


update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
    case msg of
        ClickedNext ->
            ( model, Cmd.none )

        BandSelected ->
            ( Step1Type (Just Band), Cmd.none )

        SoloArtistSelected ->
            ( Step1Type (Just SoloArtist), Cmd.none )



-- SUBSCRIPTIONS


subscriptions : Model -> Sub Msg
subscriptions model =
    Sub.none



-- VIEW


view : Model -> Html Msg
view model =
    case model of
        Step1Type Nothing ->
            div []
                [ div [ Attr.class "bg-slate-800 flex rounded-md mb-5" ]
                    [ div [ Attr.class "bg-slate-800 border-r border-slate-500 rounded-l-md p-8" ]
                        [ viewStepsIndicators model ]
                    , div [ Attr.class "rounded-r-md p-8" ]
                        [ form []
                            [ div [ Attr.class "mb-8" ] [ h3 [ Attr.class "text-xl" ] [ text "Are they a solo artist or a band?" ] ]
                            , div [ Attr.class "flex" ]
                                [ label [ Attr.class "mr-8 bg-slate-900 rounded-md w-36 cursor-pointer p-4" ]
                                    [ span [ Attr.class "flex items-center justify-center py-8 px-12" ] [ i [ Attr.class "fas text-5xl text-slate-700 fa-user" ] [] ]
                                    , span [ Attr.class "block flex items-center" ]
                                        [ input [ Attr.name "band", Attr.class "mr-2 w-5 h-5 border-slate-700 text-slate-700", Attr.type_ "radio", onCheck (always SoloArtistSelected) ] []
                                        , span [ Attr.class "block" ] [ text "Solo Artist" ]
                                        ]
                                    ]
                                , label [ Attr.class "mr-8 bg-slate-900 rounded-md w-36 cursor-pointer p-4" ]
                                    [ span [ Attr.class "flex items-center justify-center py-8 px-12" ] [ i [ Attr.class "fas text-5xl text-slate-700 fa-users" ] [] ]
                                    , span [ Attr.class "block flex items-center" ]
                                        [ input [ Attr.name "band", Attr.class "mr-2 w-5 h-5 border-slate-700 text-slate-700", Attr.type_ "radio", onCheck (always BandSelected) ] []
                                        , span [ Attr.class "block" ] [ text "Band" ]
                                        ]
                                    ]
                                ]
                            ]
                        ]
                    ]
                , a [ Attr.class "text-white cursor-pointer bg-green-500 rounded-md py-1 px-2 hover:bg-slate-700 ml-3", onClick ClickedNext ] [ text "Next" ]
                , a [ Attr.class "text-white border-2 border-slate-500 cursor-pointer rounded-md py-1 px-2 hover:bg-slate-700 ml-3" ] [ text "Back" ]
                ]

        Step1Type (Just _) ->
            div []
                [ div [ Attr.class "bg-slate-800 flex rounded-md mb-5" ]
                    [ div [ Attr.class "bg-slate-800 border-r border-slate-500 rounded-l-md p-8" ]
                        [ viewStepsIndicators model ]
                    , div [ Attr.class "rounded-r-md p-8" ]
                        [ form []
                            [ div [ Attr.class "mb-8" ] [ h3 [ Attr.class "text-xl" ] [ text "Are they a solo artist or a band?" ] ]
                            , div [ Attr.class "flex" ]
                                [ label [ Attr.class "mr-8 bg-slate-900 rounded-md w-36 cursor-pointer p-4" ]
                                    [ span [ Attr.class "flex items-center justify-center py-8 px-12" ] [ i [ Attr.class "fas text-5xl text-slate-700 fa-user" ] [] ]
                                    , span [ Attr.class "block flex items-center" ]
                                        [ input [ Attr.name "band", Attr.class "mr-2 w-5 h-5 border-slate-700 text-slate-700", Attr.type_ "radio", onCheck (always SoloArtistSelected) ] []
                                        , span [ Attr.class "block" ] [ text "Solo Artist" ]
                                        ]
                                    ]
                                , label [ Attr.class "mr-8 bg-slate-900 rounded-md w-36 cursor-pointer p-4" ]
                                    [ span [ Attr.class "flex items-center justify-center py-8 px-12" ] [ i [ Attr.class "fas text-5xl text-slate-700 fa-users" ] [] ]
                                    , span [ Attr.class "block flex items-center" ]
                                        [ input [ Attr.name "band", Attr.class "mr-2 w-5 h-5 border-slate-700 text-slate-700", Attr.type_ "radio", onCheck (always BandSelected) ] []
                                        , span [ Attr.class "block" ] [ text "Band" ]
                                        ]
                                    ]
                                ]
                            ]
                        ]
                    ]
                , a [ Attr.class "text-white cursor-pointer bg-green-500 rounded-md py-1 px-2 hover:bg-slate-700 ml-3", onClick ClickedNext ] [ text "Next" ]
                , a [ Attr.class "text-white border-2 border-slate-500 cursor-pointer rounded-md py-1 px-2 hover:bg-slate-700 ml-3" ] [ text "Back" ]
                ]

        BandForm _ ->
            div []
                [ div [ Attr.class "bg-slate-800 flex rounded-md mb-5" ]
                    [ div [ Attr.class "bg-slate-800 border-r border-slate-500 rounded-l-md p-8" ]
                        [ viewStepsIndicators model ]
                    , div [ Attr.class "rounded-r-md p-8" ]
                        [ form []
                            [ div [ Attr.class "mb-8" ] [ h3 [ Attr.class "text-xl" ] [ text "What is the name of the band?" ] ]
                            , div [ Attr.class "mb-4" ]
                                [ label [ Attr.class "block mb-2" ] [ text "Band name" ]
                                , input [ Attr.name "name", Attr.class "p-2 bg-slate-600 rounded-md", Attr.type_ "text" ] []
                                ]
                            ]
                        ]
                    ]
                , a [ Attr.class "text-white cursor-pointer bg-green-500 rounded-md py-1 px-2 hover:bg-slate-700 ml-3", onClick ClickedNext ] [ text "Next" ]
                , a [ Attr.class "text-white border-2 border-slate-500 cursor-pointer rounded-md py-1 px-2 hover:bg-slate-700 ml-3" ] [ text "Back" ]
                ]

        SoloArtistForm _ ->
            div []
                [ div [ Attr.class "bg-slate-800 flex rounded-md mb-5" ]
                    [ div [ Attr.class "bg-slate-800 border-r border-slate-500 rounded-l-md p-8" ]
                        [ viewStepsIndicators model ]
                    , div [ Attr.class "rounded-r-md p-8" ]
                        [ form []
                            [ div [ Attr.class "mb-8" ] [ h3 [ Attr.class "text-xl" ] [ text "What is the artist called?" ] ]
                            , div [ Attr.class "mb-4" ]
                                [ label [ Attr.class "block mb-2" ] [ text "First name" ]
                                , input [ Attr.name "first_name", Attr.class "p-2 bg-slate-600 rounded-md", Attr.type_ "text" ] []
                                ]
                            , div [ Attr.class "mb-4" ]
                                [ label [ Attr.class "block mb-2" ] [ text "Last name" ]
                                , input [ Attr.name "last_name", Attr.class "p-2 bg-slate-600 rounded-md", Attr.type_ "text" ] []
                                ]
                            ]
                        ]
                    ]
                , a [ Attr.class "text-white cursor-pointer bg-green-500 rounded-md py-1 px-2 hover:bg-slate-700 ml-3", onClick ClickedNext ] [ text "Next" ]
                , a [ Attr.class "text-white border-2 border-slate-500 cursor-pointer rounded-md py-1 px-2 hover:bg-slate-700 ml-3" ] [ text "Back" ]
                ]


viewStepsIndicators : Model -> Html msg
viewStepsIndicators model =
    case model of
        Step1Type _ ->
            ul []
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

        BandForm _ ->
            ul []
                [ li [ Attr.class "mb-8 flex items-center" ]
                    [ span [ Attr.class "block w-8 h-8 rounded-full bg-slate-500 mr-4 flex items-center justify-center" ] [ i [ Attr.class "fas text-slate-700 fa-circle" ] [] ]
                    , a [] [ text "Type" ]
                    ]
                , li [ Attr.class "mb-8 flex items-center" ]
                    [ span [ Attr.class "block w-8 h-8 rounded-full bg-slate-500 mr-4 flex items-center justify-center" ] [ i [ Attr.class "fas text-slate-700 fa-circle" ] [] ]
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

        SoloArtistForm _ ->
            ul []
                [ li [ Attr.class "mb-8 flex items-center" ]
                    [ span [ Attr.class "block w-8 h-8 rounded-full bg-slate-500 mr-4 flex items-center justify-center" ] [ i [ Attr.class "fas text-slate-700 fa-circle" ] [] ]
                    , a [] [ text "Type" ]
                    ]
                , li [ Attr.class "mb-8 flex items-center" ]
                    [ span [ Attr.class "block w-8 h-8 rounded-full bg-slate-500 mr-4 flex items-center justify-center" ] [ i [ Attr.class "fas text-slate-700 fa-circle" ] [] ]
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
