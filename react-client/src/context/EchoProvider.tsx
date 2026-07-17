import {EchoClient} from "../api/EchoClient.ts";
import {createContext, type ReactNode, useEffect, useState} from "react";

interface EchoState {
    client: EchoClient | null;
    connected: boolean;
}

export const EchoContext = createContext<EchoState>({client: null, connected: false});

export function EchoProvider({children}: {children: ReactNode}) {
    const [client, setClient] = useState<EchoClient | null>(null);

    const [connected, setConnected] = useState(false);

    useEffect(() => {
        const echo = new EchoClient("ws://localhost:69");

        echo.connect().then(() =>
        {
            setConnected(true);
        });

        setClient(echo);
    }, [])

    return (
        <EchoContext.Provider value={{client, connected}}>{children}</EchoContext.Provider>
    );

}