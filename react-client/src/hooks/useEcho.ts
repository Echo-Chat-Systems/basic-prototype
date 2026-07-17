import {useContext} from "react";
import {EchoContext} from "../context/EchoProvider.tsx";

export function useEcho(){
    return useContext(EchoContext);
}