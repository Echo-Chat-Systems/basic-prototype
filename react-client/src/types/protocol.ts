export interface Envelope<T> {
    mid: string;
    target: string;

    data: {
        action: string;
        params: T;
    }
}

export interface PendingRequest<T> {
    resolve: (value: T) => void;
    reject: (error: Error) => void;
}