import * as signalR from "@microsoft/signalr";

import React, { useEffect } from "react";

import { ISignalRWrapperProps } from "./SignalRWrapper.types";
import axios from "axios";

export const SignalRWrapper: React.FC<ISignalRWrapperProps> = (
  props: ISignalRWrapperProps
) => {
  const { connectionData, eventName, eventHandlerCallback,  } = props;
  let connection: signalR.HubConnection | null = null;

  const startConnection = () => {
    axios
      .get(connectionData.negotiateEndpoint, {
        headers: {
          Authorization: "Bearer " + connectionData.userAccessToken,
          "x-ms-signalr-user-id": connectionData.signalRUserId,
          ...connectionData.negotiateCustomHeaders
        },
      })
      .then((response) => {
        const negotiateResponse = response.data;
        connection = new signalR.HubConnectionBuilder()
          .withUrl(negotiateResponse.url, {
            accessTokenFactory: () => negotiateResponse.accessToken,
          })
          .configureLogging(signalR.LogLevel.Information)
          .build();
        connection
          .start()
          .then(() => {
            console.log("SignalR connection established.");
            connection?.on(eventName, eventHandlerCallback);
          })
          .catch((error) => {
            console.error("Error starting SignalR connection:", error);
          });
      });
  };

  const stopConnection = () => {
    if (connection) {
      connection
        .stop()
        .then(() => {
          console.log("SignalR connection stopped.");
        })
        .catch((error) => {
          console.error("Error stopping SignalR connection:", error);
        });
    }
  };

  useEffect(() => {
    startConnection();
    return () => {
      stopConnection();
    };
  }, []);

  return null;
};
