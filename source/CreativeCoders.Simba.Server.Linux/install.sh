#!/bin/bash

SERVICE_NAME="simba-server"

APP_DIR="/opt/simbasrv"
USER_NAME="simba-user"

if systemctl status "$SERVICE_NAME" >/dev/null 2>&1; then
  systemctl stop simba-server
  systemctl disable simba-server
fi

if [ -d "$APP_DIR" ]; then
  rm -rf "${APP_DIR:?}/"*
else
  mkdir "$APP_DIR"
fi

cp -R -f . "$APP_DIR"

if id -u "$USER_NAME" >/dev/null 2>&1; then
  echo "User $USER_NAME exists."
else
  useradd -r -s /bin/false "$USER_NAME"
fi

chown -R "$USER_NAME" "${APP_DIR:?}/"*

dotnet /opt/simbasrv/simbasrv.dll --install
