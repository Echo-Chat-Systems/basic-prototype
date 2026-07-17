import {StrictMode} from 'react'
import {createRoot} from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import {EchoProvider} from "./context/EchoProvider.tsx";

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <EchoProvider>
            <App/>
        </EchoProvider>
    </StrictMode>,
)
