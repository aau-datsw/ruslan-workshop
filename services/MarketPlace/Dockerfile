FROM ruby:2.6
RUN apt-get update -qq && apt-get install -y nodejs postgresql-client && rm -rf /var/lib/apt/lists/*
RUN gem install bundler
RUN mkdir /marketplace
WORKDIR /marketplace
COPY Gemfile /marketplace/Gemfile
COPY Gemfile.lock /marketplace/Gemfile.lock
RUN bundle install
COPY . /marketplace

# Add a script to be executed every time the container starts.
COPY entrypoint.sh /usr/bin/
RUN chmod +x /usr/bin/entrypoint.sh
ENTRYPOINT ["entrypoint.sh"]
EXPOSE 3000

# Start the main process.
CMD ["bundle", "exec", "rails", "server", "-b", "0.0.0.0"]
