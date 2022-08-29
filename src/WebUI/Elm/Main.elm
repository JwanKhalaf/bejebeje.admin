module Main exposing (..)

import Browser
import Html exposing (Html, button, div, form, h3, i, input, label, li, pre, span, text, ul)
import Html.Attributes as Attr
import Html.Events exposing (onCheck, onClick, onInput)
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
    | SoloArtistFirstNameUpdated String
    | SoloArtistLastNameUpdated String
    | SoloArtistMaleGenderSelected
    | SoloArtistFemaleGenderSelected
    | ClickedNext


update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
    case msg of
        ClickedNext ->
            case model of
                Step1Type (Just SoloArtist) ->
                    ( SoloArtistForm (SoloStep2Name "" ""), Cmd.none )

                Step1Type (Just Band) ->
                    ( BandForm (BandStep2Name ""), Cmd.none )

                SoloArtistForm (SoloStep2Name firstName lastName) ->
                    ( SoloArtistForm (SoloStep3Gender firstName lastName Male), Cmd.none )

                _ ->
                    ( model, Cmd.none )

        SoloArtistSelected ->
            ( Step1Type (Just SoloArtist), Cmd.none )

        BandSelected ->
            ( Step1Type (Just Band), Cmd.none )

        SoloArtistFirstNameUpdated newFirstName ->
            case model of
                SoloArtistForm (SoloStep2Name firstName lastName) ->
                    ( SoloArtistForm (SoloStep2Name newFirstName lastName), Cmd.none )

                _ ->
                    ( model, Cmd.none )

        SoloArtistLastNameUpdated newLastName ->
            case model of
                SoloArtistForm (SoloStep2Name firstName lastName) ->
                    ( SoloArtistForm (SoloStep2Name firstName newLastName), Cmd.none )

                _ ->
                    ( model, Cmd.none )

        SoloArtistMaleGenderSelected ->
            case model of
                SoloArtistForm (SoloStep3Gender firstName lastName gender) ->
                    ( SoloArtistForm (SoloStep3Gender firstName lastName Male), Cmd.none )

                _ ->
                    ( model, Cmd.none )

        SoloArtistFemaleGenderSelected ->
            case model of
                SoloArtistForm (SoloStep3Gender firstName lastName gender) ->
                    ( SoloArtistForm (SoloStep3Gender firstName lastName Female), Cmd.none )

                _ ->
                    ( model, Cmd.none )



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
                [ div [ Attr.class "bg-slate-800 rounded-md mb-5" ]
                    [ div [ Attr.class "rounded-r-md p-8" ]
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
                    , div [ Attr.class "bg-slate-800 border-t-2 border-slate-500 p-8" ]
                        [ div [ Attr.class "mb-8" ]
                            [ div [ Attr.class "mb-2 text-base font-medium dark:text-white" ] [ text "1 of 5" ]
                            , div [ Attr.class "w-full bg-slate-500 rounded-full h-2.5 mb-4 dark:bg-slate-600" ]
                                [ div [ Attr.class "bg-slate-900 h-2.5 rounded-full dark:bg-gray-300 w-1/5" ] []
                                ]
                            ]
                        , button [ Attr.class "text-white border-2 border-slate-500 cursor-not-allowed bg-slate-500 rounded-md py-1 px-2", Attr.disabled True] [ text "Next" ]
                        ]
                    ]
                ]

        Step1Type (Just _) ->
            div []
                [ div [ Attr.class "bg-slate-800 rounded-md mb-5" ]
                    [ div [ Attr.class "rounded-r-md p-8" ]
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
                    , div [ Attr.class "bg-slate-800 border-t-2 border-slate-500 p-8" ]
                        [ div [ Attr.class "mb-8" ]
                            [ div [ Attr.class "mb-2 text-base font-medium dark:text-white" ] [ text "1 of 5" ]
                            , div [ Attr.class "w-full bg-slate-500 rounded-full h-2.5 mb-4 dark:bg-slate-600" ]
                                [ div [ Attr.class "bg-slate-900 h-2.5 rounded-full dark:bg-gray-300 w-1/5" ] []
                                ]
                            ]
                        , button [ Attr.class "text-white border-2 border-green-500 cursor-pointer bg-green-500 rounded-md py-1 px-2 hover:bg-green-400", onClick ClickedNext ] [ text "Next" ]
                        , button [ Attr.class "text-white border-2 border-slate-500 cursor-pointer rounded-md py-1 px-2 hover:bg-slate-700 ml-3" ] [ text "Back" ]
                        ]
                    ]
                ]

        SoloArtistForm (SoloStep2Name _ _) ->
            div []
                [ div [ Attr.class "bg-slate-800 rounded-md mb-5" ]
                    [ div [ Attr.class "rounded-r-md p-8" ]
                        [ form []
                            [ div [ Attr.class "mb-8" ] [ h3 [ Attr.class "text-xl" ] [ text "What is the artist called?" ] ]
                            , div [ Attr.class "mb-4" ]
                                [ label [ Attr.class "block mb-2" ] [ text "First name" ]
                                , input [ Attr.name "first_name", Attr.class "p-2 bg-slate-600 rounded-md", Attr.type_ "text", onInput SoloArtistFirstNameUpdated ] []
                                ]
                            , div [ Attr.class "mb-4" ]
                                [ label [ Attr.class "block mb-2" ] [ text "Last name" ]
                                , input [ Attr.name "last_name", Attr.class "p-2 bg-slate-600 rounded-md", Attr.type_ "text", onInput SoloArtistLastNameUpdated ] []
                                ]
                            ]
                        ]
                    , div [ Attr.class "bg-slate-800 border-t-2 border-slate-500 p-8" ]
                        [ div [ Attr.class "mb-8" ]
                            [ div [ Attr.class "mb-2 text-base font-medium dark:text-white" ] [ text "2 of 5 - Artist Name" ]
                            , div [ Attr.class "w-full bg-slate-500 rounded-full h-2.5 mb-4 dark:bg-slate-600" ]
                                [ div [ Attr.class "bg-slate-900 h-2.5 rounded-full dark:bg-gray-300 w-2/5" ] []
                                ]
                            ]
                        , button [ Attr.class "text-white border-2 border-green-500 cursor-pointer bg-green-500 rounded-md py-1 px-2 hover:bg-green-400", onClick ClickedNext ] [ text "Next" ]
                        , button [ Attr.class "text-white border-2 border-slate-500 cursor-pointer rounded-md py-1 px-2 hover:bg-slate-700 ml-3" ] [ text "Back" ]
                        ]
                    ]
                ]

        SoloArtistForm (SoloStep3Gender _ _ _) ->
            div []
                [ div [ Attr.class "bg-slate-800 rounded-md mb-5" ]
                    [ div [ Attr.class "rounded-r-md p-8" ]
                        [ form []
                            [ div [ Attr.class "mb-8" ] [ h3 [ Attr.class "text-xl" ] [ text "What gender is the artist?" ] ]
                            , div [ Attr.class "flex" ]
                                [ label [ Attr.class "mr-8 bg-slate-900 rounded-md w-36 cursor-pointer p-4" ]
                                    [ span [ Attr.class "flex items-center justify-center py-8 px-12" ] [ i [ Attr.class "fas text-5xl text-slate-700 fa-mars" ] [] ]
                                    , span [ Attr.class "block flex items-center" ]
                                        [ input [ Attr.name "band", Attr.class "mr-2 w-5 h-5 border-slate-700 text-slate-700", Attr.type_ "radio", onCheck (always SoloArtistMaleGenderSelected) ] []
                                        , span [ Attr.class "block" ] [ text "Male" ]
                                        ]
                                    ]
                                , label [ Attr.class "mr-8 bg-slate-900 rounded-md w-36 cursor-pointer p-4" ]
                                    [ span [ Attr.class "flex items-center justify-center py-8 px-12" ] [ i [ Attr.class "fas text-5xl text-slate-700 fa-venus" ] [] ]
                                    , span [ Attr.class "block flex items-center" ]
                                        [ input [ Attr.name "band", Attr.class "mr-2 w-5 h-5 border-slate-700 text-slate-700", Attr.type_ "radio", onCheck (always SoloArtistFemaleGenderSelected) ] []
                                        , span [ Attr.class "block" ] [ text "Female" ]
                                        ]
                                    ]
                                ]
                            ]
                        ]
                    , div [ Attr.class "bg-slate-800 border-t-2 border-slate-500 p-8" ]
                        [ div [ Attr.class "mb-8" ]
                            [ div [ Attr.class "mb-2 text-base font-medium dark:text-white" ] [ text "3 of 5 - Artist Gender" ]
                            , div [ Attr.class "w-full bg-slate-500 rounded-full h-2.5 mb-4 dark:bg-slate-600" ]
                                [ div [ Attr.class "bg-slate-900 h-2.5 rounded-full dark:bg-gray-300 w-3/5" ] []
                                ]
                            ]
                        , button [ Attr.class "text-white border-2 border-green-500 cursor-pointer bg-green-500 rounded-md py-1 px-2 hover:bg-green-400", onClick ClickedNext ] [ text "Next" ]
                        , button [ Attr.class "text-white border-2 border-slate-500 cursor-pointer rounded-md py-1 px-2 hover:bg-slate-700 ml-3" ] [ text "Back" ]
                        ]
                    ]
                ]

        SoloArtistForm SoloStep4Photo ->
            div []
                [ div [ Attr.class "bg-slate-800 rounded-md mb-5" ]
                    [ div [ Attr.class "rounded-r-md p-8" ]
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
                    , div [ Attr.class "bg-slate-800 border-t-2 border-slate-500 p-8" ]
                        [ div [ Attr.class "mb-8" ]
                            [ div [ Attr.class "mb-2 text-base font-medium dark:text-white" ] [ text "4 of 5 - Artist Name" ]
                            , div [ Attr.class "w-full bg-slate-500 rounded-full h-2.5 mb-4 dark:bg-slate-600" ]
                                [ div [ Attr.class "bg-slate-900 h-2.5 rounded-full dark:bg-gray-300 w-4/5" ] []
                                ]
                            ]
                        , button [ Attr.class "text-white border-2 border-green-500 cursor-pointer bg-green-500 rounded-md py-1 px-2 hover:bg-green-400", onClick ClickedNext ] [ text "Next" ]
                        , button [ Attr.class "text-white border-2 border-slate-500 cursor-pointer rounded-md py-1 px-2 hover:bg-slate-700 ml-3" ] [ text "Back" ]
                        ]
                    ]
                ]

        BandForm _ ->
            div []
                [ div [ Attr.class "bg-slate-800 rounded-md mb-5" ]
                    [ div [ Attr.class "rounded-r-md p-8" ]
                        [ form []
                            [ div [ Attr.class "mb-8" ] [ h3 [ Attr.class "text-xl" ] [ text "What is the name of the band?" ] ]
                            , div [ Attr.class "mb-4" ]
                                [ label [ Attr.class "block mb-2" ] [ text "Band name" ]
                                , input [ Attr.name "name", Attr.class "p-2 bg-slate-600 rounded-md", Attr.type_ "text" ] []
                                ]
                            ]
                        ]
                    , div [ Attr.class "bg-slate-800 border-t-2 border-slate-500 p-8" ]
                        [ div [ Attr.class "mb-8" ]
                            [ div [ Attr.class "mb-2 text-base font-medium dark:text-white" ] [ text "2 of 5 - Band Name" ]
                            , div [ Attr.class "w-full bg-slate-500 rounded-full h-2.5 mb-4 dark:bg-slate-600" ]
                                [ div [ Attr.class "bg-slate-900 h-2.5 rounded-full dark:bg-gray-300 w-1/5" ] []
                                ]
                            ]
                        , button [ Attr.class "text-white border-2 border-green-500 cursor-pointer bg-green-500 rounded-md py-1 px-2 hover:bg-green-400", onClick ClickedNext ] [ text "Next" ]
                        , button [ Attr.class "text-white border-2 border-slate-500 cursor-pointer rounded-md py-1 px-2 hover:bg-slate-700 ml-3" ] [ text "Back" ]
                        ]
                    ]
                ]
