export type ISignalRWrapperProps = {
  connectionData: ISignalRConnectionData;
  eventName: string;
  eventHandlerCallback: EventCallback;
};

export interface ISignalRConnectionData {
  userAccessToken: string;
  negotiateEndpoint: string;
  signalRUserId: string;
  negotiateCustomHeaders: Record<string, string>;
}

type EventCallback = (data: any) => void;
