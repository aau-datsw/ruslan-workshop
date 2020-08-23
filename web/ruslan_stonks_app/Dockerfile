FROM debian:latest

# Install flutter dependencies
RUN apt-get update 
RUN apt-get install -y curl git wget zip unzip libgconf-2-4 gdb libstdc++6 libglu1-mesa fonts-droid-fallback lib32stdc++6 python3 xz-utils snapd
RUN apt-get clean

# Clone the flutter repo
RUN git clone https://github.com/flutter/flutter.git /usr/local/flutter

# Run flutter doctor and set path
RUN /usr/local/flutter/bin/flutter doctor -v
ENV PATH="/usr/local/flutter/bin:/usr/local/flutter/bin/cache/dart-sdk/bin:${PATH}"


# Enable flutter web
RUN flutter channel master
RUN flutter upgrade
RUN flutter config --enable-web

# Copy files to container and build
RUN mkdir /usr/local/ruslan_stonks_app
COPY . /usr/local/ruslan_stonks_app
WORKDIR /usr/local/ruslan_stonks_app

RUN flutter pub get

CMD flutter run -d web-server --web-hostname 0.0.0.0 --web-port 5001 --dart-define=RUSLAN_API_PORT=$RUSLAN_API_PORT --dart-define=RUSLAN_API_HOST=$RUSLAN_API_HOST