import type { Envelope, PendingRequest} from "../types/protocol.ts";

export class EchoClient {
    private socket: WebSocket;

    private pending: Map<string, PendingRequest<any>> = new Map();

    constructor(url: string) {
        this.socket = new WebSocket(url);

        this.socket.onmessage = this.handleMessage.bind(this);
    }

    connect() : Promise<void> {
        return new Promise((resolve, reject) => {
            this.socket.onopen = () => resolve;
            this.socket.onerror = () => reject;
        })
    }

    async request<TResponse, TRequest>(
        target: string,
        action: string,
        params: TRequest
    ): Promise<TResponse>
    {
        const mid = crypto.randomUUID();


        const message: Envelope<TRequest> =
            {
                mid,
                target,

                data:
                    {
                        action,
                        params
                    }
            };


        const response =
            new Promise<TResponse>((resolve, reject) =>
            {
                this.pending.set(
                    mid,
                    {
                        resolve,
                        reject
                    });
            });


        this.socket.send(
            JSON.stringify(message));


        return response;
    }


    send<T>(
        target: string,
        action: string,
        params: T)
    {
        const message: Envelope<T> =
            {
                mid: crypto.randomUUID(),

                target,

                data:
                    {
                        action,
                        params
                    }
            };


        this.socket.send(
            JSON.stringify(message));
    }


    private handleMessage(event: MessageEvent) {
        const message: Envelope<any> = JSON.parse(event.data);

        const pending = this.pending.get(message.mid);
        if (!pending){
            // Handle as an event message
            return;
        }

        this.pending.delete(message.mid);
        pending.resolve(message.data.params);
    }

}